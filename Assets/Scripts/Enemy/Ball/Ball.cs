using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;
using static BallSpawnersManager;
using static GameConfigContainer;
using System.ArrayExtensions;
using System.Linq;
using DG.Tweening;

public class Ball : MonoBehaviour
{
    [SerializeField] GameObject DeathAnimGO;
    [SerializeField] TextMeshProUGUI textMeshProUGUI;
    [SerializeField] SpriteRenderer spriteRenderer;
    private double currentLives;
    private new Rigidbody2D rigidbody2D;
    public static Action<double, Vector2> OnDestroy { get; set; }
    public static Action<double> OnTakeDamage { get; set; }

    public BallSpawnSettings ballSpawnSettings;
    public double InitialLives { get; set; }
    public double CurrentLives
    {
        get => currentLives; set => currentLives = value;
    }

    public int Number { get; set; }
    int type;
    Tween tween;
    private int count;

    private void Awake()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
    }
    private void FixedUpdate()
    {
        if(rigidbody2D.position.y > gameConfig.maxBallPositionY)
        {
            rigidbody2D.AddForce(Vector2.down * gameConfig.ballChangePosForceY, ForceMode2D.Force);
        }
        if(rigidbody2D.angularVelocity > gameConfig.maxBallAngularVelocity)
        {
            rigidbody2D.angularDrag = gameConfig.baseBallAngularDrag + (rigidbody2D.angularVelocity - gameConfig.maxBallAngularVelocity) * gameConfig.ballAngularDragCoef;
        }
    }
    public void Constructor(BallSpawnSettings ballSpawnSettings, int number, bool child = false)
    {
        this.ballSpawnSettings = ballSpawnSettings;
        tween = DOTween.Sequence();
        SetBallType(ballSpawnSettings.type);
        SetLayerOrder(number);
        SetBallLife(ballSpawnSettings.lives);
        SetBallColour();
        if (child) GetComponent<Collider2D>().isTrigger = false;
    }

    private void SetLayerOrder(int number)
    {
        this.Number = number;
        int layerOrder = number * (type+1);
        GetComponentsInChildren<Renderer>().ToList().ForEach(a => a.sortingOrder = layerOrder);
        GetComponentsInChildren<Canvas>().ToList().ForEach(a => a.sortingOrder = layerOrder);
    }

    private void SetBallType(int type)
    {
        this.type = type;
    }

    private void SetBallColour()
    {
        float value = (float)(CurrentLives / (mediumBallLives * 1.1f));
        Color color = gameConfig.ballColorGradient.Evaluate(value);
        spriteRenderer.color = color;
        return;
    }
    private void SetBallLife(double life)
    {
        InitialLives = life;
        CurrentLives = life;
        UpdateLifeText();
    }

    private void UpdateLifeText()
    {
        if (CurrentLives <= 0)
        {
            textMeshProUGUI.text = "";
            return;
        }
        textMeshProUGUI.text = NumberHandler.NumberToTextInOneLine(CurrentLives);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("SideBorder"))
        {
            GetComponent<Collider2D>().isTrigger = false;
        }
    }
    private void OnParticleCollision(GameObject other)
    {
        if (other.layer == 11)
        {
            TakeDamage(CannonManager.Cannon.Damage, DamageSource.bullet);
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 9 && 5 - rigidbody2D.velocity.y>0)
        {
            rigidbody2D.AddForce(new Vector2(
                gameConfig.ballChangePosForceY * 0.3f * Mathf.Min(0.5f, Mathf.Abs(Mathf.Max(0, 3 - rigidbody2D.velocity.x))) * Mathf.Sign(rigidbody2D.velocity.x), 
                gameConfig.ballChangePosForceY * Random.Range(0.8f, 1.1f) * (5 - rigidbody2D.velocity.y)), ForceMode2D.Impulse);
        }

    }
    public void TakeDamage(double damage, DamageSource damageSource)
    {
        if (damage >= CurrentLives)
        {
            damage = CurrentLives;
        }
        OnTakeDamage(damage);
        TaskActiones.Instance.DealDamage(damage);
        CurrentLives -= damage;
        if (currentLives <= 0)
        {
            if (type > 0)
            {
                Shoot(-1);
                Shoot(1);
            }
            TaskActiones.Instance.DefeatEnemies(1);
            if (damageSource == DamageSource.body)
            {
                TaskActiones.Instance.DefeatEnemiesByBody(1);
            }
            else if (damageSource == DamageSource.bullet)
            {
                TaskActiones.Instance.DefeatEnemiesByBullets(1);
            }
            if(type == 0)
            {
                TaskActiones.Instance.DefeatSmallEnemies(1);
            }
            else if(type == 1)
            {
                TaskActiones.Instance.DefeatMediumEnemies(1);
            }
            else if(type == 2)
            {
                TaskActiones.Instance.DefeatBigEnemies(1);
            }
            Destruction();
        }

        UpdateLifeText();
        Vibrate();
        SetBallColour();
    }

    private void Vibrate()
    {
        tween.Complete();
        Vector3 scale = gameObject.transform.localScale;
        tween = DOTween.To(a => gameObject.transform.localScale = scale * gameConfig.ScaleEnemyAnimationCurve.Evaluate(a), 0, 1, gameConfig.timeToDownScaleEnemy)
            .OnComplete(() => gameObject.transform.localScale = scale);
    }

    private void Shoot(int sign)
    {
        GameObject ballGO = CreateABall();
        Rigidbody2D ballRigidbody2D = ballGO.GetComponent<Rigidbody2D>();
        ballRigidbody2D.AddForce(GetShotDirection() * GetForceToShoot(), ForceMode2D.Impulse);
        List<int> rand = HelperClass.GetRandomObjectsFromList(new List<int>() { -1, 1 });
        ballRigidbody2D.AddTorque(rand[0] * Random.Range(gameConfig.torqueAmountMinMax.x, gameConfig.torqueAmountMinMax.y) / 10, ForceMode2D.Impulse);

        float GetForceToShoot()
        {
            return Random.Range(gameConfig.ballForceToShoot.x, gameConfig.ballForceToShoot.y);
        }
        Vector2 GetShotDirection()
        {
            int randomAngle = sign * Random.Range(gameConfig.ballShotAngles.x, gameConfig.ballShotAngles.y);
            Vector3 rotatedVector = HelperClass.VectorFromAngle(randomAngle);
            return rotatedVector;
        }

        GameObject CreateABall()
        {
            GameObject ballGameObject = Instantiate(gameConfig.ballPrefabs[type-1], gameObject.transform.position, Quaternion.identity);
            Ball ball = ballGameObject.GetComponent<Ball>();
            ball.Constructor(new BallSpawnSettings(ballSpawnSettings.type-1, ballSpawnSettings.lives, ballSpawnSettings.spawner, ballSpawnSettings.waiting), Number + count, true);
            count++;
            return ballGameObject;
        }
    }
    public void Destruction(bool withCallback = true)
    {
        if(withCallback) OnDestroy(InitialLives, transform.position);
        SoundManager.Instance.Bubble(0.5f);
        Instantiate(DeathAnimGO, transform.position, Quaternion.identity).transform.localScale = transform.localScale;
        Destroy(gameObject);
    }
}
public enum DamageSource
{
    bullet,
    body
}