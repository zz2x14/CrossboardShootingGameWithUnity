using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Pool 
{
    [SerializeField] private GameObject prefab;
    public GameObject Prefab => prefab;
    [SerializeField] private int poolSize;
    public int PoolSize => poolSize;

    public int PoolRuntimeSize => quene.Count;

    private Queue<GameObject> quene;

    private Transform parentTransform;
    
    public void Init(Transform parentTransform)
    {
        quene = new Queue<GameObject>();

        this.parentTransform = parentTransform;

        for (int i = 0; i < poolSize; i++)
        {
            quene.Enqueue(CopyGO());
        }
    }

    private GameObject CopyGO()
    {
        GameObject copy = GameObject.Instantiate(prefab,parentTransform);

        copy.SetActive(false);

        return copy;
    }

    private GameObject GetAvaliableGO()
    {
        GameObject avaliableGO = null;

        if(quene.Count > 0 && !quene.Peek().activeSelf)
        {
            avaliableGO = quene.Dequeue();
        }
        else
        {
            avaliableGO = CopyGO();
        }

        quene.Enqueue(avaliableGO);

        return avaliableGO; 
    }

    public GameObject GetPreparedGO()
    {
        GameObject preparedGO = GetAvaliableGO();
        preparedGO.SetActive(true);
        return preparedGO;
    }
    public GameObject GetPreparedGO(Vector3 pos)
    {
        GameObject preparedGO = GetAvaliableGO();
        preparedGO.SetActive(true);
        preparedGO.transform.position = pos;
        return preparedGO;
    }
    public GameObject GetPreparedGO(Vector3 pos,Quaternion rotation)
    {
        GameObject preparedGO = GetAvaliableGO();
        preparedGO.SetActive(true);
        preparedGO.transform.position = pos;
        preparedGO.transform.rotation = rotation;
        return preparedGO;
    }
  
}
