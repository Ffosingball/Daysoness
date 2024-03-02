using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class zenzeleysk_main : MonoBehaviour
{
    /*private Coroutine reduce_hp=null;
    private int my_hp=15;
    public float detectionRadius = 20f;
    public Sprite level1, level2, level3, bited, nothing_pic;
    private bool touchSword=false, wasAttackedBySword=false, normal=true;
    public float XcoordCenter=0, YcoordCenter=0;

    private Rigidbody2D rb2d;
    public SpriteRenderer spriteRenderer;
    private Collider2D collider;

    private Transform player;
    public Transform boxSpider;



    private void Start() 
    {
        collider = GetComponent<Collider2D>();
        rb2d = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("player").transform;

        spriteRenderer.color = new Color(1f, 1f, 1f, 1f);
        spriteRenderer.sprite = nothing_pic;
        collider.isTrigger = true;
    }



    void Update()
    {
        if(hp_and_else.dead)
        {
            if (reduce_hp != null)
            {
                StopCoroutine(reduce_hp);
                reduce_hp = null;
                spriteRenderer.sprite = alive;
            }
        }



        if(touchSword && item_pick.canAttack && wasAttackedBySword==false)
        {
            wasAttackedBySword=true;

            if(item_pick.whichSword==1)
                my_hp-=5;
            else if(item_pick.whichSword==2)
                my_hp-=3;
            
            StartCoroutine(Get_damage());
            stillAlive();
        }


        if(item_pick.canAttack==false)
            wasAttackedBySword=false;



        float distanceToPlayer = Vector2.Distance(Vector3(XcoordCenter,YcoordCenter,-0.4f), player.position);

        if (my_hp>0 && distanceToPlayer <= detectionRadius && Movement.moving==false)
        {
             Movement.canMove=false;

            if(reduce_hp==null)
                reduce_hp=StartCoroutine(Bite());
        }
    }



    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (my_hp>0)
        {
            switch (collision.gameObject.tag)
            {
                case "AK_bullet":
                    my_hp--;
                    Destroy(collision.gameObject);
                    StartCoroutine(Get_damage());
                    stillAlive();
                    break;
                case "pistol_bullet":
                    my_hp-=4;
                    Destroy(collision.gameObject);
                    StartCoroutine(Get_damage());
                    stillAlive();
                    break;
                case "anihilator_bullet":
                    my_hp-=5;
                    Destroy(collision.gameObject);
                    StartCoroutine(Get_damage());
                    stillAlive();
                    break;
                case "lazer_bullet":
                    my_hp-=2;
                    Destroy(collision.gameObject);
                    StartCoroutine(Get_damage());
                    stillAlive();
                    break;
                case "swords":
                    touchSword=true;
                    break;
            }
        }
    }



    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.tag=="swords")
            touchSword=false;
    }



    private void stillAlive()
    {
        if (my_hp<1)
        {
            canGo=false;
            collider.isTrigger = true;

            if (reduce_hp!=null)
            {
                StopCoroutine(reduce_hp);
                reduce_hp = null;
                spriteRenderer.sprite = alive;
            }

            if (go_circle!=null)
            {
                StopCoroutine(go_circle);
                go_circle = null;
            }

            spriteRenderer.sprite = dead;
            StartCoroutine(After_death());
        }
    }



    private IEnumerator After_death() 
    {
        yield return new WaitForSeconds(16f);
        for (float i=1f; i>0; i-=0.04f)
        {
            spriteRenderer.color = new Color(1f, 1f, 1f, i);
            yield return new WaitForSeconds(0.04f);
        }

        Destroy(gameObject);
    }



    private IEnumerator Bite() 
    {
        while(hp_and_else.real_hp>0 || my_hp>0)
        {
            yield return new WaitForSeconds(0.2f);

            spriteRenderer.sprite = bited;

            hp_and_else.shield-=2;
            if(hp_and_else.shield>=75)
                hp_and_else.real_hp-=0;
            else if(hp_and_else.shield>=50)
                hp_and_else.real_hp-=0.8f;
            else if(hp_and_else.shield>=25)
                hp_and_else.real_hp-=1.4f;
            else if(hp_and_else.shield>0)
                hp_and_else.real_hp-=1.8f;
            else
                hp_and_else.real_hp-=2f;

            hp_and_else.real_hp=hp_and_else.real_hp<0?0:hp_and_else.real_hp;
            hp_and_else.shield=hp_and_else.shield<0?0:hp_and_else.shield;

            yield return new WaitForSeconds(0.1f);
            spriteRenderer.sprite = alive;
            yield return new WaitForSeconds(0.7f);
        }
    }



    private IEnumerator Get_damage() 
    {
        spriteRenderer.color = new Color(1f, 0.2f, 0.2f, 1f);
        yield return new WaitForSeconds(0.15f);
        spriteRenderer.color = new Color(1f, 1f, 1f, 1f);
    }*/
}
