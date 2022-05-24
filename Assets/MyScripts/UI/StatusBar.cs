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
        if(TryGetComponent(out Canvas canvas))//HUD合并后并不需要了 但是 玩家和敌人头上的画布还是需要相机的 
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

        //当前的值大于目标值，说明为减少情况
        if(curFillAmount > targetFillAmount)
        {
            frontBarImage.fillAmount = targetFillAmount;
            fillCor = StartCoroutine(BufferedFillCor(backBarImage));

            return;//两种情况只会出现一种 直接返回 不用再进行后面的逻辑判断
        }
        //当前的值小于目标值，说明为增加情况
        if (curFillAmount < targetFillAmount)
        {
            backBarImage.fillAmount = targetFillAmount;
            fillCor = StartCoroutine(BufferedFillCor(frontBarImage));
        }

        //说明：ex：血量减少时，前景血量直接扣除，后景血条缓慢扣除；血量增加时，后景血条直接增加，前景血条缓慢增加
    }

    protected virtual IEnumerator BufferedFillCor(Image targetFillAmountImage)
    {
        if(enableDealyFill)
            yield return delayFillWFS;//等待短暂的时间再开始填充

        fillTimer = 0f;
        previousFillAmount = curFillAmount;

        while (fillTimer < 1f)
        {
            fillTimer += fillSpeed * Time.deltaTime;
            curFillAmount = Mathf.Lerp(previousFillAmount, targetFillAmount, fillTimer);
            targetFillAmountImage.fillAmount = curFillAmount;//当前填充值逐渐达到目标填充值 - 缓冲效果
            yield return null;
        }
    }

}
