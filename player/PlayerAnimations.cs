using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAnimations : MonoBehaviour
{
    //Animation sprites
    [SerializeField] private Sprite[] moveLeftSprites;
    [SerializeField] private Sprite[] moveRightSprites;
    [SerializeField] private Sprite[] moveUpSprites;
    [SerializeField] private Sprite[] moveDownSprites;
    [SerializeField] private Sprite[] idleLeftSprites;
    [SerializeField] private Sprite[] idleRightSprites;
    [SerializeField] private Sprite[] idleUpSprites;
    [SerializeField] private Sprite[] idleDownSprites;
    [SerializeField] private Sprite[] longIdleLeftSprites;
    [SerializeField] private Sprite[] longIdleRightSprites;
    [SerializeField] private Sprite[] longIdleUpSprites;
    [SerializeField] private Sprite[] longIdleDownSprites;
    //Time after which moving sprites should be changed
    [SerializeField] private float moveFlipTime=0.2f;
    //Time after which idle sprites should be changed
    [SerializeField] private float idleFlipTime=0.4f;
    //Time after which character will go into long wait animation state
    //(Currently after that time main character will sit down and wait)
    [SerializeField] private float timeToWaitUntilLongIdleAnimation=10f;
    //Offset for the shadow
    [SerializeField] private Vector3 longAnimationUpOffset;
    [SerializeField] private Vector3 longAnimationDownOffset;
    [SerializeField] private Vector3 longAnimationLeftOffset;
    [SerializeField] private Vector3 longAnimationRightOffset;
    [SerializeField] private Transform shadow;
    //Time after which player will exit the attack state
    [SerializeField] private float timeToCancelFireSprites=1f;

    //Curent index of the sprite in the array
    private int currentSprite=0;
    //References to other components
    private Movement movement;
    private SpriteRenderer spriteRenderer;
    //Direction in which player now moves
    private Directions currentDirection;
    //Time counter to flip sprites
    private float timePassed=0f;
    //Time counter to enter long wait animation state
    private float timePassedForLongIdleAnimation;
    //Time counter to exit sttack state
    private float timePassedSinceFireStoped=0f;
    //Flag which tells whether player is moving state or not
    private bool movingAnimation=true;
    //Stores initial shadow position
    private Vector3 usualShadowPosition;
    //Flag which tells whether player is long wait animation state or not
    private bool longWaitAnimation=false;
    //Stores which animation to use
    private Action animationPicker;
    //Is player attacking now
    private bool attacking=false;
    private Directions previousDirection;
    //Current angle between main character, mouse and x-axis
    private float currentAngle;


    //Getters and setters
    public Directions getDirection()
    {
        return currentDirection;
    }

    public bool isLongWaitAnimation()
    {
        return longWaitAnimation;
    }

    public float getCurrentAngle()
    {
        return currentAngle;
    }

    public float getTimeToCancelAttackingAnimation()
    {
        return timeToCancelFireSprites;
    }

    public bool isAttackingAnimation()
    {
        return attacking || timePassedSinceFireStoped<timeToCancelFireSprites;
    }



    void Start()
    {
        movement = transform.parent.gameObject.GetComponent<Movement>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        currentDirection=Directions.Down;
        usualShadowPosition = shadow.localPosition;
        spriteRenderer.sprite = longIdleDownSprites[0];
        animationPicker = LongWaitAnimation;
        longWaitAnimation=true;
        timePassedForLongIdleAnimation=timeToWaitUntilLongIdleAnimation+1f;
        timePassedSinceFireStoped+=timeToCancelFireSprites;

        EventsManager.OnStopFire+=StopAttackAnimation;
        EventsManager.OnStartFire+=StartAttackAnimation;
    }

    

    void Update()
    {
        //Increase counters
        timePassed+=Time.deltaTime;
        timePassedForLongIdleAnimation+=Time.deltaTime;
        timePassedSinceFireStoped+=Time.deltaTime;

        //Calculate angle
        GetAngleToMouse();

        //Select which animation to show
        if(movement.getIfCharacterMoves())
        {
            longWaitAnimation=false;

            if(isAttackingAnimation())
                animationPicker=FireMovementAnimation;
            else
                animationPicker=UsualMovementAnimation;
        }
        else if(!longWaitAnimation)
        {
            if(isAttackingAnimation())
                animationPicker=FireWaitAnimation;
            else
                animationPicker=ShortWaitAnimation;
        }
        
        //Show animation
        animationPicker();

        if(timePassed<0)
            timePassed=0f;
    }



    //Starts attack animation
    private void StartAttackAnimation()
    {
        timePassedForLongIdleAnimation=0f;
        longWaitAnimation=false;
        attacking=true;
    }



    //Stops attack animation
    private void StopAttackAnimation()
    {
        timePassedForLongIdleAnimation=0f;
        timePassedSinceFireStoped=0f;
        attacking=false;
    }



    //This method shows movement animation
    private void UsualMovementAnimation()
    {
        //If time passed then flip sprite
        if(timePassed>moveFlipTime || !movingAnimation)
        {
            timePassed-=moveFlipTime;
            currentSprite++;
            //Get current direction
            Vector2 direction = movement.getMovementDirection();
            movingAnimation=true;
            shadow.localPosition = usualShadowPosition;

            //Get current direction
            if(direction.y>0.7)
                currentDirection=Directions.Up;
            else if(direction.y<-0.7)
                currentDirection=Directions.Down;
            else if(direction.x>0.7)
                currentDirection=Directions.Right;
            else if(direction.x<-0.7)
                currentDirection=Directions.Left;
            
            SetNewSprite(moveUpSprites, moveDownSprites, moveLeftSprites, moveRightSprites);

            timePassedForLongIdleAnimation=0f;
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



    //This method shows movement animation while attacking
    private void FireMovementAnimation()
    {
        //Check if time to change sprite
        if(timePassed>moveFlipTime || !movingAnimation)
        {
            timePassed-=moveFlipTime;
            currentSprite++;
            Vector2 direction = movement.getMovementDirection();
            movingAnimation=true;
            shadow.localPosition = usualShadowPosition;
            timePassedForLongIdleAnimation=0f;

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

            SetNewSprite(moveUpSprites, moveDownSprites, moveLeftSprites, moveRightSprites);
        }
    }



    //This method shows movement animation while standing on a place
    private void FireWaitAnimation()
    {
        currentDirection = GetMouseDirection();

        //Check if time to change sprite
        if(timePassed>idleFlipTime || movingAnimation || currentDirection!=previousDirection)
        {
            timePassed-=idleFlipTime;
            currentSprite++;
            movingAnimation=false;

            SetNewSprite(idleUpSprites, idleDownSprites, idleLeftSprites, idleRightSprites);

            previousDirection = currentDirection;
        }
    }



    //This method shows wait animation just after movement
    private void ShortWaitAnimation()
    {
        //Check if time to change sprite
        if(timePassed>idleFlipTime || movingAnimation)
        {
            timePassed-=idleFlipTime;
            currentSprite++;
            movingAnimation=false;

            SetNewSprite(idleUpSprites, idleDownSprites, idleLeftSprites, idleRightSprites);
        }

        //Check if time to enter long wait animation state
        if(timePassedForLongIdleAnimation>timeToWaitUntilLongIdleAnimation)
        {
            longWaitAnimation=true;
            timePassed+=idleFlipTime;
            animationPicker=LongWaitAnimation;
        }
    }



    //This method shows long wait animation
    private void LongWaitAnimation()
    {
        //Check if time to change sprite
        if(timePassed>idleFlipTime)
        {
            timePassed-=idleFlipTime;
            currentSprite++;

            SetNewSprite(longIdleUpSprites, longIdleDownSprites, longIdleLeftSprites, longIdleRightSprites);

            //Set correct shadow position
            switch(currentDirection)
            {
                case Directions.Up:
                    shadow.localPosition = usualShadowPosition+longAnimationUpOffset;
                    break;
                case Directions.Down:
                    shadow.localPosition = usualShadowPosition+longAnimationDownOffset;
                    break;
                case Directions.Left:
                    shadow.localPosition = usualShadowPosition+longAnimationLeftOffset;
                    break;
                case Directions.Right:
                    shadow.localPosition = usualShadowPosition+longAnimationRightOffset;
                    break;
            }
        }
    }



    //This method sets new sprite depending on the current direction 
    //in which character is looking
    private void SetNewSprite(Sprite[] up, Sprite[] down, Sprite[] left, Sprite[] right)
    {
        switch(currentDirection)
        {
            case Directions.Up:
                if(currentSprite>=up.Length)
                    currentSprite=0;

                spriteRenderer.sprite = up[currentSprite];
                break;
            case Directions.Down:
                if(currentSprite>=down.Length)
                    currentSprite=0;

                spriteRenderer.sprite = down[currentSprite];
                break;
            case Directions.Left:
                if(currentSprite>=left.Length)
                    currentSprite=0;

                spriteRenderer.sprite = left[currentSprite];
                break;
            case Directions.Right:
                if(currentSprite>=right.Length)
                    currentSprite=0;

                spriteRenderer.sprite = right[currentSprite];
                break;
            case Directions.BackwardsUp:
                if(currentSprite>=down.Length)
                    currentSprite=0;

                spriteRenderer.sprite = down[down.Length-currentSprite-1];
                break;
            case Directions.BackwardsDown:
                if(currentSprite>=up.Length)
                    currentSprite=0;

                spriteRenderer.sprite = up[up.Length-currentSprite-1];
                break;
            case Directions.BackwardsLeft:
                if(currentSprite>=right.Length)
                    currentSprite=0;

                spriteRenderer.sprite = right[right.Length-currentSprite-1];
                break;
            case Directions.BackwardsRight:
                if(currentSprite>=left.Length)
                    currentSprite=0;

                spriteRenderer.sprite = left[left.Length-currentSprite-1];
                break;
        }
    }
}
