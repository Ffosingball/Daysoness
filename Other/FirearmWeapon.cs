using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;
using System.Runtime.CompilerServices;

public class FirearmWeapon : MonoBehaviour
{
    //How many bullets has a single magazine
    [SerializeField] private int catridgeCapacity;
    //damage of a single bullet
    [SerializeField] private float dmgPerPatron;
    //How many magazines of this weapon can player carry
    [SerializeField] private int maxAmountOfCatridges;
    //Flag which indicates weather a weapon is automatic or manual
    [SerializeField] private bool automatic;
    //Flag which indicates if bullet can pierce more than one enemy
    [SerializeField] private bool piercingBullets;
    //Maximum amount of enemies that a single bullet can pierce
    [SerializeField] private int piercingAmount;
    //Time between firing bullets
    [SerializeField] private float firePeriod;
    //Time to reaload a weapon
    [SerializeField] private float realoadTime;
    [SerializeField] private WeaponTypes type;
    private int currentNumOfBullets=0;
    private int currentNumOfCatridges=0;
    //Does player have this weapon or not
    private bool haveThisWeapon=false;
    //fireNullets stores coroutine which fires bullets and reloads it at certain rate
    //rechargeWait stores reload weapon coroutine so if needed it can be canceled
    private Coroutine fireBullets, rechargeWait;
    //Time passed since previous shot
    private float timeSinceLastShot=0f;
    [SerializeField] private UIManager uiManager;
    //How far bullet raycast will go
    [SerializeField] private float bulletRange = 300f;
    //Reference to animation component of its weapon
    private FirearmWeaponAnimation firearmWeaponAnimation;


    //Getters and setters
    public int getMaxAmountOfCatridges()
    {
        return maxAmountOfCatridges;
    }

    public bool isFiring()
    {
        return fireBullets!=null;
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
    }



    private void Start()
    {
        firearmWeaponAnimation = GetComponent<FirearmWeaponAnimation>();
        EventsManager.OnWeaponSwitched+=CancelRecharge;
    }



    private void Update()
    {
        //Increase counter
        timeSinceLastShot+=Time.deltaTime;
    }



    public void StartFire()
    {
        //Just starts fire bullets coroutine if weapon do not reloads
        if(rechargeWait==null)
            fireBullets = StartCoroutine(FireBullets());
    }



    //Stops fire bullet coroutine
    public void StopFire()
    {
        if(fireBullets!=null)
        {
            StopCoroutine(fireBullets);
            fireBullets=null;
        }
    }



    //Starts reload wait time for this weapon
    public void Recharge()
    {
        firearmWeaponAnimation.StartedRecharge();

        if(rechargeWait==null && haveThisWeapon && currentNumOfCatridges>0)
        {
            rechargeWait = StartCoroutine(RealoadWait());
            uiManager.StartCatridgeReloadAnimation(realoadTime);
        }
    }



    //Cancells reload wait
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



    //Creates a raycast in the direction of the mouse and checks if someone was hitted
    private void CreateABullet()
    {
        EventsManager.CallOnFireBullet();
        //Get mouse direction 
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        Vector3 direction = (mousePosition - transform.position).normalized;

        //Set mask in which look for colliders
        LayerMask hitMask = LayerMask.GetMask("Enemy", "Barrier");

        //Get all colliders on the way of a bullet
        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, direction, bulletRange, hitMask);

        float piercedEnemies=0;
        for (int i=0; i<hits.Length; i++)
        {//Traverse through objects in the array
            if(hits[i].collider.gameObject.tag=="barrier")
                break;//If bullet hits an obstacle then bullet is stuck there
            else
            {
                //Otherwise get parent of  the hitted collider, because all enemies has
                //separate hitbox for bullets as child of the original enemy
                GameObject parent = hits[i].collider.gameObject.transform.parent.gameObject;

                if(parent.TryGetComponent<CommonEnemyBehaviour>(out CommonEnemyBehaviour commonEnemyBehaviour))
                {//Check if hitted object is an enemy
                    if(!commonEnemyBehaviour.IsDead())
                    {
                        commonEnemyBehaviour.TakeDamage(dmgPerPatron);
                        piercedEnemies++;

                        //Proceed piercing bullets
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
        timeSinceLastShot=0f;
    }



    //Coroutine which fires a bullet
    private IEnumerator FireBullets()
    {
        //Check if weapon automatic or not
        if(automatic)
        {
            //Check that there are still some bullets and magazines left
            while(currentNumOfBullets>0 || currentNumOfCatridges>0)
            {
                //If there are some bullets then fire them
                while (currentNumOfBullets>0)
                {
                    while(timeSinceLastShot<firePeriod)
                    {
                        yield return null;
                    }

                    CreateABullet();
                }

                //If bullets finished then reload weapon if possible
                Recharge();
                while(currentNumOfBullets<=0)
                {
                    yield return null;
                }
            }
        }
        else
        {
            //Check that weapon colldown passed before firing another bullet
            while(timeSinceLastShot<firePeriod)
            {
                yield return null;
            }

            //Fire bullet if can
            if(currentNumOfBullets>0)
                CreateABullet();
            else
            {
                //Reload if can
                Recharge();

                while(currentNumOfBullets<=0)
                {
                    yield return null;
                }

                CreateABullet();
            }
        }
        
        //otherwise stop fire
        StopFire();
    }



    //Reload coroutine
    private IEnumerator RealoadWait()
    {
        //If there some spare magazines then start reloading weapon
        if(currentNumOfCatridges>0)
        {
            float timePassed=0f;

            while(timePassed<realoadTime)
            {
                timePassed+=Time.deltaTime;
                yield return null;
            }
            
            firearmWeaponAnimation.FinishedRecharge();
            currentNumOfCatridges--;
            currentNumOfBullets = catridgeCapacity;
        }
        else
            CancelRecharge();

        rechargeWait=null;
    }
}