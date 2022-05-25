using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : Character
{
    [SerializeField] private bool enableRegenerate = true;
    [SerializeField] private float healthRegenerateInterval;
    [SerializeField,Range(0,0.1f)] private float healthRegeneratePercent;
    [SerializeField] private StatusBar_HUD statusBar_HUD;

    [Header("玩家输入")]
    [SerializeField] private PlayerInput playerInput;

    [Header("普通射击")]
    [SerializeField] private GameObject standardBulletPrefab;
    [SerializeField] private GameObject standardMoveOffsetUpperBulletPrefab;
    [SerializeField] private GameObject standardMoveOffsetLowerBulletPrefab;
    [SerializeField] private Transform muzzleTopPoint;
    [SerializeField] private Transform muzzleMiddlePoint;
    [SerializeField] private Transform muzzleBottomPoint;
    [SerializeField,Range(0,3)] private int powerLevel;
    [SerializeField] private float fireInterval;
    [SerializeField] private AudioData shootAudioData;

    [Header("闪避")]
    [SerializeField] private float dodgeEnergyCost;
    [SerializeField] private float maxRollAngle;
    [SerializeField] private float dodgeRollSpeed;
    [SerializeField] private Vector3 dodgeScale;
    [SerializeField] private AudioData dodgeAudioData;

    private float curDodgeAngle;
    private bool isDodging = false;
    private float dodgeDuration;

    [Header("能量爆发")]
    [SerializeField] private GameObject overdriveBullet;
    [SerializeField] private float overdriveDodgeChangeRate;
    [SerializeField] private float overdriveShootIntervalChangeRate;
    [SerializeField] private float overdriveMoveSpeedChangeRate;
    [SerializeField] private float overdriveBulletTimeDuration;

    [Header("导弹攻击")]
    [SerializeField] private Transform missileMuzzlePoint;

    private bool isOverdriving;

    private Collider2D coll2D;
    private PlayerMovement playerMovement;
    private PlayerMissileSystem playerMissileSystem;

    private WaitForSeconds fireWaitForSeconds;
    private WaitForSeconds fireInOverdriveOnWFS;
    private WaitForSeconds healthRegenerateWFS;
    
    private Coroutine startHealthRegenerateCor;

    private void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
        playerMissileSystem = GetComponent<PlayerMissileSystem>(); 
        coll2D = GetComponent<Collider2D>();

        playerInput.EnableGameplayInput();

        fireWaitForSeconds = new WaitForSeconds(fireInterval);//避免在循环内声明新的变量
        fireInOverdriveOnWFS = new WaitForSeconds(fireInterval / overdriveShootIntervalChangeRate);
        healthRegenerateWFS = new WaitForSeconds(healthRegenerateInterval);

        statusBar_HUD.InitializeFillAmount(curHealth,maxHealth);

        dodgeDuration = maxRollAngle / dodgeRollSpeed;
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        playerInput.OnStartFire += Fire;
        playerInput.OnStopFire += StopFire;
        playerInput.OnPlayerDodge += Dodge;
        playerInput.OnPlayerOverdrive += Overdrive;
        playerInput.OnLaunch += LauchMissile;

        PlayerOverdrive.On += OverdriveOn;
        PlayerOverdrive.Off += OverdriveOff;
    }

    private void OnDisable()
    {
        playerInput.OnStartFire -= Fire;
        playerInput.OnStopFire -= StopFire;
        playerInput.OnPlayerDodge -= Dodge;
        playerInput.OnPlayerOverdrive -= Overdrive;
        playerInput.OnLaunch -= LauchMissile;

        PlayerOverdrive.On -= OverdriveOn;
        PlayerOverdrive.Off -= OverdriveOff;
    }

    private void Fire()
    {
        StartCoroutine(nameof(StartFireCor));
    }

    private void StopFire()
    {
        StopCoroutine(nameof(StartFireCor));
    }

    IEnumerator StartFireCor()
    {
        while (true)
        {
            switch (powerLevel)
            {
                case 0:
                    PoolManager.Instance.Release(isOverdriving ? overdriveBullet : standardBulletPrefab, 
                        muzzleMiddlePoint.position, muzzleMiddlePoint.rotation);
                    break;
                case 1:
                    PoolManager.Instance.Release(standardBulletPrefab, muzzleTopPoint.position, muzzleTopPoint.rotation);
                    PoolManager.Instance.Release(standardBulletPrefab, muzzleBottomPoint.position, muzzleBottomPoint.rotation);
                    break;
                case 2:
                    PoolManager.Instance.Release(standardMoveOffsetUpperBulletPrefab, muzzleTopPoint.position, muzzleTopPoint.rotation);
                    PoolManager.Instance.Release(standardBulletPrefab, muzzleMiddlePoint.position, muzzleMiddlePoint.rotation);
                    PoolManager.Instance.Release(standardMoveOffsetLowerBulletPrefab, muzzleBottomPoint.position, muzzleBottomPoint.rotation);
                    break;
                default:
                    break;
            }

            AudioManager.Instance.PlayRandomSFX(shootAudioData);

            yield return isOverdriving ? fireInOverdriveOnWFS : fireWaitForSeconds;
        }
    }

    public override void TakenDamage(float damageValue)
    {
        base.TakenDamage(damageValue);

        if (gameObject.activeSelf && curHealth > 0 && enableRegenerate)
        {
            if(startHealthRegenerateCor != null)
            {
                StopCoroutine(startHealthRegenerateCor);//在回复血量期间再次受到时，先停止回复血量协程再开启
            }

            startHealthRegenerateCor = StartCoroutine(HealthRegenerateCor(healthRegenerateWFS, healthRegeneratePercent));
        }

        statusBar_HUD.UpdateFillAmount(curHealth,maxHealth);
    }

    protected override void RestoreHealth(float value)
    {
        base.RestoreHealth(value);

        statusBar_HUD.UpdateFillAmount(curHealth, maxHealth);
    }

    protected override void Die()
    {
        statusBar_HUD.UpdateFillAmount(0f,maxHealth);
        
        base.Die();

        GameManager.Instance.GameState = GameState.GameOver;//死亡游戏结束
    }

    private void Dodge()
    {
        if (!PlayerEnergy.Instance.IsEnergyEnough(dodgeEnergyCost) || isDodging) return;

        StartCoroutine(nameof(DodgeCor));//认真看自己的代码！
    }

    IEnumerator DodgeCor()
    {
        isDodging = true;
        coll2D.isTrigger = true;

        PlayerEnergy.Instance.UseEnergy(dodgeEnergyCost);
        AudioManager.Instance.PlaySFX(dodgeAudioData);

        curDodgeAngle = 0;

        float t = 0;
        float t1 = 0; 

        while(curDodgeAngle < maxRollAngle)
        {
            curDodgeAngle += dodgeRollSpeed * Time.deltaTime;
            transform.rotation = Quaternion.AngleAxis(curDodgeAngle, Vector3.right);

            if(curDodgeAngle < maxRollAngle / 2)//旋转进度一半为分界线
            {
                t += 1 / dodgeDuration * Time.deltaTime;//闪避旋转时间持续1s，t 增加达到该持续时间的单位(每1s)增加量 - 作为当前缩放值到目标缩放值的线性时间插值参数
                transform.localScale = Vector3.Lerp(transform.localScale, dodgeScale, t);
            }
            else
            {
                t1 += 1/dodgeDuration* Time.deltaTime;
                transform.localScale = Vector3.Lerp(transform.localScale, Vector3.one, t1);
            }

            yield return null;
        }

        coll2D.isTrigger = false;
        isDodging = false;
    }

    private void Overdrive()
    {
        if (!PlayerEnergy.Instance.IsEnergyEnough(PlayerEnergy.MAXENERGY)) return;

        PlayerOverdrive.On.Invoke();
    }

    private void OverdriveOn()
    {
        isOverdriving = true;
        dodgeEnergyCost *= overdriveDodgeChangeRate;
        playerMovement.MoveSpeed *= overdriveMoveSpeedChangeRate;

        TimeController.Instance.StartBulletTime(overdriveBulletTimeDuration,overdriveBulletTimeDuration);
    }

    private void OverdriveOff()
    {
        isOverdriving = false;
        dodgeEnergyCost /= overdriveDodgeChangeRate;
        playerMovement.MoveSpeed /= overdriveMoveSpeedChangeRate;
    }

    private void LauchMissile()
    {
        playerMissileSystem.LauchMissile(missileMuzzlePoint) ;
    }

}
