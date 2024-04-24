using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.U2D;


public class Anchor : MonoBehaviour
{
    public enum CurrentState
    {
        isReady,isShooting,isDropping,isWallContacted, isObjectContacted
    }
    [Header("Ball")]
    public Rigidbody2D ballRigidbody;
    public float force;
    [Header("自身狀態")]
    public CurrentState currentState;
    Rigidbody2D body;
    CapsuleCollider2D capsuleCollider2;
    Quaternion currentAngle;
    [Header("BoxLayer")]
    public GameObject targetObject;
    public FixedJoint2D targetJoint;
    [Header("Gun")]
    public GrapplingGun gun;
    public Transform shootPoint;
    [Header("粒子系統")]
    public ParticleSystem particle;
    public ParticleSystem mossParticle;
    public Transform mossParticleTransform;
    float mossTouchedTimes = 0;
    public SpriteShape mossSpriteShape;

    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        capsuleCollider2 = GetComponent<CapsuleCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        CheckState();

    }
    void CheckState()
    {
        if (currentState == CurrentState.isReady)
        {
            transform.position = shootPoint.position;
            transform.rotation = shootPoint.rotation;

        }
        else if((currentState == CurrentState.isShooting || currentState == CurrentState.isDropping) && body.velocity != Vector2.zero)
        {
            float angle = Mathf.Atan2(body.velocity.y, body.velocity.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle);
            currentAngle = Quaternion.Euler(0, 0, angle);
        }
        else
        {
            return;
        }

    }
    public void Ready()
    {
        currentState = CurrentState.isReady;
        
        body.bodyType = RigidbodyType2D.Static;
        capsuleCollider2.enabled = false;
        mossTouchedTimes = 0;
        if(gun.outOfAction == false)
        {
            particle.Play();
        }   
    }
    public void Shooting(Vector2 Speed)
    {
        currentState = CurrentState.isShooting;
        body.bodyType = RigidbodyType2D.Dynamic;
        capsuleCollider2.enabled = true;
        body.velocity = Speed;
        ballRigidbody.AddForce(-Speed * force, ForceMode2D.Impulse);
    }
    public void Dropping()
    {
        currentState = CurrentState.isDropping;
        body.bodyType = RigidbodyType2D.Dynamic;
        if (targetObject != null)
        {
            if(targetObject.GetComponent<FixedJoint2D>() != null)
            {
                Destroy(targetJoint);
            }
            targetObject = null;
        }
       /* if(GetComponent<ParentConstraint>() != null)
        {
            Destroy(GetComponent<ParentConstraint>());
        }*/
       
 
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {

        if(currentState == CurrentState.isShooting)
        {
            if (collision.gameObject.layer == LayerMask.NameToLayer("WallLayer"))
            {
                currentState = CurrentState.isWallContacted;
                body.bodyType = RigidbodyType2D.Static;
                transform.rotation = currentAngle;
            }
            if (collision.gameObject.layer == LayerMask.NameToLayer("BoxLayer"))
            {
                targetObject = collision.gameObject;
                currentState = CurrentState.isObjectContacted;
                body.velocity = Vector2.zero;
                targetJoint = collision.gameObject.AddComponent<FixedJoint2D>();
                targetJoint.autoConfigureConnectedAnchor = false;
                targetJoint.connectedBody = body;
                targetJoint.anchor = collision.gameObject.transform.InverseTransformPoint(collision.contacts[0].point);
                targetJoint.connectedAnchor = Vector2.zero;
            }
            if (collision.gameObject.layer == LayerMask.NameToLayer("UnHookedWall") && mossTouchedTimes == 0 && collision.gameObject.GetComponent<SpriteShapeController>() != null)
            {

                if (collision.gameObject.GetComponent<SpriteShapeController>().spriteShape == mossSpriteShape)
                {
                    float angle;
                    if (collision.contacts[0].normal.x > 0)
                    {
                        angle = -Vector2.Angle(Vector2.up, collision.contacts[0].normal);
                    }
                    else
                    {
                        angle = Vector2.Angle(Vector2.up, collision.contacts[0].normal);
                    }

                    mossParticleTransform.position = collision.contacts[0].point;
                    mossParticleTransform.eulerAngles = new Vector3(0, 0, angle);
                    mossParticle.Play();
                    mossTouchedTimes += 1;
                }
                else
                {
                    return;
                }

            }
            if (collision.gameObject.layer == LayerMask.NameToLayer("FrontAnchorBox"))
            {
                if(this.name == "FrontAnchor")
                {
                    targetObject = collision.gameObject;
                    currentState = CurrentState.isObjectContacted;
                    body.velocity = Vector2.zero;
                    targetJoint = collision.gameObject.AddComponent<FixedJoint2D>();
                    targetJoint.autoConfigureConnectedAnchor = false;
                    targetJoint.connectedBody = body;
                    targetJoint.anchor = collision.gameObject.transform.InverseTransformPoint(collision.contacts[0].point);
                    targetJoint.connectedAnchor = Vector2.zero;
                }
                else if(this.name == "RearAnchor")
                {
                    gun.StopShooting();
                    gun.Dropping();
                    
                }

            }
            if (collision.gameObject.layer == LayerMask.NameToLayer("RearAnchorBox"))
            {
                if (this.name == "RearAnchor")
                {
                    targetObject = collision.gameObject;
                    currentState = CurrentState.isObjectContacted;
                    body.velocity = Vector2.zero;
                    targetJoint = collision.gameObject.AddComponent<FixedJoint2D>();
                    targetJoint.autoConfigureConnectedAnchor = false;
                    targetJoint.connectedBody = body;
                    targetJoint.anchor = collision.gameObject.transform.InverseTransformPoint(collision.contacts[0].point);
                    targetJoint.connectedAnchor = Vector2.zero;
                }
                else if (this.name == "FrontAnchor")
                {
                    gun.StopShooting();
                    gun.Dropping();
                }
            }
            
        }

        
    }



}
