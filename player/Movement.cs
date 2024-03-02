using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public GameObject camera_;
    public Transform gun_in_hand_2, sword_in_hand;
    public float speed=5f, time=0;
    public static Vector3 currentPos;
    public static float angle, timeMove=0;
    private Rigidbody2D playerRigid;
    private Coroutine TurnIt;
    public static bool moving=false, canMove=true, timeToTurn=false, flipSword=false;
    private bool clickedMouse=false;



    private void awake()
    {
        camera_ = GetComponent<GameObject>();
        playerRigid = GetComponent<Rigidbody2D>();
    }



    private void Start() 
    {
        StartCoroutine(TimeCounter());
    }




    private void FixedUpdate() 
    {
        if(canMove)
        {
            if(Input.GetKey(KeyCode.D) && Input.GetKey(KeyCode.W))
            {
                transform.Translate(new Vector3(0.7f,0.7f,0)*speed*Time.deltaTime);
                camera_.transform.position = posForCamera();
                Flip("right");

                moving=true;
            }
            else if(Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.W))
            {
                transform.Translate(new Vector3(-0.7f,0.7f,0)*speed*Time.deltaTime);
                camera_.transform.position = posForCamera();
                Flip("left");

                moving=true;
            }
            else if(Input.GetKey(KeyCode.D) && Input.GetKey(KeyCode.S))
            {
                transform.Translate(new Vector3(0.7f,-0.7f,0)*speed*Time.deltaTime);
                camera_.transform.position = posForCamera();
                Flip("right");

                moving=true;
            }
            else if(Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.S))
            {
                transform.Translate(new Vector3(-0.7f,-0.7f,0)*speed*Time.deltaTime);
                camera_.transform.position = posForCamera();
                Flip("left");

                moving=true;
            }
            else if(Input.GetKey(KeyCode.D))
            {
                transform.Translate(new Vector3(1,0,0)*speed*Time.deltaTime);
                camera_.transform.position = posForCamera();
                Flip("right");

                moving=true;
            }
            else if(Input.GetKey(KeyCode.A))
            {
                transform.Translate(new Vector3(1,0,0)*-speed*Time.deltaTime);
                camera_.transform.position = posForCamera();
                Flip("left");

                moving=true;
            }

            else if(Input.GetKey(KeyCode.S))
            {
                transform.Translate(new Vector3(0,1,0)*-speed*Time.deltaTime);
                camera_.transform.position = posForCamera();

                moving=true;
            }

            else if(Input.GetKey(KeyCode.W))
            {
                transform.Translate(new Vector3(0,1,0)*speed*Time.deltaTime);
                camera_.transform.position = posForCamera();

                moving=true;
            }
            else
            {
                moving=false;
            }
        }
    }



    private void Flip(string side)
    {
        /*Debug.Log(clickedMouse+"mouse");
        Debug.Log(flipSword+"flip");*/

        if(side=="left" && transform.localScale.x>0)
        {
            Vector3 newScale = transform.localScale;
            newScale.x *= -1;
            transform.localScale = newScale;
            
            if(clickedMouse)
                flipSword=true;
        }
        else if(side=="right" && transform.localScale.x<0)
        {
            Vector3 newScale = transform.localScale;
            newScale.x *= -1;
            transform.localScale = newScale;

            if(clickedMouse)
                flipSword=true;
        }

        clickedMouse=false;
    }



    private void Update() 
    {
        canMove=timeMove<=0?true:false;



        camera_.transform.position = posForCamera();
        change();

        if(item_pick.canAttack==false)
        {
            Vector3 newScale = transform.localScale;
            if(newScale.x>0)
            {
                Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector3 direction = mousePosition - sword_in_hand.position;
                angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                sword_in_hand.rotation=Quaternion.Euler(new Vector3(0, 0, angle));
            }
            else
            {
                Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector3 direction = mousePosition - sword_in_hand.position;
                angle = (Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg)-180;
                angle=angle<-180?angle+360:angle;
                sword_in_hand.rotation=Quaternion.Euler(new Vector3(0, 0, angle));
            }
        }



        if (Input.GetMouseButtonDown(0))
        {
            time=2f;
            TurnIt=StartCoroutine(TurnWeapon());
            clickedMouse=true;
        }



        if (Input.GetMouseButtonUp(0))
        {
            if (TurnIt != null)
            {
                StopCoroutine(TurnIt);
                TurnIt = null;
            }

            timeToTurn=true;
        }



        if(Input.GetKeyDown(KeyCode.E))
        {
            time=0;
            timeToTurn=true;
        }



        if(Input.GetKeyDown(KeyCode.Q))
        {
            time=0;
            timeToTurn=true;
        }



        if (time<=0 && timeToTurn)
        {
            Vector3 newScale = gun_in_hand_2.localScale;

            //Debug.Log(transform.localScale.x);
            //Debug.Log(newScale.x);

            //transform.localScale.x!=newScale.x
            if((transform.localScale.x>0 && newScale.x<0) || (transform.localScale.x<0 && newScale.x<0))
            {
                newScale.x *= -1;
                gun_in_hand_2.localScale = newScale;
            }

            gun_in_hand_2.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
            timeToTurn=false;
        }
    }



    private IEnumerator TurnWeapon() 
    {
        while(true && item_pick.canAttack==false)
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3 direction = mousePosition - gun_in_hand_2.position;
            angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            if(moving)
            {
                if(transform.localScale.x>0)
                {
                    if (angle<=85 && angle>=-85)
                    {
                        FlipWeapon("right");
                        gun_in_hand_2.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
                    }
                    else if((angle<=95 && angle>=85)||(angle<=-85 && angle>=-95))
                    {
                        gun_in_hand_2.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
                    }
                    else
                    {
                        FlipWeapon("left");
                        angle=angle+180;
                        gun_in_hand_2.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
                    }
                }
                else
                {
                    if (angle<=85 && angle>=-85)
                    {
                        FlipWeapon("left");
                        gun_in_hand_2.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
                    }
                    else if((angle<=95 && angle>=85)||(angle<=-85 && angle>=-95))
                    {
                        gun_in_hand_2.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
                    }
                    else
                    {
                        FlipWeapon("right");
                        angle=angle+180;
                        gun_in_hand_2.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
                    }
                }
            }
            else
            {
                FlipWeapon("right");

                if (angle<=85 && angle>=-85)
                {
                    Flip("right");

                    gun_in_hand_2.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
                }
                else if((angle<=95 && angle>=85)||(angle<=-85 && angle>=-95))
                {
                    gun_in_hand_2.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
                }
                else
                {
                    Flip("left");

                    if(angle<0)
                    {
                        angle=angle+180;
                        gun_in_hand_2.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
                    }
                    else
                    {
                        angle=angle-180;
                        gun_in_hand_2.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
                    }
                }
            }

            time+=0.02f;

            yield return new WaitForSeconds(0.02f);
        }
    }




    private void FlipWeapon(string side)
    {
        if(side=="left" && gun_in_hand_2.localScale.x>0)
        {
            Vector3 newScale = gun_in_hand_2.localScale;
            newScale.x *= -1;
            gun_in_hand_2.localScale = newScale;
        }
        else if(side=="right" && gun_in_hand_2.localScale.x<0)
        {
            Vector3 newScale = gun_in_hand_2.localScale;
            newScale.x *= -1;
            gun_in_hand_2.localScale = newScale;
        }
    }




    public void change()
    {
        currentPos=transform.position;
    }




    private Vector3 posForCamera()
    {
        Vector3 curPos=transform.position;
        curPos.z=-10f;
        
        return curPos;
    }



    private IEnumerator TimeCounter() 
    {
        while(true)
        {
            yield return new WaitForSeconds(0.1f);
            time-=0.1f;
            timeMove-=0.1f;
        }
    }
}
