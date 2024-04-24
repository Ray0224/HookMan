using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GrapplingGun : MonoBehaviour
{

    [Header("Rope segment ref. GameObject")]
    public GameObject ropeSegmentObj;

    [Header("GameObject connected to rope")]
    public GameObject hook;
    public GameObject gunHolder;

    [Header("Rope segments settings")]
    public Transform rope;
    public LineRenderer lineRenderer;
    [Range(0.1f, 0.5f)] public float ropeSegLen = 0.25f;
    [Range(20, 50)] public int numSegment = 35;
    public bool useSpring = false;
    public float dampingRatio = 1f;
    public float frequency = 0f;
    

    [Header("Hook shooting & shrinking")]
    [Range(10f, 100f)] public float hookShootSpeed = 50f;
    public float shrinkToHookForce = 1f;
    [Range(0f, 50f)] public float shrinkToHookTimeThreshold = 0.04f;
     public float shrinkToPlayerTimeThreshold = 0.02f;

    public Transform shootPoint;
    public GunParticle gunParticle;
    
    [Header("Gun settings")]
    public float rotationSpeed = 100f;

    [Header("Energy Charging Settings")]
    public GrapplingGun anotherGun;
    public bool energyCharged = false;
    public float chargeTime = 1.0f;
    public float energyShootSpeed = 10f;
    public int minSegToChargeAnotherGun = 1;
    [HideInInspector]
    public float shrinkTime = 0f;
    
    [Header("Gun repairing settings")]
    public float repairTime = 1f;
    public bool outOfAction = true;

    [HideInInspector]
    public List<GameObject> ropeSegments = new List<GameObject>();
    
    public state currentState;
    [HideInInspector]
    public bool shrinkRope;

    public enum state
    {
        isReady,
        isShooting,
        isDropping,
        isHooked,
    }
    
    private HingeJoint2D gunHolderHingeJoint;
    private DistanceJoint2D gunHolderDistanceJoint;
    private SpringJoint2D gunHolderSpringJoint;
    private Vector2 segConnectedAnchor = new Vector2(-0.5f, 0f);
    private Vector2 hookConnectedAnchor = new Vector2(-0.07f, 0f);
    private float outOfActionTime;
    
    
    // Start is called before the first frame update
    void Start()
    {
        gunHolderHingeJoint = gunHolder.AddComponent<HingeJoint2D>();
        gunHolderHingeJoint.enabled = false;
        gunHolderHingeJoint.autoConfigureConnectedAnchor = false;

        gunHolderDistanceJoint = gunHolder.AddComponent<DistanceJoint2D>();
        gunHolderDistanceJoint.enabled = false;
        gunHolderDistanceJoint.autoConfigureDistance = false;
        gunHolderDistanceJoint.distance = ropeSegLen;
        gunHolderDistanceJoint.maxDistanceOnly = true;

        gunHolderSpringJoint = gunHolder.AddComponent<SpringJoint2D>();
        gunHolderSpringJoint.enabled = false;
        gunHolderSpringJoint.autoConfigureDistance = false;
        gunHolderSpringJoint.dampingRatio = dampingRatio;
        gunHolderSpringJoint.distance = 0.005f;
        gunHolderSpringJoint.frequency = frequency;

        GameObject newSegment = GenerateRopeSegment(shootPoint.position, Vector2.right);
        ropeSegments.Add(newSegment);

        initToReady();
    }

    // Update is called once per frame
    void Update()
    {   
        Vector3 lookVec;
        if(currentState == state.isReady){
            lookVec = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue()) - gunHolder.transform.position;
            RotateGun(lookVec, false);
        }
        else{
            lookVec = GetDistanceVec(gunHolder.transform, ropeSegments[ropeSegments.Count - 1].transform);
            RotateGun(lookVec, true);
        }

    }

    void FixedUpdate(){
        if(outOfAction){
            outOfActionTime += Time.fixedDeltaTime;
            
            if(outOfActionTime >= repairTime){
                outOfAction = false;
            }
        }

        if(currentState == state.isShooting){
            if (hook.GetComponent<Anchor>().currentState == Anchor.CurrentState.isWallContacted || hook.GetComponent<Anchor>().currentState == Anchor.CurrentState.isObjectContacted)
            {
                StopShooting();
                currentState = state.isHooked;
            }
            else if(shrinkRope){
                
                shrinkTime += Time.fixedDeltaTime;

                if(!gunHolderDistanceJoint.enabled){
                    StopShooting();
                }

                if(shrinkTime >= shrinkToPlayerTimeThreshold){
                    ShrinkToPlayer();
                    shrinkTime = 0f;
                }

                if(ropeSegments.Count == 1){
                    initToReady();
                }
            }
            else
            {
                LengthenRope("round");
            }
        }
        else if(shrinkRope && currentState == state.isHooked){
            shrinkTime += Time.fixedDeltaTime;

            //if(shrinkTime >= shrinkToHookTimeThreshold){

            if(ShrinkToHook()){
                shrinkTime = 0f;
            }
            else if(shrinkTime >= chargeTime && anotherGun.currentState == state.isHooked && ropeSegments.Count >= minSegToChargeAnotherGun){
                anotherGun.energyCharged = true;
            }
            
        }
        else if(currentState == state.isDropping)
        {
           shrinkTime += Time.fixedDeltaTime;
            
            if(shrinkTime >= shrinkToPlayerTimeThreshold){
                ShrinkToPlayer();
                shrinkTime = 0f;
            }
                
            if(ropeSegments.Count == 1){
                initToReady();
            }
        }
    }

    public void Shoot(){
        currentState = state.isShooting;
        hook.transform.position = shootPoint.transform.position;

        gunParticle.PlayShootParticle();

        Vector2 forceVec = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue()) - gunHolder.transform.position;
        forceVec = forceVec.normalized;
        /*
        if(useSpring){
            gunHolderSpringJoint.enabled = false;
            ropeSegments[0].GetComponent<SpringJoint2D>().enabled = true;
        }
        else{
            gunHolderDistanceJoint.enabled = false;
            gunHolderHingeJoint.enabled = false;
            ropeSegments[0].GetComponent<DistanceJoint2D>().enabled = true;
            ropeSegments[0].GetComponent<HingeJoint2D>().enabled = true;
            
        }
        */
        gunHolderDistanceJoint.enabled = false;
        gunHolderHingeJoint.enabled = false;
        gunHolderSpringJoint.enabled = false;
        ropeSegments[0].GetComponent<DistanceJoint2D>().enabled = true;
        ropeSegments[0].GetComponent<HingeJoint2D>().enabled = true;
        
        hook.GetComponent<Anchor>().Shooting(forceVec * hookShootSpeed);
        lineRenderer.enabled = true;     
    }

    public void StopShooting(){
        ZeroRopeSpeed();
        ConnectJoint(gunHolder, ropeSegments[ropeSegments.Count - 1], Vector2.zero, segConnectedAnchor);
    }

    public void Dropping(){
        shrinkTime = 0f;
        hook.GetComponent<Anchor>().Dropping();
        
        energyCharged = false;
        if(anotherGun.energyCharged){
            anotherGun.energyChargedShoot();
        }
        currentState = state.isDropping;
    }

    public void energyChargedShoot(){
        Vector2 forceVec;
        if(ropeSegments.Count == 1){
            forceVec = GetDistanceVec(gunHolder.transform, ropeSegments[ropeSegments.Count - 1].transform);
        }
        else{
            forceVec = GetDistanceVec(ropeSegments[ropeSegments.Count - 1].transform, ropeSegments[ropeSegments.Count - 2].transform);
        }
        forceVec = forceVec.normalized;
        
        if(energyCharged){
            energyCharged = false;
            
            for(int i=1;i<ropeSegments.Count;i++){
                ropeSegments[i].GetComponent<Rigidbody2D>().AddForce(2 * forceVec, ForceMode2D.Impulse);
            }
            
            gunHolder.GetComponent<Rigidbody2D>().AddForce(energyShootSpeed * forceVec, ForceMode2D.Impulse);
        }
    }


    // Called when player collide with trap.
    public void ShutDown(){
        outOfActionTime = 0f;
    
        if(currentState != state.isReady){
            Dropping();
        }

        outOfAction = true;
    }

    private void ShrinkToPlayer(){
        if(ropeSegments.Count > 1){
            ConnectJoint(ropeSegments[1], hook, -segConnectedAnchor, hookConnectedAnchor);

            GameObject ropeSegment = ropeSegments[0];

            hook.transform.position = ropeSegment.transform.position;
            hook.transform.rotation = ropeSegment.transform.rotation;

            ropeSegments.RemoveAt(0);

            Destroy(ropeSegment);
        }
    }

    private bool ShrinkToHook()
    {
        Rigidbody2D rigidBody = gunHolder.GetComponent<Rigidbody2D>();
        GameObject nearestSegment = ropeSegments[ropeSegments.Count - 1];

        Vector2 distanceVec = GetDistanceVec(gunHolder.transform, ropeSegments[ropeSegments.Count - 1].transform);
        Vector2 forceVec = distanceVec.normalized;

        if(ropeSegments.Count > 2){
            rigidBody.AddForce(forceVec * shrinkToHookForce);
        }
        
        /*best = 4 / 7 */
        if (ropeSegments.Count > 2 && distanceVec.magnitude <= ropeSegLen * 0.575f && shrinkTime >= shrinkToHookTimeThreshold && !energyCharged)
        {
            ConnectJoint(gunHolder, ropeSegments[ropeSegments.Count - 2], Vector2.zero, segConnectedAnchor);
            ropeSegments.RemoveAt(ropeSegments.Count - 1);
            Destroy(nearestSegment);
            return true;
        }
        else{
            return false;
        }
    }

    private void initToReady(){
        lineRenderer.enabled = false;

        GameObject firstSegment = ropeSegments[0];
        ConnectJoint(firstSegment, hook, -1 * segConnectedAnchor, hookConnectedAnchor);
        ConnectJoint(gunHolder, firstSegment, Vector2.zero, segConnectedAnchor);

        firstSegment.GetComponent<HingeJoint2D>().enabled = false;
        firstSegment.GetComponent<DistanceJoint2D>().enabled = false;
        firstSegment.GetComponent<SpringJoint2D>().enabled = false;

        currentState = state.isReady;
        hook.GetComponent<Anchor>().Ready();
    }

    private void LengthenRope(string mode){
        GameObject nearestSegment = ropeSegments[ropeSegments.Count - 1];

        Vector2 distanceVec = GetDistanceVec(gunHolder.transform, nearestSegment.transform);
        float distance = distanceVec.magnitude;
        int numNewSeg = 0;

        if(mode == "round"){
            numNewSeg = Mathf.RoundToInt(distance / ropeSegLen);
        }
        else if(mode == "ceiling"){
            numNewSeg = Mathf.CeilToInt(distance / ropeSegLen);
        }
        numNewSeg = Mathf.Min(numNewSeg, numSegment - ropeSegments.Count);

        for(int i=0;i < numNewSeg;i++){
            Vector2 newPosition = (Vector2)nearestSegment.transform.position - distanceVec.normalized * ropeSegLen / 2;
            GameObject newSegment = GenerateRopeSegment(newPosition, distanceVec);
            
            ConnectJoint(newSegment, nearestSegment, -1 * segConnectedAnchor, segConnectedAnchor);

            //.GetComponent<Rigidbody2D>().AddForce(distanceVec.normalized * 0.5f, ForceMode2D.Impulse);
            //Debug.Log(distanceVec);
            ropeSegments.Add(newSegment);

            nearestSegment = newSegment;

            distanceVec = GetDistanceVec(gunHolder.transform, nearestSegment.transform);
            distance = distanceVec.magnitude;

            if(ropeSegments.Count == numSegment){
                StopShooting();
            }
        }

    }

    private void ConnectJoint(GameObject obj1, GameObject obj2, Vector2 anchor, Vector2 connectedAnchor){
        HingeJoint2D hingeJoint;
        DistanceJoint2D distanceJoint;
        SpringJoint2D springJoint;

        if(obj1 == gunHolder){
            hingeJoint = gunHolderHingeJoint;
            distanceJoint = gunHolderDistanceJoint;
            springJoint = gunHolderSpringJoint;

            if(useSpring){
                springJoint.connectedBody = obj2.GetComponent<Rigidbody2D>();
                springJoint.anchor = anchor;
                springJoint.connectedAnchor = connectedAnchor;
                /*
                springJoint.autoConfigureConnectedAnchor = false;
                springJoint.autoConfigureDistance = false;
                springJoint.dampingRatio = dampingRatio;
                springJoint.distance = ropeSegLen;
                springJoint.frequency = frequency;
                */
                springJoint.enabled = true;

                distanceJoint.connectedBody = obj2.GetComponent<Rigidbody2D>();
                /*
                distanceJoint.distance = ropeSegLen;
                */
                distanceJoint.enabled = true;

                hingeJoint.connectedBody = obj2.GetComponent<Rigidbody2D>();
                return;
            }

        }
        else{
            hingeJoint = obj1.GetComponent<HingeJoint2D>();
            distanceJoint = obj1.GetComponent<DistanceJoint2D>();
            springJoint = obj1.GetComponent<SpringJoint2D>();
        }

        
        distanceJoint.connectedBody = obj2.GetComponent<Rigidbody2D>();
        /*
        distanceJoint.distance = ropeSegLen;
        */
        distanceJoint.enabled = true;

        hingeJoint.connectedBody = obj2.GetComponent<Rigidbody2D>();
        /*
        distanceJoint.distance = ropeSegLen;
        hingeJoint.autoConfigureConnectedAnchor = false;
        */
        hingeJoint.anchor = anchor;
        hingeJoint.connectedAnchor = connectedAnchor;
        hingeJoint.enabled = true;
    }

    private void ZeroRopeSpeed(){
        foreach (GameObject seg in ropeSegments)
        {
            Rigidbody2D rigidBody = seg.GetComponent<Rigidbody2D>();
            rigidBody.velocity = Vector3.zero;   
        }
    }

    private GameObject GenerateRopeSegment(Vector2 position, Vector2 rotationVec){
        GameObject obj = Instantiate(ropeSegmentObj, position, Quaternion.identity);

        HingeJoint2D hingeJoint = obj.GetComponent<HingeJoint2D>();
        DistanceJoint2D distanceJoint = obj.GetComponent<DistanceJoint2D>();
        SpringJoint2D springJoint = obj.GetComponent<SpringJoint2D>();
        
        obj.transform.rotation =  Quaternion.FromToRotation(Vector2.right, rotationVec);

        obj.transform.parent = rope;
        obj.transform.localScale -= new Vector3(1 - ropeSegLen, 0f, 0f);
        obj.SetActive(true);

        springJoint.autoConfigureConnectedAnchor = false;
        springJoint.autoConfigureDistance = false;
        springJoint.dampingRatio = dampingRatio;
        springJoint.distance = 0.005f;
        springJoint.frequency = frequency;

        distanceJoint.distance = ropeSegLen;
        hingeJoint.autoConfigureConnectedAnchor = false;
        
        return obj;
    }

    private void RotateGun(Vector3 lookVec, bool rotateOverTime){
        float angle = Mathf.Atan2(lookVec.y, lookVec.x) * Mathf.Rad2Deg;

        Quaternion targetRotation = Quaternion.AngleAxis(angle, Vector3.forward);

        if(rotateOverTime){
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
        else{
            transform.rotation = targetRotation;
        }
         
    }

    private Vector2 GetDistanceVec(Transform trans1, Transform trans2){
        return trans2.position - trans1.position;
    }

}