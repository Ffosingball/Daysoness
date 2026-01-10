using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotBehaviour : MonoBehaviour
{
    private PlayerComponent playerComponent;
    private Transform playerTransform;
    [SerializeField] private CommonEnemyBehaviour commonEnemyBehaviour;



    private void Start()
    {
        playerComponent = GameObject.FindGameObjectWithTag("player").transform.GetComponent<PlayerComponent>();
        playerTransform = GameObject.FindGameObjectWithTag("player").transform;
        
        EventsManager.OnRobotsActivate += activateRobot;
        EventsManager.OnDamageTaken += checkWhoIsDamaged;
        EventsManager.OnAllRobotsDeactivate += deactivateRobot;
    }



    private void Update()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);

        if(distanceToPlayer>=commonEnemyBehaviour.getDetectionRange())
            commonEnemyBehaviour.setIsActive(false);
    }


    private void activateRobot(Vector2 callerPosition)
    {
        float distanceToCaller = Vector2.Distance(transform.position, callerPosition);
        if(distanceToCaller<=commonEnemyBehaviour.getDetectionRange())
            commonEnemyBehaviour.setIsActive(true);
    }



    private void checkWhoIsDamaged(GameObject victim)
    {
        if(victim==gameObject)
            commonEnemyBehaviour.setIsActive(true);
    }



    private void deactivateRobot()
    {
        //Debug.Log("Robot deactivated");
        commonEnemyBehaviour.setIsActive(false);
        commonEnemyBehaviour.StopAttack();
        commonEnemyBehaviour.StopPursuit();
    }



    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag=="player")
        {
            EventsManager.CallOnRobotsActivate(collision.gameObject.transform.position);
        }
    }
}
