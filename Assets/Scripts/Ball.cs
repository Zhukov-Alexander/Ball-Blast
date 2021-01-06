using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class Ball : MonoBehaviour
{
    [SerializeField] GameConfig gameConfig;
    LevelManager levelManager;
    [SerializeField] TextMeshProUGUI textMeshProUGUI;
    [SerializeField] SpriteRenderer spriteRenderer;
    float currentLife;
    float initialLife;
    int size;
    public void Constructor(float life, int size, bool child = false)
    {
        levelManager = FindObjectOfType<LevelManager>();
        levelManager.balls.Add(this);

        SetBallSize(size);
        SetBallLife(life);
        SetBallColour();
        if (child) GetComponent<Collider2D>().isTrigger = false;
    }
    private void SetBallSize(int size)
    {
        this.size = size;
        for (int i = 0; i < gameConfig.ballScales.Count; i++)
        {
            if (this.size == i)
            {
                float value = gameConfig.ballScales[i];
                transform.localScale = transform.localScale * new Vector2(value, value);
                return;
            }
        }
    }

    private void SetBallColour()
    {
        float value = currentLife / levelManager.MediumBallLife;
        for (int i = 0; i < gameConfig.ballColors.Count; i++)
        {
            if (value <= gameConfig.ballColorThresholds[i])
            {
                spriteRenderer.color = gameConfig.ballColors[i];
                 return;
            }
        }
    }
    private void SetBallLife(float life)
    {
        initialLife = life;
        currentLife = life;
        UpdateLifeText();
    }

    private void UpdateLifeText()
    {
        textMeshProUGUI.text = NumberHandler.NumberToTextInOneLine(currentLife);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.layer == 9)
        {
            GetComponent<Collider2D>().isTrigger = false;
        }
    }
    private void OnParticleCollision(GameObject other)
    {
        if (other.layer == 11)
        {
            Bullet bullet = other.GetComponent<Bullet>();
            RemoveLife(bullet.Damage);
        }
    }
    void RemoveLife(float damage)
    {
        currentLife -= damage;
        UpdateLifeText();
        CheckLife();
    }
    void CheckLife()
    {
        if (currentLife <= 0)
        {
            if (size > 0)
            {
                Shoot(-1);
                Shoot(1);
            }
            Destroy();
        }
    }
    private void Shoot(int sign)
    {
        GameObject ballGO = CreateABall();
        Rigidbody2D ballRigidbody2D = ballGO.GetComponent<Rigidbody2D>();
        ballRigidbody2D.AddForce(GetShotDirection() * gameConfig.ballForceToShoot, ForceMode2D.Impulse);

        Vector2 GetShotDirection()
        {
            int randomAngle = Random.Range(gameConfig.minBallSpawnerShootAngle, gameConfig.maxBallSpawnerShootAngle);
            Vector3 rotatedVector = Quaternion.AngleAxis(sign * randomAngle, Vector3.back) * Vector3.up;
            return rotatedVector;
        }

        GameObject CreateABall()
        {
            GameObject ballGameObject = Instantiate(gameConfig.ballPrefab, gameObject.transform.position, Quaternion.identity);
            Ball ball = ballGameObject.GetComponent<Ball>();
            ball.Constructor(initialLife, size - 1, true);
            return ballGameObject;
        }
    }
    public void Destroy()
    {
        levelManager.balls.Remove(this);
        Destroy(gameObject);
    }
}
