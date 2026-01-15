using UnityEngine;

//This component is attached to the player and it checks if enemy entered player
//hitbox than tells enemy to attack player. If enemy exited player then tells it to
// pursuit player
public class CheckEnemyAttacks : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag=="Enemy" && collision.gameObject.layer==8)
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
        if(collision.gameObject.tag=="Enemy" && collision.gameObject.layer==8)
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
