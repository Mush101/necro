using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{

    public Enums.Components component;
    GameObject summoningMenu, player;

    // Start is called before the first frame update
    void Start(){
        summoningMenu = GameObject.Find("Player/SummoningMenu");
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update(){
        transform.Rotate(Vector3.forward);

        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.up, 0.1f, LayerMask.GetMask("Player"));
        if (hit.collider != null){
            Collect();
        }

        hit = Physics2D.Raycast(transform.position, Vector2.down, 0.5f, LayerMask.GetMask("Slime"));
        if (hit.collider != null){
            hit.transform.gameObject.GetComponent<Slime>().Carry(gameObject);
        }
    }

    void Collect(){
        if(component == Enums.Components.none){
            player.GetComponent<Player>().GetMacguffin();
        }else{
            summoningMenu.GetComponent<SummoningMenu>().NewItem(component);
        }
        Destroy(gameObject);
    }
}
