using System;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using System.Collections.Generic;

public class PlayerAnimations : MonoBehaviour
{
    //Time after which character will go into long wait animation state
    //(Currently after that time main character will sit down and wait)
    [SerializeField] private float timeToWaitUntilLongIdleAnimation=10f;
    //Offset for the shadow
    //Time after which player will exit the attack state
    [SerializeField] private float timeToCancelFireSprites=1f;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip[] longWaitSoundClips;
    [SerializeField] private Vector2 timeBetweenSounds;
    [SerializeField] private Animator animator;
    [SerializeField] private float animationSlowingDuringAttack=0.2f;

    //References to other components
    private Movement movement;
    //Direction in which player now moves
    private Directions currentDirection;
    private Directions previousDirection;
    //Current angle between main character, mouse and x-axis
    private float currentAngle;
    private Coroutine longWaitSoundsCoroutine, fireStopCoroutine, longIdleWaitCoroutine;
    private AnimationState currentAnimationState;
    private AnimationState previousAnimationState;


    //Getters and setters
    public Directions getDirection()
    {
        return currentDirection;
    }

    public float getCurrentAngle()
    {
        return currentAngle;
    }

    public float getTimeToCancelAttackingAnimation()
    {
        return timeToCancelFireSprites;
    }

    public AnimationState getAnimationState()
    {
        return currentAnimationState;
    }



    void Start()
    {
        movement = transform.parent.gameObject.GetComponent<Movement>();
        currentDirection=Directions.Down;
        currentAnimationState = AnimationState.LongIdle;
        longWaitSoundsCoroutine=StartCoroutine(longWaitSounds());

        EventsManager.OnStopFire+=StopAttackAnimation;
        EventsManager.OnStartFire+=StartAttackAnimation;
    }

    

    void Update()
    {
        CalculateDirection();
        SelectAnimation();

        previousDirection = currentDirection;
        previousAnimationState = currentAnimationState;
    }



    private void CalculateDirection()
    {
        //Calculate angle
        GetAngleToMouse();

        //Get direction
        Vector2 direction = movement.getMovementDirection();

        if(previousAnimationState==AnimationState.AttackIdle || previousAnimationState==AnimationState.AttackMoving)
        {
            //Get mouse direction
            Directions mouseDirection = GetMouseDirection();

            //Get direction for the player movement
            switch(mouseDirection)
            {
                case Directions.Up:
                    if(direction.y<-0.7)
                        currentDirection=Directions.BackwardsDown;
                    else
                        currentDirection=Directions.Up;
                    break;
                case Directions.Down:
                    if(direction.y>0.7)
                        currentDirection=Directions.BackwardsUp;
                    else
                        currentDirection=Directions.Down;
                    break;
                case Directions.Left:
                    if(direction.x>0.7)
                        currentDirection=Directions.BackwardsRight;
                    else
                        currentDirection=Directions.Left;
                    break;
                case Directions.Right:
                    if(direction.x<-0.7)
                        currentDirection=Directions.BackwardsLeft;
                    else
                        currentDirection=Directions.Right;
                    break;
            }
        }
        else
        {
            if(direction.y>0.7)
                currentDirection=Directions.Up;
            else if(direction.y<-0.7)
                currentDirection=Directions.Down;
            else if(direction.x>0.7)
                currentDirection=Directions.Right;
            else if(direction.x<-0.7)
                currentDirection=Directions.Left;
        }
    }



    private void SelectAnimation()
    {
        if(previousAnimationState!=currentAnimationState || previousDirection!=currentDirection)
        {
            if(currentAnimationState==AnimationState.AttackIdle || currentAnimationState==AnimationState.AttackMoving)
                animator.speed = 1f-animationSlowingDuringAttack;
            else
                animator.speed = 1f;
            
            switch(currentAnimationState,currentDirection)
            {
                case (AnimationState.AttackIdle,Directions.Up):
                case (AnimationState.Idle,Directions.Up):
                    animator.Play("IdleUp");
                    break;
                case (AnimationState.AttackIdle,Directions.Down):
                case (AnimationState.Idle,Directions.Down):
                    animator.Play("IdleDown");
                    break;
                case (AnimationState.AttackIdle,Directions.Right):
                case (AnimationState.Idle,Directions.Right):
                    animator.Play("IdleLeft");
                    break;
                case (AnimationState.AttackIdle,Directions.Left):
                case (AnimationState.Idle,Directions.Left):
                    animator.Play("IdleRight");
                    break;
                case (AnimationState.Moving,Directions.Up):
                case (AnimationState.AttackMoving,Directions.Up):
                    animator.Play("WalkUp");
                    break;
                case (AnimationState.Moving,Directions.Down):
                case (AnimationState.AttackMoving,Directions.Down):
                    animator.Play("WalkDown");
                    break;
                case (AnimationState.Moving,Directions.Right):
                case (AnimationState.AttackMoving,Directions.Right):
                    animator.Play("WalkLeft");
                    break;
                case (AnimationState.Moving,Directions.Left):
                case (AnimationState.AttackMoving,Directions.Left):
                    animator.Play("WalkRight");
                    break;
                case (AnimationState.AttackMoving,Directions.BackwardsUp):
                    animator.Play("WalkDownBackwards");
                    break;
                case (AnimationState.AttackMoving,Directions.BackwardsDown):
                    animator.Play("WalkUpBackwards");
                    break;
                case (AnimationState.AttackMoving,Directions.BackwardsLeft):
                    animator.Play("WalkRightBackwards");
                    break;
                case (AnimationState.AttackMoving,Directions.BackwardsRight):
                    animator.Play("WalkLeftBackwards");
                    break;
                case (AnimationState.LongIdle,Directions.Up):
                    animator.Play("LongIdleDown");
                    break;
                case (AnimationState.LongIdle,Directions.Down):
                    animator.Play("LongIdleUp");
                    break;
                case (AnimationState.LongIdle,Directions.Left):
                    animator.Play("LongIdleLeft");
                    break;
                case (AnimationState.LongIdle,Directions.Right):
                    animator.Play("LongIdleRight");
                    break;
            }
        }
    }



    //Starts attack animation
    private void StartAttackAnimation()
    {
        if(currentAnimationState==AnimationState.Idle || currentAnimationState==AnimationState.LongIdle || currentAnimationState==AnimationState.AttackIdle)
            currentAnimationState = AnimationState.AttackIdle;
        else
            currentAnimationState = AnimationState.AttackMoving;


        if(longWaitSoundsCoroutine!=null)
        {
            StopCoroutine(longWaitSoundsCoroutine);
            longWaitSoundsCoroutine=null;
            audioSource.Stop();
        }

        if(fireStopCoroutine!=null)
        {
            StopCoroutine(fireStopCoroutine);
            fireStopCoroutine=null;
        }

        if(longIdleWaitCoroutine!=null)
        {
            StopCoroutine(longIdleWaitCoroutine);
            longIdleWaitCoroutine=null;
        }
    }



    //Starts moving animation
    public void StartMovingAnimation()
    {
        if(currentAnimationState==AnimationState.Idle || currentAnimationState==AnimationState.LongIdle)
            currentAnimationState = AnimationState.Moving;
        else
            currentAnimationState = AnimationState.AttackMoving;


        if(longWaitSoundsCoroutine!=null)
        {
            StopCoroutine(longWaitSoundsCoroutine);
            longWaitSoundsCoroutine=null;
            audioSource.Stop();
        }

        if(longIdleWaitCoroutine!=null)
        {
            StopCoroutine(longIdleWaitCoroutine);
            longIdleWaitCoroutine=null;
        }
    }



    public void CancelLongWaitAnimation()
    {
        if(currentAnimationState==AnimationState.LongIdle)
            currentAnimationState = AnimationState.Idle;

        if(longWaitSoundsCoroutine!=null)
        {
            StopCoroutine(longWaitSoundsCoroutine);
            longWaitSoundsCoroutine=null;
            audioSource.Stop();
        }
    }



    //Stops attack animation
    private void StopAttackAnimation()
    {
        Action action = () => { 
            if(currentAnimationState==AnimationState.AttackMoving)
                currentAnimationState = AnimationState.Moving;
            else
            {
                currentAnimationState = AnimationState.Idle;
                Action action = () => {currentAnimationState=AnimationState.LongIdle;};
                longIdleWaitCoroutine = StartCoroutine(timedEvent(timeToWaitUntilLongIdleAnimation,action));
            }};
        fireStopCoroutine = StartCoroutine(timedEvent(timeToCancelFireSprites,action));
    }


    //Stops moving animation
    public void StopMovingAnimation()
    {
        if(currentAnimationState==AnimationState.AttackMoving)
            currentAnimationState = AnimationState.AttackIdle;
        else
        {
            currentAnimationState = AnimationState.Idle;
            Action action = () => {currentAnimationState=AnimationState.LongIdle;};
            longIdleWaitCoroutine = StartCoroutine(timedEvent(timeToWaitUntilLongIdleAnimation,action));
        }
    }



    //This method calculates angle between character, mouse and x-axis and stores
    //in in the currentAngle
    private void GetAngleToMouse()
    {
        Vector2 a = transform.position;
        Vector2 b = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());

        float angleRad = Mathf.Atan2(b.y - a.y, b.x - a.x);
        currentAngle = angleRad * Mathf.Rad2Deg;
    }



    //This mouse returns direction in which mouse is right now
    private Directions GetMouseDirection()
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



    private IEnumerator longWaitSounds()
    {
        while(true)
        {
            yield return new WaitForSeconds(UnityEngine.Random.Range(timeBetweenSounds.x,timeBetweenSounds.y));

            audioSource.Stop();
            audioSource.clip = longWaitSoundClips[UnityEngine.Random.Range(0,longWaitSoundClips.Length)];
            audioSource.Play();

            yield return new WaitForSeconds(audioSource.clip.length);
        }
    }



    private IEnumerator timedEvent(float waitForSec, Action action)
    {
        yield return new WaitForSeconds(waitForSec);
        action();
    }
}
