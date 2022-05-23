using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BulletBase : MonoBehaviour
{
    [SerializeField] protected AudioData[] bulletHitAudioDatas;
    [SerializeField] private float damage;
    [SerializeField] private GameObject hitVFX;
    [SerializeField] private float flySpeed;
    [SerializeField] protected Vector2 moveDir;

    protected Transform target;

    protected virtual void OnEnable()
    {
        StartCoroutine(MoveByDirCor());
    }

    IEnumerator MoveByDirCor()
    {
        while (gameObject.activeSelf)
        {
            transform.Translate(moveDir * flySpeed *Time.deltaTime);
            yield return null;
        }
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
}
