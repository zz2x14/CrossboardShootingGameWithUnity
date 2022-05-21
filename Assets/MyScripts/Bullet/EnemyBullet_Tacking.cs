using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet_Tacking : BulletBase
{
    private void Awake()
    {
        target = FindObjectOfType<PlayerController>().transform;
    }

    protected override void OnEnable()
    {
        if (target.gameObject.activeSelf)
        {
            StartCoroutine(nameof(GetAccurateMoveDirCor));
        }
        base.OnEnable();
    }

    //��Ϊfloat�Ĳ���ȷ���ȵ�һ֡��ø���׼ȷ��λ�ö��õ�����
    IEnumerator GetAccurateMoveDirCor()
    {
        yield return null;

        moveDir = (target.transform.position - transform.position).normalized;
    }

}
