using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch : MonoBehaviour
{
    LayerMask switchLayer = new LayerMask();
    Vector3 originalPos;
    public float distance;
    [Header("改變目標")]
    public Transform target;
    Vector3 targetPos;
    public Vector3 offset;
    public float speed;
    // Start is called before the first frame update
    void Start()
    {
        originalPos = transform.position;
        targetPos = target.position;
        switchLayer = LayerMask.GetMask("BoxLayer") | LayerMask.GetMask("Player");
    }

    // Update is called once per frame
    void Update()
    {
        Check();
    }
    void Check()
    {
        Vector2 origin = transform.position;
        RaycastHit2D hit2D = Physics2D.Raycast(origin, Vector2.up, distance, switchLayer);
        if(hit2D == true)
        {
            transform.position = new Vector2(originalPos.x, originalPos.y - 0.2f);
            ToTheGoal();
        }
        else
        {
            transform.position = originalPos;
            ToTheOrigin();
        }
    }
    void ToTheGoal()
    {
        if(target.position != targetPos + offset)
        {
            target.position += (targetPos + offset- target.position).normalized * Time.deltaTime* speed;
        }
    }
    void ToTheOrigin()
    {
        if (target.position != targetPos)
        {
            target.position += (targetPos-target.position).normalized * Time.deltaTime * speed;
        }
    }
}
