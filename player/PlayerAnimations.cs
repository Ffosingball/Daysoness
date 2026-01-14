using UnityEngine;

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
    [SerializeField] private float scaleDecreaseToVertical=0.7f;

    private int currentSprite=0;
    private Movement movement;
    private SpriteRenderer spriteRenderer;
    private Directions currentDirection;
    private float timePassed=0f;
    private float timePassedForLongIdleAnimation=10f;
    private bool movingAnimation=true;
    private Vector3 usualShadowPosition;
    private bool longWaitAnimation=false;
    private float initScale;


    public Directions getDirection()
    {
        return currentDirection;
    }

    public bool isLongWaitAnimation()
    {
        return longWaitAnimation;
    }



    void Start()
    {
        movement = transform.parent.gameObject.GetComponent<Movement>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        currentDirection=Directions.Down;
        usualShadowPosition = shadow.localPosition;
        spriteRenderer.sprite = longIdleDownSprites[0];
        initScale = transform.localScale.x;
    }

    

    void Update()
    {
        timePassed+=Time.deltaTime;
        timePassedForLongIdleAnimation+=Time.deltaTime;
        longWaitAnimation=false;

        if(movement.getIfCharacterMoves())
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
        else if(timePassedForLongIdleAnimation<timeToWaitUntilLongIdleAnimation)
        {
            if(timePassed>idleFlipTime || movingAnimation)
            {
                timePassed-=idleFlipTime;
                currentSprite++;
                movingAnimation=false;

                SetNewSprite(idleUpSprites, idleDownSprites, idleLeftSprites, idleRightSprites);
            }
        }
        else
        {
            longWaitAnimation=true;

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

        if(timePassed<0)
            timePassed=0f;
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
        }
    }
}
