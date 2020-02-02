using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour
{
    public Sprite up, down;
    private bool isPressed;
    public int number = 1;
    public static bool[] open;
    public bool isOpen;

    // Start is called before the first frame update
    void Start()
    {
        open = new bool[10];
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position , Vector2.down, 0.1f, LayerMask.GetMask("Player", "Skeleton"));
        isOpen = hit.collider != null;

        if(isOpen){
            GetComponent<SpriteRenderer>().sprite = down;
        }else{
            GetComponent<SpriteRenderer>().sprite = up;
        }

        open[number] = isOpen;
    }
}
