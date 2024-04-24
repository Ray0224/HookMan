using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamaraShake : MonoBehaviour
{
    Vector3 originalPosition;
    [SerializeField]
    float shakePower;
    [SerializeField]
    int shakeTimes;
    [SerializeField]
    float shakeTime;
    private CamaraShake() { }
    private static CamaraShake instance;
    public static CamaraShake Instance()
    {
        return instance;
    }
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }

    }
    // Start is called before the first frame update
    void Start()
    {
        originalPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void Shake()
    {
        StartCoroutine(CameraShake());
    }
    IEnumerator CameraShake()
    {
        for(int i = 0; i<shakeTimes; i++)
        {
            if(i%2 == 0)
            {
                float x = Random.insideUnitCircle.x;
                float y = Random.insideUnitCircle.y;
                transform.position += new Vector3(x, y, 0) * shakePower;
                yield return new WaitForSeconds(shakeTime);
            }
            else 
            {
                transform.position = originalPosition;
                yield return new WaitForSeconds(shakeTime);
            }

        }
        transform.position = originalPosition;
    }
}
