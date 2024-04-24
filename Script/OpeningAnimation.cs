using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class OpeningAnimation : MonoBehaviour
{
    public PlayerInput playerInput1;
    public PlayerInput playerInput2;
    public SpriteRenderer[] cats;
    public GameObject Cat;
    public GameObject shadow;
    public CameraFollow cameraFollow;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Initialized()
    {
        for (int i = 0; i < cats.Length; i++)
        {
            cats[i].color = new Color(1, 1, 1, 0);
        }
        shadow.SetActive(false);
        playerInput1.enabled = false;
        playerInput2.enabled = false;
        cameraFollow.enabled = false;
    }
    public void DestroyCat()
    {
        for(int i = 0; i < cats.Length; i++)
        {
            cats[i].color = new Color(1, 1, 1, 1);
        }
        shadow.SetActive(true);
        playerInput1.enabled = true;
        playerInput2.enabled = true;
        cameraFollow.enabled = true;
        Destroy(Cat);
    }
    
}
