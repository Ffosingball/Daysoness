using UnityEngine;

public class EnemyAnimation : MonoBehaviour
{
    //Animation sprites if only has 2 sides
    [SerializeField] private Sprite[] moveHorizontalSprites;
    [SerializeField] private Sprite[] moveVerticalSprites;
    [SerializeField] private Sprite[] idleHorizontalSprites;
    [SerializeField] private Sprite[] idleVerticalSprites;
    [SerializeField] private Sprite[] attackHorizontalSprites;
    [SerializeField] private Sprite[] attackVerticalSprites;
    //Animation sprites if only has all 4 sides
    [SerializeField] private Sprite[] moveUpSprites;
    [SerializeField] private Sprite[] moveDownSprites;
    [SerializeField] private Sprite[] moveLeftSprites;
    [SerializeField] private Sprite[] moveRightSprites;
    [SerializeField] private Sprite[] idleUpSprites;
    [SerializeField] private Sprite[] idleDownSprites;
    [SerializeField] private Sprite[] idleLeftSprites;
    [SerializeField] private Sprite[] idleRightSprites;
    [SerializeField] private Sprite[] attackUpSprites;
    [SerializeField] private Sprite[] attackDownSprites;
    [SerializeField] private Sprite[] attackLeftSprites;
    [SerializeField] private Sprite[] attackRightSprites;
    [SerializeField] private Sprite[] longIdleUpSprites;
    [SerializeField] private Sprite[] longIdleDownSprites;
    [SerializeField] private Sprite[] longIdleLeftSprites;
    [SerializeField] private Sprite[] longIdleRightSprites;
    //For everyone
    [SerializeField] private Sprite[] deadSprites;
    //Time after which moving sprites should be changed
    [SerializeField] private float flipTime=0.2f;
    [SerializeField] private GameObject shadow;
    [SerializeField] private bool hideShadowWhenVertical=false;
    [SerializeField] private bool hasAll4Sides=false;

    //Curent index of the sprite in the array
    private int currentSprite=0;
    //Stores current animation state
    private AnimationStates currentAnimation;
    private SpriteRenderer spriteRenderer;
    //Direction in which enemy is moving
    private Directions currentDirection;
    //Time counter to flip sprites
    private float timePassed=0f;
    private Transform playerTransform;
    private AnimationStates previousAnimation;
    private Vector2 movementDirection;
    private float currentAngle;


    public void setMovementDirection(Vector2 _movementDirection)
    {
        movementDirection = _movementDirection;
    }

    public void setCurrentAnimation(AnimationStates state)
    {
        currentAnimation = state;

        if(currentAnimation==AnimationStates.Dead)
            currentSprite=0;
    }



    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        timePassed+=flipTime;
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

        currentAnimation = AnimationStates.Idle;
    }

    

    void Update()
    {
        //Increase counters
        timePassed+=Time.deltaTime;

        //Calculate angle
        GetAngleToPlayer();

        if(timePassed>flipTime || previousAnimation!=currentAnimation)
        {
            timePassed-=flipTime;
            currentSprite++;

            switch(currentAnimation)
            {
                case AnimationStates.Idle:
                    //Debug.Log("Idle");
                    IdleAnimation();
                    break;
                case AnimationStates.Moving:
                    //Debug.Log("Moving");
                    MovingAnimation();
                    break;
                case AnimationStates.IdleAttacking:
                    //Debug.Log("attack");
                    IdleAttackAnimation();
                    break;
                case AnimationStates.LongIdle:
                    //Debug.Log("inactive");
                    LongIdleAnimation();
                    break;
                case AnimationStates.Dead:
                    //Debug.Log("Dead");
                    DeadAnimation();
                    break;
            }
        }

        if(timePassed<0)
            timePassed=0f;

        previousAnimation=currentAnimation;
    }



    //This method shows movement animation
    private void MovingAnimation()
    {
        //Get current direction
        if(movementDirection.x>0.7)
            currentDirection=Directions.Right;
        else if(movementDirection.x<-0.7)
            currentDirection=Directions.Left;
        else if(movementDirection.y>0.7)
            currentDirection=Directions.Up;
        else if(movementDirection.y<-0.7)
            currentDirection=Directions.Down;
            
        if(hasAll4Sides)
            SetNewSprite(moveRightSprites, moveUpSprites, moveLeftSprites, moveDownSprites);
        else
            SetNewSprite(moveHorizontalSprites, moveVerticalSprites);
    }



    private void DeadAnimation()
    {
        if(deadSprites.Length==1)
            currentSprite=0;

        if(currentSprite<deadSprites.Length)
        {
            shadow.SetActive(false);
            spriteRenderer.sprite = deadSprites[currentSprite];
            transform.rotation = Quaternion.Euler(0f,0f,0f);
            spriteRenderer.flipX = false;
        }
        
        if(deadSprites.Length==1)
            currentSprite+=2;
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



    //This method shows movement animation while attacking
    private void IdleAttackAnimation()
    {
        currentDirection = GetPlayerDirection();

        if(hasAll4Sides)
            SetNewSprite(attackRightSprites, attackUpSprites, attackLeftSprites, attackDownSprites);
        else
            SetNewSprite(attackHorizontalSprites, attackVerticalSprites);
    }



    //This method shows wait animation just after movement
    private void IdleAnimation()
    {
        if(hasAll4Sides)
            SetNewSprite(idleRightSprites, idleUpSprites, idleLeftSprites, idleDownSprites);
        else
            SetNewSprite(idleHorizontalSprites, idleVerticalSprites);
    }



    private void LongIdleAnimation()
    {
        if(hasAll4Sides)
            SetNewSprite(longIdleRightSprites, longIdleUpSprites, longIdleLeftSprites, longIdleDownSprites);
        else
            SetNewSprite(idleHorizontalSprites, idleVerticalSprites);
    }



    //This method sets new sprite depending on the current direction 
    //in which character is looking
    private void SetNewSprite(Sprite[] horizontal, Sprite[] vertical, Sprite[] left=null, Sprite[] down=null)
    {
        //Debug.Log("Up: "+vertical.Length+"; Right: "+horizontal.Length+"; Down: "+down.Length+"; Left: "+left.Length);
        //HORIZONTAL - RIGHT, VERTICAL - UP
        switch(currentDirection)
        {
            case Directions.Up:
                if(currentSprite>=vertical.Length)
                    currentSprite=0;

                if(hasAll4Sides)
                    spriteRenderer.sprite = vertical[currentSprite];
                else
                {
                    spriteRenderer.sprite = vertical[currentSprite];
                    transform.rotation = Quaternion.Euler(0f,0f,180f);
                    spriteRenderer.flipX = false;
                }

                if(hideShadowWhenVertical)
                    shadow.SetActive(false);
                break;
            case Directions.Down:
                if(currentSprite>=vertical.Length)
                    currentSprite=0;

                if(hasAll4Sides)
                    spriteRenderer.sprite = down[currentSprite];
                else
                {
                    spriteRenderer.sprite = vertical[currentSprite];
                    transform.rotation = Quaternion.Euler(0f,0f,0f);
                    spriteRenderer.flipX = false;
                }

                if(hideShadowWhenVertical)
                    shadow.SetActive(false);
                break;
            case Directions.Left:
                if(currentSprite>=horizontal.Length)
                    currentSprite=0;

                if(hasAll4Sides)
                    spriteRenderer.sprite = left[currentSprite];
                else
                {
                    spriteRenderer.sprite = horizontal[currentSprite];
                    transform.rotation = Quaternion.Euler(0f,0f,0f);
                    spriteRenderer.flipX = true;
                }

                if(hideShadowWhenVertical)
                    shadow.SetActive(true);
                break;
            case Directions.Right:
                if(currentSprite>=horizontal.Length)
                    currentSprite=0;

                if(hasAll4Sides)
                    spriteRenderer.sprite = horizontal[currentSprite];
                else
                {
                    spriteRenderer.sprite = horizontal[currentSprite];
                    transform.rotation = Quaternion.Euler(0f,0f,0f);
                    spriteRenderer.flipX = false;
                }

                if(hideShadowWhenVertical)
                    shadow.SetActive(true);
                break;
            case Directions.BackwardsUp:
                if(currentSprite>=vertical.Length)
                    currentSprite=0;

                if(hasAll4Sides)
                    spriteRenderer.sprite = down[vertical.Length-currentSprite-1];
                else
                {
                    spriteRenderer.sprite = vertical[vertical.Length-currentSprite-1];
                    transform.rotation = Quaternion.Euler(0f,0f,0f);
                    spriteRenderer.flipX = false;
                }

                if(hideShadowWhenVertical)
                    shadow.SetActive(false);
                break;
            case Directions.BackwardsDown:
                if(currentSprite>=vertical.Length)
                    currentSprite=0;

                if(hasAll4Sides)
                    spriteRenderer.sprite = vertical[vertical.Length-currentSprite-1];
                else
                {
                    spriteRenderer.sprite = vertical[vertical.Length-currentSprite-1];
                    transform.rotation = Quaternion.Euler(0f,0f,180f);
                    spriteRenderer.flipX = false;
                }

                if(hideShadowWhenVertical)
                    shadow.SetActive(false);
                break;
            case Directions.BackwardsLeft:
                if(currentSprite>=horizontal.Length)
                    currentSprite=0;

                if(hasAll4Sides)
                    spriteRenderer.sprite = horizontal[horizontal.Length-currentSprite-1];
                else
                {
                    spriteRenderer.sprite = horizontal[horizontal.Length-currentSprite-1];
                    transform.rotation = Quaternion.Euler(0f,0f,0f);
                    spriteRenderer.flipX = false;
                }

                if(hideShadowWhenVertical)
                    shadow.SetActive(true);
                break;
            case Directions.BackwardsRight:
                if(currentSprite>=horizontal.Length)
                    currentSprite=0;

                if(hasAll4Sides)
                    spriteRenderer.sprite = left[horizontal.Length-currentSprite-1];
                else
                {
                    spriteRenderer.sprite = horizontal[horizontal.Length-currentSprite-1];
                    transform.rotation = Quaternion.Euler(0f,0f,0f);
                    spriteRenderer.flipX = true;
                }

                if(hideShadowWhenVertical)
                    shadow.SetActive(true);
                break;
        }
    }
}
