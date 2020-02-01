using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    enum Directions{left, right, up, down}

    private bool movement;
    public float movementSpeed = 0.2f;
    public float gridLine = 0.5f;
    private Vector3 goal;
    private Directions direction;


    // Start is called before the first frame update
    void Start(){
        movement = false;
        goal = transform.position;
        direction = Directions.right;
    }

    // Update is called once per frame
    void Update(){
        // StopAtGrid();
        // if(!movement){
        //     HandleInput();
        // }
        // transform.position = Vector3.MoveTowards(transform.position, goal, movementSpeed);
        Animate();
    }

    void StopAtGrid(){
        // if ((transform.position - goal).magnitude == 0){
        //     movement = false;
        // }
    }

    void HandleInput(){
        // if(Input.GetAxis("Horizontal") > 0){
        //     movement = true;
        //     goal = new Vector3(transform.position.x + gridLine, transform.position.y);
        //     direction = Directions.right;
        // }else if(Input.GetAxis("Horizontal") < 0){
        //     movement = true;
        //     goal = new Vector3(transform.position.x - gridLine, transform.position.y);
        //     direction = Directions.left;
        // }
    }

    void Animate(){
        if(direction == Directions.left && transform.localScale.x>-1){
            transform.localScale += new Vector3(-0.1f, 0);
        }else if (direction == Directions.right && transform.localScale.x<1){
            transform.localScale += new Vector3(0.1f, 0);
        }
    }
}
