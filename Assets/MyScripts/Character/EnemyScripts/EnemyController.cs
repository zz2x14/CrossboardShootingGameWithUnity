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
            if (Vector3.Distance(transform.position, nextTargetPoint) > Mathf.Epsilon)
            {
                transform.position = Vector3.MoveTowards(transform.position, nextTargetPoint, moveSpeed * Time.deltaTime);
                transform.rotation = Quaternion.AngleAxis((nextTargetPoint - transform.position).normalized.y * moveRotateAngle, Vector3.right);
                //此处旋转角度不需要带上deltaTime?
            }
            else
            {
                nextTargetPoint = ViewportTool.Instance.GetEnemyRandomMovePosInHalfView(paddingX, paddingY);
            }

            yield return null;
        }
    }

    IEnumerator FireRandomlyCor()
    {
        while (gameObject.activeSelf)
        {
            yield return new WaitForSeconds(Random.Range(minFireIntervel, maxFireIntervel));

            foreach (GameObject bullet in bullets)
            {
                PoolManager.Instance.Release(bullet, muzzlePoint.position, muzzlePoint.rotation);
            }
        }
    }

}
