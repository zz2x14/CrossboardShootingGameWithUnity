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

    public GameObject RandomEnemy => enemyList.Count > 0 ? enemyList[Random.Range(0, enemyList.Count)] : null;
   
    protected override void Awake()
    {
        base.Awake();

        spawnIntervalWFS = new WaitForSeconds(spawnInterval);
        waveIntervalWFS = new WaitForSeconds(waveInterval); 
        unitlNoEnemy = new WaitUntil(()=>enemyList.Count == 0); //需要一个返回值为bool的委托 - 当敌人列表数量等于0时
    }

    IEnumerator Start()//将Start直接变为协程（原本思路：用一个协程再开启一个协程 ）
    {
        while (enableSpawn)
        {
            //yield return unitlNoEnemy;//等到满足条件 - 没有敌人时
            waveHUDUI.SetActive(true);
            yield return waveIntervalWFS;//等到每波间隔
            waveHUDUI.SetActive(false);
            yield return StartCoroutine(nameof(SpawnEnemiesRandomlyCor));//若正在生成敌人，等到生成完毕后再生成 - 此行也启用了该协程
        }
        
    }

    IEnumerator SpawnEnemiesRandomlyCor()
    {
        eachWaveEnemiesAmount = Mathf.Clamp(eachWaveEnemiesAmount, minSpawnAmount + waveNum / 3, maxSpawnAmount);

        for (int i = 0; i < eachWaveEnemiesAmount; i++)
        {
            enemyList.Add(PoolManager.Instance.Release(enemyPrefabs[Random.Range(0,enemyPrefabs.Length)]));//随机生成敌人,并添加进敌人列表内

            yield return spawnIntervalWFS;
        }

        yield return unitlNoEnemy;//没有敌人后再继续执行相关(循环)逻辑
        //将此代码放在此处 - 避免敌人还没全部生成完毕就被消灭，导致出现 后续敌人不会继续生成，直接生成下一波 的情况

        waveNum++;
    }

    public void RemoveFromEnemyList(GameObject go)
    {
        enemyList.Remove(go);
    }
}
