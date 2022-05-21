using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewportTool : SingletonTool<ViewportTool>
{
    private float minX;
    private float maxX;
    private float minY;
    private float maxY;
    private float middleX;

    [SerializeField] private float paddingX;
    [SerializeField] private float paddingY;

    
    private void Start()
    {
        Vector3 buttomLeftPoint = Camera.main.ViewportToWorldPoint(new Vector2(0f, 0f));
        Vector3 topRightPoint = Camera.main.ViewportToWorldPoint(new Vector2(1f,1f));

        middleX = Camera.main.ViewportToWorldPoint(new Vector2(0.5f, 0)).x;

        minX = buttomLeftPoint.x;
        minY = buttomLeftPoint.y;
        maxX = topRightPoint.x;
        maxY = topRightPoint.y;
    }

    //限制player在视口屏幕上的移动范围
    public Vector3 GetLimitPosWithViewport(Vector3 playerPos)
    {
        Vector3 limitPos = Vector3.zero;

        limitPos.x = Mathf.Clamp(playerPos.x, minX + paddingX, maxX - paddingX);
        limitPos.y = Mathf.Clamp(playerPos.y, minY + paddingY, maxY - paddingY);

        return limitPos;
    }

    public Vector3 GetRandomEnemySpawnPos(float paddingX,float paddingY )
    {
        Vector3 randomPos = Vector3.zero;

        randomPos.x = maxX + paddingX;
        randomPos.y = Random.Range(minY + paddingY, maxY - paddingY);
        
        return randomPos;
    }

    public Vector3 GetEnemyRandomMovePosInHalfView(float paddingX,float paddingY)
    {
        Vector3 randomPos = Vector3.zero;

        randomPos.x = Random.Range(middleX, maxX - paddingX);
        randomPos.y = Random.Range(minY+paddingY,maxY-paddingY);

        return randomPos;
    }

    public Vector3 EnemyMoveRandomInAllView(float paddingX, float paddingY)
    {
        Vector3 randomPos = Vector3.zero;

        randomPos.x = Random.Range(minX + paddingX, maxX - paddingX);
        randomPos.y = Random.Range(minY + paddingY, maxY - paddingY);

        return randomPos;
    }
}
