using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fishman : MonoBehaviour
{
    public enum Directions {left, right}

    public Directions direction = Directions.right;
    private Rigidbody2D rb;
    public float moveSpeed = 10;

    public Sprite frame1, frame2, frame3, frame4;

    private bool dying = false;
    public bool inWater = false;
    public bool riding = false;

    // Start is called before the first frame update
    void Start(){
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update(){
        if(Mathf.Abs(transform.position.x % 1.0f) < 0.5f){
            GetComponent<SpriteRenderer>().sprite = frame1;
            if(inWater){
                GetComponent<SpriteRenderer>().sprite = frame3;
            }
        }else{
            GetComponent<SpriteRenderer>().sprite = frame2;
            if(inWater){
                GetComponent<SpriteRenderer>().sprite = frame4;
            }
        }

        if(dying){
            transform.localScale *= 0.9f;
            if (transform.localScale.y < 0.01){
                Destroy(gameObject);
            }
        }else{

            if(!inWater){
                if (direction == Directions.right){
                    transform.localScale = new Vector3(1,1);
                }else{
                    transform.localScale = new Vector3(-1,1);
                }
            }

            if(!inWater){
                RaycastHit2D hit = Physics2D.Raycast(transform.position , Vector2.down, 0.1f, LayerMask.GetMask("Water"));
                if(hit.collider != null){
                    inWater = true;
                    Rigidbody2D rb = GetComponent<Rigidbody2D>();
                    rb.gravityScale = 0;
                    rb.velocity = new Vector3();
                    rb.angularVelocity = 0.0f;
                }
            }
            
        }

    }

    void FixedUpdate(){
        if (inWater){

        }
        
        if(!dying && (!inWater || riding)){
            if (direction == Directions.right){
                rb.velocity = new Vector3(1.0f, rb.velocity.y) * moveSpeed;
                RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.right, 0.3f, LayerMask.GetMask("Ground", "Vines"));
                if (hit.collider != null){
                    dying = true;
                    rb.velocity = new Vector3(0, rb.velocity.y);
                }
            }else{
                rb.velocity = new Vector3(-1.0f, rb.velocity.y) * moveSpeed;
                RaycastHit2D hit = Physics2D.Raycast(transform.position, -Vector2.right, 0.3f, LayerMask.GetMask("Ground", "Vines"));
                if (hit.collider != null){
                    dying = true;
                    rb.velocity = new Vector3(0, rb.velocity.y);
                }
            }
        }
        
    }

    public void GetGhosted(){
        Destroy(gameObject);
    }
}
