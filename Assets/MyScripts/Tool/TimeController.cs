using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TimeController : SingletonTool<TimeController>
{
    [SerializeField] private float scaleOfBulletTime;

    private float defaultFixdTime;

    private float bulletTimeTimer;

    private float timeScaleBeforePaused;

    protected override void Awake()
    {
        base.Awake();

        defaultFixdTime = Time.fixedDeltaTime;
    }

    public void Pause()
    {
        timeScaleBeforePaused = Time.timeScale;//��¼��Ϸ��ͣǰ��timescale���ûص���Ϸʱ����ȷ������֮ǰ��timescale

        Time.timeScale = 0f;

        GameManager.Instance.GameState = GameState.Paused;
    }
    public void Unpause()
    {
        Time.timeScale = timeScaleBeforePaused;

        GameManager.Instance.GameState = GameState.Playing;
    }

    public void StartBulletTime(float duration)
    {
        Time.timeScale = scaleOfBulletTime;
        StartCoroutine(TimeSlowOutCor(duration));
    }

    public void StartBulletTime(float inDuration,float outDuration)
    {
        StartCoroutine(TimeSlowInAndOut(inDuration,outDuration));
    }

    public void StartBulletTime(float inDuration,float keepSlowInDuration,float outDuration)
    {
        StartCoroutine(TimeSlowInKeepAndOut(inDuration, keepSlowInDuration, outDuration));
    }

    //�����ر���timescale �� �����ػָ���������timescale
    public void SlowIn(float duration)
    {
        StartCoroutine(TimeSlowInCor(duration));
    }
    public void SlowOut(float duration)
    {
        StartCoroutine(TimeSlowOutCor(duration));
    }

    IEnumerator TimeSlowInAndOut(float inDuration, float outDuration)
    {
        yield return StartCoroutine(TimeSlowInCor(inDuration));//�𽥱���timescale��ɺ��ٽ����𽥻ָ���������timescale
        StartCoroutine(TimeSlowOutCor(outDuration));
    }

    IEnumerator TimeSlowInKeepAndOut(float inDuration,float keepSlowInDuration,float outDuration)
    {
        yield return StartCoroutine(TimeSlowInCor(inDuration));
        yield return keepSlowInDuration;//����һ��ʱ�������timescale���ٿ�ʼ�𽥻ָ���������timescale
        StartCoroutine(TimeSlowOutCor(outDuration));
    }

    IEnumerator TimeSlowInCor(float duration)
    {
        bulletTimeTimer = 0;

        while (bulletTimeTimer < 1f)
        {
            if(GameManager.Instance.GameState != GameState.Paused)
            {
                bulletTimeTimer += 1 / duration * Time.unscaledDeltaTime;
                Time.timeScale = Mathf.Lerp(1f, scaleOfBulletTime, bulletTimeTimer);
                Time.fixedDeltaTime = defaultFixdTime * Time.timeScale;//��ʹ���˹̶�֡ ��ô�̶�֡��Ҫ�ı� - ����������ԵĿ��ٸ�
                //�Ƿ���Ը������� ��deltatime��fixeddeltatime ֱ�Ӹĳ�timescale����ĳЩ�����ܵ���һ��������Ӱ�� ��˼���� ��������Ϸ�ĳ��ڿ���
            }

            yield return null;//Э���ڲ����ʱ �ȴ�����Ҫд�������� д���ڲ�����û���� ������жϽ�����ѭ���߼�
        }
    }
    IEnumerator TimeSlowOutCor(float duration)
    {
        bulletTimeTimer = 0;

        while(bulletTimeTimer < 1f)
        {
            if(GameManager.Instance.GameState != GameState.Paused)
            {
                bulletTimeTimer += 1 / duration * Time.unscaledDeltaTime;
                Time.timeScale = Mathf.Lerp(scaleOfBulletTime, 1f, bulletTimeTimer);
                Time.fixedDeltaTime = defaultFixdTime * Time.timeScale;
            }

            yield return null;
        }
    }
  

}
