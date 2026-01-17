using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    //Movement speed when player do not attacks
    [SerializeField] private float usualSpeed=5f;
    //Movement speed when player attacks
    [SerializeField] private float attackingSpeed=2.5f;
    //Reference to another component
    [SerializeField] private PlayerAnimations playerAnimation;


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

    public Vector2 getMovementDirection()
    {
        return movementDirection;
    }

    public bool getIfCharacterMoves()
    {
        return characterMoves;
    }

    public float getUsualSpeed()
    {
        return usualSpeed;
    }

    public float getAttackingSpeed()
    {
        return attackingSpeed;
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
            if(playerAnimation.isAttackingAnimation())
                playerRigid.linearVelocity = movementDirection * attackingSpeed;
            else
                playerRigid.linearVelocity = movementDirection * usualSpeed;

            if(movementDirection.x==0 && movementDirection.y==0)
                characterMoves=false;
            else
                characterMoves=true;
        }
    }
}
