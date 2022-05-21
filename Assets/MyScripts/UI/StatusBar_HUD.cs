using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusBar_HUD : StatusBar
{
    [SerializeField] private Text barValuePercentText;


    private void UpdateValuePercent()
    {
        barValuePercentText.text = Mathf.RoundToInt(targetFillAmount * 100)  + "%";
    }

    public override void InitializeFillAmount(float curValue, float maxValue)
    {
        base.InitializeFillAmount(curValue, maxValue);

        UpdateValuePercent();
    }


    public override void UpdateFillAmount(float curValue, float maxValue)
    {
        base.UpdateFillAmount(curValue, maxValue);

        UpdateValuePercent();
    }

    protected override IEnumerator BufferedFillCor(Image targetFillAmountImage)
    {
        UpdateValuePercent();
        return base.BufferedFillCor(targetFillAmountImage);
    }

}
