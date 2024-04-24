using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogTunel2 : MonoBehaviour
{
    public GameObject[] targetObjects;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            for(int i = 0; i < targetObjects.Length; i++)
            {
                targetObjects[i].SetActive(false);
            }
            
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            for (int i = 0; i < targetObjects.Length; i++)
            {
                targetObjects[i].SetActive(true);
            }

        }
    }
}
