using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class RopeSimulator : MonoBehaviour
{
    
    private LineRenderer lineRenderer;
    private List<RopeSegment> ropeSegments = new List<RopeSegment>();
    private float ropeSegLen = 0.25f;
    private int numSegment = 35;


    // Start is called before the first frame update
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        Vector3 ropeStartPoint = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());

        for (int i = 0; i < numSegment; i++)
        {
            this.ropeSegments.Add(new RopeSegment(ropeStartPoint));
            ropeStartPoint.y -= ropeSegLen;
        }
    }

    // Update is called once per frame
    void Update()
    {   
        DrawRope();
    }


    void FixedUpdate(){
        Simulate();
    }

    private void Simulate(){

        // Simulation
        Vector2 gravityForce = new Vector2(0f, -5f);

        for(int i = 0;i < numSegment;i++){

            RopeSegment seg = this.ropeSegments[i];
            Vector2 velocity = seg.newPos - seg.oldPos;

            seg.oldPos = seg.newPos;
            seg.newPos += velocity;


            Collider2D hitCollider = Physics2D.OverlapCircle(seg.newPos, 0.01f);
            
            if(hitCollider == null){
                seg.newPos += gravityForce * Time.fixedDeltaTime;
            }
            else{ 
                Debug.Log("HIT");
            }

            ropeSegments[i] = seg;
        }

        // Constraint
        for(int i = 0;i<100;i++){
            ApplyConstraint();
        }
        
    }

    private void ApplyConstraint(){
        RopeSegment firstSeg = ropeSegments[0];
        firstSeg.newPos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        ropeSegments[0] = firstSeg;

        for(int i = 0 ; i < numSegment-1 ; i++){
            RopeSegment seg1 = ropeSegments[i];
            RopeSegment seg2 = ropeSegments[i+1];

            float distance = (seg1.newPos - seg2.newPos).magnitude;
            float error = distance - ropeSegLen;

            Vector2 distanceVec = (seg2.newPos - seg1.newPos).normalized;
            Vector2 adjustVec1 = distanceVec, adjustVec2 = distanceVec;

            Collider2D hitCollider = Physics2D.OverlapCircle(seg1.newPos, 0.01f);
            
            if(hitCollider){
                adjustVec1 = FindAdjustVector(seg1, distanceVec, hitCollider);
            }
            

           hitCollider = Physics2D.OverlapCircle(seg2.newPos, 0.01f);
            
            if(hitCollider){
                adjustVec2 = FindAdjustVector(seg2, distanceVec, hitCollider);
            }
            
            if(i != 0){
                seg1.newPos += adjustVec1 * error * 0.5f;
                seg2.newPos -= adjustVec2 * error * 0.5f;
            }
            else{
                seg2.newPos -= adjustVec2 * error;
            }

            ropeSegments[i] = seg1;
            ropeSegments[i+1] = seg2;
        }


    }

    private Vector2 FindAdjustVector(RopeSegment seg, Vector3 baseVec, Collider2D collider){
        for(float deg = 0;deg <= 90 ;deg++){
            Vector2 adjustVec = Quaternion.AngleAxis(deg, -Vector3.forward) * baseVec; 
            Vector2 point = seg.newPos + adjustVec * 0.02f;
            
            if(!collider.bounds.Contains(point)){
                return adjustVec;
            }

            adjustVec = Quaternion.AngleAxis(deg, -Vector3.forward) * baseVec;
            point = seg.newPos + adjustVec * 0.02f;

            if(!collider.bounds.Contains(point)){
                return adjustVec;
            }
        }

        return baseVec;
    }

    private void DrawRope(){
        Vector3[] ropePositions = new Vector3[numSegment];
        for(int i = 0 ; i < numSegment ; i++){
            ropePositions[i] = ropeSegments[i].newPos;
        }

        lineRenderer.positionCount = ropePositions.Length;
        lineRenderer.SetPositions(ropePositions);
    }


    public struct RopeSegment
    {
        public Vector2 newPos, oldPos;

        public RopeSegment(Vector2 pos){
            newPos = pos;
            oldPos = pos;
        }
    }
}