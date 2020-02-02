using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{

    public GameObject summoningMenu;
    public GameObject player;
    
    // Start is called before the first frame update
    void Start(){
        summoningMenu = GameObject.Find("Player/SummoningMenu");
        summoningMenu.SetActive(false);
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update(){
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.up, 0.1f, LayerMask.GetMask("Player"));
        if (hit.collider != null){
            DoCheckpoint();
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
