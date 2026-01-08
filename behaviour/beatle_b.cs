using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class beatle_b : MonoBehaviour
{
    private Coroutine go_circle=null, one_bite=null;
    private int my_hp=5;
    public float moveSpeed = 3f;
    public float detectionRadius = 7f;
    public Sprite alive, dead, bited;
    public LayerMask barrierLayer;
    private bool canGo=true, touchSword=false, wasAttackedBySword=false;
    public float XcoordOne=0, YcoordOne=0, XcoordTwo=0, YcoordTwo=0;

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
        spriteRenderer.sprite = alive;
        collider.isTrigger = false;
        spriteRenderer.flipX = false;
    }



    void Update()
    {
        if(touchSword)// && item_pick.canAttack && wasAttackedBySword==false)
        {
            wasAttackedBySword=true;

            //if(item_pick.whichSword==1)
                my_hp-=5;
            //else if(item_pick.whichSword==2)
                my_hp-=3;
            
            StartCoroutine(Get_damage());
            stillAlive(true);
        }


        //if(item_pick.canAttack==false)
            wasAttackedBySword=false;

        

        //if(hp_and_else.dead && one_bite!=null)
        //{
            //StopCoroutine(one_bite);
        //}



        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (my_hp>0 && canGo)
        {
            if (distanceToPlayer <= detectionRadius)
            {
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



    private void stillAlive(bool doIt)
    {
        if (my_hp<1)
        {
            canGo=false;
            collider.isTrigger = true;

            if (go_circle!=null)
            {
                StopCoroutine(go_circle);
                go_circle = null;
            }
            if (doIt)
                statistica.beatleKilled++;
            
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
        spriteRenderer.sprite = bited;

        /*hp_and_else.shield-=10;
        if(hp_and_else.shield>=75)
            hp_and_else.real_hp-=0;
        else if(hp_and_else.shield>=50)
            hp_and_else.real_hp-=0.1f;
        else if(hp_and_else.shield>=25)
            hp_and_else.real_hp-=0.3f;
        else if(hp_and_else.shield>0)
            hp_and_else.real_hp-=0.6f;
        else
            hp_and_else.real_hp-=1f;

        hp_and_else.real_hp=hp_and_else.real_hp<0?0:hp_and_else.real_hp;
        hp_and_else.shield=hp_and_else.shield<0?0:hp_and_else.shield;
        
        yield return new WaitForSeconds(0.2f);

        my_hp-=5;
        stillAlive(false);

        if(hp_and_else.poisionLevel<=1)
        {
            hp_and_else.poisionLevel++;

            yield return new WaitForSeconds(0.3f);

            for(int i=0; i<10; i++)
            {
                hp_and_else.real_hp-=1f;
                yield return new WaitForSeconds(1f);
            }

            hp_and_else.poisionLevel--;
        }*/
        yield return new WaitForSeconds(0.2f);

    }




    private IEnumerator Get_damage() 
    {
        spriteRenderer.color = new Color(1f, 0.2f, 0.2f, 1f);
        yield return new WaitForSeconds(0.15f);
        spriteRenderer.color = new Color(1f, 1f, 1f, 1f);
    }



    private IEnumerator Movement_to_point() 
    {
        while(true)
        {
            Vector3 zero=new Vector3(XcoordOne,YcoordOne,0);
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
                distanceToCenter = Vector2.Distance(transform.position, zero);
            }

            zero=new Vector3(XcoordTwo,YcoordTwo,0);
            distanceToCenter = Vector2.Distance(transform.position, zero);

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
                distanceToCenter = Vector2.Distance(transform.position, zero);
            }
        }
    }
}
