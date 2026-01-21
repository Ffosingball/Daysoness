using UnityEngine;

public class RobotBehaviour : MonoBehaviour
{
    private Transform playerTransform;
    [SerializeField] private CommonEnemyBehaviour commonEnemyBehaviour;



    private void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("player").transform;
        
        //Subscribe functions to these events
        EventsManager.OnRobotsActivate += activateRobot;
        EventsManager.OnDamageTaken += checkWhoIsDamaged;
        EventsManager.OnAllRobotsDeactivate += deactivateRobot;
        EventsManager.OnGameObjectDelete += unsubscribeRobot;
    }



    private void Update()
    {
        //If player is out of range then deactivate
        float distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);

        if(distanceToPlayer>=commonEnemyBehaviour.getDetectionRange())
            commonEnemyBehaviour.setIsActive(false);
    }


    //Activate robot if player in range
    private void activateRobot(Vector2 callerPosition)
    {
        float distanceToCaller = Vector2.Distance(transform.position, callerPosition);
        if(distanceToCaller<=commonEnemyBehaviour.getDetectionRange())
            commonEnemyBehaviour.setIsActive(true);
    }


    //If robot is damaged then activate all robots in the player range
    private void checkWhoIsDamaged(GameObject victim)
    {
        if(victim==gameObject)
            EventsManager.CallOnRobotsActivate(transform.position);
    }



    private void deactivateRobot()
    {
        commonEnemyBehaviour.setIsActive(false);
        commonEnemyBehaviour.StopAttack();
        commonEnemyBehaviour.StopPursuit();
    }



    private void unsubscribeRobot(GameObject _gameObject)
    {
        if(gameObject==_gameObject)
        {
            EventsManager.OnRobotsActivate -= activateRobot;
            EventsManager.OnDamageTaken -= checkWhoIsDamaged;
            EventsManager.OnAllRobotsDeactivate -= deactivateRobot;
            EventsManager.OnGameObjectDelete -= unsubscribeRobot;
        }
    }



    private void OnCollisionEnter2D(Collision2D collision)
    {
        //If player hits a robot then activate everyone in range
        if(collision.gameObject.tag=="player" && !commonEnemyBehaviour.IsDead())
        {
            EventsManager.CallOnRobotsActivate(collision.gameObject.transform.position);
        }
    }
}
