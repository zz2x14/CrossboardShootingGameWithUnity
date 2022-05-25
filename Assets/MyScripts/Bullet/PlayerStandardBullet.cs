using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStandardBullet : BulletBase
{
    private TrailRenderer trail;

    protected virtual void Awake()
    {
        trail = GetComponentInChildren<TrailRenderer>();
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        if (moveDir != Vector2.right)
        {
            //�������������� �ӵ�û�г�����ȷ�ķ���(ע�⣺�˴���ת���ǡ��ӵ�ͷ����λ��)
            transform.GetChild(0).rotation = Quaternion.FromToRotation(Vector2.right, moveDir);
        }
    }

    private void OnDisable()
    {
        trail.Clear();//ɾ�� TrailRenderer �е����е㡣 ���ڴ���λ�����¿�ʼ�켣�ǳ����á� - �����ӵ��ؼ�������βЧ��
    }

    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        PlayerEnergy.Instance.ObtainEnergy(PlayerEnergy.ENERGYADDEDVALUE);

        base.OnCollisionEnter2D(collision);
    }

    
}
