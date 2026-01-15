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
    [SerializeField] private float idleFlipTime=1f;
    [SerializeField] private float timeToWaitUntilLongIdleAnimation=10f;
    [SerializeField] private Vector3 longAnimationUpOffset;
    [SerializeField] private Vector3 longAnimationDownOffset;
    [SerializeField] private Vector3 longAnimationLeftOffset;
    [SerializeField] private Vector3 longAnimationRightOffset;
    [SerializeField] private Transform shadow;
    [SerializeField] private float timeToCancelFireSprites=1f;

    private int currentSprite=0;
    private Movement movement;
    private SpriteRenderer spriteRenderer;
    private Directions currentDirection;
    private float timePassed=0f;
    private float timePassedForLongIdleAnimation;
    private float timePassedSinceFireStoped=0f;
    private bool movingAnimation=true;
    private Vector3 usualShadowPosition;
    private bool longWaitAnimation=false;
    private Action animationPicker;
    private bool attacking=false;
    private Directions previousDirection;
    private float currentAngle;


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

    public bool isAttacking()
    {
        return attacking;
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

        EventsManager.OnStopFire+=StopAttackAnimation;
        EventsManager.OnStartFire+=StartAttackAnimation;
    }

    

    void Update()
    {
        timePassed+=Time.deltaTime;
        timePassedForLongIdleAnimation+=Time.deltaTime;
        timePassedSinceFireStoped+=Time.deltaTime;
        //longWaitAnimation=false;

        GetAngleToMouse();

        if(movement.getIfCharacterMoves())
        {
            longWaitAnimation=false;

            if(attacking || timePassedSinceFireStoped<timeToCancelFireSprites)
                animationPicker=FireMovementAnimation;
            else
                animationPicker=UsualMovementAnimation;
        }
        else if(!longWaitAnimation)
        {
            if(attacking || timePassedSinceFireStoped<timeToCancelFireSprites)
                animationPicker=FireWaitAnimation;
            else
                animationPicker=ShortWaitAnimation;
        }

        animationPicker();

        if(timePassed<0)
            timePassed=0f;
    }



    private void StartAttackAnimation()
    {
        timePassedForLongIdleAnimation=0f;
        longWaitAnimation=false;
        attacking=true;
    }



    private void StopAttackAnimation()
    {
        timePassedForLongIdleAnimation=0f;
        timePassedSinceFireStoped=0f;
        attacking=false;
    }



    private void UsualMovementAnimation()
    {
        if(timePassed>moveFlipTime || !movingAnimation)
        {
            timePassed-=moveFlipTime;
            currentSprite++;
            Vector2 direction = movement.getMovementDirection();
            movingAnimation=true;
            shadow.localPosition = usualShadowPosition;

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



    private void GetAngleToMouse()
    {
        Vector2 a = transform.position;
        Vector2 b = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());

        float angleRad = Mathf.Atan2(b.y - a.y, b.x - a.x);
        currentAngle = angleRad * Mathf.Rad2Deg;
    }



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



    private void FireMovementAnimation()
    {
        if(timePassed>moveFlipTime || !movingAnimation)
        {
            timePassed-=moveFlipTime;
            currentSprite++;
            Vector2 direction = movement.getMovementDirection();
            movingAnimation=true;
            shadow.localPosition = usualShadowPosition;
            timePassedForLongIdleAnimation=0f;

            Directions mouseDirection = GetMouseDirection();

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



    private void FireWaitAnimation()
    {
        currentDirection = GetMouseDirection();

        if(timePassed>idleFlipTime || movingAnimation || currentDirection!=previousDirection)
        {
            timePassed-=idleFlipTime;
            currentSprite++;
            movingAnimation=false;

            SetNewSprite(idleUpSprites, idleDownSprites, idleLeftSprites, idleRightSprites);

            previousDirection = currentDirection;
        }
    }



    private void ShortWaitAnimation()
    {
        if(timePassed>idleFlipTime || movingAnimation)
        {
            timePassed-=idleFlipTime;
            currentSprite++;
            movingAnimation=false;

            SetNewSprite(idleUpSprites, idleDownSprites, idleLeftSprites, idleRightSprites);
        }

        if(timePassedForLongIdleAnimation>timeToWaitUntilLongIdleAnimation)
        {
            longWaitAnimation=true;
            timePassed+=idleFlipTime;
            animationPicker=LongWaitAnimation;
        }
    }



    private void LongWaitAnimation()
    {
        if(timePassed>idleFlipTime)
        {
            timePassed-=idleFlipTime;
            currentSprite++;

            SetNewSprite(longIdleUpSprites, longIdleDownSprites, longIdleLeftSprites, longIdleRightSprites);

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
