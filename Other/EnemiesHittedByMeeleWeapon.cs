using System.Collections.Generic;
using UnityEngine;

//This component is attached to the player and it checks if enemy entered player
//hitbox than tells enemy to attack player. If enemy exited player then tells it to
// pursuit player
public class EnemiesHittedByMeeleWeapon : MonoBehaviour
{
    [SerializeField] private MeeleWeapon meeleWeapon;

    private List<CommonEnemyBehaviour> hittedEnemies;



    private void Start()
    {
        hittedEnemies = new List<CommonEnemyBehaviour>();
    }



    public void DealDamageToCollectedEnemies()
    {
        foreach(CommonEnemyBehaviour behaviour in hittedEnemies)
        {
            behaviour.TakeDamage(meeleWeapon.getDMG());
        }
    }



    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag=="Enemy" && collision.gameObject.layer==6)
        {
            CommonEnemyBehaviour commonEnemyBehaviour = collision.transform.parent.gameObject.GetComponent<CommonEnemyBehaviour>();
            if(!commonEnemyBehaviour.IsDead())
            {
                bool addToList=true;

                foreach(CommonEnemyBehaviour behaviour in hittedEnemies)
                {
                    if(behaviour==commonEnemyBehaviour)
                        addToList=false;
                }

                if(addToList)
                    hittedEnemies.Add(commonEnemyBehaviour);
            }
        }
    }



    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.tag=="Enemy" && collision.gameObject.layer==6)
        {
            CommonEnemyBehaviour commonEnemyBehaviour = collision.transform.parent.gameObject.GetComponent<CommonEnemyBehaviour>();
            hittedEnemies.Remove(commonEnemyBehaviour);
        }
    }
}