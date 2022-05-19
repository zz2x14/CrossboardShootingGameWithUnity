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
    }

    private void Update()
    {
       material.mainTextureOffset += rollVelocity * Time.deltaTime;
    }
}
