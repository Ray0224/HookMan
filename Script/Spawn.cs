using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour
{
    public GameObject[] spawnTemplate;
    public float[] respawnTime;

    private float[] timer;
    private GameObject[] spawnedObject;
    
    void Start()
    {
        timer = new float[spawnTemplate.Length];
        spawnedObject = new GameObject[spawnTemplate.Length];
        
        for (int i = 0 ; i < spawnTemplate.Length ; i++){
            spawnedObject[i] = Instantiate(spawnTemplate[i], spawnTemplate[i].transform.parent);
            spawnedObject[i].SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0 ; i < spawnedObject.Length ; i++){
            if(spawnedObject[i] == null){
                timer[i] += Time.deltaTime;

                if(timer[i] >= respawnTime[i]){
                    timer[i] = 0;
                    spawnedObject[i] = Instantiate(spawnTemplate[i], spawnTemplate[i].transform.parent);
                    spawnedObject[i].SetActive(true);
                }
            }
        }
    }
}
