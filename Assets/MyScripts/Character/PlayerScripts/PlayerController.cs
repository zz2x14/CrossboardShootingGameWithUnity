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

    [Header("�������")]
    [SerializeField] private PlayerInput playerInput;

    [Header("��ͨ���")]
    [SerializeField] private GameObject standardBulletPrefab;
    [SerializeField] private GameObject standardMoveOffsetUpperBulletPrefab;
    [SerializeField] private GameObject standardMoveOffsetLowerBulletPrefab;
    [SerializeField] private Transform muzzleTopPoint;
    [SerializeField] private Transform muzzleMiddlePoint;
    [SerializeField] private Transform muzzleBottomPoint;
    [SerializeField,Range(0,3)] private int powerLevel;
    [SerializeField] private float fireInterval;

    [Header("����")]
    [SerializeField] private int dodgeEnergyCost;
    [SerializeField] private float maxRollAngle;
    [SerializeField] private float dodgeRollSpeed;
    [SerializeField] private Vector3 dodgeScale;
    private float curDodgeAngle;
    private bool isDodging = false;
    private float  dodgeDuration;

    private Collider2D coll2D;

    private WaitForSeconds fireWaitForSeconds;
    private WaitForSeconds healthRegenerateWFS;

    private Coroutine startHealthRegenerateCor;

    private void Start()
    {
        coll2D = GetComponent<Collider2D>();

        playerInput.EnableGameplayInput();

        fireWaitForSeconds = new WaitForSeconds(fireInterval);//������ѭ���������µı���
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
    }

    private void OnDisable()
    {
        playerInput.OnStartFire -= Fire;
        playerInput.OnStopFire -= StopFire;
        playerInput.OnPlayerDodge -= Dodge;
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
                StopCoroutine(startHealthRegenerateCor);//�ڻظ�Ѫ���ڼ��ٴ��ܵ�ʱ����ֹͣ�ظ�Ѫ��Э���ٿ���
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

    private void Dodge()
    {
        if (!PlayerEnergy.Instance.IsEnergyEnough(dodgeEnergyCost) || isDodging) return;

        StartCoroutine(nameof(DodgeCor));//���濴�Լ��Ĵ��룡
    }

    IEnumerator DodgeCor()
    {
        isDodging = true;
        coll2D.isTrigger = true;

        PlayerEnergy.Instance.UseEnergy(dodgeEnergyCost);

        curDodgeAngle = 0;

        float t = 0;
        float t1 = 0; 

        while(curDodgeAngle < maxRollAngle)
        {
            curDodgeAngle += dodgeRollSpeed * Time.deltaTime;
            transform.rotation = Quaternion.AngleAxis(curDodgeAngle, Vector3.right);

            if(curDodgeAngle < maxRollAngle / 2)//��ת����һ��Ϊ�ֽ���
            {
                t += 1 / dodgeDuration * Time.deltaTime;//������תʱ�����1s��t ���Ӵﵽ�ó���ʱ��ĵ�λ(ÿ1s)������ - ��Ϊ��ǰ����ֵ��Ŀ������ֵ������ʱ���ֵ����
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

}
