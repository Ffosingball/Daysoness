using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerComponent : MonoBehaviour
{
    [SerializeField] private int maxHP=20;
    [SerializeField] private int maxShieldLevel=4;
    [SerializeField] private int maxPoisonLevel=8;
    [SerializeField] private int shieldDurability=20;
    [SerializeField] private UIManager uiManager;
    [SerializeField] private Vector2 spawnpoint;
    [SerializeField] private ItemManager itemManager;

    private int currentHP=0;
    private int currentShieldLevel=0;
    private int currentShieldDurability=0;
    private bool dead=false;
    private Transform transform;
    private bool poisoned=false;
    private int currentPoisonLevel = 0;

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


    public void increaseCurrentShieldLevel()
    {
        currentShieldLevel++;
    }


    private void Start()
    {
        activePoisons = new Coroutine[maxPoisonLevel];
        activePoisonsTypes = new PoisonTypes[maxPoisonLevel];

        for(int i=0; i<activePoisonsTypes.Length; i++)
        {
            activePoisonsTypes[i] = PoisonTypes.None;
        }

        currentHP = maxHP;

        uiManager.ChangeHPBar(currentHP);
        uiManager.ChangeShieldBar(currentShieldLevel);
        uiManager.SetPoisonIcon(currentPoisonLevel);
        itemManager.Recalculation();

        transform.position = new Vector3(spawnpoint.x,spawnpoint.y,0);

        uiManager.Respawned();
    }



    public void Respawn()
    {
        currentHP = maxHP;
        dead=false;
        currentPoisonLevel=0;

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
        
        uiManager.ChangeHPBar(currentHP);
        uiManager.ChangeShieldBar(currentShieldLevel);
        uiManager.SetPoisonIcon(currentPoisonLevel);
        itemManager.Recalculation();

        transform.position = new Vector3(spawnpoint.x,spawnpoint.y,0);

        uiManager.Respawned();
    }



    public void Heal(int hpToHeal)
    {
        currentHP+=hpToHeal;
        if(currentHP>maxHP)
            currentHP=maxHP;

        uiManager.ChangeHPBar(currentHP);
    }



    public void StartPoisonEffect(float effectDuration, float damagePeriod, int dmgInPeriod, PoisonTypes poisonType)
    {
        for(int i=0; i<activePoisonsTypes.Length; i++)
        {
            if(activePoisonsTypes[i]==poisonType || activePoisonsTypes[i]==PoisonTypes.None)
            {
                activePoisonsTypes[i]=poisonType;
                StopCoroutine(activePoisons[i]);
                activePoisons[i] = StartCoroutine(poisonEffect(effectDuration, damagePeriod, dmgInPeriod,i));

                break;
            }
        }
    }



    private IEnumerator poisonEffect(float effectDuration, float damagePeriod, int dmgInPeriod, int indexInArray)
    {
        yield return 1f;
        //COMPLETE CODE HERE
        StopPoison(indexInArray);
    }



    private void StopPoison(int index)
    {
        StopCoroutine(activePoisons[index]);
        activePoisons[index]=null;
        activePoisonsTypes[index] = PoisonTypes.None;
    }



    public void TakeDamage(int rawDMG)
    {
        
    }



    public void TakeShieldDamage(int rawDMG)
    {
        
    }



    public void DecreaseShieldLevel(int rawDMG)
    {
        
    }



    public void Die()
    {
        
    }
}
