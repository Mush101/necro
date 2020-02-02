using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mummy : MonoBehaviour
{

    bool droppingBandage = true;
    float bandageLength = 1.0f;
    GameObject bandage;

    // Start is called before the first frame update
    void Start(){
        bandage = transform.Find("Bandage").gameObject;
        
        RaycastHit2D hit = Physics2D.Raycast(transform.position + new Vector3(0, - 1.5f), Vector2.down, 0.4f, LayerMask.GetMask("Ground", "Vines", "Spikes", "Water"));
        if (hit.collider != null){
            droppingBandage = false;
            bandageLength = 0.0f;
        }
    }

    // Update is called once per frame
    void Update(){
        if(droppingBandage){
            DropBandage();
        }
        bandage.transform.localScale = new Vector3(1,bandageLength);
        bandage.transform.position = transform.position + new Vector3(0, - 0.5f - bandageLength/2);
    }

    void DropBandage(){
        bandageLength+=0.01f;

        RaycastHit2D hit = Physics2D.Raycast(transform.position + new Vector3(0, - 0.5f - bandageLength), Vector2.down, 0.1f, LayerMask.GetMask("Ground", "Vines", "Spikes", "Water"));
        if (hit.collider != null){
            droppingBandage = false;
        }
    }
    public void GetGhosted(){
        Destroy(gameObject);
    }
}
