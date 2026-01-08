using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerComponent : MonoBehaviour
{
    [SerializeField] private float maxHP=20;
    [SerializeField] private int maxShieldLevel=4;
    [SerializeField] private int maxPoisonLevel=8;
    [SerializeField] private float shieldDurability=20;
    [SerializeField] private UIManager uiManager;
    [SerializeField] private Vector2 spawnpoint;
    [SerializeField] private ItemManager itemManager;

    private float currentHP=0;
    private int currentShieldLevel=0;
    private float currentShieldDurability=0;
    private bool dead=false;
    //private Transform transform;
    //private bool poisoned=false;
    //private int currentPoisonLevel = 0;

    private Coroutine[] activePoisons;
    private PoisonTypes[] activePoisonsTypes;


    public int getCurrentShieldLevel()
    {
        return currentShieldLevel;
    }


    public int getMaxShieldLevel()
    {
        return maxShieldLevel;
    }


    public bool isDead()
    {
        return dead;
    }


    public void increaseCurrentShieldLevel()
    {
        if(currentShieldLevel==0)
        {
            currentShieldDurability=shieldDurability;
            currentShieldLevel++;
        }
        else if(currentShieldLevel==maxShieldLevel)
            currentShieldDurability=shieldDurability;
        else
            currentShieldLevel++;

         uiManager.ChangeShieldBar(currentShieldLevel);
    }


    private void Start()
    {
        //transform = GetComponent<Transform>();

        activePoisons = new Coroutine[maxPoisonLevel];
        activePoisonsTypes = new PoisonTypes[maxPoisonLevel];

        for(int i=0; i<activePoisonsTypes.Length; i++)
        {
            activePoisonsTypes[i] = PoisonTypes.None;
        }

        currentHP = maxHP;

        uiManager.ChangeHPBar(currentHP, maxHP);
        uiManager.ChangeShieldBar(currentShieldLevel);
        uiManager.RemoveAllPoisonIcons();
        //itemManager.Recalculation();

        transform.position = new Vector3(spawnpoint.x,spawnpoint.y,0);

        uiManager.RemoveDeadScreen();
    }



    public void Respawn()
    {
        currentHP = maxHP;
        dead=false;
        //currentPoisonLevel=0;

        if(currentShieldLevel>0)
            currentShieldLevel--;

        for(int i=0; i<activePoisons.Length; i++)
        {
            if(activePoisons[i]!=null)
            {
                StopCoroutine(activePoisons[i]);
                activePoisons[i]=null;
                activePoisonsTypes[i] = PoisonTypes.None;
            }
        }
        
        uiManager.ChangeHPBar(currentHP, maxHP);
        uiManager.ChangeShieldBar(currentShieldLevel);
        uiManager.RemoveAllPoisonIcons();
        itemManager.Recalculation();

        transform.position = new Vector3(spawnpoint.x,spawnpoint.y,0);

        uiManager.RemoveDeadScreen();
    }



    public void Heal(int hpToHeal)
    {
        currentHP+=hpToHeal;
        if(currentHP>maxHP)
            currentHP=maxHP;

        uiManager.ChangeHPBar(currentHP, maxHP);
    }



    public void StartPoisonEffect(float effectDuration, float damagePeriod, int dmgInPeriod, PoisonTypes poisonType)
    {
        for(int i=0; i<activePoisonsTypes.Length; i++)
        {
            if(activePoisonsTypes[i]==poisonType || activePoisonsTypes[i]==PoisonTypes.None)
            {
                activePoisonsTypes[i]=poisonType;
                StopCoroutine(activePoisons[i]);
                activePoisons[i] = StartCoroutine(poisonEffect(effectDuration, damagePeriod, dmgInPeriod,i,poisonType));
                uiManager.SetPoisonIcon(poisonType);

                break;
            }
        }
    }



    private IEnumerator poisonEffect(float effectDuration, float damagePeriod, int dmgInPeriod, int indexInArray, PoisonTypes poisonType)
    {
        float timePassed=0f;

        while(timePassed<effectDuration)
        {
            TakeDamage(dmgInPeriod);
            timePassed+=damagePeriod;

            yield return new WaitForSeconds(damagePeriod); 
        }

        StopPoison(indexInArray, poisonType);
    }



    private void StopPoison(int index, PoisonTypes poisonType)
    {
        StopCoroutine(activePoisons[index]);
        activePoisons[index]=null;
        activePoisonsTypes[index] = PoisonTypes.None;
        uiManager.RemovePoisonIcon(poisonType);
    }



    public void TakeDamage(float rawDMG)
    {
        switch(currentShieldLevel)
        {
            case 0:
                currentHP-=rawDMG;
                break;
            case 1:
                currentHP-=rawDMG*0.9f;
                TakeShieldDamage(rawDMG);
                break;
            case 2:
                currentHP-=rawDMG*0.7f;
                TakeShieldDamage(rawDMG);
                break;
            case 3:
                currentHP-=rawDMG*0.4f;
                TakeShieldDamage(rawDMG);
                break;
            case 4:
                TakeShieldDamage(rawDMG);
                break;
        }

        uiManager.ChangeHPBar(currentHP, maxHP);
        if(currentHP<=0)
           Die();
    }



    public void TakeShieldDamage(float rawDMG)
    {
        float removeDurability = rawDMG/2f;

        currentShieldDurability-=removeDurability;
        while(currentShieldDurability<0 && currentShieldLevel>0)
        {
            removeDurability=-1f*currentShieldDurability;
            currentShieldDurability = shieldDurability;
            currentShieldLevel--;
            currentShieldDurability-=removeDurability;
        }

        uiManager.ChangeShieldBar(currentShieldLevel);
    }



    public void DecreaseShieldLevel(int n)
    {
        currentShieldLevel-=n;
        if(currentShieldLevel<=0)
        {
            currentShieldLevel=0;
            currentShieldDurability=0;
        }

        uiManager.ChangeShieldBar(currentShieldLevel);
    }



    public void Die()
    {
        dead=true;
        uiManager.SetDeadScreen();
        itemManager.CancelRechargeWeapon();
        itemManager.CancelUsingFirstAid();
    }
}
