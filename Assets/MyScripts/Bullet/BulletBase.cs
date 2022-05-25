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
        //ʹ�������������ŵ㣨�˴����������ӵ����߼���������һ������
        //���ö�Ӧ��������ײ - ʹ��TryGetComponent<Character>�����ײ�������ϵ�Character�ű�
        if (collision.gameObject.TryGetComponent<Character>(out Character target))
        {
            AudioManager.Instance.PlayRandomSFX(bulletHitAudioDatas);

            target.TakenDamage(damage);

            //collision.GetContact(0) - ������ײ�ĵ�һ���� //��ײ��ķ���...��̫�� ����Ч��ȷʵ����
            PoolManager.Instance.Release(hitVFX, collision.GetContact(0).point, Quaternion.LookRotation(collision.GetContact(0).normal));

            gameObject.SetActive(false);
        }
    }

    public void SetTarget(GameObject target)
    {
        this.target = target;//λ�ò�����Ϊ�� �������
    }
}
