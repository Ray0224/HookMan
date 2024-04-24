using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.U2D;

public class LogTunel : MonoBehaviour
{
    public GameObject Group_SR;
    public GameObject Group_SSR;
    public SpriteRenderer[] outside_SR;
    public SpriteShapeRenderer[] outside_SSR;
    public float fadeSpeed;
    public bool isTouchingPlayer;
    public Ball ball;
    [Header("¿O¥ú®ÄªG")]
    public Light2D light_Outside;
    public float lightFade;

    void Start()
    {
        ball = GameObject.FindObjectsOfType<Ball>()[0];
        outside_SR = Group_SR.GetComponentsInChildren<SpriteRenderer>();
        outside_SSR = Group_SSR.GetComponentsInChildren<SpriteShapeRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.gameObject.layer == LayerMask.NameToLayer("Player")|| collision.gameObject.tag == "Player")
        {
            if (light_Outside.intensity > 0.15)
            {
                light_Outside.intensity -= Time.deltaTime * lightFade;
            }
            if (outside_SR.Length > 0)
            {
                for (int i = 0; i < outside_SR.Length; i++)
                {
                    float a = outside_SR[i].color.a;
                    if (a > 0)
                    {
                        a -= Time.deltaTime * fadeSpeed;
                    }
                    outside_SR[i].color = new Color(outside_SR[i].color.r, outside_SR[i].color.g, outside_SR[i].color.b, a);
                }
            }
            if (outside_SSR.Length > 0)
            {
                for (int i = 0; i < outside_SSR.Length; i++)
                {
                    float a = outside_SSR[i].color.a;
                    if (a > 0)
                    {
                        a -= Time.deltaTime * fadeSpeed;
                    }
                    outside_SSR[i].color = new Color(outside_SSR[i].color.r, outside_SSR[i].color.g, outside_SSR[i].color.b, a);
                }
            }
            ball.isTouchingTunel = true;
            isTouchingPlayer = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player") || collision.gameObject.tag == "Player")
        {
            ball.isTouchingTunel = false;
            isTouchingPlayer = false;
            StartCoroutine(LightOn());
            StartCoroutine(SRResume());
            StartCoroutine(SSRResume());
        }
    }
    
    IEnumerator LightOn()
    {
        if(ball.isTouchingTunel == true)
        {
            yield break;
        }
        else
        {
            light_Outside.intensity += Time.deltaTime * lightFade;
            if(light_Outside.intensity < 1)
            {
                yield return 0;
                StartCoroutine(LightOn());
            }
            else
            {
                yield break;
            }
        }
    }
    IEnumerator SRResume()
    {
        if (isTouchingPlayer == true || outside_SR.Length == 0)
        {
            yield break;
        }
        else
        {
            for (int i = 0; i < outside_SR.Length; i++)
            {
                float a = outside_SR[i].color.a;
                if (a < 1)
                {
                    a += Time.deltaTime * fadeSpeed;
                    outside_SR[i].color = new Color(outside_SR[i].color.r, outside_SR[i].color.g, outside_SR[i].color.b, a);
                }
                if(i == outside_SR.Length - 1)
                {
                    yield return 0;
                    StartCoroutine(SRResume());
                }
            }
            
        }
    }
    IEnumerator SSRResume()
    {
        if (isTouchingPlayer == true || outside_SSR.Length == 0)
        {
            yield break;
        }
        else
        {
            for (int i = 0; i < outside_SSR.Length; i++)
            {
                float a = outside_SSR[i].color.a;
                if (a < 1)
                {
                    a += Time.deltaTime * fadeSpeed;
                    outside_SSR[i].color = new Color(outside_SSR[i].color.r, outside_SSR[i].color.g, outside_SSR[i].color.b, a);
                }
                if (i == outside_SR.Length - 1)
                {
                    yield return 0;
                    StartCoroutine(SSRResume());
                }
            }

        }
    }
}
