using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    enum Directions{left, right, up, down}

    private Vector3 movement;
    public float movementSpeed = 2.0f;
    private Directions direction;
    private Rigidbody2D rb;
    private bool canGoUpLadder, canGoDownLadder, isOnLadder;


    // Start is called before the first frame update
    void Start(){
        direction = Directions.right;
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update(){
        FigureOutLadders();
        HandleInput();
        Animate();
    }

    void FixedUpdate(){
        Move();
    }

    void FigureOutLadders(){
        RaycastHit2D hit = Physics2D.Raycast(transform.position + new Vector3(0,-0.4f), Vector2.up, 0.4f, LayerMask.GetMask("Ladder"));
        canGoUpLadder = hit.collider != null;
        hit = Physics2D.Raycast(transform.position + new Vector3(0, -0.5f), -Vector2.up, 0.5f, LayerMask.GetMask("Ladder"));
        canGoDownLadder = hit.collider != null;
    }

    void MountLadder(){
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Ground"), true);
        isOnLadder = true;
        rb.gravityScale = 0.0f;
        transform.position = new Vector3(Mathf.Round(transform.position.x+0.5f)-0.5f, transform.position.y);
    }

    void DismountLadder(){
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Ground"), false);
        isOnLadder = false;
        rb.gravityScale = 1.0f;
    }

    void HandleInput(){
        movement = new Vector3();
        if(!isOnLadder){
            if(Input.GetAxis("Horizontal") > 0){
                movement += Vector3.right;
                direction = Directions.right;
            }else if(Input.GetAxis("Horizontal") < 0){
                movement += Vector3.left;
                direction = Directions.left;
            }else if(Input.GetAxis("Vertical") > 0){
                if(canGoUpLadder){
                    MountLadder();
                }
            }else if(Input.GetAxis("Vertical") < 0){
                if(canGoDownLadder){
                    MountLadder();
                }
            }
        }else{
            if(Input.GetAxis("Vertical") > 0){
                movement += Vector3.up;
                direction = Directions.up;
                if (!canGoUpLadder){
                    DismountLadder();
                }
            }else if(Input.GetAxis("Vertical") < 0){
                movement -= Vector3.up;
                direction = Directions.down;
                if (!canGoDownLadder){
                    DismountLadder();
                }
            }
        }
    }

    void Move(){
        // Check for walking off edges.
        if(direction == Directions.right){
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.right - Vector2.up, 1.0f, LayerMask.GetMask("Ground"));
            if (hit.collider == null){
                movement = new Vector3();
            }
        }else if (direction == Directions.left){
            RaycastHit2D hit = Physics2D.Raycast(transform.position, - Vector2.right - Vector2.up, 1.0f, LayerMask.GetMask("Ground"));
            if (hit.collider == null){
                movement = new Vector3();
            }
        }
        if(!isOnLadder){
            rb.velocity = movement * movementSpeed + new Vector3(0, rb.velocity.y);
        }else{ 
            rb.velocity = movement * movementSpeed;
        }
    }

    void Animate(){
        if(direction == Directions.left && transform.localScale.x>-1){
            transform.localScale += new Vector3(-0.1f, 0);
        }else if (direction == Directions.right && transform.localScale.x<1){
            transform.localScale += new Vector3(0.1f, 0);
        }
    }
}
