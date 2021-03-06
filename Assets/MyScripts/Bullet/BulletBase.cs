using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BulletBase : MonoBehaviour
{
    [SerializeField] protected AudioData[] bulletHitAudioDatas;
    [SerializeField] private float damage;
    [SerializeField] private GameObject hitVFX;
    [SerializeField] protected float flySpeed;
    [SerializeField] protected Vector2 moveDir;

    protected GameObject target;

    protected virtual void OnEnable()
    {
        StartCoroutine(MoveByDirCor());
    }

    IEnumerator MoveByDirCor()
    {
        while (gameObject.activeSelf)
        {
            BulletNormalMove();
            yield return null;
        }
    }

    public void BulletNormalMove()
    {
        transform.Translate(moveDir * flySpeed * Time.deltaTime);
    }

   protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        //使用这样方法的优点（此处）：所有子弹的逻辑可以在这一处处理
        //设置对应的物理碰撞 - 使用TryGetComponent<Character>获得碰撞对象身上的Character脚本
        if (collision.gameObject.TryGetComponent<Character>(out Character target))
        {
            AudioManager.Instance.PlayRandomSFX(bulletHitAudioDatas);

            target.TakenDamage(damage);

            //collision.GetContact(0) - 两者碰撞的第一个点 //碰撞点的法线...不太懂 不过效果确实更好
            PoolManager.Instance.Release(hitVFX, collision.GetContact(0).point, Quaternion.LookRotation(collision.GetContact(0).normal));

            gameObject.SetActive(false);
        }
    }

    public void SetTarget(GameObject target)
    {
        this.target = target;//位置不可以为空 物体可以
    }
}
