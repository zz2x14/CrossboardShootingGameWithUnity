using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerOverdriveBullet : PlayerStandardBullet
{
    [SerializeField] private BulletGuidanceSystem bulletGuidanceSystem;

    protected override void OnEnable()
    {
        SetTarget(EnemyManager.Instance.RandomEnemy);

        transform.rotation = Quaternion.identity;//����ʱ������תֵ - ����׷�ٵ��˺��ٿ����Ƕȳ��ֻ���

       if(target == null)
       {
           base.OnEnable();//��������ڵ����� �ӵ���������
       }
       else
       {
           StartCoroutine(bulletGuidanceSystem.HomingCor(target));
       }
    }

}
