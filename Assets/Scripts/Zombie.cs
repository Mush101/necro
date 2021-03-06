﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie : MonoBehaviour
{

    public enum Directions {left, right}

    public Directions direction = Directions.right;
    private Rigidbody2D rb;
    public float moveSpeed = 10;

    public Sprite frame1, frame2;

    private bool dying = false;
    private bool hopping = false;

    private GameObject vine;
    
    // Start is called before the first frame update
    void Start(){
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update(){
        if(Mathf.Abs(transform.position.x % 0.5f) < 0.25f){
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

            if(!hopping){
                rb.velocity = new Vector3(1.0f, rb.velocity.y) * moveSpeed;
                RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.up, 0.01f, LayerMask.GetMask("Vines"));
                if (hit.collider != null){
                    hopping = true;
                    rb.velocity = new Vector3(0, rb.velocity.y);
                    vine = hit.transform.gameObject;
                    vine.GetComponent<Vines>().crushed = true;
                }
            }
            
        }

    }

    void FixedUpdate(){
        if(!hopping){
            if(!dying){
                if (direction == Directions.right){
                    rb.velocity = new Vector3(1.0f, rb.velocity.y) * moveSpeed;
                    RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.right, 0.3f, LayerMask.GetMask("Ground", "Spikes", "Holes"));
                    if (hit.collider != null){
                        dying = true;
                        rb.velocity = new Vector3(0, rb.velocity.y);
                    }
                }else{
                    rb.velocity = new Vector3(-1.0f, rb.velocity.y) * moveSpeed;
                    RaycastHit2D hit = Physics2D.Raycast(transform.position, -Vector2.right, 0.3f, LayerMask.GetMask("Ground", "Spikes", "Holes"));
                    if (hit.collider != null){
                        dying = true;
                        rb.velocity = new Vector3(0, rb.velocity.y);
                    }
                }
            }
        } else {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, -Vector2.up, 0.4f, LayerMask.GetMask("Ground"));
            if (hit.collider != null){
                rb.velocity = new Vector3(0,2);
            }
        }
        
    }

    public void GetGhosted(){
        if(vine!=null){
            vine.GetComponent<Vines>().crushed = false;
        }
        Destroy(gameObject);
    }
}
