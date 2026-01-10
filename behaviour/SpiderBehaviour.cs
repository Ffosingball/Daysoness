using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderBehaviour : MonoBehaviour
{
    [SerializeField] private Vector2 nestCoord;

    private Transform playerTransform;
    [SerializeField] private CommonEnemyBehaviour commonEnemyBehaviour;



    private void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("player").transform;
        nestCoord = transform.position;
        commonEnemyBehaviour.setTargetDestination(nestCoord);
    }
}
