using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour
{
    public Material material1;
    public Material material2;
    [Header("ÀR¤îª¬ºA")]
    public float MovingSpeed;
    public float MovingPower;
    public float WindPower;


    [Header("³Q¸I¨ì")]
    public float MovingSpeed_Move;
    public float MovingPower_Move;
    public float WindPower_Move;

    public float shakeTime;
    // Start is called before the first frame update
    void Start()
    {
        material1 = GetComponent<SpriteRenderer>().material;
        material2 = transform.GetChild(0).GetComponent<SpriteRenderer>().material;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            StartCoroutine(GrassShake());
        }
    }


    
    IEnumerator GrassShake()
    {
        material1.SetFloat("_MovingSpeed", MovingSpeed_Move);
        material1.SetFloat("_MovingPower", MovingPower_Move);
        material1.SetFloat("_WindPower", WindPower_Move);
        material2.SetFloat("_MovingSpeed", MovingSpeed_Move);
        material2.SetFloat("_MovingPower", MovingPower_Move);
        material2.SetFloat("_WindPower", WindPower_Move);
        yield return new WaitForSecondsRealtime(shakeTime);
        material1.SetFloat("_MovingSpeed", MovingSpeed);
        material1.SetFloat("_MovingPower", MovingPower);
        material1.SetFloat("_WindPower", WindPower);
        material2.SetFloat("_MovingSpeed", MovingSpeed);
        material2.SetFloat("_MovingPower", MovingPower);
        material2.SetFloat("_WindPower", WindPower);
    }
}
