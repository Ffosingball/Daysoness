using UnityEngine;
using System.Collections;

public class MeeleWeapon : MonoBehaviour
{
    [SerializeField] private float dmgPerSwing;
    [SerializeField] private float swingPeriod;
    private bool haveThisWeapon=false;
    [SerializeField] private WeaponTypes type;
    [SerializeField] private GameObject meeleWeapon;
    private Coroutine swinging;
    private float initZRotation;
    private int attackRowNum=0;

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



    public void StartSwing()
    {
        //attackRowNum=0;
        swinging=StartCoroutine(SwingProcess());
    }


    public void StopSwing()
    {
        if(swinging!=null)
        {
            StopCoroutine(swinging);
            swinging=null;
        }

        meeleWeapon.transform.rotation=Quaternion.Euler(0,0,initZRotation);
    }


    private IEnumerator SwingProcess()
    {
        initZRotation = meeleWeapon.transform.eulerAngles.z;
        float currentRotation=initZRotation-45;
        meeleWeapon.transform.rotation=Quaternion.Euler(0,0,currentRotation);

        while(true)
        {
            for(float i=0f;i<swingPeriod/2f;i+=swingPeriod/40f)
            {
                currentRotation=currentRotation+(90/20);
                meeleWeapon.transform.rotation=Quaternion.Euler(0,0,currentRotation);
                yield return new WaitForSeconds(swingPeriod/40f);
            }

            for(float i=0f;i<swingPeriod/2f;i+=swingPeriod/40f)
            {
                currentRotation=currentRotation-(90/20);
                meeleWeapon.transform.rotation=Quaternion.Euler(0,0,currentRotation);
                yield return new WaitForSeconds(swingPeriod/40f);
            }

            Debug.Log("Swinging");
            attackRowNum++;
        }
    }
}