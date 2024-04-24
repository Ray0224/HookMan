using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LogTunel1 : MonoBehaviour
{
    public SpriteRenderer[] outsideLogs;
    public float fadeSpeed;
    public bool isTouchingPlayer;
    public Ball ball;
    [Header("¿O¥ú®ÄªG")]
    public Light2D light_Outside;
    public Light2D light_Inside;
    public float lightFade_Outside;
    public float lightFade_Inside;

    void Start()
    {
        ball = GameObject.FindObjectsOfType<Ball>()[0];
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        
        light_Inside.intensity = 0;
        if(collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            if (light_Outside.intensity > 0.15)
            {
                light_Outside.intensity -= Time.deltaTime * lightFade_Outside;
            }
            for (int i = 0; i < outsideLogs.Length; i++)
            {
                float a = outsideLogs[i].color.a;
                if (a > 0)
                {
                    a -= Time.deltaTime * fadeSpeed;
                }
                outsideLogs[i].color = new Color(outsideLogs[i].color.r, outsideLogs[i].color.g, outsideLogs[i].color.b, a);

            }
            ball.isTouchingTunel = true;
            isTouchingPlayer = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            ball.isTouchingTunel = false;
            isTouchingPlayer = false;
            StartCoroutine(LightOutsideOn());
            StartCoroutine(AResume());
            StartCoroutine(LightInsideOn());
            
        }
    }
    
    IEnumerator LightOutsideOn()
    {
        if(ball.isTouchingTunel == true)
        {
            yield break;
        }
        else
        {
            light_Outside.intensity += Time.deltaTime * lightFade_Outside;
            if(light_Outside.intensity < 1)
            {
                yield return 0;
                StartCoroutine(LightOutsideOn());
            }
            else
            {
                yield break;
            }
        }
    }
    IEnumerator LightInsideOn()
    {
        if (ball.isTouchingTunel == true)
        {
            yield break;
        }
        else
        {
            light_Inside.intensity += Time.deltaTime * lightFade_Inside;
            if (light_Inside.intensity < 1)
            {
                yield return 0;
                StartCoroutine(LightInsideOn());
            }
            else
            {
                yield break;
            }
        }
    }
    IEnumerator AResume()
    {
        Debug.Log("resume");
        if (isTouchingPlayer == true)
        {
            yield break;
        }
        else
        {
            for (int i = 0; i < outsideLogs.Length; i++)
            {
                float a = outsideLogs[i].color.a;
                if (a < 1)
                {
                    a += Time.deltaTime * fadeSpeed;
                    outsideLogs[i].color = new Color(outsideLogs[i].color.r, outsideLogs[i].color.g, outsideLogs[i].color.b, a);
                    
                }
                if(i == outsideLogs.Length - 1)
                {
                    if (a >= 1)
                    {
                        light_Inside.intensity = 1;

                    }
                    yield return 0;
                    StartCoroutine(AResume());
                }
            }
            
        }
    }
}
