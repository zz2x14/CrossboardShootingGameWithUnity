using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [Header("�ƶ�")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float moveRotateAngle;

    [Header("����")]
    [SerializeField] private GameObject[] bullets;
    [SerializeField] private AudioData shootAudioData;
    [SerializeField] private Transform muzzlePoint;
    [SerializeField] private float minFireIntervel;
    [SerializeField] private float maxFireIntervel;

    private Vector3 modelSize;
    private float paddingX;
    private float paddingY;

    private void Awake()
    {
        modelSize = transform.GetChild(0).GetComponent<MeshRenderer>().bounds.size;
        paddingX = modelSize.x / 2;
        paddingY = modelSize.y / 2; 
    }

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
            //�˴���Ӧ�޸�Ϊ moveSpeed * Time.fixedDeltaTime - ��ζ������֮��ľ��� ���� ��λ�̶�ÿ֡���ƶ��ٶ����ܹ��ƶ��ľ���
            if (Vector3.Distance(transform.position, nextTargetPoint) > Mathf.Epsilon)
            {
                //�����·�ʹ����waitForFixedUpdate(); ��ô�˴���Ӧ��ҲӦ���޸�Ϊ  * Time.fixedDeltaTime
                transform.position = Vector3.MoveTowards(transform.position, nextTargetPoint, moveSpeed * Time.deltaTime);
                transform.rotation = Quaternion.AngleAxis((nextTargetPoint - transform.position).normalized.y * moveRotateAngle, Vector3.right);
                //�˴���ת�ǶȲ���Ҫ����deltaTime?
            }
            else
            {
                nextTargetPoint = ViewportTool.Instance.GetEnemyRandomMovePosInHalfView(paddingX, paddingY);
            }

            yield return null;//yield return new waitForFixedUpdate(); - �������Ա���֡���ܵ�ʱֵ��С ���˲�����ȷ���ƶ����µ����λ��
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
