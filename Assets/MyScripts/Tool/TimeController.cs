using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeController : PersistentSingletonTool<TimeController>
{
    [SerializeField] private float scaleOfBulletTime;

    private float defaultFixdTime;

    private float bulletTimeTimer;

    protected override void Awake()
    {
        base.Awake();

        defaultFixdTime = Time.fixedDeltaTime;
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

    //单纯地变慢timescale 和 单纯地恢复到正常的timescale
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
        yield return StartCoroutine(TimeSlowInCor(inDuration));//逐渐变慢timescale完成后再进行逐渐恢复到正常的timescale
        StartCoroutine(TimeSlowOutCor(outDuration));
    }

    IEnumerator TimeSlowInKeepAndOut(float inDuration,float keepSlowInDuration,float outDuration)
    {
        yield return StartCoroutine(TimeSlowInCor(inDuration));
        yield return keepSlowInDuration;//保持一段时间的慢速timescale后再开始逐渐恢复至正常的timescale
        StartCoroutine(TimeSlowOutCor(outDuration));
    }

    IEnumerator TimeSlowInCor(float duration)
    {
        bulletTimeTimer = 0;

        while (bulletTimeTimer < 1f)
        {
            bulletTimeTimer += 1 / duration * Time.unscaledDeltaTime;
            Time.timeScale = Mathf.Lerp(1f, scaleOfBulletTime, bulletTimeTimer);
            Time.fixedDeltaTime = defaultFixdTime * Time.timeScale;//若使用了固定帧 那么固定帧需要改变 - 避免出现明显的卡顿感
            //是否可以根据需求 将deltatime、fixeddeltatime 直接改成timescale避免某些操作受到上一行所述的影响 ：思考后 不利于游戏的长期开发
            yield return null;
        }
    }
    IEnumerator TimeSlowOutCor(float duration)
    {
        bulletTimeTimer = 0;

        while(bulletTimeTimer < 1f)
        {
            bulletTimeTimer += 1 / duration * Time.unscaledDeltaTime;
            Time.timeScale = Mathf.Lerp(Time.timeScale, 1f, bulletTimeTimer);
            Time.fixedDeltaTime = defaultFixdTime * Time.timeScale;
            yield return null;
        }
    }
  

}
