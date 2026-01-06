using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class zen_big : MonoBehaviour
{
    private Coroutine reduce_hp=null;
    private int my_hp;
    public Sprite level1, level2, level3, bited, nothing_pic, dead;
    //public LayerMask barrierLayer;
    private bool touchSword=false, wasAttackedBySword=false, canHit=false;
    public float XcoordHole=0, YcoordHole=0, damage=6f,wait_time=0,hold_player=7.2f;
    public int detectionRadius=20, its_hp=15, level=3;

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

        my_hp=its_hp;
    }



    void Update()
    {
        if(true)//hp_and_else.dead)
        {
            if (reduce_hp != null)
            {
                StopCoroutine(reduce_hp);
                reduce_hp = null;
                spriteRenderer.sprite = nothing_pic;
                //Debug.Log("остановил");
            }
        }



        if(touchSword)// && item_pick.canAttack && wasAttackedBySword==false && canHit)
        {
            wasAttackedBySword=true;

            //if(item_pick.whichSword==1)
                my_hp-=5;
            //else if(item_pick.whichSword==2)
                my_hp-=3;
            
            StartCoroutine(Get_damage());
            stillAlive();
        }


        //if(item_pick.canAttack==false)
            wasAttackedBySword=false;



        float distanceToPlayer = Vector2.Distance(new Vector3(XcoordHole,YcoordHole,-0.4f), player.position);

        if (distanceToPlayer >= detectionRadius)
        {
            //Movement.timeMove=0;
        }

        /*if (my_hp>0 && distanceToPlayer <= detectionRadius && Movement.moving==false && hp_and_else.dead==false)
        {
            if(reduce_hp==null)
                reduce_hp=StartCoroutine(Bite());
        }*/
    }



    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (my_hp>0 && canHit)
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
            collider.isTrigger = true;

            if (reduce_hp!=null)
            {
                StopCoroutine(reduce_hp);
                reduce_hp = null;
            }

            switch(level)
            {
                case 1:
                    statistica.zenSmallKilled++;
                    break;
                case 2:
                    statistica.zenMediumKilled++;
                    break;
                case 3:
                    statistica.zenBigKilled++;
                    break;
            }

            StartCoroutine(After_death());
        }
    }



    private IEnumerator After_death() 
    {
        //Movement.timeMove=0;

        spriteRenderer.sprite = dead;

        yield return new WaitForSeconds(14f);

        for (float i=1f; i>0; i-=0.04f)
        {
            spriteRenderer.color = new Color(1f, 1f, 1f, i);
            yield return new WaitForSeconds(0.04f);
        }

        yield return new WaitForSeconds(15f);

        my_hp=its_hp;
        spriteRenderer.color = new Color(1f, 1f, 1f, 1f);
    }



    private IEnumerator Bite() 
    {
        //Debug.Log("Начала кусать");
        //Movement.timeMove=hold_player;

        Vector3 pos = player.position;
        pos.y-=0.23f;
        pos.z=-0.6f;

        boxSpider.position=pos;

        yield return new WaitForSeconds(wait_time);

        collider.isTrigger = false;
        canHit=true;

        spriteRenderer.sprite = level1;
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.sprite = level2;
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.sprite = level3;
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.sprite = bited;

        yield return new WaitForSeconds(0.2f);

        //Debug.Log("кусаю");

        /*hp_and_else.shield-=(int)damage;
        if(hp_and_else.shield>=75)
            hp_and_else.real_hp-=0;
        else if(hp_and_else.shield>=50)
            hp_and_else.real_hp-=damage*0.4f;
        else if(hp_and_else.shield>=25)
            hp_and_else.real_hp-=damage*0.7f;
        else if(hp_and_else.shield>0)
            hp_and_else.real_hp-=damage*0.1f;
        else
            hp_and_else.real_hp-=damage;

        hp_and_else.real_hp=hp_and_else.real_hp<0?0:hp_and_else.real_hp;
        hp_and_else.shield=hp_and_else.shield<0?0:hp_and_else.shield;*/

        yield return new WaitForSeconds(0.1f);
        spriteRenderer.sprite = level3;
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.sprite = level2;
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.sprite = level1;
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.sprite = nothing_pic;

        collider.isTrigger = true;
        canHit=false;

        yield return new WaitForSeconds(6.3f);
        
        //Debug.Log("закончил кусать");

        Next_step();
    }



    private void Next_step()
    {
        if(reduce_hp!=null)
        {
            reduce_hp=null;
        }
    }


    private IEnumerator Get_damage() 
    {
        spriteRenderer.color = new Color(1f, 0.2f, 0.2f, 1f);
        yield return new WaitForSeconds(0.15f);
        spriteRenderer.color = new Color(1f, 1f, 1f, 1f);
    }
}
