using System.Collections;
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

    private GameObject checkpoint;

    // Start is called before the first frame update
    void Start(){
        direction = Directions.right;
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update(){
        FigureOutLadders();
        ShowSummoningMenu();
        if(!isSummoning){
            HandleInput();
        }
        Animate();
    }

    void FixedUpdate(){
        if(!isSummoning){
            Move();
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
}
