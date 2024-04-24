using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTail : MonoBehaviour
{
    public int length;
    public LineRenderer lineRenderer;
    public float smoothSpeed;
    public float speedThreshold;

    private Vector3[] tailPoints;
    private Vector3[] tailPointsV;
    

    void Start(){
        lineRenderer.positionCount = length;
        tailPoints = new Vector3[length];
        tailPointsV = new Vector3[length];

    }
    // Update is called once per frame
    void Update()
    {

        bool show = false;

        tailPoints[0] = transform.position;

        for(int i=1;i<length;i++){    
            tailPoints[i] = Vector3.SmoothDamp(tailPoints[i], tailPoints[i-1], ref tailPointsV[i], smoothSpeed);

            if(tailPointsV[i].magnitude > speedThreshold){
                show = true;
            }
        }

        lineRenderer.SetPositions(tailPoints);

        lineRenderer.enabled = show;
    }
}
