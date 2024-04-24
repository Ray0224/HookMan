using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DropMaker : MonoBehaviour
{
    
    public Vector2 V0;
    public Transform makingPosition;
    public float makingTime;
    public GameObject dropPrefabs;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Drop());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    IEnumerator Drop()
    {
        GameObject drop = Instantiate(dropPrefabs,makingPosition.position,Quaternion.identity);
        if(drop.GetComponent<Rigidbody2D>()!= null)
        {
            
            drop.GetComponent<Rigidbody2D>().velocity = V0;
        }
        yield return new WaitForSecondsRealtime(makingTime);
        StartCoroutine(Drop());
    }
}
