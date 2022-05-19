using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private PlayerInput playerInput;

    [SerializeField] private GameObject standardBulletPrefab;
    [SerializeField] private Transform muzzlePoint;
    [SerializeField] private float fireInterval;

    private WaitForSeconds fireWaitForSeconds;

    private void Start()
    {
        playerInput.EnableGameplayInput();

        fireWaitForSeconds = new WaitForSeconds(fireInterval);//避免在循环内声明新的变量
    }

    private void OnEnable()
    {
        playerInput.OnStartFire += Fire;
        playerInput.OnStopFire += StopFire;
    }

    private void OnDisable()
    {
        playerInput.OnStartFire -= Fire;
        playerInput.OnStopFire -= StopFire;
    }

    private void Fire()
    {
        StartCoroutine(nameof(StartFireCor));
    }

    private void StopFire()
    {
        StopCoroutine(nameof(StartFireCor));
    }


    IEnumerator StartFireCor()
    {
        while (true)
        {
            PoolManager.Instance.Release(standardBulletPrefab,muzzlePoint.position,muzzlePoint.rotation);

            yield return fireWaitForSeconds;
        }
    }


}
