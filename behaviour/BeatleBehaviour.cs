using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeatleBehaviour : MonoBehaviour
{
    //Coordinates of the root which beetle will follow
    [SerializeField] private Vector2[] routeCoords;

    private PlayerComponent playerComponent;
    [SerializeField] private CommonEnemyBehaviour commonEnemyBehaviour;
    [SerializeField] private float poisonDuration;
    [SerializeField] private float poisonDMGPeriod;
    [SerializeField] private float poisonDMG;
    //Flag which tells whther beetle should follow positions in the array
    //from start to the end and again from start to end of the array (true)
    //or from start to end then from end to start and again (false)
    [SerializeField] private bool positionsInCircle=false;
    [SerializeField] private EnemyAnimation enemyAnimation;

    //Current choosed route position
    private int goToTarget=0;
    //Flag which whether the beetle should go forward or backwards in the array of route positions
    private bool goForward=true;
    private bool poisonedPlayer=false;



    private void Start()
    {
        playerComponent = GameObject.FindGameObjectWithTag("player").GetComponent<PlayerComponent>();
        commonEnemyBehaviour.setTargetDestination(routeCoords[goToTarget]);
    }



    private void Update()
    {
        //If it reached current route position, then select new one
        if(commonEnemyBehaviour.getAtTargetDestination())
        {
            //Check hoe next route position should be choosed
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

            //Set new target destination
            commonEnemyBehaviour.setTargetDestination(routeCoords[goToTarget]);
        }

        //If beetle started attack then poison player and die
        if(commonEnemyBehaviour.IsAttacking() && !poisonedPlayer)
        {
            playerComponent.StartPoisonEffect(poisonDuration,poisonDMGPeriod,poisonDMG,PoisonTypes.Beetle);
            commonEnemyBehaviour.setDoNotCancelAttack(true);
            poisonedPlayer=true;
            StartCoroutine(waitToDie(enemyAnimation.getFlipTime()*enemyAnimation.getNumOfAttackSprites()));
        }
    } 



    private IEnumerator waitToDie(float time)
    {
        yield return new WaitForSeconds(time);

        commonEnemyBehaviour.Die();
    }
}
