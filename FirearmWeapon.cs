using UnityEngine;
using System.Collections;

public class FirearmWeapon : MonoBehaviour
{
    [SerializeField] private int catridgeCapacity;
    [SerializeField] private float dmgPerPatron;
    [SerializeField] private int maxAmountOfCatridges;
    [SerializeField] private bool automatic;
    [SerializeField] private float firePeriod;
    [SerializeField] private float realoadTime;
    //[SerializeField] private float bulletSpeed;
    [SerializeField] private GameObject bullet;
    [SerializeField] private WeaponTypes type;
    private int currentNumOfBullets=0;
    private int currentNumOfCatridges=0;
    [SerializeField] private float bulletSpeed;
    private bool haveThisWeapon=false;
    private Coroutine fireBullets, rechargeWait;
    [SerializeField] private UIManager uiManager;


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
        fireBullets = StartCoroutine(FireBullets());
    }


    public void StopFire()
    {
        if(fireBullets!=null)
        {
            StopCoroutine(fireBullets);
            fireBullets=null;
        }
    }


    public void Recharge()
    {
        rechargeWait = StartCoroutine(RealoadWait());
        uiManager.StartCatridgeReloadAnimation(realoadTime);
    }


    public void CancelRecharge()
    {
        if(rechargeWait!=null)
        {
            StopCoroutine(rechargeWait);
            uiManager.CancelCatridgeReloadAnimation();
            rechargeWait=null;
        }

        if(fireBullets!=null)
        {
            StopCoroutine(fireBullets);
            fireBullets=null;
        }
    }


    private void CreateABullet()
    {
        Vector3 bulletStart = transform.position;
        GameObject newBullet = Instantiate(bullet, bulletStart, Quaternion.Euler(0f,0f,0f));

        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 direction = mousePosition - newBullet.transform.position;
        float angle = (Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg)-90;
        angle=angle<-180?angle+360:angle;
        newBullet.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
                
        BulletBehaviour bulletBehaviour = newBullet.GetComponent<BulletBehaviour>();
        bulletBehaviour.setSpeed(bulletSpeed);
        bulletBehaviour.setDMG(dmgPerPatron);
        newBullet.SetActive(true);

        currentNumOfBullets--;
    }


    private IEnumerator FireBullets()
    {
        if(automatic)
        {
            while(currentNumOfBullets>0 || currentNumOfCatridges>0)
            {
                while (currentNumOfBullets>0)
                {
                    CreateABullet();

                    yield return new WaitForSeconds(firePeriod);
                }

                Recharge();
                while(currentNumOfBullets<=0)
                {
                    yield return null;
                }
            }
        }
        else
        {
            if(currentNumOfBullets>0)
                CreateABullet();
            else
            {
                Recharge();

                while(currentNumOfBullets<=0)
                {
                    yield return null;
                }

                CreateABullet();
            }
        }

        StopFire();
    }



    private IEnumerator RealoadWait()
    {
        if(currentNumOfCatridges>0)
        {
            float timePassed=0f;

            while(timePassed<realoadTime)
            {
                timePassed+=Time.deltaTime;
                yield return null;
            }

            currentNumOfCatridges--;
            currentNumOfBullets = catridgeCapacity;
        }
        else
            CancelRecharge();
    }
}