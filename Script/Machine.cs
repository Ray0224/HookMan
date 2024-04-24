using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Machine : MonoBehaviour
{
    public GameObject xrayScene;
    public Transform handler1;
    public Transform handler2;
    public float maxOffset_x;
    public float maxOffset_y;
    public Vector2 anchorOffset1;
    public Vector2 anchorOffset2;
    public float maxAngle1;
    public float maxAngle2;
    Vector2 originPos;
    Vector2 originVector1;
    Vector2 originVector2;
    Vector2 anchorPos1;
    Vector2 anchorPos2;
    // Start is called before the first frame update
    void Start()
    {
        originPos = xrayScene.transform.position;
        originVector1 = -anchorOffset1.normalized;
        originVector2 = -anchorOffset2.normalized;
        //初始化錨點座標
        Vector2 a = handler1.position;
        Vector2 b = handler2.position;
        anchorPos1 = a + anchorOffset1;
        anchorPos2 = b + anchorOffset2;



    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }
    private void Move()
    {
        Vector2 a = handler1.position;
        Vector2 b = handler2.position;
        Vector2 currentVector1 = (a - anchorPos1).normalized;
        Vector2 currentVector2 = (b - anchorPos2).normalized;
        float angleOffset1 =  Vector2.Angle(originVector1, currentVector1);
        float angleOffset2 =  Vector2.Angle(originVector2, currentVector2);
        float xOffset = (angleOffset1 / maxAngle1) * maxOffset_x;
        float yOffset = (angleOffset2 / maxAngle2) * maxOffset_y;
        xrayScene.transform.position = originPos + new Vector2(xOffset, yOffset);

    }
}
