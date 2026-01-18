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
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip[] stepClips;
    [SerializeField] private float gapBetweenStepClips=0.04f;


    private Vector2 movementDirection;
    private Rigidbody2D playerRigid;
    //characterMoves tells whether player is moving now or not
    //canMove sets weather player can move or not
    private bool characterMoves=false, canMove=true;
    private Coroutine stepsSound;


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
            {
                characterMoves=false;
                StopStepsSound();
            }
            else
            {
                characterMoves=true;
                StartStepsSound();
            }
        }
    }



    private void StartStepsSound()
    {
        if(stepsSound==null)
            stepsSound = StartCoroutine(PlayStepSounds());
    }



    private void StopStepsSound()
    {
        if(stepsSound!=null)
        {
            StopCoroutine(stepsSound);
            stepsSound=null;

            audioSource.Stop();
        }
    }



    private IEnumerator PlayStepSounds()
    {
        while(true)
        {
            int selectedClip = Random.Range(0,stepClips.Length);
            audioSource.Stop();
            audioSource.clip = stepClips[selectedClip];
            audioSource.Play();

            yield return new WaitForSeconds(stepClips[selectedClip].length+gapBetweenStepClips);
        }
    }
}
