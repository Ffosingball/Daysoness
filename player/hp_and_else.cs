using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class hp_and_else : MonoBehaviour
{

    public Image imageHP, imageProtect, imageReheal, poisionIcon;
    public Text firstAidText;
    public Sprite[] hp_pics=new Sprite[20];
    public Sprite[] shield_pics=new Sprite[5];
    public Sprite recharge_4, recharge_3, recharge_2, recharge_1, nothing_pic, poision_1, poision_2;
    public int hp=19;
    static public int shield=0, poisionLevel=0;
    static public bool dead=false;
    static public float real_hp=19;
    private int firstAidAmount=0;
    private Coroutine cannotHeal=null, waitForHealing=null;
    public GameObject deadScreen, ui_elemen;
    private Vector3 curPos;



    private void Start() 
    {
        imageReheal.sprite = nothing_pic;
        deadScreen.SetActive(false);
        ui_elemen.SetActive(true);
    }



    void Update()
    {
        hp=Mathf.RoundToInt(real_hp);

        if(hp>0 && hp<20)
            imageHP.sprite=hp_pics[hp];
        else if(hp<=0)
        {
            dead=true;
            DeadScreen();
        }

        if(poisionLevel==0)
            poisionIcon.sprite=nothing_pic;
        else if(poisionLevel==1)
            poisionIcon.sprite=poision_1;
        else if(poisionLevel==2)
            poisionIcon.sprite=poision_2;


        if(shield>75)
            imageProtect.sprite=shield_pics[4];
        else if(shield>50 && shield<=75)
            imageProtect.sprite=shield_pics[3];
        else if(shield>25 && shield<=50)
            imageProtect.sprite=shield_pics[2];
        else if(shield>0 && shield<=25)
            imageProtect.sprite=shield_pics[1];
        else
            imageProtect.sprite=shield_pics[0];

        Vector3 newPos=transform.position;
        if(cannotHeal!=null)
        {
            if(curPos!=newPos)
            {
                StopCoroutine(cannotHeal);
                cannotHeal=null;
                imageReheal.sprite = nothing_pic;
            }
        }

        
        if(Input.GetKeyDown(KeyCode.LeftControl))
        {
            if(firstAidAmount>0 && cannotHeal==null && waitForHealing==null)
            {
                curPos=transform.position;
                cannotHeal=StartCoroutine(Healing());
            }
        }
    }



    private void DeadScreen()
    {
        Movement.timeMove=0f;
        Time.timeScale = 0f;
        mousePicture.change_coursor_menu();
        deadScreen.SetActive(true);
        ui_elemen.SetActive(false);
        reCalculation();
    }



    public void Respawn()
    {
        transform.position=new Vector3(0,0,-0.5f);
        mousePicture.change_coursor_target();
        deadScreen.SetActive(false);
        ui_elemen.SetActive(true);
        Time.timeScale = 1f;
        dead=false;
    }



    private void reCalculation()
    {
        real_hp=19;
        hp=19;
        poisionLevel=0;
        firstAidAmount/=2;
        firstAidText.text=""+firstAidAmount;
        shield=0;
    }



    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.gameObject.tag)
        {
            case "first_aid":
                if(firstAidAmount<10)
                {
                    firstAidAmount++;
                    firstAidText.text=""+firstAidAmount;
                    Destroy(collision.gameObject);
                }
                statistica.itemPicked++;
                break;
            case "shield":
                if (shield<=75)
                {
                    shield+=25;
                    Destroy(collision.gameObject);
                }
                statistica.itemPicked++;
                break;
        }
    }



    private IEnumerator Healing() 
    {
        imageReheal.sprite = recharge_4;
        yield return new WaitForSeconds(0.5f);

        imageReheal.sprite = recharge_3;
        yield return new WaitForSeconds(0.5f);

        imageReheal.sprite = recharge_2;
        yield return new WaitForSeconds(0.5f);

        imageReheal.sprite = recharge_1;
        yield return new WaitForSeconds(0.5f);
        imageReheal.sprite = nothing_pic;

        firstAidAmount--;
        firstAidText.text=""+firstAidAmount;
        real_hp+=5;
        real_hp=real_hp>19?19:real_hp;

        StopIt();
    }



    void StopIt()
    {
        if(cannotHeal!=null)
        {
            StopCoroutine(cannotHeal);
            cannotHeal=null;
        }

        waitForHealing=StartCoroutine(WaitHeal());
    }



    private IEnumerator WaitHeal() 
    {
        imageReheal.sprite = recharge_4;
        yield return new WaitForSeconds(0.5f);

        imageReheal.sprite = recharge_3;
        yield return new WaitForSeconds(0.5f);

        imageReheal.sprite = recharge_2;
        yield return new WaitForSeconds(0.5f);

        imageReheal.sprite = recharge_1;
        yield return new WaitForSeconds(0.5f);
        imageReheal.sprite = nothing_pic;

        AbsoluteStop();
    }



    void AbsoluteStop()
    {
        if(waitForHealing!=null)
        {
            StopCoroutine(waitForHealing);
            waitForHealing=null;
        }
    }
}
