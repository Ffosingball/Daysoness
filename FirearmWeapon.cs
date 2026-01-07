using UnityEngine;
using System.Collections;

public class FirearmWeapon : MonoBehaviour
{
    [SerializeField] private int catridgeCapacity;
    [SerializeField] private float dmgPerPatron;
    [SerializeField] private int maxAmountOfCatridges;
    [SerializeField] private bool automatic;
    [SerializeField] private float firePeriod;
    [SerializeField] private WeaponTypes type;
    private int currentNumOfBullets=0;
    private int currentNumOfCatridges=0;
    [SerializeField] private float bulletSpeed;
    private bool haveThisWeapon=false;
    private Coroutine fireBullets;


    public int getMaxAmountOfCatridges()
    {
        return maxAmountOfCatridges;
    }

    public WeaponTypes getWeaponType()
    {
        return type;
    }

    public int getCurrentNumOfCatridges()
    {
        return currentNumOfCatridges;
    }

    public int getCurrentNumOfBullets()
    {
        return currentNumOfBullets;
    }

    public void setCurrentNumOfCatridges(int newNum)
    {
        if(newNum<0)
            currentNumOfCatridges=0;
        else
            currentNumOfCatridges=newNum;
    }

    public bool getHaveThisWeapon()
    {
        return haveThisWeapon;
    }

    public void setHaveThisWeapon(bool val)
    {
        haveThisWeapon=val;
    }

    public void increaseCatridgeCount()
    {
        currentNumOfCatridges++;
        //Debug.Log("Increased num of catridges!");
    }



    public void StartFire()
    {
        

    }


    public void StopFire()
    {
        
    }


    public void Recharge()
    {
        
    }


    public void CancelRecharge()
    {
        
    }


    private IEnumerator FireBullets()
    {
        while (currentNumOfBullets>0)
        {
            Vector3 bulletStart = transform.position;
            //GameObject tempBullet = Instantiate(bullet_lazer, bulletStart, Quaternion.Euler(0f,0f,0f));

            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            //Vector3 direction = mousePosition - tempBullet.transform.position;
            //float angle = (Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg)-90;
            //angle=angle<-180?angle+360:angle;

            //Destroy(tempBullet);

            bulletStart = transform.position;
            //GameObject tempBullet2 = Instantiate(bullet_AK, bulletStart, Quaternion.Euler(0f,0f,0f));
            //tempBullet2.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
            //tempBullet2.SetActive(true);
            statistica.firedBullets++;

            //StartCoroutine(Shot());

            //AK_47.bullets--;
            //showWeapon();

            yield return new WaitForSeconds(0.1f);
        }
    }
}