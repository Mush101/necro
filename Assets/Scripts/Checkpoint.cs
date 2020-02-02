using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{

    public GameObject summoningMenu;
    public GameObject player;

    public Sprite off, on1, on2, on3;

    private float timer = 0.0f;
    
    // Start is called before the first frame update
    void Start(){
        summoningMenu = GameObject.Find("Player/SummoningMenu");
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update(){
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.up, 0.1f, LayerMask.GetMask("Player"));
        if (hit.collider != null){
            if(Input.GetAxis("Vertical") > 0){
                DoCheckpoint();
            }
            transform.Find("up").gameObject.SetActive(true);
        }else{
            transform.Find("up").gameObject.SetActive(false);
        }

        timer +=0.1f;
        if(timer>3){
            timer = 0.0f;
        }

        if(player.GetComponent<Player>().checkpoint == gameObject){
            if (timer>2){
                GetComponent<SpriteRenderer>().sprite = on3;
            }else if (timer>1){
                GetComponent<SpriteRenderer>().sprite = on2;
            }else{
                GetComponent<SpriteRenderer>().sprite = on1;
            }
        }else{
            GetComponent<SpriteRenderer>().sprite = off;
        }
    }

    void DoCheckpoint(){

        player.GetComponent<Player>().DoCheckpoint(gameObject);

        summoningMenu.GetComponent<SummoningMenu>().DoCheckpoint();

        GameObject[] monsters = GameObject.FindGameObjectsWithTag("Monster");

        foreach (GameObject other in monsters){
            if(other.GetComponent<Zombie>()){
                other.GetComponent<Zombie>().GetGhosted();
            }else if(other.GetComponent<Slime>()){
                other.GetComponent<Slime>().GetGhosted();
            }else if(other.GetComponent<Skeleton>()){
                other.GetComponent<Skeleton>().GetGhosted();
            }else{
                Destroy(other);
            }
        }
    }
}
