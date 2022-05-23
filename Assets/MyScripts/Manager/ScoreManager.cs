using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager :  PersisentSingletonTool<ScoreManager>
{
    private int score;
    private int curScore;

    [SerializeField] private Vector3 scoreTextUITargetScale;

    public void ResetScore()
    {
        score = 0;
        curScore = 0;
        PlayerScoreDisplay.UpdateScore(score);
    }
    
    public void AddScore(int scoreNumAdded)
    {
        curScore += scoreNumAdded;
        StartCoroutine(ScoreTextChangeEffectCor());
    }

    IEnumerator ScoreTextChangeEffectCor()
    {
        PlayerScoreDisplay.ChangeTextScale(scoreTextUITargetScale);
        while(score < curScore)//˼·����Ҫ
        {
            score += 1;

            PlayerScoreDisplay.UpdateScore(score);

            yield return null;
        }
        PlayerScoreDisplay.ChangeTextScale(Vector3.one);
    }
}
