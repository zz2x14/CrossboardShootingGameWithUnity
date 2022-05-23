using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [Header("移动")]
    [SerializeField] private float paddingX;
    [SerializeField] private float paddingY;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float moveRotateAngle;

    [Header("攻击")]
    [SerializeField] private GameObject[] bullets;
    [SerializeField] private AudioData shootAudioData;
    [SerializeField] private Transform muzzlePoint;
    [SerializeField] private float minFireIntervel;
    [SerializeField] private float maxFireIntervel;

    private void OnEnable()
    {
        StartCoroutine(nameof(MoveRandomlyCor));
        StartCoroutine(nameof(FireRandomlyCor));
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    IEnumerator MoveRandomlyCor()
    {
        transform.position = ViewportTool.Instance.GetRandomEnemySpawnPos(paddingX, paddingY);

        Vector3 nextTargetPoint = ViewportTool.Instance.GetEnemyRandomMovePosInHalfView(paddingX, paddingY);

        while (gameObject.activeSelf)
        {
            //此处对应修改为 moveSpeed * Time.fixedDeltaTime - 意味着两点之间的距离 大于 单位固定每帧的移动速度所能够移动的距离
            if (Vector3.Distance(transform.position, nextTargetPoint) > Mathf.Epsilon)
            {
                //如若下方使用了waitForFixedUpdate(); 那么此处对应的也应该修改为  * Time.fixedDeltaTime
                transform.position = Vector3.MoveTowards(transform.position, nextTargetPoint, moveSpeed * Time.deltaTime);
                transform.rotation = Quaternion.AngleAxis((nextTargetPoint - transform.position).normalized.y * moveRotateAngle, Vector3.right);
                //此处旋转角度不需要带上deltaTime?
            }
            else
            {
                nextTargetPoint = ViewportTool.Instance.GetEnemyRandomMovePosInHalfView(paddingX, paddingY);
            }

            yield return null;//yield return new waitForFixedUpdate(); - 这样可以避免帧数很低时值过小 敌人不能正确的移动到新的随机位置
        }
    }

    IEnumerator FireRandomlyCor()
    {
        while (gameObject.activeSelf)
        {
            yield return new WaitForSeconds(Random.Range(minFireIntervel, maxFireIntervel));

            foreach (GameObject bullet in bullets)
            {
                AudioManager.Instance.PlayRandomSFX(shootAudioData);
                PoolManager.Instance.Release(bullet, muzzlePoint.position, muzzlePoint.rotation);
            }
        }
    }

}
