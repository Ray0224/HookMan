using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shadow : MonoBehaviour
{
    LayerMask shadowLayer = new LayerMask();
    public List<GameObject> shadows;
    public CircleCollider2D ballCollider;
    public float radius;
    public float raycastDistance;
    public float offset;
    public List<float> localScalex = new List<float>();
    public List<float> localScaley = new List<float>();
    // Start is called before the first frame update
    void Start()
    {
        shadowLayer = (LayerMask.GetMask("BoxLayer")) | (LayerMask.GetMask("WallLayer")) | (LayerMask.GetMask("Trap")) |
        (LayerMask.GetMask("UnHookedWall")) | (LayerMask.GetMask("JumpPed")) | (LayerMask.GetMask("FrontAnchorBox")) | (LayerMask.GetMask("RearAnchorBox"));
        radius = ballCollider.radius;
        for (int i = 0; i<shadows.Count; i++)
        {
            localScalex.Add(shadows[i].transform.localScale.x);
            localScaley.Add(shadows[i].transform.localScale.y);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        for(int i = 0; i < shadows.Count; i++)
        {
            //y座標尚未改
            RaycastHit2D hit = RaycastHit2D(shadows[i].transform,i);
            RaycastHit2D hit2 = RaycastHit2DSecond(shadows[i].transform,i);
        }
        
    }

    RaycastHit2D RaycastHit2D(Transform origin ,int index)
    {
        float xOffset = localScalex[0]*6 - localScalex[0] * index;
        float a = Mathf.Acos(xOffset / radius);
        float yOffset = -radius * Mathf.Sin(a) + offset;
        Vector2 originPos = new Vector2(transform.position.x + xOffset, transform.position.y + yOffset);
        RaycastHit2D hit = Physics2D.Raycast(originPos, Vector2.down, raycastDistance, shadowLayer);
        
        Debug.DrawRay(originPos, Vector2.down * raycastDistance, Color.blue);
        if(hit == true)
        {
            origin.gameObject.SetActive(true);
            float shadowOffset = hit.point.x - transform.position.x;
            float rate = (raycastDistance - hit.distance) / raycastDistance;
            float shadowOffsetCaculated = shadowOffset * rate;
            origin.localScale = new Vector3(localScalex[index] * rate, origin.localScale.y, origin.localScale.z);
            origin.position = new Vector3(transform.position.x + shadowOffsetCaculated, hit.point.y , origin.position.z);
            
        }
        else
        {
            origin.gameObject.SetActive(false);
        }
        return hit;
    }

    RaycastHit2D RaycastHit2DSecond(Transform origin, int index)
    {
        float xOffset = origin.position.x - transform.position.x;
        float a = Mathf.Acos(xOffset / radius);
        float yOffset = -radius * Mathf.Sin(a) + offset;
        Vector2 originPos = new Vector2(origin.position.x, transform.position.y + yOffset);
        RaycastHit2D hit = Physics2D.Raycast(originPos, Vector2.down, raycastDistance,shadowLayer);
        Debug.DrawRay(originPos, Vector2.down * raycastDistance, Color.red);
        if (hit == true)
        {
            origin.gameObject.SetActive(true); 
            float rate = (raycastDistance - hit.distance) / raycastDistance;
            origin.localScale = new Vector3(origin.localScale.x, localScaley[index] * rate, origin.localScale.z);
            origin.position = new Vector3(origin.position.x, hit.point.y, origin.position.z);
        }
        else
        {
            origin.gameObject.SetActive(false);
        }
        return hit;
    }
    //斜面太陡影子會變成很多塊
    void ThirdCorrect(int index)
    {
        //從中間開始校正
        if(index == (shadows.Count+1)/2)
        {
            
        }
        
    }


}
