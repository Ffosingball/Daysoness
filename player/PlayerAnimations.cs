using UnityEngine;

public class PlayerAnimations : MonoBehaviour
{
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
    [SerializeField] private float moveFlipTime=0.2f;
    [SerializeField] private float idleFlipTime=1f;
    [SerializeField] private float timeToWaitUntilLongIdleAnimation=10f;

    private int currentSprite=0;
    private Movement movement;
    private SpriteRenderer spriteRenderer;
    private Directions currentDirection;
    private float timePassed=0f;
    private float timePassedForLongIdleAnimation=0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        movement = transform.parent.gameObject.GetComponent<Movement>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        currentDirection=Directions.Down;

        spriteRenderer.sprite = longIdleDownSprites[0];
    }

    // Update is called once per frame
    void Update()
    {
        timePassed+=Time.deltaTime;
        timePassedForLongIdleAnimation+=Time.deltaTime;

        if(movement.getIfCharacterMoves())
        {
            if(timePassed>moveFlipTime)
            {
                timePassed-=moveFlipTime;
                currentSprite++;
                Vector2 direction = movement.getMovementDirection();

                if(direction.y>0.7)
                    currentDirection=Directions.Up;
                else if(direction.y<-0.7)
                    currentDirection=Directions.Down;
                else if(direction.x>0.7)
                    currentDirection=Directions.Right;
                else if(direction.x<-0.7)
                    currentDirection=Directions.Left;
                
                switch(currentDirection)
                {
                    case Directions.Up:
                        if(currentSprite>=moveUpSprites.Length)
                            currentSprite=0;

                        spriteRenderer.sprite = moveUpSprites[currentSprite];
                        break;
                    case Directions.Down:
                        if(currentSprite>=moveDownSprites.Length)
                            currentSprite=0;

                        spriteRenderer.sprite = moveDownSprites[currentSprite];
                        break;
                    case Directions.Left:
                        if(currentSprite>=moveLeftSprites.Length)
                            currentSprite=0;

                        spriteRenderer.sprite = moveLeftSprites[currentSprite];
                        break;
                    case Directions.Right:
                        if(currentSprite>=moveRightSprites.Length)
                            currentSprite=0;

                        spriteRenderer.sprite = moveRightSprites[currentSprite];
                        break;
                }

                timePassedForLongIdleAnimation=0f;
            }
        }
        else if(timePassedForLongIdleAnimation<timeToWaitUntilLongIdleAnimation)
        {
            if(timePassed>idleFlipTime)
            {
                timePassed-=idleFlipTime;
                currentSprite++;

                switch(currentDirection)
                {
                    case Directions.Up:
                        if(currentSprite>=idleUpSprites.Length)
                            currentSprite=0;

                        spriteRenderer.sprite = idleUpSprites[currentSprite];
                        break;
                    case Directions.Down:
                        if(currentSprite>=idleDownSprites.Length)
                            currentSprite=0;

                        spriteRenderer.sprite = idleDownSprites[currentSprite];
                        break;
                    case Directions.Left:
                        if(currentSprite>=idleLeftSprites.Length)
                            currentSprite=0;

                        spriteRenderer.sprite = idleLeftSprites[currentSprite];
                        break;
                    case Directions.Right:
                        if(currentSprite>=idleRightSprites.Length)
                            currentSprite=0;

                        spriteRenderer.sprite = idleRightSprites[currentSprite];
                        break;
                }
            }
        }
        else
        {
            if(timePassed>idleFlipTime)
            {
                timePassed-=idleFlipTime;
                currentSprite++;

                switch(currentDirection)
                {
                    case Directions.Up:
                        if(currentSprite>=longIdleUpSprites.Length)
                            currentSprite=0;

                        spriteRenderer.sprite = longIdleUpSprites[currentSprite];
                        break;
                    case Directions.Down:
                        if(currentSprite>=longIdleDownSprites.Length)
                            currentSprite=0;

                        spriteRenderer.sprite = longIdleDownSprites[currentSprite];
                        break;
                    case Directions.Left:
                        if(currentSprite>=longIdleLeftSprites.Length)
                            currentSprite=0;

                        spriteRenderer.sprite = longIdleLeftSprites[currentSprite];
                        break;
                    case Directions.Right:
                        if(currentSprite>=longIdleRightSprites.Length)
                            currentSprite=0;

                        spriteRenderer.sprite = longIdleRightSprites[currentSprite];
                        break;
                }
            }
        }
    }
}
