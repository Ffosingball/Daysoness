using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] private float speed=5f;


    private Vector2 movementDirection;
    private Rigidbody2D playerRigid;
    private Coroutine TurnIt;
    private bool characterMoves=false, canMove=true;


    public void setMovementDirection(Vector2 direction)
    {
        movementDirection = direction;
    }


    public bool getIfCharacterMoves()
    {
        return characterMoves;
    }



    private void Start() 
    {
        playerRigid = GetComponent<Rigidbody2D>();
    }



    private void FixedUpdate() 
    {
        //Character movement
        if(canMove)
        {
            //transform.Translate(new Vector3(movementDirection.x,movementDirection.y,0f)*speed*Time.deltaTime);
            playerRigid.linearVelocity = movementDirection * speed;

            if(movementDirection.x==0 && movementDirection.y==0)
                characterMoves=false;
            else
                characterMoves=true;

            /*if(movementDirection.x>0)
                Flip("right");
            else
                Flip("left");*/
        }
    }
}
