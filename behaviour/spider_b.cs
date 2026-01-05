using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spider_b : MonoBehaviour
{

    private Coroutine reduce_hp=null, go_circle=null, checkAttack=null;
    private int my_hp=10;
    public float moveSpeed = 2f;
    public float detectionRadius = 10f;
    public Sprite alive, dead, bited;
    public LayerMask barrierLayer;
    private bool canGo=true, touchSword=false, wasAttackedBySword=false;
    public float XcoordNest=0, YcoordNest=0;

    private Rigidbody2D rb2d;
    public SpriteRenderer spriteRenderer;
    private Collider2D collider;

    private Transform player;
    public Transform boxSpider;



    private void Start() 
    {
        //spriteRenderer = GetComponent<SpriteRenderer>();
        collider = GetComponent<Collider2D>();
        rb2d = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("player").transform;

        spriteRenderer.color = new Color(1f, 1f, 1f, 1f);
        spriteRenderer.sprite = alive;
        collider.isTrigger = false;
        spriteRenderer.flipX = false;
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

            if (checkAttack != null)
            {
                StopCoroutine(checkAttack);
                checkAttack = null;
            }
        }



        if(touchSword)// && item_pick.canAttack && wasAttackedBySword==false)
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



        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (my_hp>0 && canGo)
        {
            if (distanceToPlayer <= detectionRadius)
            {
                //Debug.Log("У подхода к игроку!");
                if (go_circle != null)
                {
                    StopCoroutine(go_circle);
                    go_circle = null;
                }

                Vector2 direction = (player.position - transform.position).normalized;

                RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, detectionRadius, barrierLayer);

                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

                if (angle<=90 && angle>=-90)
                {
                    spriteRenderer.flipX = false;
                }
                else
                {
                    spriteRenderer.flipX = true;
                }

                if (hit.collider == null)
                {
                    transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
                    boxSpider.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
                    transform.Translate(new Vector3(1,0,0)*moveSpeed*Time.deltaTime);
                    boxSpider.position = boxSpider.position;
                }
                else
                {
                    angle+=50;
                    transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
                    boxSpider.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
                    transform.Translate(new Vector3(1,0,0)*moveSpeed*Time.deltaTime);
                    boxSpider.position = boxSpider.position;
                }
            }
            else
            {
                canGo=true;

                if (go_circle==null)
                {
                    go_circle=StartCoroutine(Movement_to_point());
                }
            }
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



    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name=="Main_character" && checkAttack==null)
        {
            //Debug.Log("Коснулся!");
            canGo=false;
            if(reduce_hp==null)
                reduce_hp=StartCoroutine(Bite());
        }

        StopCheckAttack();
    }



    private void stillAlive()
    {
        //Debug.Log("Я вызван!");
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

            //Debug.Log("Я умер");
            statistica.spidersKilled++;
            spriteRenderer.sprite = dead;
            StartCoroutine(After_death());
        }
        //Debug.Log("Ясно");
    }



    private void OnCollisionExit2D(Collision2D other) 
    {
        canGo=true;

        if(checkAttack==null)
        {
            //Debug.Log("Начал проверку!");
            checkAttack=StartCoroutine(CheckIt());
        }
    }



    private IEnumerator CheckIt()
    {
        //Debug.Log("В начале!");
        yield return new WaitForSeconds(0.2f);

        if (reduce_hp != null)
        {
            StopCoroutine(reduce_hp);
            reduce_hp = null;
            spriteRenderer.sprite = alive;
        }
        //Debug.Log("В конце!");

        StopCheckAttack();
    }



    private void StopCheckAttack()
    {
        if(checkAttack!=null)
        {
            StopCoroutine(checkAttack);
            checkAttack = null;
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
            //Debug.Log("Я наношу урон!");
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
            //Debug.Log("Я укусил!");

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
    }



    private IEnumerator Movement_to_point() 
    {
        Vector3 zero=new Vector3(XcoordNest,YcoordNest,0);
        float distanceToCenter = Vector2.Distance(transform.position, zero);

        while(distanceToCenter>=0.1f)
        {
            Vector2 direction = (zero - transform.position).normalized;

            RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, detectionRadius, barrierLayer);

            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            //Debug.Log("1: "+angle);

            if (angle<=90 && angle>=-90)
            {
                spriteRenderer.flipX = false;
            }
            else
            {
                spriteRenderer.flipX = true;
            }

            if (hit.collider == null)
            {
                transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
                boxSpider.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
                transform.Translate(new Vector3(1,0,0)*moveSpeed*0.02f);
                boxSpider.position = boxSpider.position;
            }
            else
            {
                angle+=50;
                transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
                boxSpider.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
                transform.Translate(new Vector3(1,0,0)*moveSpeed*0.02f);
                boxSpider.position = boxSpider.position;
            }

            yield return new WaitForSeconds(0.02f);
            //Debug.Log("Я в цыкле!");
            distanceToCenter = Vector2.Distance(transform.position, zero);
        }
        //Debug.Log("Я вышел из цикла!");

        //stop=true;
        Next_step();
    }



    private void Next_step()
    {
        //Debug.Log("Я у некст степ!");
        if(go_circle!=null)
        {
            StopCoroutine(go_circle);
            go_circle=StartCoroutine(Wait_till());
        }
    }



    private IEnumerator Wait_till() 
    {
        //wait=true;

        while(true)
        {
            //Debug.Log("Я жду!");
            spriteRenderer.flipX = false;
            yield return new WaitForSeconds(1f);
        }
    }
}
