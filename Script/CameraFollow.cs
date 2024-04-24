using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("相機跟隨")]
    public GameObject target;
    public float maxDistance;
    public float smooth;

    [Header("遠視角")]
    public float moveSpeed;
    public float maxFarDistance;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey("space"))
        {
            FarawayView();
        }
        else
        {
            Follow();
        }
        
    }
    void Follow()
    {
        Vector3 pos = new Vector3(transform.position.x, transform.position.y - 2, 0);
        float distance = (pos - target.transform.position).magnitude;
        if (distance > maxDistance)
        {
            Vector3 targetPos = new Vector3(target.transform.position.x, target.transform.position.y + 2, -10);
            transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * smooth);
        }
    }
    void FarawayView()
    {
        Vector2 cameraPos_Screen = Camera.main.WorldToScreenPoint(transform.position);
        Vector2 mousePos_Screen = Input.mousePosition;
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(mousePos_Screen);
        Vector3 targetPos = target.transform.position;
        Vector2 finalPos_Screen = (mousePos - targetPos).magnitude <= maxFarDistance ? mousePos_Screen :
            Camera.main.WorldToScreenPoint(targetPos + (mousePos - targetPos).normalized * maxFarDistance);
        Vector2 newPosScreen = Vector2.Lerp(cameraPos_Screen, finalPos_Screen, Time.deltaTime * moveSpeed);
        Vector2 newPos = Camera.main.ScreenToWorldPoint(newPosScreen);
        transform.position = new Vector3(newPos.x, newPos.y, -10);  
    }
}
