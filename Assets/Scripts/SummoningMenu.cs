using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummoningMenu : MonoBehaviour
{

    Enums.Components currentComponent;

    bool justPressed = true;
    bool justPressedButton = true;
    public GameObject player, summoningCircle1, summoningCircle2;

    // Start is called before the first frame update
    void Start(){
        currentComponent = Enums.Components.bone;
    }

    // Update is called once per frame
    void Update(){

        if(Input.GetAxis("Horizontal") > 0){
            if(!justPressed){
                switch(currentComponent){
                    case Enums.Components.bone:
                        currentComponent = Enums.Components.cloth;
                        break;
                    case Enums.Components.cloth:
                        currentComponent = Enums.Components.goo;
                        break;
                }
                justPressed = true;
            }
        }else if(Input.GetAxis("Horizontal") < 0){
            if(!justPressed){
                switch(currentComponent){
                    case Enums.Components.goo:
                        currentComponent = Enums.Components.cloth;
                        break;
                    case Enums.Components.cloth:
                        currentComponent = Enums.Components.bone;
                        break;
                }
                justPressed = true;
            }
        }else{
            justPressed = false;
        }

        if(Input.GetButton("Fire2")){
            if(!justPressedButton){
                justPressedButton = true;

                if(summoningCircle2.GetComponent<SummoningCircle>().component != Enums.Components.none){
                    summoningCircle2.GetComponent<SummoningCircle>().component = Enums.Components.none;
                }else if(summoningCircle1.GetComponent<SummoningCircle>().component != Enums.Components.none){
                    summoningCircle1.GetComponent<SummoningCircle>().component = Enums.Components.none;
                }else{
                    player.GetComponent<Player>().StopSummoning();
                    gameObject.SetActive(false);
                    summoningCircle1.SetActive(false);
                    summoningCircle2.SetActive(false);
                }
            }
        }else if(Input.GetButton("Fire1")){
            if(!justPressedButton){
                justPressedButton = true;
                
                if(summoningCircle1.GetComponent<SummoningCircle>().component == Enums.Components.none){
                    summoningCircle1.GetComponent<SummoningCircle>().component = currentComponent;

                }else if(summoningCircle2.GetComponent<SummoningCircle>().component == Enums.Components.none){
                    summoningCircle2.GetComponent<SummoningCircle>().component = currentComponent;
                    
                }
            }
        }else{
            justPressedButton = false;
        }

        MoveArrow();
    }

    void MoveArrow(){

        GameObject arrow = transform.Find("Arrow").gameObject;

        switch(currentComponent){
            case Enums.Components.bone:
                arrow.transform.position = new Vector3(transform.Find("Bone").position.x, arrow.transform.position.y);
                break;
            case Enums.Components.cloth:
                arrow.transform.position = new Vector3(transform.Find("Cloth").position.x, arrow.transform.position.y);
                break;
            case Enums.Components.goo:
                arrow.transform.position = new Vector3(transform.Find("Goo").position.x, arrow.transform.position.y);
                break;
        }
    }
}
