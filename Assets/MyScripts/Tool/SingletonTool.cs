using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingletonTool<T> : MonoBehaviour where T : SingletonTool<T>
{ 
    public static T Instance { get; private set; }

    protected virtual void Awake()
    {
        if(Instance == null)
        {
            Instance = (T)this;
        }
        else
        {
            if(Instance != (T)this)
            {
                Destroy(gameObject);
            }
        }
    }

   
}
