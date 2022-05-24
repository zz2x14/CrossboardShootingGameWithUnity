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
                //计算目标和子弹之间的角度 使子弹Z轴始终朝向目标 - 追踪子弹所需要的效果和视角效果
                targetDir = target.transform.position - transform.position;
                transform.rotation = Quaternion.AngleAxis(Mathf.Atan2(targetDir.y, targetDir.x) * Mathf.Rad2Deg, Vector3.forward);
                transform.rotation *= Quaternion.Euler(0, 0, ballisticAngle);//让子弹Z轴随机旋转度数 - 达到视觉上的弧度效果
                //注意：Quaternion.Euler 需要*=保证正确的持续旋转，若只是等于就只为旋转到某个角度后固定不动了
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
