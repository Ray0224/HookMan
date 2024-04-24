using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedDetect : MonoBehaviour
{
    public float timeInterval;
    public float time;
    public Vector3 t0Position;
    public Vector3 t1Position;
    public Vector3 speed;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (time == 0)
        {
            t0Position = transform.position;
        }
        time += Time.deltaTime;
        
        if(time >= timeInterval)
        {           
            t1Position = transform.position;
            Vector3 offset = (t1Position - t0Position);
            speed = offset / time;
            time = 0;
        }
    }
}
