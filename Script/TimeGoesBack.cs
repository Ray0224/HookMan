using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeGoesBack : MonoBehaviour
{
    public GrapplingGun frontGun;
    public GrapplingGun RearGun;
    public GameObject[] targets;
    List<Vector3> positions = new List<Vector3>();
    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < targets.Length; i++)
        {
            positions.Add(targets[i].transform.position);
        }

    }

    // Update is called once per frame
    void Update()
    {
 
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer == LayerMask.NameToLayer("Anchor"))
        {
            frontGun.Dropping();
            RearGun.Dropping();
            for (int i = 0; i < targets.Length; i++)
            {
                targets[i].transform.position = positions[i];
            }
        }
    }
}
