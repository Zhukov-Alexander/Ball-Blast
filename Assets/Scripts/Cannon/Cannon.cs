using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using static GameConfigContainer;

public class Cannon : MonoBehaviour
{
    [SerializeField] protected CannonSettings cannonSettings;
    [SerializeField] protected Rigidbody2D cannonRigidbody2D;
    [SerializeField] protected Collider2D auraCollider;
    [SerializeField] SliderScript livesSlider;
    [SerializeField] protected Transform cannonBodyTransform;
    [SerializeField] protected List<ParticleSystem> particleSystems;
    [SerializeField] protected List<GameObject> particleSystemsParents;
    [SerializeField] protected int amountOfBallsSpawnedAtATime = 1;
    [SerializeField] protected float particleSystemGOYMoveAmount = -0.1f;
    [SerializeField] protected float cannonBodyYMoveAmount = -0.05f;
    [SerializeField] protected float chillProportionStart = 0f;
    [SerializeField] protected float chillProportion = 0.5f;
    [SerializeField] protected float centerOfMassYChange = -0.5f;
    [SerializeField] float volume = 1;
    [SerializeField] float pitch = 1;
    protected List<Sequence> sequences;
    private float health;
    Sequence livesTweener;
    List<int> ballIDs;
    float speed;
    float gravityMin;
    float gravityMax;

    public float Damage { get; set; }
    public float MoveForce { get; set; }
    public float Piercing { get; set; }
    public float CurrentHealth
    {
        get => health; set
        {
            livesTweener.Append(livesSlider.TweenSlider(MaximumHealth, health, value, Mathf.Abs(health - value) / MaximumHealth * gameConfig.cannonSliderDurationCoef)).SetEase(Ease.OutQuad);
            health = value;
            if (health <= 0)
            {
                canCollideWithBall = false;
                SoundManager.Instance.Vibrate();
                OnLostAllLives();
            }
        }
    }
    public CannonSettings CannonSettings { get => cannonSettings; }
    public float Armor { get; set; }
    public static Action OnLostAllLives { get; set; }
    public bool CanMove { get; set; }
    internal int PrefabNumber { get; set; }
    public float MaximumHealth { get; set; }
    public float BulletsPerSecond { get; set; }
    public Boss Boss
    {
        get
        {
            if (boss == null)
                boss = FindObjectOfType<Boss>();
            return boss;
        }
    }

    ContactFilter2D contactFilter2DLeftWall;
    ContactFilter2D contactFilter2DRightWall;
    Boss boss;
    private bool canCollideWithBall = true;

    private void Awake()
    {
        speed = particleSystems[0].main.startSpeed.constant;
        gravityMin = particleSystems[0].main.gravityModifier.constantMin;
        gravityMax = particleSystems[0].main.gravityModifier.constantMax;

        contactFilter2DRightWall.layerMask = LayerMask.GetMask("RightWall");
        contactFilter2DRightWall.useLayerMask = true;
        contactFilter2DRightWall.useTriggers = true;
        contactFilter2DLeftWall.layerMask = LayerMask.GetMask("LeftWall");
        contactFilter2DLeftWall.useLayerMask = true;
        contactFilter2DLeftWall.useTriggers = true;

        cannonRigidbody2D.centerOfMass = new Vector2(cannonRigidbody2D.centerOfMass.x, cannonRigidbody2D.centerOfMass.y + centerOfMassYChange);
        UpgradeManager.OnUpgrade += UpdateProperties;
        LevelMenu.AddToStart(StartShooting);
        LevelMenu.AddToEnd(StopShooting);
        LevelMenu.AddToStart(CanMoveTrue);
        LevelMenu.AddToEnd(CanMoveFalse);
        LastChanceMenu.OnLastChanceTaken += UpdateProperties;
        ResultsPanel.OnHide += UpdateProperties;
    }
    private void OnDestroy()
    {
        UpgradeManager.OnUpgrade -= UpdateProperties;
        LevelMenu.RemoveFromStart(StartShooting);
        LevelMenu.RemoveFromEnd(StopShooting);
        LevelMenu.RemoveFromStart(CanMoveTrue);
        LevelMenu.RemoveFromEnd(CanMoveFalse);
        LastChanceMenu.OnLastChanceTaken -= UpdateProperties;
        ResultsPanel.OnHide -= UpdateProperties;
    }
    void CanMoveTrue()
    {
        CanMove = true;
    }
    void CanMoveFalse()
    {
        CanMove = false;
    }
    private void FixedUpdate()
    {
        if (CanMove)
        {
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);
                Vector3 touchInWorld = touch.GetTouchPoint();
                if (cannonRigidbody2D.position.x < touchInWorld.x - gameConfig.moveBuffer)
                {
                    if (!auraCollider.IsTouching(contactFilter2DRightWall))
                        Move(Vector2.right);
                }
                else if (cannonRigidbody2D.position.x > touchInWorld.x + gameConfig.moveBuffer)
                {
                    if (!auraCollider.IsTouching(contactFilter2DLeftWall))
                        Move(Vector2.left);
                }
            }
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                if (!auraCollider.IsTouching(contactFilter2DLeftWall))
                {
                    Move(Vector2.left);
                }
            }
            if (Input.GetKey(KeyCode.RightArrow))
            {
                if (!auraCollider.IsTouching(contactFilter2DRightWall))
                {
                    Move(Vector2.right);
                }
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Ball") && canCollideWithBall && !ballIDs.Contains(collision.GetInstanceID()))
        {
            Debug.Log("collision.ID " + collision.GetInstanceID());
            ballIDs.Add(collision.GetInstanceID());
            collision.enabled = false;
            Ball ball = collision.gameObject.GetComponentInParent<Ball>();
            float damage = ball.CurrentLives;
            ball.TakeDamage(damage);
            if (LevelModManager.CurrentLevelMod == LevelMod.Campain)
            {
                if ((LevelProgression.currentPoints > LevelProgression.maxPoints) || HelperClass.NearlyEqual(LevelProgression.currentPoints, LevelProgression.maxPoints, LevelProgression.maxPoints * gameConfig.epsilon))
                {
                    return;
                }
            }
            TakeDamage(damage);
        }
    }
    private void OnParticleCollision(GameObject other)
    {
        if(other.layer == 18)
        {
            TakeDamage(Boss.Damage);
        }
    }
    void TakeDamage(float damage)
    {
        damage -= Armor;
        if (damage <= 0) damage = 0;
        if (damage > CurrentHealth) damage = CurrentHealth;
        CurrentHealth -= damage;
    }
    public void UpdateProperties()
    {
        ballIDs = new List<int>();
        canCollideWithBall = true;
        SetBulletsPerSecond();
        SetBullletDamage();
        SetMoveForce();
        SetBonusProbability();
        SetHealth();
        SetArmor();
    }
    void SetBulletsPerSecond()
    {
        float multiplyer = CannonSettings.bulletsPerSecondMultiplyer * BackgroundManager.Background.backgroundSettings.bulletsPerSecondMultiplyer;
        BulletsPerSecond = gameConfig.bulletsPerSecond * multiplyer * Progression.GetBulletsPerSecondProgression();
        foreach (var item in particleSystems)
        {
            ParticleSystem.MainModule mainModule = item.main;
            mainModule.startSpeed = speed * multiplyer * Progression.GetBulletsSpeedProgression();
            ParticleSystem.MinMaxCurve minMax = mainModule.gravityModifier;
            minMax.constantMin = gravityMin * multiplyer * Progression.GetBulletsSpeedProgression();
            minMax.constantMax = gravityMax * multiplyer * Progression.GetBulletsSpeedProgression();
        }
    }

    void SetBullletDamage()
    {
        float multiplyer = CannonSettings.bulletsDamageMultiplyer * BackgroundManager.Background.backgroundSettings.bulletsDamageMultiplyer;
        Damage = gameConfig.bulletDamage * multiplyer * Progression.GetStatAmountProgression(SavedValues.Instance.BulletDamageUpgradeLevel);
    }

    void SetMoveForce()
    {
        float multiplyer = CannonSettings.cannonMoveForceMultiplyer * BackgroundManager.Background.backgroundSettings.cannonMoveForceMultiplyer;
        MoveForce = gameConfig.cannonMoveForce * multiplyer * Progression.GetCannonMoveForceProgression();
    }

    void SetBonusProbability()
    {
        float multiplyer = CannonSettings.bonusProbabilityMultiplyer * BackgroundManager.Background.backgroundSettings.bonusProbabilityMultiplyer;
        Piercing = gameConfig.bonusProbability * multiplyer * Progression.GetBonusProbabilityProgression();
    }
    void SetHealth()
    {
        float multiplyer = CannonSettings.healthMultiplyer * BackgroundManager.Background.backgroundSettings.healthMultiplyer;
        MaximumHealth = gameConfig.health * multiplyer * Progression.GetStatAmountProgression(SavedValues.Instance.CannonHealthUpgradeLevel);
        CurrentHealth = MaximumHealth;
    }
    void SetArmor()
    {
        float multiplyer = CannonSettings.armorMultiplyer * BackgroundManager.Background.backgroundSettings.armorMultiplyer;
        Armor = gameConfig.armor * multiplyer * Progression.GetStatAmountProgression(SavedValues.Instance.CannonArmorUpgradeLevel);
    }

    void Move(Vector2 direction)
    {
        cannonRigidbody2D.AddForce(direction * MoveForce, ForceMode2D.Force);
    }
    public void StartShooting()
    {
        StartCoroutine(Shooting());
    }
    public virtual void StopShooting()
    {
        sequences.ForEach(a => a.onStepComplete += () => a.Kill());
    }
    protected virtual IEnumerator Shooting()
    {
        float timeBetweenShots = 1 / BulletsPerSecond * amountOfBallsSpawnedAtATime;
        sequences = new List<Sequence>();
        for (int i = 0; i < particleSystems.Count; i++)
        {
            sequences.Add(GetParticleSystemSequence(particleSystemsParents[i], particleSystems[i], timeBetweenShots * particleSystems.Count).SetLoops(-1).Play());
            yield return new WaitForSeconds(timeBetweenShots);
        }
    }
    protected virtual Sequence GetParticleSystemSequence(GameObject particleSystemGO, ParticleSystem particleSystem, float timeBetweenShots)
    {
        float particleSystem1GOPosY = particleSystemGO.transform.localPosition.y;
        float chillTimeStart = chillProportionStart * timeBetweenShots;
        float chillTimeEnd = chillProportion * timeBetweenShots;
        float moveTime = (timeBetweenShots - chillTimeEnd - chillTimeStart) / 2;
        Sequence particleSystem1Sequence = DOTween.Sequence();
        Tween particleSystem1TweenDown = particleSystemGO.transform.DOLocalMoveY(particleSystem1GOPosY + particleSystemGOYMoveAmount, moveTime).SetEase(Ease.OutCubic);
        Tween particleSystem1TweenUp = particleSystemGO.transform.DOLocalMoveY(particleSystem1GOPosY, moveTime).SetEase(Ease.OutCubic);

        float bodyPosY = cannonBodyTransform.localPosition.y;
        Sequence bodySequence = DOTween.Sequence();
        Tween bodyTweenDown = cannonBodyTransform.DOLocalMoveY(bodyPosY + cannonBodyYMoveAmount, moveTime / 2).SetEase(Ease.OutSine);
        Tween bodyTweenUp = cannonBodyTransform.DOLocalMoveY(bodyPosY, moveTime / 2).SetEase(Ease.InSine);
        bodySequence.Append(bodyTweenDown).Append(bodyTweenUp);

        ParticleSystem.MainModule mainModule = particleSystem.main;

        particleSystem1Sequence.AppendInterval(chillTimeStart);
        particleSystem1Sequence.AppendCallback(() => particleSystem.Emit(amountOfBallsSpawnedAtATime));
        particleSystem1Sequence.AppendCallback(() => SoundManager.Instance.Gun(volume, pitch));
        particleSystem1Sequence.Append(particleSystem1TweenDown);
        particleSystem1Sequence.Join(bodySequence);
        particleSystem1Sequence.AppendInterval(chillTimeEnd);
        particleSystem1Sequence.Append(particleSystem1TweenUp);
        return particleSystem1Sequence;
    }
}
