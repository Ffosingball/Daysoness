using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerComponent : MonoBehaviour
{
    //Maximum health which player can have
    [SerializeField] private float maxHP=20;
    //Maximum shield level which player can have
    [SerializeField] private int maxShieldLevel=4;
    //This variable sets maximum amount of poisons to which player can be affected
    [SerializeField] private int maxPoisonLevel=8;
    //How many dmg can absorb one level of the shield
    [SerializeField] private float shieldDurability=20;
    [SerializeField] private UIManager uiManager;
    //Position where player will appear after death
    [SerializeField] private Vector2 spawnpoint;
    [SerializeField] private ItemManager itemManager;
    [SerializeField] private Color blinkColor;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private float damageBlinkPeriod=0.15f;
    [SerializeField] private float damageBlinkTransparency=0.6f;

    private float currentHP=0;
    private int currentShieldLevel=0;
    private float currentShieldDurability=0;
    private bool dead=false;
    private Material material;
    private Coroutine damageB;

    private Coroutine[] activePoisons;
    private PoisonTypes[] activePoisonsTypes;


    //Getters and setters
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



    //Increases shield level of the player
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



    //Initialize player
    private void Start()
    {
        activePoisons = new Coroutine[maxPoisonLevel];
        activePoisonsTypes = new PoisonTypes[maxPoisonLevel];

        for(int i=0; i<activePoisonsTypes.Length; i++)
        {
            activePoisonsTypes[i] = PoisonTypes.None;
        }

        currentHP = maxHP;
        material = spriteRenderer.material;
        material.SetFloat("_BlinkAmount", 0f);

        uiManager.ChangeHPBar(currentHP, maxHP);
        uiManager.ChangeShieldBar(currentShieldLevel);
        uiManager.RemoveAllPoisonIcons();

        transform.position = new Vector3(spawnpoint.x,spawnpoint.y,0);

        uiManager.RemoveDeadScreen();
    }



    //Respawn player
    public void Respawn()
    {
        //Recover player
        currentHP = maxHP;
        dead=false;

        //Remove one level of the shield
        if(currentShieldLevel>0)
            currentShieldLevel--;

        //Remove all poisons
        for(int i=0; i<activePoisons.Length; i++)
        {
            if(activePoisons[i]!=null)
            {
                StopCoroutine(activePoisons[i]);
                activePoisons[i]=null;
                activePoisonsTypes[i] = PoisonTypes.None;
            }
        }
        
        //Reset UI
        uiManager.ChangeHPBar(currentHP, maxHP);
        uiManager.ChangeShieldBar(currentShieldLevel);
        uiManager.RemoveAllPoisonIcons();
        itemManager.Recalculation();

        transform.position = new Vector3(spawnpoint.x,spawnpoint.y,0);

        uiManager.RemoveDeadScreen();
    }



    //Recove health of the player
    public void Heal(int hpToHeal)
    {
        currentHP+=hpToHeal;
        if(currentHP>maxHP)
            currentHP=maxHP;

        uiManager.ChangeHPBar(currentHP, maxHP);
    }



    //Start new or reset current poison effect
    public void StartPoisonEffect(float effectDuration, float damagePeriod, float dmgInPeriod, PoisonTypes poisonType)
    {
        for(int i=0; i<activePoisonsTypes.Length; i++)
        {
            //Check if this slot is empty or has poison of the same type
            if(activePoisonsTypes[i]==poisonType || activePoisonsTypes[i]==PoisonTypes.None)
            {
                //Then refresh or create that poison
                activePoisonsTypes[i]=poisonType;
                if(activePoisons[i]!=null)
                    StopCoroutine(activePoisons[i]);
                activePoisons[i] = StartCoroutine(poisonEffect(effectDuration, damagePeriod, dmgInPeriod,i,poisonType));
                uiManager.SetPoisonIcon(poisonType);

                break;
            }
        }
    }



    //Coroutine of the poison effect which periodically deals damage 
    private IEnumerator poisonEffect(float effectDuration, float damagePeriod, float dmgInPeriod, int indexInArray, PoisonTypes poisonType)
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



    //Stop this type of poison
    private void StopPoison(int index, PoisonTypes poisonType)
    {
        StopCoroutine(activePoisons[index]);
        activePoisons[index]=null;
        activePoisonsTypes[index] = PoisonTypes.None;
        uiManager.RemovePoisonIcon(poisonType);
    }



    //Calculates which damage should receive a player
    public void TakeDamage(float rawDMG)
    {
        //Deal damage depending on the shield level
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

        if(damageB!=null)
        {
            StopCoroutine(damageB);
            damageB=null;
        }

        damageB=StartCoroutine(DamageBlink());

        uiManager.ChangeHPBar(currentHP, maxHP);
        if(currentHP<=0)//Die if health is less then 0
           Die();

        //Debug.Log("HP: "+currentHP);
    }



    //Animation of changing color when enemy receives damage
    private IEnumerator DamageBlink()
    {
        material.SetColor("_BlinkColor", blinkColor);
        float currentBlinkAmount=damageBlinkTransparency;
        material.SetFloat("_BlinkAmount", damageBlinkTransparency);

        float timePassed=0f;
        while(timePassed<damageBlinkPeriod)
        {
            currentBlinkAmount = Mathf.Lerp(damageBlinkTransparency,0f,timePassed/damageBlinkPeriod);

            material.SetFloat("_BlinkAmount", currentBlinkAmount);
            timePassed+=Time.deltaTime;
            yield return null;
        }

        damageB=null;
    }



    //Calculate how many damage a shield should receive
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



    //Decreses shield level
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



    //Player dies when this is called
    public void Die()
    {
        dead=true;
        uiManager.SetDeadScreen();
        itemManager.CancelRechargeWeapon();
        itemManager.CancelUsingFirstAid();
        EventsManager.CallOnAllRobotsDeactivate();

        if(damageB!=null)
        {
            StopCoroutine(damageB);
            damageB=null;
        }

        material.SetFloat("_BlinkAmount", 0f);
    }
}
