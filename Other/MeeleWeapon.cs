using UnityEngine;
using System.Collections;

public class MeeleWeapon : MonoBehaviour
{
    [SerializeField] private float dmgPerFirstSwing;
    [SerializeField] private float dmgPerSecondSwing;
    //How long takes a single swing
    [SerializeField] private float swingPeriod;
    //Flag which tells if player has this weapon or not
    private bool haveThisWeapon=false;
    [SerializeField] private WeaponTypes type;
    [SerializeField] private EnemiesHittedByMeeleWeapon enemiesChecker;
    //Stores swing coroutine
    private Coroutine swinging;



    private void Start()
    {
        EventsManager.OnWeaponSwitched+=StopSwing;
    }



    //Getters and setters
    public WeaponTypes getWeaponType()
    {
        return type;
    }

    public bool getHaveThisWeapon()
    {
        return haveThisWeapon;
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
        if(swinging==null)
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
    }



    //This coroutine periodically swings weapon and during that updates consequent swing row
    //number so enemies would know that next swing started and they should take damage from
    //that
    private IEnumerator SwingProcess()
    {
        yield return new WaitForSeconds(swingPeriod/4f);

        enemiesChecker.DealDamageToCollectedEnemies(dmgPerFirstSwing);
        yield return new WaitForSeconds(swingPeriod/2f);

        enemiesChecker.DealDamageToCollectedEnemies(dmgPerSecondSwing);
        yield return new WaitForSeconds(swingPeriod/4f);

        swinging=null;
    }
}