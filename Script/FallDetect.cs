using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallDetect : MonoBehaviour
{
    [SerializeField]
    private Vector2 detectDirection;
    [SerializeField]
    private float detectDistance;
    [SerializeField]
    private LayerMask detectMask;
    
    // Update is called once per frame
    void Update()
    {

        RaycastHit2D hit = Physics2D.Raycast(transform.position, detectDirection, detectDistance, detectMask);
        
        Debug.DrawRay(transform.position, detectDirection.normalized * detectDistance);
        
        if (hit.collider != null)
        {
            GetComponent<Rigidbody2D>().bodyType =  RigidbodyType2D.Dynamic;
        }
    }
}
