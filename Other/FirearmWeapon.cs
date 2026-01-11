using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;

public class FirearmWeapon : MonoBehaviour
{
    [SerializeField] private int catridgeCapacity;
    [SerializeField] private float dmgPerPatron;
    [SerializeField] private int maxAmountOfCatridges;
    [SerializeField] private bool automatic;
    [SerializeField] private bool piercingBullets;
    [SerializeField] private int piercingAmount;
    [SerializeField] private float firePeriod;
    [SerializeField] private float realoadTime;
    [SerializeField] private WeaponTypes type;
    private int currentNumOfBullets=0;
    private int currentNumOfCatridges=0;
    private bool haveThisWeapon=false;
    private Coroutine fireBullets, rechargeWait;
    [SerializeField] private UIManager uiManager;
    [SerializeField] private float bulletRange = 300f;
    //[SerializeField] private LayerMask barrierLayer;


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
        if(rechargeWait==null && haveThisWeapon && currentNumOfCatridges>0)
        {
            rechargeWait = StartCoroutine(RealoadWait());
            uiManager.StartCatridgeReloadAnimation(realoadTime);
        }
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
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        Vector3 direction = (mousePosition - transform.position).normalized;

        //Debug.DrawRay(transform.position, direction*200f, Color.red, 3f);
        LayerMask hitMask = LayerMask.GetMask("Enemy", "Barrier");
        //RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, bulletRange, hitMask);

        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, direction, bulletRange, hitMask);

        float piercedEnemies=0;
        for (int i=0; i<hits.Length; i++)
        {
            if(hits[i].collider.gameObject.tag=="barrier")
                break;
            else
            {
                GameObject parent = hits[i].collider.gameObject.transform.parent.gameObject;

                if(parent.TryGetComponent<CommonEnemyBehaviour>(out CommonEnemyBehaviour commonEnemyBehaviour))
                {
                    if(!commonEnemyBehaviour.IsDead())
                    {
                        commonEnemyBehaviour.TakeDamage(dmgPerPatron);
                        piercedEnemies++;

                        if(piercingBullets)
                        {
                            if(piercedEnemies>=piercingAmount)
                                break;
                        }
                        else
                            break;
                    }
                }
            }
        }

        EventsManager.CallOnRobotsActivate(transform.position);
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

        rechargeWait=null;
    }
}