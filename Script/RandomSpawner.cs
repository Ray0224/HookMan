using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSpawner : MonoBehaviour
{
    public GameObject spawn;
    public int numSpawner;
    public float minCoolDown;
    public float maxCoolDown;

    public Transform startPosition;
    public Transform endPosition;

    private float[] spawnLeftTime;

    private int player_layer_idx = 10;
    
    void Start(){
        spawnLeftTime = new float[numSpawner];
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        // If the player is the colliding object.
        if(collider.gameObject.layer == player_layer_idx)
        {
            for(int i=0 ; i < numSpawner ; i++){
                spawnLeftTime[i] = Random.Range(minCoolDown, maxCoolDown);
            }
        }
    }

    void OnTriggerStay2D(Collider2D collider)
    {
        // If the player is the colliding object.
        if(collider.gameObject.layer == player_layer_idx)
        {
            for(int i=0 ; i < numSpawner ; i++){
                spawnLeftTime[i] -= Time.fixedDeltaTime;

                if(spawnLeftTime[i] <= 0){
                    Vector3 spawnPosition = Vector3.Lerp(startPosition.position, endPosition.position, Random.Range(0f, 1f));

                    GameObject spawnedObj = Instantiate(spawn, spawnPosition, spawn.transform.rotation, spawn.transform.parent);
                    spawnedObj.SetActive(true);

                    spawnLeftTime[i] = Random.Range(minCoolDown, maxCoolDown);
                }
            }
        }
    }

    
}
