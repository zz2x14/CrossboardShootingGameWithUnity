using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private PlayerInput playerInput;

    [Header("��ɫ�ƶ�")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float accelerationTime;
    [SerializeField] private float decelerationTime;
    [SerializeField] private float moveRotateAngle;

    private Rigidbody2D rb;

    private Coroutine speedChangeCor;

    private Vector3 previousVelocity;
    private Quaternion previousRotation;
    private WaitForFixedUpdate fixedUpdateNull = new WaitForFixedUpdate();

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
            Quaternion.AngleAxis(moveRotateAngle * moveInput.y, Vector3.right)));//���Ž�ɫ��X����תN��( * �������Yֵ����ͬ�����ƶ�ʱ��Ӧ��ͬ�ĽǶ�)

        StartCoroutine(nameof(LimitPlayerPosCoroutine));
    }

    private void StopMove()
    {
        if (speedChangeCor != null)
        {
            StopCoroutine(speedChangeCor);
        }
        speedChangeCor = StartCoroutine(MoveSpeedChangeCoroutine(decelerationTime, Vector2.zero,Quaternion.identity));

        StopCoroutine(nameof(LimitPlayerPosCoroutine));
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

        previousVelocity = rb.velocity;
        previousRotation = transform.rotation;//�ص㣺ʹ��lerp�����Բ�ֵ����ʱ���ı�ý��ֵ���ٸ����Ŀ��ֵ,�����ڷ����ڳ����ı����ƫ��

        while(t <= time)
        {
            //// howGet: t += 1/time * Time.fixedDeltaTime ����2s���ٵ�����ٶȣ���ôtÿ1������ 1/2 ��ֵ t = Time.fixedDeltaTime/2;
            //t += Time.fixedDeltaTime / time; 

            t += Time.fixedDeltaTime;
            rb.velocity = Vector2.Lerp(previousVelocity, targetVelocity, t / time); //?
            transform.rotation = Quaternion.Lerp(previousRotation, targerRotation, t / time);
            yield return fixedUpdateNull;//�ƶ�ʱ������̶�֡������ȴ�ͬ��Ҳ��ʹ��
        }
    }
}
