using UnityEngine;
using System.Collections;

public class MeeleWeapon : MonoBehaviour
{
    [SerializeField] private float dmgPerSwing;
    //How long takes a single swing
    [SerializeField] private float swingPeriod;
    //Flag which tells if player has this weapon or not
    private bool haveThisWeapon=false;
    [SerializeField] private WeaponTypes type;
    [SerializeField] private GameObject meeleWeapon;
    //Stores swing coroutine
    private Coroutine swinging;
    //Rotation of the weapon before swining
    private float initZRotation;
    //This number store current consequent swing
    private int attackRowNum=0;


    //Getters and setters
    public WeaponTypes getWeaponType()
    {
        return type;
    }

    public bool getHaveThisWeapon()
    {
        return haveThisWeapon;
    }

    public float getDMG()
    {
        return dmgPerSwing;
    }

    public int getAttackRowNum()
    {
        return attackRowNum;
    }

    public bool IsSwinging()
    {
        return swinging!=null;
    }

    public float getSwingPeriod()
    {
        return swingPeriod;
    }

    public void setHaveThisWeapon(bool val)
    {
        haveThisWeapon=val;
    }



    //Starts swing coroutine
    public void StartSwing()
    {
        swinging=StartCoroutine(SwingProcess());
    }



    //Stops swing coroutine
    public void StopSwing()
    {
        if(swinging!=null)
        {
            StopCoroutine(swinging);
            swinging=null;
        }

        //Reset weapon rotation
        meeleWeapon.transform.rotation=Quaternion.Euler(0,0,initZRotation);
    }



    //This coroutine periodically swings weapon and during that updates consequent swing row
    //number so enemies would know that next swing started and they should take damage from
    //that
    private IEnumerator SwingProcess()
    {
        while(true)
        {
            yield return new WaitForSeconds(swingPeriod);
            attackRowNum++;
        }
    }
}