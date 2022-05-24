using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerOverdriveBullet : PlayerStandardBullet
{
    [SerializeField] private BulletGuidanceSystem bulletGuidanceSystem;

    protected override void OnEnable()
    {
        SetTarget(EnemyManager.Instance.RandomEnemy);

        transform.rotation = Quaternion.identity;//激活时重置旋转值 - 避免追踪敌人后再开启角度出现混乱

       if(target == null)
       {
           base.OnEnable();//如果不存在敌人了 子弹正常运行
       }
       else
       {
           StartCoroutine(bulletGuidanceSystem.HomingCor(target));
       }
    }

}
