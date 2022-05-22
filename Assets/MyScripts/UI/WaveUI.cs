using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaveUI : MonoBehaviour
{
    private Text waveNumText;
    private Canvas canvas;
    private Animator anim;

    private RectTransform topLineTransform;
    private RectTransform bottomLineTransform;

    [SerializeField] private Vector3 topLinePosOffView;
    [SerializeField] private Vector3 topLinePosCenterView;
    [SerializeField] private Vector3 bottomLinePosOffView;
    [SerializeField] private Vector3 bottomLinePosCenterView;

    [SerializeField] private float animCostTime;
    [SerializeField] private float animDuration;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        canvas = GetComponent<Canvas>();

        waveNumText = transform.GetChild(0).GetComponent<Text>();
        //topLineTransform = transform.GetChild(0).GetComponent<RectTransform>();
        //bottomLineTransform = transform.GetChild(1).GetComponent<RectTransform>();

        canvas.worldCamera = Camera.main; 
    }

    private void OnEnable()
    {
        waveNumText.text = $"- WAVE{EnemyManager.Instance.WaveNum} -";

        anim.Play("WaveUIEffect");
      
    }

    //IEnumerator UIMoveCor(RectTransform rectTransform,Vector3 targetPos,Vector3 startPos)
    //{
    //    yield return StartCoroutine(LineMovementCor(rectTransform,targetPos));
    //    yield return animDuration;
    //    yield return StartCoroutine(LineMovementCor(rectTransform,startPos));
    //}

    //IEnumerator LineMovementCor(RectTransform rectTransform,Vector3 targetPos)
    //{
    //    float t = 0;
       
    //    while(t < animCostTime)
    //    {
    //        t += 1 / animCostTime * Time.deltaTime;
    //        rectTransform.localPosition = Vector3.Lerp(rectTransform.localPosition, targetPos, t);
    //        yield return null;
    //    }
    //}

    

}
