using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] private float speed=5f;


    private Vector2 movementDirection;
    private Rigidbody2D playerRigid;
    //characterMoves tells whether player is moving now or not
    //canMove sets weather player can move or not
    private bool characterMoves=false, canMove=true;


    //Setters and getters
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
        //Check if player can move then move
        if(canMove)
        {
            playerRigid.linearVelocity = movementDirection * speed;

            if(movementDirection.x==0 && movementDirection.y==0)
                characterMoves=false;
            else
                characterMoves=true;
        }
    }
}
