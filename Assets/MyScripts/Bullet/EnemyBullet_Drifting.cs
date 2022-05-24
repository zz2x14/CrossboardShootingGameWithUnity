using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet_Drifting : BulletBase
{
    private void Awake()
    {
        target = FindObjectOfType<PlayerController>().gameObject;
    }

    protected override void OnEnable()
    {
        if (target.gameObject.activeSelf)
        {
            StartCoroutine(nameof(GetAccurateMoveDirCor));
        }
        base.OnEnable();
    }

    //因为float的不精确，等到一帧获得更加准确的位置而得到方向
    IEnumerator GetAccurateMoveDirCor()
    {
        yield return null;

        moveDir = (target.transform.position - transform.position).normalized;
    }

}
