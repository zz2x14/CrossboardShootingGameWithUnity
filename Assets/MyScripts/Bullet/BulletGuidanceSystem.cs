using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletGuidanceSystem : MonoBehaviour
{
    [SerializeField] private BulletBase bullet;
    [SerializeField] private float minBallisticAngle;
    [SerializeField] private float maxBallisticAngle;

    private float ballisticAngle;
        
    private Vector3 targetDir;

    public IEnumerator HomingCor(GameObject target)
    {
        ballisticAngle = Random.Range(minBallisticAngle, maxBallisticAngle); 

        while (gameObject.activeSelf)
        {
            if (target.activeSelf)
            {
                //����Ŀ����ӵ�֮��ĽǶ� ʹ�ӵ�Z��ʼ�ճ���Ŀ�� - ׷���ӵ�����Ҫ��Ч�����ӽ�Ч��
                targetDir = target.transform.position - transform.position;
                transform.rotation = Quaternion.AngleAxis(Mathf.Atan2(targetDir.y, targetDir.x) * Mathf.Rad2Deg, Vector3.forward);
                transform.rotation *= Quaternion.Euler(0, 0, ballisticAngle);//���ӵ�Z�������ת���� - �ﵽ�Ӿ��ϵĻ���Ч��
                //ע�⣺Quaternion.Euler ��Ҫ*=��֤��ȷ�ĳ�����ת����ֻ�ǵ��ھ�ֻΪ��ת��ĳ���ǶȺ�̶�������
                bullet.BulletNormalMove();
            }
            else
            {
                bullet.BulletNormalMove();
            }

            yield return null;
        }
    }
    
}
