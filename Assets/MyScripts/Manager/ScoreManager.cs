using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ScoreManager :  PersistentSingletonTool<ScoreManager>
{
    private int score;
    private int curScore;

    public int Score => score;

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
        while(score < curScore)//思路很重要
        {
            score += 1;

            PlayerScoreDisplay.UpdateScore(score);

            yield return null;
        }
        PlayerScoreDisplay.ChangeTextScale(Vector3.one);
    }

    public void SavePlayerScoreData()
    {
        var playerScoreData = LoadPlayerScoreData();//先读取当前存档（因为要排序）

        playerScoreData.playerScoreList.Add(new PlayerScore(score, defaultName));//将新数据存入
        
        //playerScoreData.playerScoreList.Sort((x, y) => y.score.CompareTo(x.score));//排序 - 完整方法参考下方(需要的一个参数 - 泛型委托)

        DescendingSortPlayerByScore(playerScoreData);

        JsonSaveSystem.SaveByJson(PLAYERSCOREDATAFILENAME, playerScoreData);//存储
    }

    private void DescendingSortPlayerByScore(PlayerScoreData playerScoreData)
    {
        bool bubbleOver = false;

        do
        {
            bubbleOver = false;
            for (int i = 0; i < playerScoreData.playerScoreList.Count - 1; i++)
            {
                if (playerScoreData.playerScoreList[i].score < playerScoreData.playerScoreList[i + 1].score)
                {
                    PlayerScore playerScoreTemp = playerScoreData.playerScoreList[i];
                    playerScoreData.playerScoreList[i] = playerScoreData.playerScoreList[i + 1];
                    playerScoreData.playerScoreList[i + 1] = playerScoreTemp;

                    bubbleOver = true;
                }
            }

        }while (bubbleOver);
        
        //最少做一轮比较，bubbleover为true说明还未交换完毕，为false代表这轮没有交换了，已经全部排序完成
    }

    
    //public int SortScore(PlayerScore playe01,PlayerScore player02)
    //{
    //    return playe01.score.CompareTo(player02.score);
    //}

    public PlayerScoreData LoadPlayerScoreData()
    {
        PlayerScoreData playerScoreData = new PlayerScoreData();

        if (JsonSaveSystem.IsDataExists(PLAYERSCOREDATAFILENAME))//判断是否有存档
        {
            playerScoreData = JsonSaveSystem.LoadFromJson<PlayerScoreData>(PLAYERSCOREDATAFILENAME);
        }
        else//没有存档就生成10个0分无名“玩家”并存储
        {
            while (playerScoreData.playerScoreList.Count < 10)
            {
                playerScoreData.playerScoreList.Add(new PlayerScore(0, defaultName));
            }

            JsonSaveSystem.SaveByJson(PLAYERSCOREDATAFILENAME, playerScoreData);
        }

        return playerScoreData;
    }

    private const string PLAYERSCOREDATAFILENAME = "playerScoreData.json";
    private string defaultName = "No Name";

    public bool hasNewHighScore => score > LoadPlayerScoreData().playerScoreList[9].score;

    public void SetName(string newName)
    {
        defaultName = newName;
    }

    [Serializable]
    public class PlayerScore
    {
        public int score;
        public string playerName;

        public PlayerScore(int score, string playerName)
        {
            this.score = score;
            this.playerName = playerName;
        }
    }

    [Serializable]
    public class PlayerScoreData
    {
        public List<PlayerScore> playerScoreList = new List<PlayerScore>();
    }
}
