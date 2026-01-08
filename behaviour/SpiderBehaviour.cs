using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderBehaviour : MonoBehaviour
{

    private Coroutine go_circle=null;
    [SerializeField] private Sprite alive;
    [SerializeField] private LayerMask barrierLayer;
    private Vector2 nestCoord;

    private SpriteRenderer spriteRenderer;
    private Transform playerTransform;
    [SerializeField] private Transform rotationBox;
    [SerializeField] private CommonEnemyBehaviour commonEnemyBehaviour;



    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
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

                if (distanceToNest > 0.5f && go_circle==null)
                {
                    go_circle=StartCoroutine(Movement_to_point());
                }
            }
        }
    }




    private IEnumerator Movement_to_point() 
    {
        Vector3 zero=new Vector3(nestCoord.x,nestCoord.y,0);
        float distanceToCenter = Vector2.Distance(transform.position, zero);

        while(distanceToCenter>=0.1f)
        {
            Vector2 direction = (zero - transform.position).normalized;

            float detectionRadius = 2f;
            RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, detectionRadius, barrierLayer);

            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            if (angle<=90 && angle>=-90)
            {
                spriteRenderer.flipX = false;
            }
            else
            {
                spriteRenderer.flipX = true;
            }

            if (hit.collider == null)
            {
                transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
                rotationBox.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
                transform.Translate(new Vector3(1,0,0)*commonEnemyBehaviour.getSpeed()*0.02f);
            }
            else
            {
                angle+=50;
                transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
                rotationBox.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
                transform.Translate(new Vector3(1,0,0)*commonEnemyBehaviour.getSpeed()*0.02f);
            }

            yield return new WaitForSeconds(0.02f);
            distanceToCenter = Vector2.Distance(transform.position, zero);
        }
    }
}
