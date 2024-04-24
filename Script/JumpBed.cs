using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpBed : MonoBehaviour
{

    public Animator anim;
    public LayerMask mask;
    public float minJumpForce;
    public float maxJumpForce;

    void OnCollisionEnter2D(Collision2D collision)
    {

        if ((mask.value & (1 << collision.gameObject.layer)) != 0)
        {
            ContactPoint2D contact = collision.GetContact(0);

            Vector2 velocity = collision.relativeVelocity;
            Vector2 normalVec = -contact.normal.normalized;

            float normalSpeed = Vector2.Dot(velocity, normalVec);

            float jumpForce = Mathf.Min(normalSpeed, maxJumpForce);
            jumpForce = Mathf.Max(normalSpeed, minJumpForce);

            Rigidbody2D rigidbody = collision.gameObject.GetComponent<Rigidbody2D>();
            rigidbody.AddForce(jumpForce * normalVec, ForceMode2D.Impulse);

            anim.Play("Base Layer.JumpPad", 0, 0);
        }
    }
}
