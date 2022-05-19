using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewportTool : SingletonTool<ViewportTool>
{
    private float minX;
    private float maxX;
    private float minY;
    private float maxY;

    [SerializeField] private float paddingX;
    [SerializeField] private float paddingY;

    
    private void Start()
    {
        Vector3 buttomLeftPoint = Camera.main.ViewportToWorldPoint(new Vector2(0f, 0f));
        Vector3 topRightPoint = Camera.main.ViewportToWorldPoint(new Vector2(1f,1f));

        minX = buttomLeftPoint.x;
        minY = buttomLeftPoint.y;
        maxX = topRightPoint.x;
        maxY = topRightPoint.y;
    }

    //����player���ӿ���Ļ�ϵ��ƶ���Χ
    public Vector3 GetLimitPosWithViewport(Vector3 playerPos)
    {
        Vector3 limitPos = new Vector3();

        limitPos.x = Mathf.Clamp(playerPos.x, minX + paddingX, maxX - paddingX);
        limitPos.y = Mathf.Clamp(playerPos.y, minY + paddingY, maxY - paddingY);

        return limitPos;
    }
}
