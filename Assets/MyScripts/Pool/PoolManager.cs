using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : SingletonTool<PoolManager>
{
    [SerializeField] private Pool[] playerBulletPool;
    
    private Dictionary<GameObject,Pool> poolDic = new Dictionary<GameObject,Pool>();

    protected override void Awake()
    {
        base.Awake();

        InitPool(playerBulletPool);
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
