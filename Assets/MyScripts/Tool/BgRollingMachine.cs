using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BgRollingMachine : MonoBehaviour
{
    private Material material;

    [SerializeField] private Vector2 rollVelocity;

    private void Start()
    {
       material = GetComponent<MeshRenderer>().material;

        StartCoroutine(BgKeepRollingCor());
    }

    IEnumerator BgKeepRollingCor()
    {
        while (gameObject.activeSelf)
        {
            material.mainTextureOffset += rollVelocity * Time.deltaTime;
            yield return null;
        }
    }
}
