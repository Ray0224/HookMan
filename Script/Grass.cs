using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grass : MonoBehaviour
{
    Material myMaterial;
    [Header("ÀR¤îª¬ºA")]
    public Vector2 windMovement;
    public float windDensity;
    public float windStrength;

    [Header("³Q¸I¨ì")]
    public Vector2 windMovement_Move;
    public float windDensity_Move;
    public float windStrength_Move;
    public float shakeTime;
    // Start is called before the first frame update
    void Start()
    {
        myMaterial = GetComponent<SpriteRenderer>().material;
        myMaterial.SetVector("_windMovement", windMovement);
        myMaterial.SetFloat("_windDensity", windDensity);
        myMaterial.SetFloat("_windStrength", windStrength);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            StartCoroutine(GrassShake());
        }

    }
    IEnumerator GrassShake()
    {
        myMaterial.SetVector("_windMovement", windMovement_Move);
        myMaterial.SetFloat("_windDensity", windDensity_Move);
        myMaterial.SetFloat("_windStrength", windStrength_Move);
        yield return new WaitForSecondsRealtime(shakeTime);
        myMaterial.SetVector("_windMovement", windMovement);
        myMaterial.SetFloat("_windDensity", windDensity);
        myMaterial.SetFloat("_windStrength", windStrength);
    }
}
