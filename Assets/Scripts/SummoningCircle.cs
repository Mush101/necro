using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummoningCircle : MonoBehaviour
{

    public Sprite bone, cloth, goo;

    public Enums.Components component;

    // Start is called before the first frame update
    void Start(){
        component = Enums.Components.none;
    }

    // Update is called once per frame
    void Update(){
        transform.Rotate (Vector3.forward);

        GameObject item = transform.Find("Item").gameObject;
        switch(component){
            case Enums.Components.none:
                item.SetActive(false);
                break;
            case Enums.Components.bone:
                item.SetActive(true);
                item.GetComponent<SpriteRenderer>().sprite = bone;
                break;
            case Enums.Components.cloth:
                item.SetActive(true);
                item.GetComponent<SpriteRenderer>().sprite = cloth;
                break;
            case Enums.Components.goo:
                item.SetActive(true);
                item.GetComponent<SpriteRenderer>().sprite = goo;
                break;
        }
    }
}
