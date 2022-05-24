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
            Quaternion.AngleAxis(moveRotateAngle * moveInput.y, Vector3.right)));//���Ž�ɫ��X����תN��( * �������Yֵ����ͬ�����ƶ�ʱ��Ӧ��ͬ�ĽǶ�)

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

    IEnumerator WaitDecelerateOverCor()//�ȴ�������Ϻ���ֹͣ�����ƶ� - �����򣩵��ƶ�����Ե��ֹͣ�ƶ� ����δ��� �Իᳬ����Ļ��Χ
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
