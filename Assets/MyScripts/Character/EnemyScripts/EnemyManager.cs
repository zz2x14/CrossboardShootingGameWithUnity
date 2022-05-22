using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : SingletonTool<EnemyManager>
{
    [SerializeField] private GameObject[] enemyPrefabs;
    [SerializeField] private List<GameObject> enemyList = new List<GameObject>();

    [SerializeField] private bool enableSpawn = true;

    [SerializeField] private float waveInterval;
    [SerializeField] private float spawnInterval;

    [SerializeField] private int minSpawnAmount;
    [SerializeField] private int maxSpawnAmount;

    [SerializeField] private GameObject waveHUDUI;

    private int waveNum = 1;
    private float eachWaveEnemiesAmount;

    private WaitForSeconds spawnIntervalWFS;
    private WaitForSeconds waveIntervalWFS;
    private WaitUntil unitlNoEnemy;

    public int WaveNum => waveNum;
    public float WaveInterval => waveInterval;
   
    protected override void Awake()
    {
        base.Awake();

        spawnIntervalWFS = new WaitForSeconds(spawnInterval);
        waveIntervalWFS = new WaitForSeconds(waveInterval); 
        unitlNoEnemy = new WaitUntil(()=>enemyList.Count == 0); //��Ҫһ������ֵΪbool��ί�� - �������б���������0ʱ
    }

    IEnumerator Start()//��Startֱ�ӱ�ΪЭ�̣�ԭ��˼·����һ��Э���ٿ���һ��Э�� ��
    {
        while (enableSpawn)
        {
            yield return unitlNoEnemy;//�ȵ���������
            waveHUDUI.SetActive(true);
            yield return waveIntervalWFS;//�ȵ�ÿ�����
            waveHUDUI.SetActive(false);
            yield return StartCoroutine(nameof(SpawnEnemiesRandomlyCor));//���������ɵ��ˣ��ȵ�������Ϻ������� - ����Ҳ�����˸�Э��
        }
        
    }

    IEnumerator SpawnEnemiesRandomlyCor()
    {
        eachWaveEnemiesAmount = Mathf.Clamp(eachWaveEnemiesAmount, minSpawnAmount + waveNum / 3, maxSpawnAmount);

        for (int i = 0; i < eachWaveEnemiesAmount; i++)
        {
            enemyList.Add(PoolManager.Instance.Release(enemyPrefabs[Random.Range(0,enemyPrefabs.Length)]));//������ɵ���,����ӽ������б���

            yield return spawnIntervalWFS;
        }

        waveNum++;

        yield return null;
    }

    public void RemoveFromEnemyList(GameObject go)
    {
        enemyList.Remove(go);
    }
}
