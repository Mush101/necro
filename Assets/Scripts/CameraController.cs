using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    public GameObject following;

    private Vector3 offset, doorOffset;
    private GameObject door;

    // Start is called before the first frame update
    void Start()
    {
        offset = transform.position - following.transform.position;
        door = GameObject.Find("Door");
        doorOffset = transform.position - door.transform.position;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void LateUpdate(){
        if(following.GetComponent<Player>().cameraTime > 0){
            transform.position = door.transform.position + doorOffset + new Vector3(0,2);
        }else{
            transform.position = following.transform.position + offset;
        }
    }
}
