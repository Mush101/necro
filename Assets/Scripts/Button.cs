using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour
{
    public Sprite up, down;
    private bool isPressed;
    public int number = 1;
    public static bool[] open;
    public static Color[] colours;
    public bool isOpen;

    // Start is called before the first frame update
    void Start()
    {
        open = new bool[10];
        colours = new Color[10];
        colours[1] = Color.cyan;
        colours[2] = Color.blue;
        colours[3] = Color.grey;
        colours[4] = Color.magenta;
        colours[5] = Color.red;
        colours[6] = Color.white;
        colours[7] = Color.yellow;
        colours[8] = Color.black;

        char num = gameObject.name[6];
        number = charToInt(num);
    }

    public static int charToInt(char c){
        switch(c){
            case '1':
                return 1;
            case '2':
                return 2;
            case '3':
                return 3;
            case '4':
                return 4;
            case '5':
                return 5;
            case '6':
                return 6;
            case '7':
                return 7;
            case '8':
                return 8;
        }
        return 8;
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position , Vector2.down, 0.1f, LayerMask.GetMask("Player", "Skeleton"));
        isOpen = hit.collider != null;
        if(isOpen){
            if(hit.transform.gameObject.GetComponent<Skeleton>()){
                hit.transform.gameObject.GetComponent<Skeleton>().stop = true;
            }
        }

        if(isOpen){
            GetComponent<SpriteRenderer>().sprite = down;
        }else{
            GetComponent<SpriteRenderer>().sprite = up;
        }

        open[number] = isOpen;

        GetComponent<SpriteRenderer>().color = colours[number];
    }
}
