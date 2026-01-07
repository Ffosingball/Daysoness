using UnityEngine;
using System.Collections;

public class MeeleWeapon : MonoBehaviour
{
    [SerializeField] private float dmgPerSwing;
    [SerializeField] private float swingPeriod;
    private bool haveThisWeapon=false;
    [SerializeField] private WeaponTypes type;

    public WeaponTypes getWeaponType()
    {
        return type;
    }

    public bool getHaveThisWeapon()
    {
        return haveThisWeapon;
    }

    public void setHaveThisWeapon(bool val)
    {
        haveThisWeapon=val;
    }



    public void StartSwing()
    {
        
    }


    public void StopSwing()
    {
        

    }


    private IEnumerator SwingProcess()
    {
        //canAttack=true;
        //whichSword=1;

        yield return new WaitForSeconds(0.1f);

        //float currentRotation = sword_hand.transform.eulerAngles.z;
        //currentRotation=currentRotation-45;
        
        //if(Movement.flipSword)
        //{
            //currentRotation=currentRotation-180;
        //}

        //Movement.flipSword=false;
        //sword_hand.rotation=Quaternion.Euler(0,0,currentRotation);

        for(int i=0;i<15;i++)
        {
            //currentRotation=currentRotation+6;
            //sword_hand.rotation=Quaternion.Euler(0,0,currentRotation);
            yield return new WaitForSeconds(0.013f);
        }
        statistica.swungSword++;

        //currentRotation=currentRotation-45;
        //sword_hand.rotation=Quaternion.Euler(0,0,currentRotation);

        yield return new WaitForSeconds(0.2f);

        //canAttack=false;
        //whichSword=0;
    }
}