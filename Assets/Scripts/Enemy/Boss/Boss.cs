using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using static GameConfigContainer;
using Random = UnityEngine.Random;

public class Boss : MonoBehaviour
{
    [SerializeField] ParticleSystem leftWingPS;
    [SerializeField] ParticleSystem rightWingPS;
    [SerializeField] int amountOfWingParticlesSpawnedAtATime;

    [SerializeField] ParticleSystem upPS;
    [SerializeField] int amountOfUpParticlesSpawnedAtATime;

    [SerializeField] ParticleSystem downPS;
    [SerializeField] int amountOfDownParticlesSpawnedAtATime;

    [SerializeField] BossSettings bossSettings;
    [SerializeField] BoundBox boundBox;
    [SerializeField] float angleToTurn;
    [SerializeField] Vector2 moveAmountMinMax;
    [SerializeField] float probabilityToPassTurn;
    [SerializeField] float probabilityToMoveToTarget = 0.5f;
    [SerializeField] float moveDecreseMultiplier;
    Rigidbody2D bossRB2D;
    Cannon cannon;
    float currentLives;
    Vector2 previousPosition;

    public float CurrentLives
    {
        get => currentLives; set
        {
            currentLives = value;
            if (currentLives <= 0)
            {
                Destruction();
            }
        }
    }

    public float InitialLives { get; set; }
    public float Damage { get; set; }
    public float Armor { get; set; }

    public static Action<float> OnTakeDamage { get; set; }
    public static Action<float, Vector2> OnDestroy { get; set; }
    public BossSettings BossSettings { get => bossSettings; set => bossSettings = value; }

    private void OnDrawGizmos()
    {
        Vector3 leftDown = new Vector2(boundBox.left, boundBox.up);
        Vector3 leftUp = new Vector2(boundBox.left, boundBox.down);
        Vector3 rightDown = new Vector2(boundBox.right, boundBox.up);
        Vector3 rightUp = new Vector2(boundBox.right, boundBox.down);
        Gizmos.DrawLine(leftDown, leftUp);
        Gizmos.DrawLine(rightDown, rightUp);
        Gizmos.DrawLine(leftUp, rightUp);
        Gizmos.DrawLine(leftDown, rightDown);
    }
    private void Awake()
    {
        Constractor();
        Moving();
    }
    public void Constractor()
    {
        bossRB2D = GetComponent<Rigidbody2D>();
        cannon = FindObjectOfType<Cannon>();
        float multiplyer = Progression.GetBossfightProgression();
        InitialLives = gameConfig.bossLives * BossSettings.livesCoef * multiplyer;
        CurrentLives = InitialLives;
        Damage = gameConfig.bossDamage * BossSettings.damageCoef * multiplyer;
        Armor = gameConfig.bossArmor * BossSettings.armorCoef * multiplyer;
    }
    private void OnParticleCollision(GameObject other)
    {
        if (other.layer == 11)
        {
            TakeDamage(cannon.Damage);
        }
    }
    public void TakeDamage(float damage)
    {
        damage -= Armor;
        if (damage < 0) damage = 0;
        if (damage > CurrentLives) damage = CurrentLives;
        CurrentLives -= damage;
        OnTakeDamage.Invoke(damage);
    }
    private void Destruction()
    {
        SoundManager.Instance.Bubble(0.7f);
        OnDestroy(InitialLives, transform.position);
        Destroy(gameObject);
    }
    public void ShootWingParticles()
    {
        leftWingPS.Emit(amountOfWingParticlesSpawnedAtATime);
        rightWingPS.Emit(amountOfWingParticlesSpawnedAtATime);
    }
    public void ShootDownParticles()
    {
        downPS.Emit(amountOfDownParticlesSpawnedAtATime);
    }
    public void ShootUpParticles()
    {
        upPS.Emit(amountOfUpParticlesSpawnedAtATime);
    }
    void Moving()
    {
        Vector2 point = GetPointToMove();
        float duration = (point - bossRB2D.position).magnitude / (gameConfig.bossSpeed * BossSettings.speedCoef);
        bossRB2D.DOMove(point, duration).SetEase(Ease.Linear).OnComplete(() => Moving());
    }
    Vector2 GetPointToMove()
    {
        if (previousPosition == Vector2.zero)
        {
            previousPosition = bossRB2D.position;
            float x = Random.Range(boundBox.left, boundBox.right);
            float y = Random.Range(boundBox.down, boundBox.up);
            return new Vector2(x, y);
        }
        else
        {
            float moveAmount = Random.Range(this.moveAmountMinMax.x, this.moveAmountMinMax.y);
            Vector2 currentPosition = bossRB2D.position;
            Vector2 initialVector = (currentPosition - previousPosition).normalized;
            List<Vector2> rotatedPositions = GetRotatedPositions(moveAmount, initialVector, currentPosition);
            List<KeyValuePair<Vector2, Vector2>> positionPairs = new List<KeyValuePair<Vector2, Vector2>>();
            for (int i = 0; i < rotatedPositions.Count/2; i++)
            {
                positionPairs.Add(new KeyValuePair<Vector2, Vector2>(rotatedPositions[i], rotatedPositions[rotatedPositions.Count - 1 - i]));
            }
            foreach (var item in positionPairs)
            {
                float rand = Random.value;
                if (rand > probabilityToPassTurn)
                {
                    if (CompareXDistance(item.Key, item.Value, cannon.transform.position))
                    {
                        if (Random.value < probabilityToMoveToTarget)
                            return item.Key;
                        else
                            return item.Value;
                    }
                    else
                    {
                        if (Random.value < probabilityToMoveToTarget)
                            return item.Value;
                        else
                            return item.Key;
                    }
                }
            }
            return new Vector2();
        }
    }
    bool CompareXDistance(Vector2 first, Vector2 second, Vector2 compared)
    {
        if(Mathf.Abs(first.x-compared.x) < Mathf.Abs(second.x - compared.x))
        {
            return true;
        }
        return false;
    }
    List<Vector2> GetRotatedPositions(float moveAmount, Vector2 initialVector, Vector2 currentPosition)
    {

            List<Vector2> rotatedPositions = new List<Vector2>();
            for (int i = 0; i < 360/angleToTurn; i++)
            {
                Vector2 rotatedVector = HelperClass.RotateBy(initialVector, angleToTurn * i);
                Vector2 endPosition = currentPosition + rotatedVector * moveAmount;
                if (CheckIfInsideBoundBox(endPosition))
                {
                    rotatedPositions.Add(endPosition);
                }
            }
            if(rotatedPositions.Count == 0)
            {
                if(moveAmount < 0.1)
                {
                    rotatedPositions.Add(new Vector2());
                }
                else
                    rotatedPositions = GetRotatedPositions(moveAmount * moveDecreseMultiplier, initialVector, currentPosition);
            }
            return rotatedPositions;
    }
    bool CheckIfInsideBoundBox(Vector2 point)
    {
        if(point.x>boundBox.left && point.x<boundBox.right && point.y>boundBox.down && point.y<boundBox.up)
        {
            return true;
        }
        return false;
    }
}
[Serializable] public class BoundBox
{
    public float left;
    public float right;
    public float up;
    public float down;
}
