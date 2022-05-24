using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusBar : MonoBehaviour
{
    [SerializeField] private Image backBarImage;
    [SerializeField] private Image frontBarImage;

    protected float curFillAmount;
    protected float targetFillAmount;

    [SerializeField] private bool enableDealyFill = true;

    [SerializeField] private float delayFillTime;
    [SerializeField] private float fillSpeed;

    private WaitForSeconds delayFillWFS;
    private float fillTimer;

    private Coroutine fillCor;

    private float previousFillAmount;

    private void Awake()
    {
        if(TryGetComponent(out Canvas canvas))//HUD�ϲ��󲢲���Ҫ�� ���� ��Һ͵���ͷ�ϵĻ���������Ҫ����� 
        {
            canvas.worldCamera = Camera.main;
        }
        
        delayFillWFS = new WaitForSeconds(delayFillTime);
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    public virtual void InitializeFillAmount(float curValue,float maxValue)
    {
        curFillAmount = curValue / maxValue;
        targetFillAmount = curFillAmount;
        backBarImage.fillAmount = curFillAmount;
        frontBarImage.fillAmount = curFillAmount;
    }

    public virtual void UpdateFillAmount(float curValue,float maxValue)
    {
        targetFillAmount = curValue / maxValue;

        if(fillCor != null)
        {
            StopCoroutine(fillCor);
        }

        //��ǰ��ֵ����Ŀ��ֵ��˵��Ϊ�������
        if(curFillAmount > targetFillAmount)
        {
            frontBarImage.fillAmount = targetFillAmount;
            fillCor = StartCoroutine(BufferedFillCor(backBarImage));

            return;//�������ֻ�����һ�� ֱ�ӷ��� �����ٽ��к�����߼��ж�
        }
        //��ǰ��ֵС��Ŀ��ֵ��˵��Ϊ�������
        if (curFillAmount < targetFillAmount)
        {
            backBarImage.fillAmount = targetFillAmount;
            fillCor = StartCoroutine(BufferedFillCor(frontBarImage));
        }

        //˵����ex��Ѫ������ʱ��ǰ��Ѫ��ֱ�ӿ۳�����Ѫ�������۳���Ѫ������ʱ����Ѫ��ֱ�����ӣ�ǰ��Ѫ����������
    }

    protected virtual IEnumerator BufferedFillCor(Image targetFillAmountImage)
    {
        if(enableDealyFill)
            yield return delayFillWFS;//�ȴ����ݵ�ʱ���ٿ�ʼ���

        fillTimer = 0f;
        previousFillAmount = curFillAmount;

        while (fillTimer < 1f)
        {
            fillTimer += fillSpeed * Time.deltaTime;
            curFillAmount = Mathf.Lerp(previousFillAmount, targetFillAmount, fillTimer);
            targetFillAmountImage.fillAmount = curFillAmount;//��ǰ���ֵ�𽥴ﵽĿ�����ֵ - ����Ч��
            yield return null;
        }
    }

}
