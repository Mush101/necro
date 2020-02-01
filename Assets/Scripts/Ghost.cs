using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost : MonoBehaviour
{
    public enum Directions {left, right}

    public Directions direction = Directions.right;
    private Rigidbody2D rb;
    public float moveSpeed = 10;

    public Sprite frame1, frame2;

    private bool dying = false;

    // Start is called before the first frame update
    void Start(){
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update(){
        if(Mathf.Abs(transform.position.x % 1.0f) < 0.5f){
            GetComponent<SpriteRenderer>().sprite = frame1;
        }else{
            GetComponent<SpriteRenderer>().sprite = frame2;
        }

        if(dying){
            transform.localScale *= 0.9f;
            if (transform.localScale.y < 0.01){
                Destroy(gameObject);
            }
        }else{

            if (direction == Directions.right){
                transform.localScale = new Vector3(1,1);
            }else{
                transform.localScale = new Vector3(-1,1);
            }
            
        }

        KillPeople();

    }

    void KillPeople(){
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.up, 0.01f, LayerMask.GetMask("Skeleton", "Slime", "Zombie"));
        if (hit.collider != null){
            GameObject other = hit.transform.gameObject;
            if(other.GetComponent<Zombie>()){
                other.GetComponent<Zombie>().GetGhosted();
            }else if(other.GetComponent<Slime>()){
                other.GetComponent<Slime>().GetGhosted();
            }else if(other.GetComponent<Skeleton>()){
                other.GetComponent<Skeleton>().GetGhosted();
            }
            GetGhosted();
        }
    }

    void FixedUpdate(){
        if(!dying){
            if (direction == Directions.right){
                rb.velocity = new Vector3(1.0f, rb.velocity.y) * moveSpeed;
            }else{
                rb.velocity = new Vector3(-1.0f, rb.velocity.y) * moveSpeed;
            }
        }
        
    }

    void GetGhosted(){
        Destroy(gameObject);
    }
}
