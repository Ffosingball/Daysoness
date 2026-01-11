using UnityEngine;

//Yhis component is attached to the player and it checks if enemy entered player
//hitbox than let the enemy attack player. If enemy exited player then let it pursuit
//player
public class CheckEnemyAttacks : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag=="Enemy")
        {
            CommonEnemyBehaviour commonEnemyBehaviour = collision.gameObject.GetComponent<CommonEnemyBehaviour>();
            if(!commonEnemyBehaviour.IsDead())
            {
                commonEnemyBehaviour.StopPursuit();
                commonEnemyBehaviour.StartAttack();
            }
        }
    }



    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.tag=="Enemy")
        {
            CommonEnemyBehaviour commonEnemyBehaviour = collision.gameObject.GetComponent<CommonEnemyBehaviour>();
            if(!commonEnemyBehaviour.IsDead())
            {
                commonEnemyBehaviour.StopAttack();
                commonEnemyBehaviour.StartPursuit();
            }
        }
    }
}
