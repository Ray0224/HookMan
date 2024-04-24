using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CartControl : MonoBehaviour
{
    public GunAnchorDetect detect;

    public float moveSpeed;
    public float rotateSpeed;

    public Transform start;
    public Transform end;
    
    public Transform cart;
    public Transform wheel1;
    public Transform wheel2;
    
    private float positionT = 0;

    void Update()
    {
        if(detect.isHookedByLeftAnchor() && detect.isHookedByRightAnchor()){
            Stop();
        }
        else if(detect.isHookedByLeftAnchor()){
            MoveLeft();
        }
        else if(detect.isHookedByRightAnchor()){
            MoveRight();
        }
        else{
            BackToStartPoint();
        }
    }

    private void MoveLeft(){
        positionT -= moveSpeed;
        positionT = Mathf.Max(0, positionT);

        cart.position = Vector3.Lerp(start.position, end.position, positionT);
        Quaternion rotation = Quaternion.Euler(0, 0, rotateSpeed);
        wheel1.rotation *= rotation;
        wheel2.rotation *= rotation;
    }

    private void MoveRight(){
        positionT += moveSpeed;
        positionT = Mathf.Min(1, positionT);

        cart.position = Vector3.Lerp(start.position, end.position, positionT);
        Quaternion rotation = Quaternion.Euler(0, 0, -rotateSpeed);
        wheel1.rotation *= rotation;
        wheel2.rotation *= rotation;
    }

    private void BackToStartPoint(){
        if(positionT <= 0){
            positionT = 0;
            return;
        }
        
        positionT -= moveSpeed * 5;
        positionT = Mathf.Max(0, positionT);

        cart.position = Vector3.Lerp(start.position, end.position, positionT);
        Quaternion rotation = Quaternion.Euler(0, 0, rotateSpeed * 5);
        wheel1.rotation *= rotation;
        wheel2.rotation *= rotation;
    }

    private void Stop(){
        return;
    }
}
