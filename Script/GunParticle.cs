using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunParticle : MonoBehaviour
{
    public GrapplingGun grapplingGun;
    public Transform shootPoint;
    public ParticleSystem shootParticle;
    public float shootParticleSpeed;
    public ParticleSystem shrinkParticle;
    public float shrinkParticleSpeed;

    // Update is called once per frame
    void Update()
    {
        shootParticle.gameObject.transform.position = shootPoint.position;
        shrinkParticle.gameObject.transform.position = shootPoint.position;

        if (grapplingGun.shrinkRope && grapplingGun.currentState != GrapplingGun.state.isReady)
        {
            if (!shrinkParticle.isPlaying)
            {
                PlayShrinkParticle();
            }
            
        }
        else
        {
            if (shrinkParticle.isPlaying)
            {
                StopShrinkParticle();
            }
        }
    }

    public void PlayShootParticle()
    {
        shootParticle.Play();
    }

    private void PlayShrinkParticle()
    {
        shrinkParticle.Play();
    }

    private void StopShrinkParticle()
    {
        shrinkParticle.Stop();
    }
}
