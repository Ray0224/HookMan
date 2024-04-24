using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunAnchorDetect : MonoBehaviour
{
    public Anchor leftAnchor;
    public Anchor rightAnchor;

    public bool isHookedByLeftAnchor(){
        return leftAnchor.targetObject == gameObject;
    } 

    public bool isHookedByRightAnchor(){
        return rightAnchor.targetObject == gameObject;
    }
}
