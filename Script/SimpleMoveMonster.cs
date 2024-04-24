using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleMoveMonster : MonoBehaviour
{
    int dir;
    public Vector2 pos1;
    public Vector2 pos2;
    Vector2 originPos;
    public float movingSpeed;
    float changeDirTime;
    public float maxChangeDirTime;

    // Start is called before the first frame update
    void Start()
    {
        originPos = transform.position;
        dir = 0;
    }

    // Update is called once per frame
    void Update()
    {
        ChangeDir();
        Move();
    }
    void ChangeDir()
    {
        changeDirTime += Time.deltaTime;
        if (changeDirTime >= maxChangeDirTime)
        {
            dir = dir == 0 ? 1 : 0;
            changeDirTime = 0;
        }
    }
    private void Move()
    {
        Vector2 pos = transform.position;
        if(dir == 0)
        {  
          transform.position = Vector2.Lerp(pos, originPos + pos1, Time.deltaTime * movingSpeed);

        }
        else 
        {
       
          transform.position = Vector2.Lerp(pos, originPos + pos2, Time.deltaTime * movingSpeed);
  
        }


    }
}
