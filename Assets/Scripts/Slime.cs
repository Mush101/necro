using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : MonoBehaviour
{
    public enum Directions {left, right}

    public Directions direction = Directions.right;
    private Rigidbody2D rb;
    public float moveSpeed = 10;

    public Sprite frame1, frame2;

    private bool dying = false;

    private GameObject carrying;

    private int timesThroughHole = 0;
    private bool inHole = false;
    private float squish = 1.0f;

    // Start is called before the first frame update
    void Start(){
        rb = GetComponent<Rigidbody2D>();
    }

    public void Carry(GameObject carry){
        if(!carrying && timesThroughHole == 1){
            carrying = carry;
            if(direction == Directions.left){
                direction = Directions.right;
            }else{
                direction = Directions.left;
            }
        }
    }

    // Update is called once per frame
    void Update(){
        if(Mathf.Abs(transform.position.x % 1.0f) < 0.5f){
            GetComponent<SpriteRenderer>().sprite = frame1;
        }else{
            GetComponent<SpriteRenderer>().sprite = frame2;
        }

        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.up, 0.1f, LayerMask.GetMask("Holes"));
        if (hit.collider != null){
            inHole = true;
        }else{
            if(inHole){
                timesThroughHole += 1;
                if(timesThroughHole>=2){
                    dying=true;
                }
            }
            inHole = false;
        }

        if (inHole && squish>0.1f){
            squish-=0.1f;
            transform.position-=new Vector3(0,0.05f);
        }else if(!inHole && squish<=0.9f){
            squish+=0.1f;
            transform.position+=new Vector3(0,0.05f);
        }

        if(dying){
            transform.localScale *= 0.9f;
            if (transform.localScale.y < 0.01){
                Destroy(gameObject);
            }
        }else{

            if (direction == Directions.right){
                transform.localScale = new Vector3(1,squish);
            }else{
                transform.localScale = new Vector3(-1,squish);
            }
            
        }

    }

    void FixedUpdate(){
        if(!dying && squish>=0.9 || squish<0.2){
            if (direction == Directions.right){
                rb.velocity = new Vector3(1.0f, rb.velocity.y) * moveSpeed;
                RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.right, 0.5f, LayerMask.GetMask("Ground", "Vines", "Spikes"));
                if (hit.collider != null){
                    dying = true;
                    rb.velocity = new Vector3(0, rb.velocity.y);
                }
            }else{
                rb.velocity = new Vector3(-1.0f, rb.velocity.y) * moveSpeed;
                RaycastHit2D hit = Physics2D.Raycast(transform.position, -Vector2.right, 0.5f, LayerMask.GetMask("Ground", "Vines", "Spikes"));
                if (hit.collider != null){
                    dying = true;
                    rb.velocity = new Vector3(0, rb.velocity.y);
                }
            }
        }

        if (carrying){
            carrying.transform.position = transform.position;
        }
        
    }

    public void GetGhosted(){
        Destroy(gameObject);
    }
}
