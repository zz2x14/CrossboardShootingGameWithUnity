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

    public float MoveSpeed
    {
        get { return moveSpeed; }
        set { moveSpeed = value; }
    }

    private Rigidbody2D rb;

    private float paddingX;
    private float paddingY;
    private Vector3 modelSize;

    private Coroutine speedChangeCor;

    private Vector3 previousVelocity;
    private Quaternion previousRotation;

    private WaitForFixedUpdate fixedUpdateNull = new WaitForFixedUpdate();
    private WaitForSeconds waitDecelerateOverWFS;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        waitDecelerateOverWFS = new WaitForSeconds(decelerationTime);

        modelSize = transform.GetChild(0).GetComponent<MeshRenderer>().bounds.size;
        paddingX = modelSize.x / 2;
        paddingY = modelSize.y / 2;
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

        StopCoroutine(nameof(WaitDecelerateOverCor));
        StartCoroutine(nameof(LimitPlayerPosCoroutine));
       
    }

    private void StopMove()
    {
        if (speedChangeCor != null)
        {
            StopCoroutine(speedChangeCor);
        }

        speedChangeCor = StartCoroutine(MoveSpeedChangeCoroutine(decelerationTime, Vector2.zero,Quaternion.identity));

        StartCoroutine(nameof(WaitDecelerateOverCor));
    }

    IEnumerator WaitDecelerateOverCor()//等待减速完毕后再停止限制移动 - （否则）当移动到边缘上停止移动 减速未完毕 仍会超出屏幕范围
    {
        yield return waitDecelerateOverWFS;

        StopCoroutine(nameof(LimitPlayerPosCoroutine));
    }

    IEnumerator LimitPlayerPosCoroutine()
    {
        while (true)
        {
            transform.position = ViewportTool.Instance.GetLimitPosWithViewport(transform.position,paddingX,paddingY);
            yield return null;
        }
    }

    IEnumerator MoveSpeedChangeCoroutine(float time,Vector2 targetVelocity,Quaternion targerRotation)
    {
        float t = 0;

        previousVelocity = rb.velocity;
        previousRotation = transform.rotation;//重点：使用lerp等线性插值函数时，改变媒介值后再赋予给目标值,避免在方法内持续改变产生偏差

        while(t <= time)
        {
            //// howGet: t += 1/time * Time.fixedDeltaTime 比如2s加速到最大速度，那么t每1秒增加 1/2 的值 t = Time.fixedDeltaTime/2;
            //t += Time.fixedDeltaTime / time; 

            t += Time.fixedDeltaTime;
            rb.velocity = Vector2.Lerp(previousVelocity, targetVelocity, t / time); //?
            transform.rotation = Quaternion.Lerp(previousRotation, targerRotation, t / time);
            yield return fixedUpdateNull;//移动时会物理固定帧，挂起等待同理也该使用
        }
    }
}
