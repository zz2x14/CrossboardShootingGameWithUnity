using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BulletBase : MonoBehaviour
{
    [SerializeField] private float flySpeed;

    [SerializeField] private Vector2 moveDir;

    private void OnEnable()
    {
        StartCoroutine(MoveByDirCor());
    }

    IEnumerator MoveByDirCor()
    {
        while (gameObject.activeSelf)
        {
            transform.Translate(moveDir * flySpeed *Time.deltaTime);
            yield return null;
        }
    }
}
