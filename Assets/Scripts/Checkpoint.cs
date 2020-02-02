﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{

    public GameObject summoningMenu;
    
    // Start is called before the first frame update
    void Start(){
        
    }

    // Update is called once per frame
    void Update(){
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.up, 0.1f, LayerMask.GetMask("Player"));
        if (hit.collider != null){
            DoCheckpoint();
        }
        if (!summoningMenu){
            summoningMenu = GameObject.Find("Player/SummoningMenu");
        }
    }

    void DoCheckpoint(){
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
