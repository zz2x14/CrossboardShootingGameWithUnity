using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDeactiveTool : MonoBehaviour
{
    [SerializeField] private float lifetime;

    [SerializeField] private bool needDestroy;

    private WaitForSeconds lifetimeWaitForSeconds;

    private void Awake()
    {
        lifetimeWaitForSeconds = new WaitForSeconds(lifetime);
    }

    private void OnEnable()
    {
        StartCoroutine(nameof(AutoDeactiveCor));
    }

    private void OnDisable()
    {
        StopCoroutine(nameof(AutoDeactiveCor));
    }

    IEnumerator AutoDeactiveCor()
    {
        yield return lifetimeWaitForSeconds;

        if (!needDestroy)
        {
            gameObject.SetActive(false);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
