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
    [SerializeField] TextMeshProUGUI textMeshProUGUI;
    [SerializeField] SpriteRenderer spriteRenderer;
    private float currentLives;
    private new Rigidbody2D rigidbody2D;
    public static Action<float,Vector2> OnDestroy { get; set; }
    public static Action<float> OnTakeDamage { get; set; }

    public float InitialLives { get; set; }
    public float CurrentLives
    {
        get => currentLives; set
        {
            currentLives = value;
            if (currentLives <= 0)
            {
                if (type > 0)
                {
                    Shoot(-1);
                    Shoot(1);
                }
                Destruction();
            }
        }
    }
    int type;
    int number;
    Tween tween;
    Vector3 scale;
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
    public void Constructor(float life, int type, int number, bool child = false)
    {
        scale = gameObject.transform.localScale;
        tween = DOTween.Sequence();
        SetBallType(type);
        SetLayerOrder(number);
        SetBallLife(life);
        SetBallColour();
        //SetBallSize();
        if (child) GetComponent<Collider2D>().isTrigger = false;
    }

    /*private void SetBallSize()
    {
        float lifeMultiplyer = CurrentLives / (mediumBallLives * 2);
        if (lifeMultiplyer > 1) lifeMultiplyer = 1;
        float scale = gameConfig.minBallScale + (gameConfig.maxBallScale - gameConfig.minBallScale) * lifeMultiplyer;
        DOTween.Complete(gameObject);
        gameObject.transform.DOScale(new Vector3(scale, scale, scale), gameConfig.timeToDownScaleEnemy).SetEase(gameConfig.ScaleEnemyAnimationCurve);
    }*/

    private void SetLayerOrder(int number)
    {
        this.number = number;
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
        float value = CurrentLives / (mediumBallLives * 1.1f);
        Color color = gameConfig.ballColorGradient.Evaluate(value);
        spriteRenderer.color = color;
        return;
    }
    private void SetBallLife(float life)
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
        textMeshProUGUI.text = NumberHandler.NumberToTextInOneLineWithoutFraction(CurrentLives);
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
            TakeDamage(CannonManager.Cannon.Damage);
        }
    }
    private void OnParticleTrigger()
    {
        
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
    public void TakeDamage(float damage)
    {
        if (damage > CurrentLives) damage = CurrentLives;
        if (LevelModManager.CurrentLevelMod == LevelMod.Campain)
        {
            OnTakeDamage(damage);
        }
        CurrentLives -= damage;
        UpdateLifeText();
        //tween.Complete();
        //transform.localScale = scale;
        tween.Kill();
        tween = DOTween.To(a => gameObject.transform.localScale = scale * gameConfig.ScaleEnemyAnimationCurve.Evaluate(a), 0, 1, gameConfig.timeToDownScaleEnemy);
        SetBallColour();
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
            ball.Constructor(InitialLives, type - 1, number + count, true);
            count++;
            return ballGameObject;
        }
    }
    public void Destruction()
    {
        SoundManager.Instance.Bubble(0.5f);
        OnDestroy(InitialLives, transform.position);
        Destroy(gameObject);
        /*GetComponentsInChildren<Collider2D>().ToList().ForEach(a => a.enabled = false);
        transform.DOScale(Vector3.zero, 0.2f).OnComplete(() => Destroy(gameObject)).SetEase(Ease.InBack);*/
    }
}
