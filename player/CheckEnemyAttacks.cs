using UnityEngine;

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
