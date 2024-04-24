using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopeDrawer : MonoBehaviour
{

    [Header("Rope start/end positions")]
    public Transform startPoint;
    public Transform hook;
    
    [Header("Grappling Gun")]
    public GrapplingGun grapplingGun;

    [Header("Line Renderer for Rope")]
    public LineRenderer lineRenderer;
    
    public Material initMaterial;
    public Material chargedMaterial;
    
    // Update is called once per frame
    void Update()
    {
        DrawRope();
    }

    void DrawRope(){
        if(grapplingGun.energyCharged){
            //lineRenderer.startColor = Color.red;
            //lineRenderer.endColor = Color.red;
            lineRenderer.material = chargedMaterial;
        }
        else{
            //lineRenderer.startColor = initColor;
            //lineRenderer.endColor = initColor;
            lineRenderer.material = initMaterial;
        }
        
        lineRenderer.positionCount = 2 + grapplingGun.ropeSegments.Count;

        Vector3[] positions = new Vector3[lineRenderer.positionCount];

        for(int i = 1 ;i<positions.Length-1;i++){
            positions[i] = grapplingGun.ropeSegments[i-1].GetComponent<Transform>().position;
        }

        positions[0] = hook.position;
        positions[positions.Length - 1] = startPoint.position;

        lineRenderer.SetPositions(positions);

        
    }
}
