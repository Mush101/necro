﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    private enum Directions{left, right, up, down}

    private Vector3 movement;
    public float movementSpeed = 2.0f;
    private Directions direction;
    private Directions returnToDirection;
    private Rigidbody2D rb;
    private bool canGoUpLadder, canGoDownLadder, isOnLadder;
    private bool isSummoning;
    public Sprite move1, move2, climb1, climb2, summon1, summon2;
    private bool turnToCentre = false;
    public float spinSpeed = 0.1f;
    private float summonAnimation = 0.0f;
    public float summonAnimSpeed = 0.01f;

    private bool justPressed;

    public GameObject checkpoint;
    private GameObject summoningMenu;

    private GameObject fishFriend;

    public int numMacguffins = 0;
    public GameObject trophyPrefab, door;

    public float cameraTime = 0.0f;
    public GameObject endScreen, startScreen;
    private bool gameStart = false;

    // Start is called before the first frame update
    void Start(){
        direction = Directions.right;
        rb = GetComponent<Rigidbody2D>();
        summoningMenu = transform.Find("SummoningMenu").gameObject;
        door = GameObject.Find("Door");
    }

    // Update is called once per frame
    void Update(){
        if(gameStart){
            if (cameraTime>0){
                cameraTime-=0.1f;
                if(numMacguffins >= 10 && cameraTime<1){
                    cameraTime = 1;
                    endScreen.SetActive(true);
            }
            }else{
                FigureOutLadders();
                ShowSummoningMenu();
                LookForFishFriend();
                if(!isSummoning){
                    summoningMenu.SetActive(false);
                    HandleInput();
                }
                Animate();
            }
        }else{
            summoningMenu.SetActive(false); 
            if(Input.GetButton("Fire1")){
                gameStart = true;                    
                startScreen.SetActive(false);
                justPressed = true;
            }
        }
    }

    void LookForFishFriend(){
        RaycastHit2D hit = Physics2D.Raycast(transform.position + new Vector3(0,0.5f), Vector2.right - Vector2.up, 1.5f, LayerMask.GetMask("Fishman"));
        if (hit.collider != null){
            GameObject fish = hit.transform.gameObject;
            if(fish.GetComponent<Fishman>().inWater && !fish.GetComponent<Fishman>().riding){
                fishFriend = fish;
                fish.GetComponent<Fishman>().riding = true;
            }
        }
        hit = Physics2D.Raycast(transform.position + new Vector3(0,0.5f), - Vector2.right - Vector2.up, 1.5f, LayerMask.GetMask("Fishman"));
        if (hit.collider != null){
            GameObject fish = hit.transform.gameObject;
            if(fish.GetComponent<Fishman>().inWater && !fish.GetComponent<Fishman>().riding){
                fishFriend = fish;
                fish.GetComponent<Fishman>().riding = true;
            }
        }
    }

    void FixedUpdate(){
        if(!isSummoning && cameraTime<=0 && gameStart){
            
            if (fishFriend == null){
                Move();
            }else{
                Ride();
            }
        }
    }

    void FigureOutLadders(){
        RaycastHit2D hit = Physics2D.Raycast(transform.position + new Vector3(0,-0.35f), Vector2.up, 0.4f, LayerMask.GetMask("Ladder"));
        canGoUpLadder = hit.collider != null;
        hit = Physics2D.Raycast(transform.position + new Vector3(0, -0.6f), -Vector2.up, 0.5f, LayerMask.GetMask("Ladder"));
        canGoDownLadder = hit.collider != null;
    }

    void MountLadder(){
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Ground"), true);
        isOnLadder = true;
        rb.gravityScale = 0.0f;
        transform.position = new Vector3(Mathf.Round(transform.position.x+0.5f)-0.5f, transform.position.y);
        turnToCentre = true;
        returnToDirection = direction;
    }

    void DismountLadder(){
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Ground"), false);
        isOnLadder = false;
        rb.gravityScale = 1.0f;
        turnToCentre = true;
        direction = returnToDirection;
    }

    void ShowSummoningMenu(){
        if(isSummoning && !turnToCentre){
            transform.Find("SummoningMenu").gameObject.SetActive(true);
            transform.Find("SummoningCircle1").gameObject.SetActive(true);
            transform.Find("SummoningCircle2").gameObject.SetActive(true);
        }
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
            }else if(Input.GetButton("Fire1") && !isSummoning){
                if(!justPressed){
                    isSummoning = true;
                    turnToCentre = true;
                    rb.velocity = new Vector3();
                }
                justPressed = true;
            }else if(Input.GetButton("Fire3") && !isSummoning){
                if(!justPressed){
                    ReturnToCheckpoint();
                }
                justPressed = true;
            }else{
                justPressed = false;
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
        if(!isOnLadder){
            rb.gravityScale = 1;
        }
        // Check for walking off edges.
        if(direction == Directions.right){
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.right - Vector2.up, 1.0f, LayerMask.GetMask("Ground", "Vines"));
            if (hit.collider == null){
                movement = new Vector3();
            }
        }else if (direction == Directions.left){
            RaycastHit2D hit = Physics2D.Raycast(transform.position, - Vector2.right - Vector2.up, 1.0f, LayerMask.GetMask("Ground", "Vines"));
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

    void Ride(){
        rb.gravityScale = 0;
        transform.position = fishFriend.transform.position + new Vector3(0,0.5f);
        if(fishFriend.transform.localScale.y < 1){
            if(fishFriend.GetComponent<Fishman>().direction == Fishman.Directions.right)
                transform.position += new Vector3(0.5f,0.5f);
            else
                transform.position += new Vector3(-0.5f,0.5f);
            fishFriend = null;
        }
    }

    void Animate(){
        if (turnToCentre){

            if(transform.localScale.x>spinSpeed){
                transform.localScale += new Vector3(-spinSpeed, 0);
            }else if(transform.localScale.x<-spinSpeed){
                transform.localScale += new Vector3(spinSpeed, 0);
            }
            if(Mathf.Abs(transform.localScale.x) < spinSpeed){
                turnToCentre = false;
            }

        }else if (isSummoning){

            if (transform.localScale.x<1){
                transform.localScale += new Vector3(spinSpeed, 0);
            }

            summonAnimation += summonAnimSpeed;
            if(summonAnimation>=1){
                summonAnimation = 0;
            }

            if(summonAnimation < 0.5f){
                GetComponent<SpriteRenderer>().sprite = summon1;
            }else{
                GetComponent<SpriteRenderer>().sprite = summon2;
            }

        }else if (isOnLadder){

            if (transform.localScale.x<1){
                transform.localScale += new Vector3(spinSpeed, 0);
            }

            if(Mathf.Abs(transform.position.y % 1) < 0.5f){
                GetComponent<SpriteRenderer>().sprite = climb1;
            }else{
                GetComponent<SpriteRenderer>().sprite = climb2;
            }

        }else{

            if(Mathf.Abs(transform.position.x % 1) < 0.5){
                GetComponent<SpriteRenderer>().sprite = move1;
            }else{
                GetComponent<SpriteRenderer>().sprite = move2;
            }

            if(direction == Directions.left && transform.localScale.x>-1){
                transform.localScale += new Vector3(-spinSpeed, 0);
            }else if (direction == Directions.right && transform.localScale.x<1){
                transform.localScale += new Vector3(spinSpeed, 0);
            }

        }
    }

    public void StopSummoning(){
        isSummoning = false;
        turnToCentre = true;
    }

    public bool IsFacingRight(){
        return direction == Directions.right;
    }

    public void DoCheckpoint(GameObject cp){
        checkpoint = cp;
    }

    public void ReturnToCheckpoint(){
        if(checkpoint)
            transform.position = checkpoint.transform.position;
    }

    public void GetMacguffin(){
        numMacguffins+=1;
        GameObject trophy = Instantiate(trophyPrefab);
        trophy.transform.position = door.transform.position + new Vector3(0,3);
        cameraTime = 20.0f;
        rb.velocity = Vector3.zero;
    }
}
