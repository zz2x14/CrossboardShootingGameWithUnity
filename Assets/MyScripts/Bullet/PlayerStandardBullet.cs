using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStandardBullet : BulletBase
{
    private TrailRenderer trail;

    private void Awake()
    {
        trail = GetComponentInChildren<TrailRenderer>();
    }

    protected override void OnEnable()
    {
        base.OnEnable();


        if (moveDir != Vector2.right)
        {
            //避免出现意外情况 子弹没有朝向正确的方向(注意：此处旋转的是“子弹头”的位置)
            transform.GetChild(0).rotation = Quaternion.FromToRotation(Vector2.right, moveDir);
        }
    }

    private void OnDisable()
    {
        trail.Clear();//删除 TrailRenderer 中的所有点。 对于从新位置重新开始轨迹非常有用。 - 处理子弹重激活后的拖尾效果
    }

    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        base.OnCollisionEnter2D(collision);

        PlayerEnergy.Instance.ObtainEnergy(PlayerEnergy.HITADDEDVALUE);
    }
}
