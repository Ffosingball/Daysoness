using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;



public class Weapon
{
    private int myField1, myField2;
    private bool myField3;
    
    public Weapon()
    {
        myField1 = 0;
        myField2 = 0;
        myField3 = false;
    }

    public int magazine
    {
        get
        {
            return myField1;
        }
        set
        {
            myField1 = value;
        }
    }
    public int bullets
    {
        get
        {
            return myField2;
        }
        set
        {
            myField2 = value;
        }
    }
    public bool haveWeapon
    {
        get
        {
            return myField3;
        }
        set
        {
            myField3 = value;
        }
    }
}




public class item_pick : MonoBehaviour
{
    public Text magazineText, bulletText;
    public Image imageGun, imageRecharge;
    public Sprite AK_pic, pistol_pic, lazer_pic, anihilator_pic, lightsaber_pic, sword_pic, nothing_pic, lightsaber, saber;
    public Sprite recharge_4, recharge_3, recharge_2, recharge_1;
    private int whichWeapon=1, howManyFirearms=0;
    private bool haveLightsaber=false, haveSword=false, started=false, changedIt=false;
    public SpriteRenderer gun_in_hand, sword_in_hand;
    public GameObject bullet_AK, bullet_pistol, bullet_lazer, bullet_anihilator;
    public float speed=30f;
    private float time=0;
    private Coroutine start_fire;
    public Transform sword_hand;
    static public bool canAttack=false, shot=false;
    static public int whichSword=0;
    
    Weapon AK_47 = new Weapon();
    Weapon pistol = new Weapon();
    Weapon anihilator = new Weapon();
    Weapon lazer = new Weapon();



    private void Start()
    {
        magazineText.text = "0";
        bulletText.text = "0";
        imageRecharge.sprite = nothing_pic;

        imageGun.sprite = AK_pic;
        gun_in_hand.sprite=nothing_pic;
        sword_in_hand.sprite=nothing_pic;
        Color newColor = imageGun.color;
        newColor.a = 0.5f;
        imageGun.color = newColor;
        StartCoroutine(Count_time());
    }



    private IEnumerator Count_time()
    {
        while(true)
        {
            yield return new WaitForSeconds(0.1f);
            time-=0.1f;
        }
    }



    private void OnTriggerEnter2D(Collider2D collision)
    {

        switch (collision.gameObject.tag)
        {
            case "AK_magazine":
                if (AK_47.magazine<6)
                {
                    AK_47.magazine++;
                    Destroy(collision.gameObject);
                }
                StartCoroutine(Change_bullets());
                showWeapon();
                statistica.itemPicked++;
                break;

            case "AK_gun":
                if (AK_47.haveWeapon==false)
                {
                    AK_47.haveWeapon=true;
                    howManyFirearms++;
                    Destroy(collision.gameObject);
                }
                showWeapon();
                statistica.itemPicked++;
                break;

            case "pistol_magazine":
                if (pistol.magazine<7)
                {
                    pistol.magazine++;
                    Destroy(collision.gameObject);
                }
                StartCoroutine(Change_bullets());
                showWeapon();
                statistica.itemPicked++;
                break;

            case "pistol_gun":
                if (pistol.haveWeapon==false)
                {
                    pistol.haveWeapon=true;
                    howManyFirearms++;
                    Destroy(collision.gameObject);
                }
                showWeapon();
                statistica.itemPicked++;
                break;

            case "anihilator_magazine":
                if (anihilator.magazine<4)
                {
                    anihilator.magazine++;
                    Destroy(collision.gameObject);
                }
                StartCoroutine(Change_bullets());
                showWeapon();
                statistica.itemPicked++;
                break;

            case "anihilator_gun":
                if (anihilator.haveWeapon==false)
                {
                    anihilator.haveWeapon=true;
                    howManyFirearms++;
                    Destroy(collision.gameObject);
                }
                showWeapon();
                statistica.itemPicked++;
                break;

            case "lazer_magazine":
                if (lazer.magazine<5)
                {
                    lazer.magazine++;
                    Destroy(collision.gameObject);
                }
                StartCoroutine(Change_bullets());
                showWeapon();
                statistica.itemPicked++;
                break;

            case "lazer_gun":
                if (lazer.haveWeapon==false)
                {
                    lazer.haveWeapon=true;
                    howManyFirearms++;
                    Destroy(collision.gameObject);
                }
                showWeapon();
                statistica.itemPicked++;
                break;
            
            case "lightsaber_item":
                if (haveLightsaber==false)
                {
                    haveLightsaber=true;
                    Destroy(collision.gameObject);
                }
                showWeapon();
                statistica.itemPicked++;
                break;

            case "blade_item":
                if (haveSword==false)
                {
                    haveSword=true;
                    Destroy(collision.gameObject);
                }
                showWeapon();
                statistica.itemPicked++;
                break;
        }
    }



    private void showWeapon()
    {
        Color newColor = imageGun.color;

        switch (whichWeapon)
        {
            case 1:
                magazineText.text = ""+AK_47.magazine;
                bulletText.text = ""+AK_47.bullets;

                imageGun.sprite = AK_pic;
                gun_in_hand.sprite=AK_47.haveWeapon==true?AK_pic:nothing_pic;
                sword_in_hand.sprite=nothing_pic;
                newColor = imageGun.color;
                newColor.a = AK_47.haveWeapon==true?1f:0.5f;
                imageGun.color = newColor;
                
                break;
            case 2:
                magazineText.text = ""+pistol.magazine;
                bulletText.text = ""+pistol.bullets;

                imageGun.sprite = pistol_pic;
                gun_in_hand.sprite=pistol.haveWeapon==true?pistol_pic:nothing_pic;
                newColor = imageGun.color;
                newColor.a = pistol.haveWeapon==true?1f:0.5f;
                imageGun.color = newColor;
                
                break;
            case 3:
                magazineText.text = ""+anihilator.magazine;
                bulletText.text = ""+anihilator.bullets;

                imageGun.sprite = anihilator_pic;
                gun_in_hand.sprite=anihilator.haveWeapon==true?anihilator_pic:nothing_pic;
                newColor = imageGun.color;
                newColor.a = anihilator.haveWeapon==true?1f:0.5f;
                imageGun.color = newColor;
                
                break;
            case 4:
                magazineText.text = ""+lazer.magazine;
                bulletText.text = ""+lazer.bullets;

                imageGun.sprite = lazer_pic;
                gun_in_hand.sprite=lazer.haveWeapon==true?lazer_pic:nothing_pic;
                sword_in_hand.sprite=nothing_pic;
                newColor = imageGun.color;
                newColor.a = lazer.haveWeapon==true?1f:0.5f;
                imageGun.color = newColor;
                
                break;
            case 5:
                magazineText.text = " ";
                bulletText.text = " ";

                imageGun.sprite = lightsaber_pic;
                sword_in_hand.sprite=haveLightsaber==true?lightsaber:nothing_pic;
                gun_in_hand.sprite=nothing_pic;
                newColor = imageGun.color;
                newColor.a = haveLightsaber==true?1f:0.5f;
                imageGun.color = newColor;
                
                break;
            case 6:
                magazineText.text = " ";
                bulletText.text = " ";

                imageGun.sprite = sword_pic;
                sword_in_hand.sprite=haveSword==true?saber:nothing_pic;
                gun_in_hand.sprite=nothing_pic;
                newColor = imageGun.color;
                newColor.a = haveSword==true?1f:0.5f;
                imageGun.color = newColor;
                
                break;
            case 7:
                magazineText.text = " ";
                bulletText.text = " ";

                imageGun.sprite = nothing_pic;
                sword_in_hand.sprite=nothing_pic;
                gun_in_hand.sprite=nothing_pic;
                
                break;
        }
    }



    private IEnumerator Change_bullets() 
    {
        if (AK_47.bullets==0 && AK_47.magazine>0 && whichWeapon==1 && AK_47.haveWeapon && started==false)
        {
            started=true;

            imageRecharge.sprite = recharge_4;
            yield return new WaitForSeconds(0.5f);

            imageRecharge.sprite = recharge_3;
            yield return new WaitForSeconds(0.5f);

            imageRecharge.sprite = recharge_2;
            yield return new WaitForSeconds(0.5f);

            imageRecharge.sprite = recharge_1;
            yield return new WaitForSeconds(0.5f);
            imageRecharge.sprite = nothing_pic;

            if (AK_47.bullets==0)
            {
                AK_47.bullets=60;
                AK_47.magazine--;
            }

            started=false;
            showWeapon();
        }

        if (pistol.bullets==0 && pistol.magazine>0 && whichWeapon==2 && pistol.haveWeapon && started==false)
        {
            started=true;

            imageRecharge.sprite = recharge_4;
            yield return new WaitForSeconds(0.5f);

            imageRecharge.sprite = recharge_3;
            yield return new WaitForSeconds(0.5f);

            imageRecharge.sprite = recharge_2;
            yield return new WaitForSeconds(0.5f);

            imageRecharge.sprite = recharge_1;
            yield return new WaitForSeconds(0.5f);
            imageRecharge.sprite = nothing_pic;

            if (pistol.bullets==0)
            {
                pistol.bullets=12;
                pistol.magazine--;
            }

            started=false;
            showWeapon();
        }

        if (anihilator.bullets==0 && anihilator.magazine>0 && whichWeapon==3 && anihilator.haveWeapon && started==false)
        {
            started=true;

            imageRecharge.sprite = recharge_4;
            yield return new WaitForSeconds(0.5f);

            imageRecharge.sprite = recharge_3;
            yield return new WaitForSeconds(0.5f);

            imageRecharge.sprite = recharge_2;
            yield return new WaitForSeconds(0.5f);

            imageRecharge.sprite = recharge_1;
            yield return new WaitForSeconds(0.5f);
            imageRecharge.sprite = nothing_pic;

            if (anihilator.bullets==0)
            {
                anihilator.bullets=20;
                anihilator.magazine--;
            }

            started=false;
            showWeapon();
        }

        if (lazer.bullets==0 && lazer.magazine>0 && whichWeapon==4 && lazer.haveWeapon && started==false)
        {
            started=true;

            imageRecharge.sprite = recharge_4;
            yield return new WaitForSeconds(0.5f);

            imageRecharge.sprite = recharge_3;
            yield return new WaitForSeconds(0.5f);

            imageRecharge.sprite = recharge_2;
            yield return new WaitForSeconds(0.5f);

            imageRecharge.sprite = recharge_1;
            yield return new WaitForSeconds(0.5f);
            imageRecharge.sprite = nothing_pic;

            if (lazer.bullets==0)
            {
                lazer.bullets=40;
                lazer.magazine--;
            }

            started=false;
            showWeapon();
        }
    }



    private IEnumerator Change_bullets_prym() 
    {
        if (AK_47.magazine>0 && whichWeapon==1 && AK_47.haveWeapon && started==false)
        {
            started=true;

            imageRecharge.sprite = recharge_4;
            yield return new WaitForSeconds(0.5f);

            imageRecharge.sprite = recharge_3;
            yield return new WaitForSeconds(0.5f);

            imageRecharge.sprite = recharge_2;
            yield return new WaitForSeconds(0.5f);

            imageRecharge.sprite = recharge_1;
            yield return new WaitForSeconds(0.5f);
            imageRecharge.sprite = nothing_pic;

            AK_47.bullets=60;
            AK_47.magazine--;

            started=false;
            showWeapon();
        }

        if (pistol.magazine>0 && whichWeapon==2 && pistol.haveWeapon && started==false)
        {
            started=true;

            imageRecharge.sprite = recharge_4;
            yield return new WaitForSeconds(0.5f);

            imageRecharge.sprite = recharge_3;
            yield return new WaitForSeconds(0.5f);

            imageRecharge.sprite = recharge_2;
            yield return new WaitForSeconds(0.5f);

            imageRecharge.sprite = recharge_1;
            yield return new WaitForSeconds(0.5f);
            imageRecharge.sprite = nothing_pic;

            pistol.bullets=12;
            pistol.magazine--;

            started=false;
            showWeapon();
        }

        if (anihilator.magazine>0 && whichWeapon==3 && anihilator.haveWeapon && started==false)
        {
            started=true;

            imageRecharge.sprite = recharge_4;
            yield return new WaitForSeconds(0.5f);

            imageRecharge.sprite = recharge_3;
            yield return new WaitForSeconds(0.5f);

            imageRecharge.sprite = recharge_2;
            yield return new WaitForSeconds(0.5f);

            imageRecharge.sprite = recharge_1;
            yield return new WaitForSeconds(0.5f);
            imageRecharge.sprite = nothing_pic;

            anihilator.bullets=20;
            anihilator.magazine--;

            started=false;
            showWeapon();
        }

        if (lazer.magazine>0 && whichWeapon==4 && lazer.haveWeapon && started==false)
        {
            started=true;

            imageRecharge.sprite = recharge_4;
            yield return new WaitForSeconds(0.5f);

            imageRecharge.sprite = recharge_3;
            yield return new WaitForSeconds(0.5f);

            imageRecharge.sprite = recharge_2;
            yield return new WaitForSeconds(0.5f);

            imageRecharge.sprite = recharge_1;
            yield return new WaitForSeconds(0.5f);
            imageRecharge.sprite = nothing_pic;

            lazer.bullets=40;
            lazer.magazine--;

            started=false;
            showWeapon();
        }
    }



    private IEnumerator AK_fire()
    {
        while (AK_47.bullets>0)
        {
            Vector3 bulletStart = transform.position;
            GameObject tempBullet = Instantiate(bullet_lazer, bulletStart, Quaternion.Euler(0f,0f,0f));

            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3 direction = mousePosition - tempBullet.transform.position;
            float angle = (Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg)-90;
            angle=angle<-180?angle+360:angle;

            Destroy(tempBullet);

            bulletStart = transform.position;
            GameObject tempBullet2 = Instantiate(bullet_AK, bulletStart, Quaternion.Euler(0f,0f,0f));
            tempBullet2.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
            tempBullet2.SetActive(true);
            statistica.firedBullets++;

            StartCoroutine(Shot());

            AK_47.bullets--;
            showWeapon();

            yield return new WaitForSeconds(0.1f);
        }
    }



    private IEnumerator lazer_fire()
    {
        while (lazer.bullets>0)
        {
            Vector3 bulletStart = transform.position;
            GameObject tempBullet = Instantiate(bullet_lazer, bulletStart, Quaternion.Euler(0f,0f,0f));

            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3 direction = mousePosition - tempBullet.transform.position;
            float angle = (Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg)-90;
            angle=angle<-180?angle+360:angle;

            Destroy(tempBullet);

            bulletStart = transform.position;
            GameObject tempBullet2 = Instantiate(bullet_lazer, bulletStart, Quaternion.Euler(0f,0f,0f));
            tempBullet2.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
            tempBullet2.SetActive(true);
            statistica.firedBullets++;

            StartCoroutine(Shot());

            lazer.bullets--;
            showWeapon();

            yield return new WaitForSeconds(0.19f);
        }
    }




    private IEnumerator lightsaber_attack()
    {
        canAttack=true;
        whichSword=1;

        yield return new WaitForSeconds(0.1f);

        float currentRotation = sword_hand.transform.eulerAngles.z;
        currentRotation=currentRotation-45;
        
        //if(Movement.flipSword)
        //{
            //currentRotation=currentRotation-180;
        //}

        //Movement.flipSword=false;
        sword_hand.rotation=Quaternion.Euler(0,0,currentRotation);

        for(int i=0;i<15;i++)
        {
            currentRotation=currentRotation+6;
            sword_hand.rotation=Quaternion.Euler(0,0,currentRotation);
            yield return new WaitForSeconds(0.013f);
        }
        statistica.swungSword++;

        currentRotation=currentRotation-45;
        sword_hand.rotation=Quaternion.Euler(0,0,currentRotation);

        yield return new WaitForSeconds(0.2f);

        canAttack=false;
        whichSword=0;
    }



    private IEnumerator saber_attack()
    {
        canAttack=true;
        whichSword=2;

        yield return new WaitForSeconds(0.1f);

        float currentRotation = sword_hand.transform.eulerAngles.z;
        currentRotation=currentRotation-90;

        //if(Movement.flipSword)
        {
            currentRotation=currentRotation-180;
        }

        //Movement.flipSword=false;
        sword_hand.rotation=Quaternion.Euler(0,0,currentRotation);

        for(int i=0;i<15;i++)
        {
            currentRotation=currentRotation+12;
            sword_hand.rotation=Quaternion.Euler(0,0,currentRotation);
            yield return new WaitForSeconds(0.013f);
        }
        statistica.swungSword++;

        currentRotation=currentRotation-90;
        sword_hand.rotation=Quaternion.Euler(0,0,currentRotation);

        yield return new WaitForSeconds(0.2f);

        canAttack=false;
        whichSword=0;
    }



    private IEnumerator Shot() 
    {
        shot=true;
        yield return new WaitForSeconds(0.1f);
        shot=false;
    }



    private void Update() 
    {
        if (hp_and_else.dead && changedIt==false)
        {
            changedIt=true;
            Recalculation();
        }



        if(Input.GetKeyDown(KeyCode.E))
        {
            whichWeapon++;
            if (whichWeapon>7)
                whichWeapon=1;
            showWeapon();
        }



        if(Input.GetKeyDown(KeyCode.Q))
        {
            whichWeapon--;
            if (whichWeapon<1)
                whichWeapon=7;
            showWeapon();
        }



        if(Input.GetKeyUp(KeyCode.R))
        {
            StartCoroutine(Change_bullets_prym());
        }



        if (Input.GetMouseButtonDown(0))
        {
            if(whichWeapon==1 && AK_47.bullets>0)
            {
                //Movement.flipSword=false;
                start_fire=StartCoroutine(AK_fire());
            }
            else if(whichWeapon==2 && pistol.bullets>0 && time<=0)
            {
                time=0.5f;
                Vector3 bulletStart = transform.position;
                GameObject tempBullet = Instantiate(bullet_pistol, bulletStart, Quaternion.Euler(0f,0f,0f));

                Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector3 direction = mousePosition - tempBullet.transform.position;
                float angle = (Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg)-90;
                angle=angle<-180?angle+360:angle;
                tempBullet.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

                tempBullet.SetActive(true);
                statistica.firedBullets++;

                //Movement.flipSword=false;
                StartCoroutine(Shot());

                pistol.bullets--;
                showWeapon();
            }
            else if(whichWeapon==3 && anihilator.bullets>0 && time<=0)
            {
                time=0.4f;
                Vector3 bulletStart = transform.position;
                GameObject tempBullet = Instantiate(bullet_anihilator, bulletStart, Quaternion.Euler(0f,0f,0f));

                Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector3 direction = mousePosition - tempBullet.transform.position;
                float angle = (Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg)-90;
                angle=angle<-180?angle+360:angle;
                tempBullet.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

                tempBullet.SetActive(true);
                statistica.firedBullets++;

                //Movement.flipSword=false;
                StartCoroutine(Shot());

                anihilator.bullets--;
                showWeapon();
            }
            else if(whichWeapon==4 && lazer.bullets>0)
            {
                //Movement.flipSword=false;
                start_fire=StartCoroutine(lazer_fire());
            }
            else if(whichWeapon==5 && canAttack==false)
            {
                StartCoroutine(lightsaber_attack());
            }
            else if(whichWeapon==6 && canAttack==false)
            {
                StartCoroutine(saber_attack());
            }
            else
            {
                //Movement.flipSword=false;
                StartCoroutine(Change_bullets());
            }

            //tempBullet.transform.Translate(new Vector3(1,0,0)*speed*Time.deltaTime);
        }



        if (Input.GetMouseButtonUp(0))
        {
            if (start_fire != null)
            {
                StopCoroutine(start_fire);
                start_fire = null;
            }
        }
    }



    public void Recalculation()
    {
        AK_47.magazine=AK_47.magazine<2?AK_47.magazine:2;
        pistol.magazine=pistol.magazine<2?pistol.magazine:2;
        lazer.magazine=lazer.magazine<2?lazer.magazine:2;
        anihilator.magazine=anihilator.magazine<2?anihilator.magazine:2;

        int dis1=0, dis2=0, whichOne=0;
        int randDis=0;

        switch(howManyFirearms)
        {
            case 0:
                break;

            case 1:
                randDis=UnityEngine.Random.Range(1,3);

                if(randDis==1)
                {
                    DeleteWeapon2();
                }

                break;

            case 2:
                randDis=UnityEngine.Random.Range(1,3);

                if(randDis==1)
                {
                    DeleteWeapon2();
                }

                randDis=UnityEngine.Random.Range(1,5);

                if(randDis==3)
                {
                    DeleteWeapon2();
                }

                break;

            case 3:
                whichOne=UnityEngine.Random.Range(1,5);
                dis1=0; 
                dis2=0;

                if(AK_47.haveWeapon==false)
                    dis1=1;
                else if(pistol.haveWeapon==false)
                    dis1=2;
                else if(anihilator.haveWeapon==false)
                    dis1=3;
                else if(lazer.haveWeapon==false)
                    dis1=4;

                whichOne=UnityEngine.Random.Range(1,5);
                    
                if(whichOne==dis1 && whichOne==4)
                    whichOne=1;
                else if(whichOne==dis1)
                    whichOne++;
                    
                dis2=whichOne;
                DeleteWeapon(whichOne);

                randDis=UnityEngine.Random.Range(1,3);

                if(randDis==1)
                {
                    whichOne=UnityEngine.Random.Range(1,5);

                    while(whichOne==dis1 || whichOne==dis2)
                    {
                        if(whichOne==4)
                            whichOne=1;
                        else
                            whichOne++;
                    }
                    
                    DeleteWeapon(whichOne);
                }

                break;

            case 4:
                whichOne=UnityEngine.Random.Range(1,5);
                dis1=0; 
                dis2=0;

                dis1=whichOne;
                DeleteWeapon(whichOne);

                randDis=UnityEngine.Random.Range(1,3);

                if(randDis==1)
                {
                    whichOne=UnityEngine.Random.Range(1,5);
                    
                    if(whichOne==dis1 && whichOne==4)
                        whichOne=1;
                    else if(whichOne==dis1)
                        whichOne++;
                    
                    dis2=whichOne;
                    DeleteWeapon(whichOne);
                }

                randDis=UnityEngine.Random.Range(1,5);

                if(randDis==3)
                {
                    whichOne=UnityEngine.Random.Range(1,5);

                    while(whichOne==dis1 || whichOne==dis2)
                    {
                        if(whichOne==4)
                            whichOne=1;
                        else
                            whichOne++;
                    }
                    
                    DeleteWeapon(whichOne);
                }

                break;
        }

        if(haveLightsaber && haveSword)
        {
            randDis=UnityEngine.Random.Range(1,3);

            if(randDis==1)
            {
                randDis=UnityEngine.Random.Range(1,3);

                if(randDis==1)
                    haveSword=false;
                
                randDis=UnityEngine.Random.Range(1,5);

                if(randDis==3)
                    haveLightsaber=false;
            }
            else
            {
                randDis=UnityEngine.Random.Range(1,3);

                if(randDis==1)
                    haveLightsaber=false;
                
                randDis=UnityEngine.Random.Range(1,5);

                if(randDis==3)
                    haveSword=false;
            }
        }
        else if(haveLightsaber)
        {
            randDis=UnityEngine.Random.Range(1,3);

            if(randDis==1)
                haveLightsaber=false;
        }
        else if(haveSword)
        {
            randDis=UnityEngine.Random.Range(1,3);

                if(randDis==1)
                    haveSword=false;
        }

        //Debug.Log("Я меняю руку!");
        whichWeapon=7;
        
        //Debug.Log("Я показываю!");
        showWeapon();
    }




    private void DeleteWeapon(int whichOne)
    {

        switch(whichOne)
        {
            case 1:
                AK_47.haveWeapon=false;
                AK_47.bullets=0;
                break;
            case 2:
                pistol.haveWeapon=false;
                pistol.bullets=0;
                break;
            case 3:
                anihilator.haveWeapon=false;
                anihilator.bullets=0;
                break;
            case 4:
                lazer.haveWeapon=false;
                lazer.bullets=0;
                break;
        }
    }



    private void DeleteWeapon2()
    {
        if(AK_47.haveWeapon)
        {
            AK_47.haveWeapon=false;
            AK_47.bullets=0;
        }
        else if(pistol.haveWeapon)
        {
            pistol.haveWeapon=false;
            pistol.bullets=0;
        }
        else if(anihilator.haveWeapon)
        {
            anihilator.haveWeapon=false;
            anihilator.bullets=0;
        }
        else if(lazer.haveWeapon)
        {
            lazer.haveWeapon=false;
            lazer.bullets=0;
        }
    }



    public void Respawned()
    {
        changedIt=false;
    }
}
