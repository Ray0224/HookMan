using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlHint : MonoBehaviour
{
    public Renderer lightRenderer;
    
    public float intensity;

    private Color lightColor;
    private GrapplingGun gun;

    // Start is called before the first frame update
    void Start()
    {
        gun = GetComponent<GrapplingGun>();
        lightColor = lightRenderer.material.color;
    }

    // Update is called once per frame
    void Update()
    {
        if(gun.currentState == GrapplingGun.state.isReady){
            lightRenderer.material.color = lightColor;
        }
        else{
            lightRenderer.material.color = lightColor / Mathf.Pow(2, intensity);
        }
    }
}
