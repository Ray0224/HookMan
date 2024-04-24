using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeOut : MonoBehaviour
{
    public float fadeOutSpeed = 0.05f;

    private Color color;
    private float alpha = 1.0f;

    void Start()
    {
        color = GetComponent<MeshRenderer>().material.color;
    }

    // Update is called once per frame
    void Update()
    {
        DecreaseOpacity();
    }

    void DecreaseOpacity(){
        alpha -= fadeOutSpeed;

        if(alpha <= 0){
            // when opacity == 0 destroy this GameOject
            Destroy(gameObject);
        }
        else{
            GetComponent<MeshRenderer>().material.color = new Color(color.r, color.g, color.b, alpha);
        }
    }
    
}
