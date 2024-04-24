using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Explodable))]
public class Shatter : MonoBehaviour
{
    public LayerMask canShatter;
    public bool fadeOut;
    public float fadeOutSpeed = 0.05f;

    private Explodable explodable;

    void Start()
    {
        explodable = GetComponent<Explodable>();
    }

    void OnCollisionEnter2D(Collision2D collision){
        
        if((canShatter.value & 1<<collision.gameObject.layer) != 0){
            explodable.explode();    
        }
        
    }

    void OnDestroy(){
        if(fadeOut){
            foreach (GameObject frag in explodable.fragments)
            {
                FadeOut fade = frag.AddComponent<FadeOut>();
                fade.fadeOutSpeed = fadeOutSpeed;
            }
        }
    }
}
