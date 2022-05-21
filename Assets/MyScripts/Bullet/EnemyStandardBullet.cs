using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStandardBullet : BulletBase
{
    protected override void OnEnable()
    {
        base.OnEnable();

        if (moveDir != Vector2.left)
        {
            transform.rotation = Quaternion.FromToRotation(Vector2.left, moveDir); 
        }
    }

}
