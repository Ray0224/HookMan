using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    Vector2 originalPosition;
    [SerializeField]
    Vector2 maxPosition;
    Rigidbody2D myBody;
    float flyTime;
    [Header("®µ≈ﬁ")]
    [SerializeField]
    float maxPatrolTime;
    [SerializeField]
    float patrolSpeed;
    [Header("∞l¿ª")]
    [SerializeField]
    float maxChasingTime;
    [SerializeField]
    float ChasingSpeed;
    [SerializeField]
    float rayOffset;
    [SerializeField]
    float rayLenth;
    // Start is called before the first frame update
    void Start()
    {
        myBody = GetComponent<Rigidbody2D>();
        originalPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Rotation();
        ChangeDirection();
        MaxPosition();
        Chase();
    }
    void Rotation()
    {
        float angle = Mathf.Atan2(myBody.velocity.y, myBody.velocity.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }
    void ChangeDirection()
    {
        if(flyTime > 0)
        {
            flyTime -= Time.deltaTime;
        }
        else
        {
            Patrol();
        }
    }
    void Patrol()
    {
        Vector2 direction = Random.insideUnitCircle;
        myBody.velocity = direction * patrolSpeed;
        flyTime = maxPatrolTime;
    }
    void Chase()
    {
        Vector2 position = transform.position;
        RaycastHit2D hit2D = Physics2D.Raycast(position + myBody.velocity.normalized * rayOffset, myBody.velocity.normalized, rayLenth,LayerMask.GetMask("Player"));
        Debug.DrawRay(position + myBody.velocity.normalized * rayOffset, myBody.velocity.normalized * rayLenth, Color.red);
        if (hit2D == true)
        {
            Vector2 direction = hit2D.transform.position - transform.position;
            myBody.velocity = direction * ChasingSpeed;
            flyTime = maxChasingTime;
        }
    }
    void MaxPosition()
    {
        float offsetX = transform.position.x - originalPosition.x;
        float offsetY = transform.position.y - originalPosition.y;
        float x = Mathf.Abs(offsetX);
        float y = Mathf.Abs(offsetY);
        if (x > maxPosition.x || y > maxPosition.y)
        {
            Vector2 direction = new Vector2(-offsetX, -offsetY).normalized;
            myBody.velocity = direction * patrolSpeed;
            flyTime = maxPatrolTime;
        }

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Patrol();
    }
}
