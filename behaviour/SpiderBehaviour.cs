using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderBehaviour : MonoBehaviour
{
    [SerializeField] private Vector2 nestCoord;

    private Transform playerTransform;
    [SerializeField] private CommonEnemyBehaviour commonEnemyBehaviour;
    [SerializeField] private float nestRange;



    private void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("player").transform;
        nestCoord = transform.position;
        commonEnemyBehaviour.setTargetDestination(nestCoord);
    }



    private void Update()
    {
        float distanceToNest = Vector2.Distance(transform.position, nestCoord);
        if(distanceToNest>nestRange)
        {
            float distancePlayerToNest = Vector2.Distance(playerTransform.position, nestCoord);
            if(distancePlayerToNest<nestRange)
                commonEnemyBehaviour.setFollowPlayer(true);
            else
                commonEnemyBehaviour.setFollowPlayer(false);
        }
        else
            commonEnemyBehaviour.setFollowPlayer(true);
    } 
}
