using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScoreDisplay : MonoBehaviour
{
    private static Text scoreNumText;

    private void Awake()
    {
        scoreNumText = GetComponent<Text>();
    }

    private void Start()
    {
        ScoreManager.Instance.ResetScore();
    }

    public static void UpdateScore(int scoreValue) => scoreNumText.text = scoreValue.ToString();
    public static void ChangeTextScale(Vector3 targetScale) => scoreNumText.rectTransform.localScale = targetScale;
}
