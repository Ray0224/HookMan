using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput1 : MonoBehaviour
{

    public GrapplingGun grapplingGun;
    public Anchor hook;

    public void OnMouseClick(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            if(grapplingGun.currentState == GrapplingGun.state.isReady && !grapplingGun.outOfAction){
                grapplingGun.Shoot();
            }
            else if(grapplingGun.currentState == GrapplingGun.state.isShooting){
                grapplingGun.StopShooting();
                grapplingGun.Dropping();
            }
            else if(hook.currentState == Anchor.CurrentState.isWallContacted || hook.currentState == Anchor.CurrentState.isObjectContacted)
            {
                grapplingGun.Dropping();
            }
        }
    }

    public void OnShrink(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            grapplingGun.shrinkRope = true;
            
        }
        else if(context.canceled)
        {
            grapplingGun.shrinkRope = false;
            grapplingGun.shrinkTime = 0;
        }
    }
}
