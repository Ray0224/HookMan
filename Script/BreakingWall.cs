using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakingWall : MonoBehaviour
{
    public float breakSpeed;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.layer == LayerMask.NameToLayer("BoxLayer") || collision.gameObject.layer == LayerMask.NameToLayer("FrontAnchorBox")||
            collision.gameObject.layer == LayerMask.NameToLayer("RearAnchorBox"))
        {
            Vector2 speed = collision.gameObject.GetComponent<Rigidbody2D>().velocity;
            Debug.Log(speed.magnitude);
            if(speed.magnitude < breakSpeed)
            {
                return;
            }
            else
            {
                Debug.Log("ok");
                Destroy(this.gameObject);
            }
        }
        
    }
}
