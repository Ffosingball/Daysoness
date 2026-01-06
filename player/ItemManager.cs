using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro.EditorUtilities;



public class ItemManager : MonoBehaviour
{
    [SerializeField] private GameObject AK47;
    [SerializeField] private GameObject LaserBlaster;
    [SerializeField] private GameObject LaserSniper;
    [SerializeField] private GameObject Pistol;
    [SerializeField] private GameObject Lightsaber;
    [SerializeField] private GameObject Knife;
    [SerializeField] private UIManager uiManager;
    [SerializeField] private PlayerComponent playerComponent;

    private GameObject currentSelectedWeapon;
    private int numOfFirstAids=0;
    //[SerializeField] private  Transform sword_hand;



    public GameObject getCurrentWeapon()
    {
        return currentSelectedWeapon;
    }


    //True - item was picked, False - item was not picked
    private bool PickUpWeapon(WeaponTypes type)
    {
        FirearmWeapon firearmWeapon=null;
        MeeleWeapon meeleWeapon=null;

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
            case WeaponTypes.Lightsaber:
                meeleWeapon = Lightsaber.GetComponent<MeeleWeapon>();
                break;
            case WeaponTypes.Knife:
                meeleWeapon = Knife.GetComponent<MeeleWeapon>();
                break;
        }

        if(firearmWeapon!=null)
        {
            if(!firearmWeapon.getHaveThisWeapon())
            {
                firearmWeapon.setHaveThisWeapon(true);
                return true;
            }
        }

        if(meeleWeapon!=null)
        {
            if(!meeleWeapon.getHaveThisWeapon())
            {
                meeleWeapon.setHaveThisWeapon(true);
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
                break;
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
        
    }



    public void SwitchWeaponBackward()
    {
        
    }



    public void RechargeWeapon()
    {
        
    }



    public void UseFirstAid()
    {
        
    }



    public void Recalculation()
    {
        //LOOSE SOME CATRIDGES AND SUPPORT ITEMS WHEN CHARACTER DIES
    }
}
