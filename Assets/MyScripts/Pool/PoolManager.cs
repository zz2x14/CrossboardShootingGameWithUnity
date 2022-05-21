using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : SingletonTool<PoolManager>
{
    [SerializeField] private Pool[] playerBulletPool;
    [SerializeField] private Pool[] enemyBulletPool;
    [SerializeField] private Pool[] vFXPool;
    
    private Dictionary<GameObject,Pool> poolDic = new Dictionary<GameObject,Pool>();

    protected override void Awake()
    {
        base.Awake();

        InitPool(playerBulletPool);
        InitPool(enemyBulletPool);
        InitPool(vFXPool);
    }

#if UNITY_EDITOR
    private void OnDestroy()
    {
        CheckPoolSize(playerBulletPool);
        CheckPoolSize(enemyBulletPool);
        CheckPoolSize(vFXPool);
    }
#endif

    private void CheckPoolSize(Pool[] pools )
    {
        foreach (var pool in pools)
        {
            if(pool.PoolRuntimeSize > pool.PoolSize)
            {
                Debug.LogWarning($"{pool.Prefab.name}runtimeSize{pool.PoolRuntimeSize}biger than size{pool.PoolSize}");
            }
        }
    }

    private void InitPool(Pool[] pool)
    {
        for (int i = 0; i < pool.Length; i++)
        {
            Transform poolParent  = new GameObject($"Pool:{pool[i].Prefab.name}").transform;
            poolParent.transform.parent = transform;

            pool[i].Init(poolParent);

            poolDic.Add(pool[i].Prefab,pool[i]);
        }
    }

    public GameObject Release(GameObject prefab)
    {
        return poolDic[prefab].GetPreparedGO();
    }
    public GameObject Release(GameObject prefab,Vector3 pos)
    {
        return poolDic[prefab].GetPreparedGO(pos);
    }
    public GameObject Release(GameObject prefab, Vector3 pos,Quaternion rotation)
    {
        return poolDic[prefab].GetPreparedGO(pos,rotation);
    }

}
