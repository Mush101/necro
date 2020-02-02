using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gate : MonoBehaviour
{

    public bool isOpen = false;
    private float openAmount = 1.0f;
    private Vector3 headPos;
    public int number = 1;

    // Start is called before the first frame update
    void Start()
    {
        headPos = transform.position + new Vector3(0, 0.5f);
    }

    // Update is called once per frame
    void Update(){

        isOpen = Button.open[number];

        if(isOpen && openAmount > 0.1f){
            openAmount -= 0.1f;
        }else if(!isOpen && openAmount <= 0.9f){
            openAmount += 0.1f;
        }

        transform.localScale = new Vector3(1,openAmount);
        transform.position = headPos - new Vector3(0,openAmount/2);
    }
}
