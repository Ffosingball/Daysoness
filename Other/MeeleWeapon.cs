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
        //Prepare weapon for swining
        initZRotation = meeleWeapon.transform.eulerAngles.z;
        float currentRotation=initZRotation-45;
        meeleWeapon.transform.rotation=Quaternion.Euler(0,0,currentRotation);

        while(true)
        {
            //Swing down
            for(float i=0f;i<swingPeriod/2f;i+=swingPeriod/40f)
            {
                currentRotation=currentRotation+(90/20);
                meeleWeapon.transform.rotation=Quaternion.Euler(0,0,currentRotation);
                yield return new WaitForSeconds(swingPeriod/40f);
            }

            //Swing up
            for(float i=0f;i<swingPeriod/2f;i+=swingPeriod/40f)
            {
                currentRotation=currentRotation-(90/20);
                meeleWeapon.transform.rotation=Quaternion.Euler(0,0,currentRotation);
                yield return new WaitForSeconds(swingPeriod/40f);
            }

            attackRowNum++;
        }
    }
}