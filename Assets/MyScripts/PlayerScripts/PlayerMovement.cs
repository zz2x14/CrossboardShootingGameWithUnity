using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private PlayerInput playerInput;

    [Header("角色移动")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float accelerationTime;
    [SerializeField] private float decelerationTime;
    [SerializeField] private float moveRotateAngle;

    private Rigidbody2D rb;

    private Coroutine speedChangeCor;
    private Coroutine limitPosCor;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        playerInput.OnPlayerMove += Move;
        playerInput.OnStopMove += StopMove;
    }

    private void OnDisable()
    {
        playerInput.OnPlayerMove -= Move;
        playerInput.OnStopMove -= StopMove;
    }

    private void Move(Vector2 moveInput)
    {
        if(speedChangeCor!= null)
        {
            StopCoroutine(speedChangeCor);
        }

       
        speedChangeCor = StartCoroutine(MoveSpeedChangeCoroutine(accelerationTime, moveInput * moveSpeed,
            Quaternion.AngleAxis(moveRotateAngle * moveInput.y, Vector3.right)));//沿着角色的X轴旋转N度( * 上输入的Y值，不同方向移动时对应不同的角度)
         
        limitPosCor = StartCoroutine(LimitPlayerPosCoroutine());
    }

    private void StopMove()
    {
        if (speedChangeCor != null)
        {
            StopCoroutine(speedChangeCor);
        }
        speedChangeCor = StartCoroutine(MoveSpeedChangeCoroutine(decelerationTime, Vector2.zero,Quaternion.identity));

        if (limitPosCor != null)
        {
            StopCoroutine("LimitPlayerPosCoroutine");
        }
        
    }

    IEnumerator LimitPlayerPosCoroutine()
    {
        while (true)
        {
            transform.position = ViewportTool.Instance.GetLimitPosWithViewport(transform.position);
            yield return null;
        }
    }

    IEnumerator MoveSpeedChangeCoroutine(float time,Vector2 targetVelocity,Quaternion targerRotation)
    {
        float t = 0;

        while(t <= time)
        {
            // howGet: t += 1/time * Time.fixedDeltaTime 比如2s加速到最大速度，那么t每1秒增加 1/2 的值 t = Time.fixedDeltaTime/2;
            t += Time.fixedDeltaTime / time; 
            rb.velocity = Vector2.Lerp(rb.velocity, targetVelocity, t / time); //?
            transform.rotation = Quaternion.Lerp(transform.rotation, targerRotation, t / time);
            yield return null;
        }
    }
}
