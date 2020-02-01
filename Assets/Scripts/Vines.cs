using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vines : MonoBehaviour
{

    public bool crushed = false;
    private float amount = 1.0f;
    private Vector3 feetPos;

    // Start is called before the first frame update
    void Start(){
        feetPos = transform.position - new Vector3(0,2);
    }

    // Update is called once per frame
    void Update(){
        if (crushed && amount > 0){
            amount -= 0.01f;
        }else if(!crushed && amount < 1){
            amount += 0.01f;
        }

        if(amount < 0){
            amount = 0;
        }else if(amount >1){
            amount = 1;
        }

        transform.localScale = new Vector3(1,amount);
        transform.position = new Vector3(feetPos.x, feetPos.y + amount * 2);
    }
}
