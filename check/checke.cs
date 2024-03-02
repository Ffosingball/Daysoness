using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class checke : MonoBehaviour
{
    public SpriteRenderer gun_in_hand;
    public Sprite pic1, pic2;
    private bool i=false;
    
    void Start()
    {
        //gun_in_hand = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.R))
        {
            if (i)
            {
                gun_in_hand.sprite = pic1;
                i=false;
            }
            else
            {
                gun_in_hand.sprite = pic2;
                i=true;
            }
        }
    }
}
