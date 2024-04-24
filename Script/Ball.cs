using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public DialogManager dialogManager;
    Rigidbody2D myBody;
    public bool isTouchingTunel;
    [Header("Cat")]
    [SerializeField]
    SpriteRenderer cat;
    [SerializeField]
    Sprite[] catSprites;
    [SerializeField]
    float thresholdSpeed;

    [Header("Particle")]
    public ParticleSystem particle;
    public Transform particleTransform;
    public float particleSpeed;

    [Header("HurtParticle")]
    public ParticleSystem hurtParticle;


    [Header("Trap")]
    public float trapForce;
    public float maxTrapTime = 3;
    public float trapTime;
    public int traptimes = 0;

    [Header("Gun")]
    public GrapplingGun frontGun;
    public GrapplingGun rearGun;
    // Start is called before the first frame update
    [Header("Speed")]
    public float timeInterval;
    float time;
    Vector3 t0Position;
    Vector3 t1Position;
    public Vector3 speed;
    void Start()
    {
        dialogManager = GameObject.FindObjectOfType<DialogManager>();
        if (this.name != "Ball")
        {
            return;
        }
        myBody = GetComponent<Rigidbody2D>();
        myBody.centerOfMass = new Vector2(0, -0.15f);
        t0Position = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if(this.name != "Ball")
        {
            return;
        }
        DetectSpeed();
        CheckAngle();
        ChangeDirection();
        //CheckTrap();
    }
    void DetectSpeed()
    {
        t1Position = transform.position;
        speed = (t1Position - t0Position) / Time.deltaTime;
        t0Position = transform.position;
        /*
        if (time == 0)
        {
            t0Position = transform.position;
        }
        time += Time.deltaTime;

        if (time >= timeInterval)
        {
            t1Position = transform.position;
            Vector3 offset = (t1Position - t0Position);
            speed = offset / time;
            time = 0;
        }*/
    }
    void CheckAngle()
    {
        if(transform.eulerAngles.z >45 && transform.eulerAngles.z <= 180)
        {
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, 45);
        }
        else if ( transform.eulerAngles.z > 180 && transform.eulerAngles.z < 315)
        {
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, 315);
        }
    }
    void ChangeDirection()
    {
        if(cat.sprite == catSprites[0])
        {
            if(myBody.velocity.x <0)
            {
                cat.sprite = catSprites[1];
            }
            else
            {
                return;
            }
        }
        else if (cat.sprite == catSprites[1])
        {
            if (myBody.velocity.x <= -thresholdSpeed)
            {
                cat.sprite = catSprites[2];
            }
            else if (myBody.velocity.x >= thresholdSpeed)
            {
                cat.sprite = catSprites[0];
            }
            else
            {
                return;
            }
        }
        else if (cat.sprite == catSprites[2])
        {
            if (myBody.velocity.x >0 )
            {
                cat.sprite = catSprites[1];
            }
            else
            {
                return;
            }
        }
    }
    void CheckTrap()
    {
        if (trapTime > 0)
        {
            trapTime -= Time.deltaTime;
            if (traptimes >= 2)
            {
                int a = Random.Range(0, dialogManager.ReuseFiles.Length);
                dialogManager.CloseOldDialogBox();
                dialogManager.ShowDialogBox(dialogManager.ReuseFiles[a]);
                traptimes = 0;
            }
        }
        else if (trapTime < 0)
        {
            trapTime = 0;
            traptimes = 0;
        }
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (this.name != "Ball")
        {
            return;
        }
        if (collision.gameObject.layer == LayerMask.NameToLayer("WallLayer") && myBody.velocity.magnitude >= particleSpeed)
        {
            float angle;
            if(collision.contacts[0].normal.x > 0)
            {
                angle = -Vector2.Angle(Vector2.up, collision.contacts[0].normal);
            }
            else
            {
                angle = Vector2.Angle(Vector2.up, collision.contacts[0].normal);
            }
            
            particleTransform.position = collision.contacts[0].point;
            particleTransform.eulerAngles = new Vector3(0,0,angle);
            particle.Play();
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (this.name != "Ball")
        {
            return;
        }
        if (collision.gameObject.layer == LayerMask.NameToLayer("Trap") ) 
        {
            myBody.velocity = Vector2.zero;
            myBody.AddForce(collision.contacts[0].normal * trapForce, ForceMode2D.Impulse);
            frontGun.ShutDown();
            rearGun.ShutDown();
            hurtParticle.Play();
            trapTime = maxTrapTime;
            traptimes++;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer == LayerMask.NameToLayer("Trigger"))
        {
            if (collision.gameObject.name == "Trigger0")
            {
                Destroy(collision.gameObject);
                dialogManager.CloseOldDialogBox();
                dialogManager.ShowDialogBox(dialogManager.textFiles[0]);

            }
            if (collision.gameObject.name == "Trigger1")
            {
                Destroy(collision.gameObject);
                dialogManager.CloseOldDialogBox();
                dialogManager.ShowDialogBox(dialogManager.textFiles[1]);
               
            }
            if (collision.gameObject.name == "Trigger2")
            {
                Destroy(collision.gameObject);
                dialogManager.CloseOldDialogBox();
                dialogManager.ShowDialogBox(dialogManager.textFiles[2]);
            }
            if (collision.gameObject.name == "Trigger3")
            {
                Destroy(collision.gameObject);
                dialogManager.CloseOldDialogBox();
                dialogManager.ShowDialogBox(dialogManager.textFiles[3]);
            }
            if (collision.gameObject.name == "Trigger4")
            {
                Destroy(collision.gameObject);
                dialogManager.CloseOldDialogBox();
                dialogManager.ShowDialogBox(dialogManager.textFiles[4]);
            }

        }

    }

}
