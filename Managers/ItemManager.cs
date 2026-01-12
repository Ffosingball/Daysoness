using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro.EditorUtilities;



public class ItemManager : MonoBehaviour
{
    //References to all weapons
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
    //How long player will use first aid before healing themselve
    [SerializeField] private float firstAidAnimationTime=2f;
    //How much first aid recovers hp
    [SerializeField] private int firstAidHealHP=10;
    //How many of each support item can player loose after death
    [SerializeField] private int afterDeathLooseMaxSupportItems=4;
    //How many every firearm weapon will loose magazines after death
    [SerializeField] private int afterDeathLooseMaxCatridges=2;
    [SerializeField] private Movement movement;

    //Current amount of first aids which player has
    private int numOfFirstAids=0;
    //Currently selected weapon
    private int weaponNumber=0;
    //List of all weapons
    private GameObject[] weaponsList;
    //Flag which tells is player using first aid or not
    private bool usingFirstAid=false;
    //How many time passed since player started using first aid
    private float timePassed=0f;


    //Getters
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
        //Deactivate all weapons and their spriteRenderers
        AK47.GetComponent<SpriteRenderer>().enabled=false;
        AK47.SetActive(false);
        Pistol.GetComponent<SpriteRenderer>().enabled=false;
        Pistol.SetActive(false);
        LaserBlaster.GetComponent<SpriteRenderer>().enabled=false;
        LaserBlaster.SetActive(false);
        LaserSniper.GetComponent<SpriteRenderer>().enabled=false;
        LaserSniper.SetActive(false);
        Lightsaber.GetComponent<SpriteRenderer>().enabled=false;
        Lightsaber.SetActive(false);
        Knife.GetComponent<SpriteRenderer>().enabled=false;
        Knife.SetActive(false);

        //Add all weapons to the array
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

        //If player uses first aid more than required than recover health 
        if(usingFirstAid && timePassed>firstAidAnimationTime)
            FinishUsingFirstAid();
        else if(usingFirstAid && movement.getIfCharacterMoves())
        {//otherwise if player moved during this period of time then cancel first aid recover
            usingFirstAid=false;
            FirstAid.SetActive(false);
            uiManager.CancelFirstAidAnimation();
        }
    }


    //This function returns True if item was picked or False if item was not picked
    private bool PickUpWeapon(WeaponTypes type)
    {
        FirearmWeapon firearmWeapon=null;
        MeeleWeapon meeleWeapon=null;
        SpriteRenderer weaponPicture=null;

        //Check which weapon type was picked and then get its components
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

        //Check if firearm was picked
        if(firearmWeapon!=null)
        {
            //Then if player picks it up first time then activate it
            if(!firearmWeapon.getHaveThisWeapon())
            {
                firearmWeapon.setHaveThisWeapon(true);
                weaponPicture.enabled=true;
                return true;
            }
        }

        //Check if meele was picked
        if(meeleWeapon!=null)
        {
            //Then if player picks it up first time then activate it
            if(!meeleWeapon.getHaveThisWeapon())
            {
                meeleWeapon.setHaveThisWeapon(true);
                weaponPicture.enabled=true;
                return true;
            }
        }

        //Otherwise it was not a weapon
        return false;
    }



    //This function returns True if item was picked or False if item was not picked
    private bool PickUpCatridge(WeaponTypes type)
    {
        FirearmWeapon firearmWeapon=null;

        //Check magazine of which weapon player picked up
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

        //Then if it was one of those weapons then increase its catridge amount
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



    //This function returns True if item was picked or False if item was not picked
    private bool PickUpSupportItem(SupportItems type)
    {
        switch(type)
        {
            //If this is a shield then increase shield level
            //If it already has maximum level then do not pick up
            case SupportItems.Shield:
                if(playerComponent.getCurrentShieldLevel()<playerComponent.getMaxShieldLevel())
                {
                    playerComponent.increaseCurrentShieldLevel();
                    uiManager.ChangeShieldBar(playerComponent.getCurrentShieldLevel());
                    return true;
                }
                break;
            //If this is a first aid then pick it up
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

        //Check what type of item was picked up, after choose correct function
        //to deal with that item. If item was picked up then destroy it
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



    //This function switches currently selected weapon to the next one in the array
    public void SwitchWeaponForward()
    {
        weaponsList[weaponNumber].SetActive(false);
        uiManager.CancelCatridgeReloadAnimation();
        weaponNumber++;

        if(weaponNumber>=weaponsList.Length)
            weaponNumber=0;

        weaponsList[weaponNumber].SetActive(true);
    }



    //This function switches currently selected weapon to the previous one in the array
    public void SwitchWeaponBackward()
    {
        weaponsList[weaponNumber].SetActive(false);
        uiManager.CancelCatridgeReloadAnimation();
        weaponNumber--;

        if(weaponNumber<=0)
            weaponNumber=weaponsList.Length-1;

        weaponsList[weaponNumber].SetActive(true);
    }



    //Tells current weapon to reload its magazine
    public void RechargeWeapon()
    {
        if(weaponsList[weaponNumber].TryGetComponent<FirearmWeapon>(out FirearmWeapon fWeapon))
        {
            fWeapon.Recharge();
        }
    }



    //Tells current weapon to cancel reloading its magazine
    public void CancelRechargeWeapon()
    {
        if(weaponsList[weaponNumber].TryGetComponent<FirearmWeapon>(out FirearmWeapon fWeapon))
        {
            fWeapon.CancelRecharge();
        }
    }



    //Starts counter for first aid usage
    public void StartUsingFirstAid()
    {
        if(numOfFirstAids>0 && !usingFirstAid)
        {
            FirstAid.SetActive(true);
            uiManager.StartFirstAidAnimation(firstAidAnimationTime);
            usingFirstAid = true;
            timePassed=0f;
        }
    }



    //If player succeds in using first aid then it will recover its health
    public void FinishUsingFirstAid()
    {
        FirstAid.SetActive(false);
        numOfFirstAids--;
        playerComponent.Heal(firstAidHealHP);
        usingFirstAid = false;
        uiManager.SetNumOfFirstAids(numOfFirstAids);
    }



    //If player fails in using first aid then it will cancel using it
    public void CancelUsingFirstAid()
    {
        FirstAid.SetActive(false);
        usingFirstAid = false;
        uiManager.CancelFirstAidAnimation();
    }



    //This function removes certain amount of support item and remove somae number of
    //magazines from every firearm weapon after player death
    public void Recalculation()
    {
        numOfFirstAids-=UnityEngine.Random.Range(0,afterDeathLooseMaxSupportItems);
        if(numOfFirstAids<0)
            numOfFirstAids=0;

        for(int i=0; i<weaponsList.Length; i++)
        {
            if(weaponsList[i].TryGetComponent<FirearmWeapon>(out FirearmWeapon firearmWeapon))
            {
                firearmWeapon.setCurrentNumOfCatridges(weaponsList[i].GetComponent<FirearmWeapon>().getCurrentNumOfCatridges()-UnityEngine.Random.Range(0,afterDeathLooseMaxCatridges));
                if(firearmWeapon.getCurrentNumOfCatridges()<0)
                    firearmWeapon.setCurrentNumOfCatridges(0);
            }
        }
    }
}
