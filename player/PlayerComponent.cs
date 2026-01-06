using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerComponent : MonoBehaviour
{
    [SerializeField] private int maxHP=20;
    [SerializeField] private int maxShieldLevel=4;
    [SerializeField] private int shieldDurability=20;

    private int currentHP=0;
    private int currentShieldLevel=0;
    private int currentShieldDurability=0;
    private bool dead=false;


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
        
    }



    public void Respawn()
    {
        
    }



    public void Heal(int hpToHeal)
    {
        
    }



    public void StartPoisonEffect()
    {
        
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
