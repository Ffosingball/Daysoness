using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderBehaviour : MonoBehaviour
{

    private Coroutine go_circle=null;
    [SerializeField] private Sprite alive;
    [SerializeField] private LayerMask barrierLayer;
    [SerializeField] private Vector2 nestCoord;

    //private SpriteRenderer spriteRenderer;
    private Transform playerTransform;
    [SerializeField] private Transform rotationBox;
    [SerializeField] private CommonEnemyBehaviour commonEnemyBehaviour;



    private void Start()
    {
        //spriteRenderer = GetComponent<SpriteRenderer>();
        playerTransform = GameObject.FindGameObjectWithTag("player").transform;
        nestCoord = transform.position;
    }



    void Update()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);

        if (!commonEnemyBehaviour.IsDead() && !(commonEnemyBehaviour.IsAttacking() || commonEnemyBehaviour.IsPursuiting()))
        {
            if (distanceToPlayer <= commonEnemyBehaviour.getDetectionRange())
            {
                commonEnemyBehaviour.StartPursuit();
            }
            else
            {
                float distanceToNest = Vector2.Distance(transform.position, nestCoord);
                //Debug.Log("Distance to nest: "+distanceToNest);

                if (distanceToNest > 0.5f && go_circle==null)
                {
                    //Debug.Log("What the fuck!!!");
                    //Debug.Log("Result: "+(distanceToNest > 0.5f));
                    go_circle=StartCoroutine(Movement_to_point());
                }
            }
        }
        else if(commonEnemyBehaviour.IsDead())
        {
            if(go_circle!=null)
            {
                StopCoroutine(go_circle);
                go_circle=null;
            }
        }
    }




    private IEnumerator Movement_to_point() 
    {
        //Debug.Log("Moving somewhere");
        Vector3 zero=new Vector3(nestCoord.x,nestCoord.y,0);
        float distanceToCenter = Vector2.Distance(transform.position, zero);
        var wait = new WaitForFixedUpdate();
        commonEnemyBehaviour.setIsMoving(true);

        while(distanceToCenter>=0.5f)
        {
            Vector2 direction = (zero - transform.position).normalized;

            commonEnemyBehaviour.setMovementDirection(direction);

            yield return wait;
            distanceToCenter = Vector2.Distance(transform.position, zero);
        }

        commonEnemyBehaviour.setIsMoving(false);
    }
}
