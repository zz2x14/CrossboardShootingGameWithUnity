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

    private WaitForSeconds fireWaitForSeconds;
    private WaitForSeconds healthRegenerateWFS;

    private Coroutine startHealthRegenerateCor;

    private void Start()
    {
        playerInput.EnableGameplayInput();

        fireWaitForSeconds = new WaitForSeconds(fireInterval);//避免在循环内声明新的变量
        healthRegenerateWFS = new WaitForSeconds(healthRegenerateInterval);

        statusBar_HUD.InitializeFillAmount(curHealth,maxHealth);
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        playerInput.OnStartFire += Fire;
        playerInput.OnStopFire += StopFire;
    }

    private void OnDisable()
    {
        playerInput.OnStartFire -= Fire;
        playerInput.OnStopFire -= StopFire;
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
                    PoolManager.Instance.Release(standardBulletPrefab, muzzleMiddlePoint.position, muzzleMiddlePoint.rotation);
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
            
            yield return fireWaitForSeconds;
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
    }

}
