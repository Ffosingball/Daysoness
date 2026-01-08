using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro.EditorUtilities;



public class ItemManager : MonoBehaviour
{
    [SerializeField] private GameObject Hand;
    [SerializeField] private GameObject AK47;
    [SerializeField] private GameObject LaserBlaster;
    [SerializeField] private GameObject LaserSniper;
    [SerializeField] private GameObject Pistol;
    [SerializeField] private GameObject Lightsaber;
    [SerializeField] private GameObject Knife;
    [SerializeField] private GameObject FirstAid;
    [SerializeField] private UIManager uiManager;
    [SerializeField] private PlayerComponent playerComponent;
    [SerializeField] private float firstAidAnimationTime=2f;
    [SerializeField] private int firstAidHealHP=10;
    [SerializeField] private int afterDeathLooseMaxSupportItems=4;
    [SerializeField] private int afterDeathLooseMaxCatridges=2;
    [SerializeField] private Movement movement;

    private int numOfFirstAids=0;
    //[SerializeField] private  Transform sword_hand;
    private int weaponNumber=0;
    private GameObject[] weaponsList;
    private bool usingFirstAid=false;
    private float timePassed=0f;


    public int getWeaponNumber()
    {
        return weaponNumber;
    }



    public GameObject getCurrentWeapon()
    {
        return weaponsList[weaponNumber];
    }


    private void Start()
    {
        AK47.GetComponent<SpriteRenderer>().enabled=false;
        Pistol.GetComponent<SpriteRenderer>().enabled=false;
        LaserBlaster.GetComponent<SpriteRenderer>().enabled=false;
        LaserSniper.GetComponent<SpriteRenderer>().enabled=false;
        Lightsaber.GetComponent<SpriteRenderer>().enabled=false;
        Knife.GetComponent<SpriteRenderer>().enabled=false;

        weaponsList = new GameObject[7];
        // 0 - nothing/hands, 1 - pistol, 2 - AK-47, 3 - Laser Blaster, 4 - Laser sniper, 5 - Lightsaber, 6 - Knife
        weaponsList[0] = Hand;
        weaponsList[1] = Pistol;
        weaponsList[2] = AK47;
        weaponsList[3] = LaserBlaster;
        weaponsList[4] = LaserSniper;
        weaponsList[5] = Lightsaber;
        weaponsList[6] = Knife;

        FirstAid.SetActive(false);
    }


    private void Update()
    {
        timePassed+=Time.deltaTime;

        if(usingFirstAid && timePassed>firstAidAnimationTime)
            FinishUsingFirstAid();
        else if(usingFirstAid && movement.getIfCharacterMoves())
        {
            usingFirstAid=false;
            FirstAid.SetActive(false);
            uiManager.CancelFirstAidAnimation();
        }
    }


    //True - item was picked, False - item was not picked
    private bool PickUpWeapon(WeaponTypes type)
    {
        FirearmWeapon firearmWeapon=null;
        MeeleWeapon meeleWeapon=null;
        SpriteRenderer weaponPicture=null;

        switch(type)
        {
            case WeaponTypes.AK47:
                firearmWeapon = AK47.GetComponent<FirearmWeapon>();
                weaponPicture = AK47.GetComponent<SpriteRenderer>();
                break;
            case WeaponTypes.LaserBlaster:
                firearmWeapon = LaserBlaster.GetComponent<FirearmWeapon>();
                weaponPicture = LaserBlaster.GetComponent<SpriteRenderer>();
                break;
            case WeaponTypes.LaserSniper:
                firearmWeapon = LaserSniper.GetComponent<FirearmWeapon>();
                weaponPicture = LaserSniper.GetComponent<SpriteRenderer>();
                break;
            case WeaponTypes.Pistol:
                firearmWeapon = Pistol.GetComponent<FirearmWeapon>();
                weaponPicture = Pistol.GetComponent<SpriteRenderer>();
                break;
            case WeaponTypes.Lightsaber:
                meeleWeapon = Lightsaber.GetComponent<MeeleWeapon>();
                weaponPicture = Lightsaber.GetComponent<SpriteRenderer>();
                break;
            case WeaponTypes.Knife:
                meeleWeapon = Knife.GetComponent<MeeleWeapon>();
                weaponPicture = Knife.GetComponent<SpriteRenderer>();
                break;
        }

        if(firearmWeapon!=null)
        {
            if(!firearmWeapon.getHaveThisWeapon())
            {
                firearmWeapon.setHaveThisWeapon(true);
                weaponPicture.enabled=true;
                return true;
            }
        }

        if(meeleWeapon!=null)
        {
            if(!meeleWeapon.getHaveThisWeapon())
            {
                meeleWeapon.setHaveThisWeapon(true);
                weaponPicture.enabled=true;
                return true;
            }
        }

        return false;
    }


    private bool PickUpCatridge(WeaponTypes type)
    {
        FirearmWeapon firearmWeapon=null;

        switch(type)
        {
            case WeaponTypes.AK47:
                firearmWeapon = AK47.GetComponent<FirearmWeapon>();
                break;
            case WeaponTypes.LaserBlaster:
                firearmWeapon = LaserBlaster.GetComponent<FirearmWeapon>();
                break;
            case WeaponTypes.LaserSniper:
                firearmWeapon = LaserSniper.GetComponent<FirearmWeapon>();
                break;
            case WeaponTypes.Pistol:
                firearmWeapon = Pistol.GetComponent<FirearmWeapon>();
                break;
        }

        if(firearmWeapon!=null)
        {
            if(firearmWeapon.getCurrentNumOfCatridges()<firearmWeapon.getMaxAmountOfCatridges())
            {
                firearmWeapon.increaseCatridgeCount();
                return true;
            }
        }

        return false;
    }



    private bool PickUpSupportItem(SupportItems type)
    {
        switch(type)
        {
            case SupportItems.Shield:
                if(playerComponent.getCurrentShieldLevel()<playerComponent.getMaxShieldLevel())
                {
                    playerComponent.increaseCurrentShieldLevel();
                    uiManager.ChangeShieldBar(playerComponent.getCurrentShieldLevel());
                    return true;
                }
                break;
            case SupportItems.FirstAid:
                numOfFirstAids++;
                uiManager.SetNumOfFirstAids(numOfFirstAids);
                return true;
        }

        return false;
    }



    private void OnTriggerEnter2D(Collider2D collision)
    {
        Func<WeaponTypes,bool> dealWithWeapons = null;
        WeaponTypes weaponType = WeaponTypes.None;

        switch (collision.gameObject.tag)
        {
            case "Weapon":
                weaponType = collision.gameObject.GetComponent<WeaponTypeComponent>().weaponType;
                dealWithWeapons = PickUpWeapon;
                break;
            case "Catridge":
                weaponType = collision.gameObject.GetComponent<WeaponTypeComponent>().weaponType;
                dealWithWeapons = PickUpCatridge;
                break;
            case "Support_item":
                SupportItems supportType = collision.gameObject.GetComponent<SupportTypeComponent>().supportType;
                if(PickUpSupportItem(supportType))
                    Destroy(collision.gameObject);
                break;
        }

        if(weaponType!=WeaponTypes.None)
        {
            if(dealWithWeapons(weaponType))
                Destroy(collision.gameObject);
        }
    }



    public void SwitchWeaponForward()
    {
        weaponsList[weaponNumber].SetActive(false);
        uiManager.CancelCatridgeReloadAnimation();
        weaponNumber++;

        if(weaponNumber>=weaponsList.Length)
            weaponNumber=0;

        weaponsList[weaponNumber].SetActive(true);
    }



    public void SwitchWeaponBackward()
    {
        weaponsList[weaponNumber].SetActive(false);
        uiManager.CancelCatridgeReloadAnimation();
        weaponNumber--;

        if(weaponNumber<=0)
            weaponNumber=weaponsList.Length-1;

        weaponsList[weaponNumber].SetActive(true);
    }



    public void RechargeWeapon()
    {
        if(weaponsList[weaponNumber].TryGetComponent<FirearmWeapon>(out FirearmWeapon fWeapon))
        {
            fWeapon.Recharge();
        }
    }



    public void CancelRechargeWeapon()
    {
        if(weaponsList[weaponNumber].TryGetComponent<FirearmWeapon>(out FirearmWeapon fWeapon))
        {
            fWeapon.CancelRecharge();
        }
    }



    public void StartUsingFirstAid()
    {
        if(numOfFirstAids>0)
        {
            FirstAid.SetActive(true);
            uiManager.StartFirstAidAnimation(firstAidAnimationTime);
            usingFirstAid = true;
            timePassed=0f;
        }
    }



    public void FinishUsingFirstAid()
    {
        FirstAid.SetActive(false);
        numOfFirstAids--;
        playerComponent.Heal(firstAidHealHP);
        usingFirstAid = false;
        uiManager.SetNumOfFirstAids(numOfFirstAids);
    }



    public void CancelUsingFirstAid()
    {
        FirstAid.SetActive(false);
        usingFirstAid = false;
        uiManager.CancelFirstAidAnimation();
    }



    public void Recalculation()
    {
        numOfFirstAids-=UnityEngine.Random.Range(0,afterDeathLooseMaxSupportItems);

        for(int i=0; i<weaponsList.Length; i++)
        {
            weaponsList[i].GetComponent<FirearmWeapon>().setCurrentNumOfCatridges(weaponsList[i].GetComponent<FirearmWeapon>().getCurrentNumOfCatridges()-UnityEngine.Random.Range(0,afterDeathLooseMaxCatridges));
        }
    }
}
