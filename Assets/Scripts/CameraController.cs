using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    public GameObject following;

    private Vector3 offset;

    // Start is called before the first frame update
    void Start()
    {
        offset = transform.position - following.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void LateUpdate(){
        transform.position = following.transform.position + offset;
    }
}
