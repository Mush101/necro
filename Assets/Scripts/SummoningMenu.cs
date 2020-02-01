using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummoningMenu : MonoBehaviour
{

    Enums.Components currentComponent;

    bool justPressed = true;
    bool justPressedButton = true;
    public GameObject player, summoningCircle1, summoningCircle2;

    public GameObject zombiePrefab, slimePrefab, skeletonPrefab, fishPrefab, ghostPrefab, mummyPrefab;

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

                }else{
                    player.GetComponent<Player>().StopSummoning();
                    gameObject.SetActive(false);
                    summoningCircle1.SetActive(false);
                    summoningCircle2.SetActive(false);
                    Summon();
                }
            }
        }else{
            justPressedButton = false;
        }

        MoveArrow();
    }

    void Summon(){

        Enums.Components c1 = summoningCircle1.GetComponent<SummoningCircle>().component;
        Enums.Components c2 = summoningCircle2.GetComponent<SummoningCircle>().component;
        if ((c1 == Enums.Components.bone && c2 == Enums.Components.cloth) || (c2 == Enums.Components.bone && c1 == Enums.Components.cloth)){
            GameObject mob = Instantiate(zombiePrefab);
            mob.transform.position = player.transform.position + new Vector3(0,0.2f);
            if(player.GetComponent<Player>().IsFacingRight())
                mob.GetComponent<Zombie>().direction = Zombie.Directions.right;
            else
                mob.GetComponent<Zombie>().direction = Zombie.Directions.left;
        }else if ((c1 == Enums.Components.bone && c2 == Enums.Components.bone)){
            GameObject mob = Instantiate(skeletonPrefab);
            mob.transform.position = player.transform.position + new Vector3(0,0.2f);
            if(player.GetComponent<Player>().IsFacingRight())
                mob.GetComponent<Skeleton>().direction = Skeleton.Directions.right;
            else
                mob.GetComponent<Skeleton>().direction = Skeleton.Directions.left;
        }else if ((c1 == Enums.Components.goo && c2 == Enums.Components.goo)){
            GameObject mob = Instantiate(slimePrefab);
            mob.transform.position = player.transform.position + new Vector3(0,0.2f);
            if(player.GetComponent<Player>().IsFacingRight())
                mob.GetComponent<Slime>().direction = Slime.Directions.right;
            else
                mob.GetComponent<Slime>().direction = Slime.Directions.left;
        }else if ((c1 == Enums.Components.cloth && c2 == Enums.Components.goo) || (c2 == Enums.Components.cloth && c1 == Enums.Components.goo)){
            GameObject mob = Instantiate(ghostPrefab);
            mob.transform.position = player.transform.position + new Vector3(0,0.2f);
            if(player.GetComponent<Player>().IsFacingRight())
                mob.GetComponent<Ghost>().direction = Ghost.Directions.right;
            else
                mob.GetComponent<Ghost>().direction = Ghost.Directions.left;
        }

        summoningCircle2.GetComponent<SummoningCircle>().component = Enums.Components.none;
        summoningCircle1.GetComponent<SummoningCircle>().component = Enums.Components.none;

    }

    void MoveArrow(){

        GameObject arrow = transform.Find("Arrow").gameObject;

        if (summoningCircle1.GetComponent<SummoningCircle>().component != Enums.Components.none && summoningCircle2.GetComponent<SummoningCircle>().component != Enums.Components.none){
            arrow.transform.position = new Vector3(transform.position.x, transform.position.y + 2.8f);
        }else{
            arrow.transform.position = new Vector3(0, transform.position.y + 0.9f);

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
}
