using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeatleBehaviour : MonoBehaviour
{
    [SerializeField] private Vector2[] routeCoords;

    private Transform playerTransform;
    private PlayerComponent playerComponent;
    [SerializeField] private CommonEnemyBehaviour commonEnemyBehaviour;
    [SerializeField] private float poisonDuration;
    [SerializeField] private float poisonDMGPeriod;
    [SerializeField] private float poisonDMG;
    [SerializeField] private bool positionsInCircle=false;
    private int goToTarget=0;
    private bool goForward=true;



    private void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("player").transform;
        playerComponent = GameObject.FindGameObjectWithTag("player").GetComponent<PlayerComponent>();
        commonEnemyBehaviour.setTargetDestination(routeCoords[goToTarget]);
    }



    private void Update()
    {
        if(commonEnemyBehaviour.getAtTargetDestination())
        {
            if(positionsInCircle)
            {
                goToTarget++;

                if(goToTarget>=routeCoords.Length)
                    goToTarget=0;
            }
            else
            {
                if(goForward)
                {
                    goToTarget++;
                    if(goToTarget>=routeCoords.Length)
                    {
                        goToTarget-=2;
                        goForward=false;
                    }
                }
                else
                {
                    goToTarget--;
                    if(goToTarget<=-1)
                    {
                        goToTarget+=2;
                        goForward=true;
                    }
                }

                if(goToTarget>=routeCoords.Length)
                    goToTarget=0;
            }

            commonEnemyBehaviour.setTargetDestination(routeCoords[goToTarget]);
        }

        if(commonEnemyBehaviour.IsAttacking())
        {
            playerComponent.StartPoisonEffect(poisonDuration,poisonDMGPeriod,poisonDMG,PoisonTypes.Beetle);
            commonEnemyBehaviour.Die();
        }
    } 
}
