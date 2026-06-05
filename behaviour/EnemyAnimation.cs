using System.Runtime.CompilerServices;
using UnityEngine;

public class EnemyAnimation : MonoBehaviour
{
    [SerializeField] Animator enemyAnimator;

    //Stores current animation state
    private AnimationState currentAnimationState;
    //Direction in which enemy is moving
    private Directions currentDirection;
    private Transform playerTransform;
    private Vector2 movementDirection;
    private float currentAngle;
    private AnimationState previousAnimationState;
    private Directions previousDirection;


    public void setMovementDirection(Vector2 _movementDirection)
    {
        movementDirection = _movementDirection;
    }

    public void setCurrentAnimationState(AnimationState state)
    {
        currentAnimationState = state;
    }



    void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("player").GetComponent<Transform>();

        switch(Random.Range(0,4))
        {
            case 0:
                currentDirection=Directions.Down;
                break;
            case 1:
                currentDirection=Directions.Up;
                break;
            case 2:
                currentDirection=Directions.Left;
                break;
            case 3:
                currentDirection=Directions.Right;
                break;
        }

        currentAnimationState = AnimationState.Idle;
    }

    

    void Update()
    {
        //Calculate angle
        GetAngleToPlayer();
        CalculateDirection();

        if(previousAnimationState!=currentAnimationState || previousDirection!=currentDirection)
            ChangeAnimation();

        previousAnimationState = currentAnimationState;
        previousDirection = currentDirection;
    }



    private void ChangeAnimation()
    {
        switch(currentAnimationState,currentDirection)
        {
            case (AnimationState.Idle,Directions.Up):
                enemyAnimator.Play("IdleUp");
                break;
            case (AnimationState.Idle,Directions.Down):
                enemyAnimator.Play("IdleDown");
                break;
            case (AnimationState.Idle,Directions.Right):
                enemyAnimator.Play("IdleRight");
                break;
            case (AnimationState.Idle,Directions.Left):
                enemyAnimator.Play("IdleLeft");
                break;
            case (AnimationState.Moving,Directions.Up):
                enemyAnimator.Play("WalkUp");
                break;
            case (AnimationState.Moving,Directions.Down):
                enemyAnimator.Play("WalkDown");
                break;
            case (AnimationState.Moving,Directions.Left):
                enemyAnimator.Play("WalkLeft");
                break;
            case (AnimationState.Moving,Directions.Right):
                enemyAnimator.Play("WalkRight");
                break;
            case (AnimationState.AttackIdle,Directions.Up):
                enemyAnimator.Play("AttackUp");
                break;
            case (AnimationState.AttackIdle,Directions.Down):
                enemyAnimator.Play("AttackDown");
                break;
            case (AnimationState.AttackIdle,Directions.Left):
                enemyAnimator.Play("AttackLeft");
                break;
            case (AnimationState.AttackIdle,Directions.Right):
                enemyAnimator.Play("AttackRight");
                break;
            case (AnimationState.LongIdle,Directions.Up):
                enemyAnimator.Play("LongIdleUp");
                break;
            case (AnimationState.LongIdle,Directions.Down):
                enemyAnimator.Play("LongIdleDown");
                break;
            case (AnimationState.LongIdle,Directions.Left):
                enemyAnimator.Play("LongIdleLeft");
                break;
            case (AnimationState.LongIdle,Directions.Right):
                enemyAnimator.Play("LongIdleRight");
                break;
            case (AnimationState.Dead,Directions.Up):
            case (AnimationState.Dead,Directions.Down):
            case (AnimationState.Dead,Directions.Left):
            case (AnimationState.Dead,Directions.Right):
                enemyAnimator.Play("Dead");
                break;
        }
    }



    private void CalculateDirection()
    {
        if(currentAnimationState==AnimationState.Moving)
        {
            if(movementDirection.x>0.7)
                currentDirection=Directions.Right;
            else if(movementDirection.x<-0.7)
                currentDirection=Directions.Left;
            else if(movementDirection.y>0.7)
                currentDirection=Directions.Up;
            else if(movementDirection.y<-0.7)
                currentDirection=Directions.Down;
        }
        else if(currentAnimationState==AnimationState.AttackIdle)
            currentDirection = GetPlayerDirection();
    }



    //This method calculates angle between character, mouse and x-axis and stores
    //in in the currentAngle
    private void GetAngleToPlayer()
    {
        Vector2 a = transform.position;
        Vector2 b = playerTransform.position;

        float angleRad = Mathf.Atan2(b.y - a.y, b.x - a.x);
        currentAngle = angleRad * Mathf.Rad2Deg;
    }



    //This mouse returns direction in which mouse is right now
    private Directions GetPlayerDirection()
    {
        if(currentAngle>-45 && currentAngle<45)
            return Directions.Right;
        else if(currentAngle>-135 && currentAngle<-45)
            return Directions.Down;
        else if(currentAngle>45 && currentAngle<135)
            return Directions.Up;
        else
            return Directions.Left;
    }
}
