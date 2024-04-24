using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject Player;
    public CameraFollow cameraFollow;
    // Start is called before the first frame update
    void Start()
    {
        Invoke("PlayerOff", 1);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void PlayerOff()
    {
        Player.SetActive(false);
    }
    public void PlayerOn()
    {
        Player.SetActive(true);
    }
    public void PlayerFollow()
    {
        cameraFollow.target = Player;
    }
}
