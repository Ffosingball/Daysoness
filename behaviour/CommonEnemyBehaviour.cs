using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System;


public class CommonEnemyBehaviour : MonoBehaviour
{
    //Maximum hp which enemy can have
    [SerializeField] private float maxHP;
    //Howl long corpse should exist before disappearence
    [SerializeField] private float deadCorpseExists=120f;
    [SerializeField] private Sprite deadBodySprite;//WILL BE REMOVED
    [SerializeField] private float attackDMG;
    //Time between attacks
    [SerializeField] private float attackPeriod;
    //Duration of taking damage animation
    [SerializeField] private float damageBlinkPeriod=0.3f;
    [SerializeField] private float speed;
    [SerializeField] private float detectionRange;
    //How close enemy should be to target to say that it reached it
    [SerializeField] private float targetDestinationRange=0.3f;
    //How close an obstacle should be to the enemy to change direction
    [SerializeField] private float distanceToBarrierToChangeDirection=1f;
    //Check if path to target destination clear every n seconds
    [SerializeField] private float checkIfTargetVisibleWithInterval=1f;
    [SerializeField] private SpriteRenderer spriteRenderer;
    //How long player should be out of view of the enemy, so thel lose them
    //and go back to their usual behaviour
    [SerializeField] private float periodToLosePlayer=5f;
    //If enemy will not be able to get to target destination in this period
    //of time, then it will be just teleported there
    [SerializeField] private float timeUntilTeleport=200f;
    //Flag to indicate if active then do some behaviour, otherwise
    //do nothing
    [SerializeField] private bool isActive=true;
    //Epsilon for float equality checks
    [SerializeField] private float epsilon=0.001f;

    private PlayerComponent playerComponent;
    private Transform playerTransform;
    //Current enemy health
    private float currentHP;
    private bool dead=false;
    //deadCountdown stores countdown Coroutine
    //attacking stores attackLoop Coroutine
    //damageB stores damageBlink Coroutine
    private Coroutine deadCountdown, attacking, damageB;
    private Color spriteColor;
    //This counter is important to ensure that enemy will not attack
    //player more frequently than attackPeriod says
    private float timePassedSinceLastAttack=0f;
    private Rigidbody2D rigidbody2d;
    //Flag which indicates whether enemy should move or not
    private bool move=false;
    private Vector2 movementDirection;
    private Vector2 targetDestination;
    //Flag which tells if enemy is pursuiting someone
    private bool pursuting;
    //Flag which tells if enemy reached target destination
    private bool atTargetDestination=false;
    //Last seen player position before they dissapered out of view
    private Vector2 lastPlayerPosition;
    //Temporary movement direction
    private Vector2 tempDirection;
    //timePassed is counter of when to check for clear path to target destination
    //timeStuck is counter which says how long enemy is not moving when it should move
    private float timePassed, timeStuck=0f, timeLeftUntilTeleport=0f;
    private Vector3 previousPosition;
    private bool followPlayer=true;
    private BoxCollider2D collider2d;

    //Setters and getters
    public bool IsDead()
    {
        return dead;
    }

    public bool IsMoving()
    {
        return move;
    }

    public bool IsAttacking()
    {
        return attacking!=null;
    }

    public bool IsPursuiting()
    {
        return pursuting;
    }

    public void setTargetDestination(Vector2 _destination)
    {
        targetDestination = _destination;
        atTargetDestination=false;
    }

    public void setAtTargetDestination(bool _atTargetDestination)
    {
        atTargetDestination = _atTargetDestination;
    }

    public void setFollowPlayer(bool _followPlayer)
    {
        followPlayer = _followPlayer;
    }

    public void setIsActive(bool _isActive)
    {
        isActive = _isActive;
        //Debug.Log("Changed activity");
    }

    public bool getAtTargetDestination()
    {
        return atTargetDestination;
    }

    public float getDetectionRange()
    {
        return detectionRange;
    }



    //Initialize enemy
    private void Start()
    {
        currentHP = maxHP;
        dead=false;
        move=false;

        rigidbody2d = GetComponent<Rigidbody2D>();
        collider2d = GetComponent<BoxCollider2D>();
        playerTransform = GameObject.FindGameObjectWithTag("player").transform;
        playerComponent = GameObject.FindGameObjectWithTag("player").GetComponent<PlayerComponent>();
    
        tempDirection=Vector2.zero;
        previousPosition = rigidbody2d.position;
    }



    private void Update()
    {
        //Increase counter
        timePassedSinceLastAttack+=Time.deltaTime;
    }



    private void FixedUpdate()
    {
        //Check that enemy is active and not dead
        if(!dead && isActive)
        {
            //If enemy do not attacks
            if(attacking==null)
            {
                //Pursuit if needed
                if(pursuting)
                    PursuitPlayer();
                else
                {
                    //Otherwise check for player and move to target if needed
                    CheckIfPlayerVisible();

                    float distanceToTarget = Vector2.Distance(transform.position, targetDestination);
                    if(distanceToTarget>targetDestinationRange)
                        MoveToTargetPosition();
                    else
                    {
                        move=false;
                        atTargetDestination=true;
                    }
                }
            }

            //If can move than move
            if(move)
            {
                //If temporary direction set than use it, otherwise use movement direction
                if(tempDirection!=Vector2.zero)
                    rigidbody2d.linearVelocity = tempDirection * speed;
                else
                    rigidbody2d.linearVelocity = movementDirection * speed;

                //If enemy position did not changed since previous fram then it stuck 
                if(Vector2.Distance(previousPosition,rigidbody2d.position)< epsilon)
                {
                    timeStuck+=Time.fixedDeltaTime;
                }
                else
                    timeStuck=0f;
            }
            else
                rigidbody2d.linearVelocity = movementDirection * 0f;
        }

        previousPosition = rigidbody2d.position;
    }



    //This functions is a behaviour of the enemy when it pursuit a player
    private void PursuitPlayer()
    {
        //Check if player in range and it do not stuck for too long
        move=true;
        float distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);

        if(distanceToPlayer<=detectionRange && timeStuck<periodToLosePlayer)
        {
            //Check if enemy should follow player
            if(followPlayer)
            {
                //Set direction to player
                Vector2 direction = (playerTransform.position - transform.position).normalized;
                //Set hitMask, include there layer with enemies and obstacles
                LayerMask hitMask = LayerMask.GetMask("Player", "Barrier");
                //Make a cast
                RaycastHit2D hit = Physics2D.BoxCast(collider2d.bounds.center, collider2d.bounds.size, 0f, direction, detectionRange, hitMask);
                
                bool playerInFront=false;
                if(hit.collider!=null)
                {
                    //Check if hitted object is player
                    if(hit.collider.gameObject.tag=="player")
                    {
                        //then go to player
                        movementDirection = direction;
                        lastPlayerPosition = playerTransform.position;
                        playerInFront=true;
                        timeStuck=0f;
                    }
                }

                if(!playerInFront)
                {
                    //If player is not visible and last remembered player position is reached
                    //then stop pursuit, otherwise go to that remembered position
                    float distanceToRememberedPoint = Vector2.Distance(transform.position, lastPlayerPosition);

                    if(distanceToRememberedPoint<targetDestinationRange)
                        StopPursuit();
                    else
                    {
                        Vector2 newDirection = (lastPlayerPosition - (Vector2)transform.position).normalized;
                        movementDirection = newDirection;
                    }
                }
            }//otherwise do not move
            else
                move=false;
        }//otherwise stop pursuit
        else
            StopPursuit();
    }



    //This function checks if playe is visible, if so then start pursuit
    private void CheckIfPlayerVisible()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);

        if(distanceToPlayer<=detectionRange && followPlayer)
        {
            Vector2 direction = (playerTransform.position - transform.position).normalized;
            LayerMask hitMask = LayerMask.GetMask("Player", "Barrier");
            RaycastHit2D hit = Physics2D.BoxCast(collider2d.bounds.center, collider2d.bounds.size, 0f, direction, detectionRange, hitMask);

            if(hit.collider!=null)
            {
                if(hit.collider.gameObject.tag=="player")
                    StartPursuit();
            }
        }
    }



    //This is a complicated behaviour of the enemy which tells how to reach the target destination
    private void MoveToTargetPosition()
    {
        move=true;
        timeLeftUntilTeleport+=Time.fixedDeltaTime;

        //Check that temporary direction is set
        if(tempDirection!=Vector2.zero)
        {
            timePassed+=Time.fixedDeltaTime;

            //If it stuck for long enough then change direction
            if(timeStuck>1f)
                SelectNewTempDirection(tempDirection);

            //Check if it is time to check if path to target is clear
            if(timePassed>checkIfTargetVisibleWithInterval)
            {
                timePassed-=checkIfTargetVisibleWithInterval;

                //If enemy is not stuck then change direction
                if(timeStuck<epsilon)
                    SelectNewTempDirection(tempDirection);

                Vector2 direction = (targetDestination - (Vector2)transform.position).normalized;
                LayerMask hitMask = LayerMask.GetMask("Barrier");
                RaycastHit2D hit = Physics2D.BoxCast(collider2d.bounds.center, collider2d.bounds.size, 0f, direction, detectionRange, hitMask);

                float distanceToTarget = Vector2.Distance(transform.position, targetDestination);
                float distanceToBarrier;
                if(hit.collider==null)
                    distanceToBarrier = distanceToTarget+1f;
                else
                    distanceToBarrier = Vector2.Distance(transform.position, hit.collider.gameObject.transform.position);
                
                //Check if target is closer than obstacle than just go straight to the target
                if(distanceToTarget<distanceToBarrier)
                    tempDirection = Vector2.zero;
            }

            //Check again that temporary direction is set, because it might be different
            if(tempDirection!=Vector2.zero)
            {
                LayerMask hitMask = LayerMask.GetMask("Barrier");
                RaycastHit2D hit = Physics2D.BoxCast(collider2d.bounds.center, collider2d.bounds.size, 0f, tempDirection, detectionRange, hitMask);
                
                float distanceToBarrierInDirection;
                if(hit.collider==null)
                    distanceToBarrierInDirection = distanceToBarrierToChangeDirection+1f;
                else
                    distanceToBarrierInDirection = Vector2.Distance(transform.position, hit.collider.gameObject.transform.position);

                //Check if it to close to the obstacle than change direction
                if(distanceToBarrierInDirection<distanceToBarrierToChangeDirection)
                    SelectNewTempDirection(tempDirection);
            }
        }//otherwise go to target
        else
        {
            timePassed=0f;

            Vector2 direction = (targetDestination - (Vector2)transform.position).normalized;
            LayerMask hitMask = LayerMask.GetMask("Barrier");
            RaycastHit2D hit = Physics2D.BoxCast(collider2d.bounds.center, collider2d.bounds.size, 0f, direction, detectionRange, hitMask);

            float distanceToTarget = Vector2.Distance(transform.position, targetDestination);
            float distanceToBarrier;
            if(hit.collider==null)
                distanceToBarrier = distanceToTarget+1f;
            else
                distanceToBarrier = Vector2.Distance(transform.position, hit.collider.gameObject.transform.position);
            
            //Check that target is closer then obstacle
            if(distanceToTarget<distanceToBarrier)
                movementDirection=direction;
            else
            {//If not then still go in direction of the target until it hit an obstacle, then 
            //select new direction
                if(distanceToBarrier<distanceToBarrierToChangeDirection)
                    SelectNewTempDirection(direction);
                else
                    movementDirection=direction;
            }

            //If it stuck for to long then go in other direction
            if(timeStuck>1f)
                SelectNewTempDirection(direction);
        }

        //If it cannot reach target for too long than just teleport there
        if(timeLeftUntilTeleport>timeUntilTeleport)
        {
            timeLeftUntilTeleport=0f;
            move=false;
            rigidbody2d.position = targetDestination;
        }
    }



    //This function selects a new random free direction for enemy to go
    private void SelectNewTempDirection(Vector2 previousDirection)
    {
        timeStuck=0f;
        //Round previous direction to whole numbers
        if(previousDirection!=tempDirection)
        {
            previousDirection.x = (float)Math.Round(previousDirection.x, MidpointRounding.AwayFromZero);
            previousDirection.y = (float)Math.Round(previousDirection.y, MidpointRounding.AwayFromZero);
        }

        //Now check all four direction if they are free or not and add them to possible
        //temporary directions list
        previousDirection = -previousDirection;
        List<Vector2> listOfPossibleDirections = new List<Vector2>();
        if(IsThisDirectionPossible(previousDirection, Vector2.up))
        {
            listOfPossibleDirections.Add(Vector2.up);
        }
        
        if(IsThisDirectionPossible(previousDirection, Vector2.down))
        {
            listOfPossibleDirections.Add(Vector2.down);
        }
        
        if(IsThisDirectionPossible(previousDirection, Vector2.left))
        {
            listOfPossibleDirections.Add(Vector2.left);
        }
        
        if(IsThisDirectionPossible(previousDirection, Vector2.right))
        {
            listOfPossibleDirections.Add(Vector2.right);
        }

        //Now pick in which direction enemy should go
        if(listOfPossibleDirections.Count==0)
        {
            move=false;
            Debug.Log(transform.gameObject.name+" stuck!");
            tempDirection = previousDirection;
        }
        else if(listOfPossibleDirections.Count==1)
            tempDirection = listOfPossibleDirections[0];
        else
            tempDirection = listOfPossibleDirections[UnityEngine.Random.Range(0,listOfPossibleDirections.Count)];
    }



    //This function returns if direction does not have obstacle for some time
    //and it does not equal to the previous one
    private bool IsThisDirectionPossible(Vector2 previousDirection, Vector2 targetDirection)
    {
        if(previousDirection!=targetDirection)
        {
            LayerMask hitMask = LayerMask.GetMask("Barrier");
            RaycastHit2D hit = Physics2D.BoxCast(collider2d.bounds.center, collider2d.bounds.size, 0f, targetDirection, detectionRange, hitMask);
            if(hit.collider==null)
                return true;

            float distanceToBarrierInDirection = Vector2.Distance(transform.position, hit.collider.gameObject.transform.position);
            if(distanceToBarrierInDirection>distanceToBarrierToChangeDirection*2f)
                return true;
        }

        return false;
    }



    public void TakeDamage(float rawDMG)
    {
        currentHP-=rawDMG;
        //Debug.Log("Damage Taken: "+rawDMG);
        EventsManager.CallOnDamageTaken(gameObject);
        //Stop previous coroutine if it is active
        if(damageB!=null)
        {
            StopCoroutine(damageB);
            spriteRenderer.color = spriteColor;
        }
        
        //Start taking damage animation
        if(!dead)
            damageB = StartCoroutine(DamageBlink());

        //Check id enemy is dead
        if(currentHP<0)
            Die();
    }



    //Animation of changing color when enemy receives damage
    private IEnumerator DamageBlink()
    {
        spriteColor = spriteRenderer.color;
        Color currentColor = new Color(1f,0f,0f,spriteColor.a);

        float timePassed=0f;
        while(timePassed<damageBlinkPeriod)
        {
            currentColor.b = Mathf.Clamp(timePassed/damageBlinkPeriod,0f,spriteColor.b);
            currentColor.g = Mathf.Clamp(timePassed/damageBlinkPeriod,0f,spriteColor.g);
            currentColor.r = Mathf.Clamp(timePassed/damageBlinkPeriod,1f,spriteColor.r);
            spriteRenderer.color = currentColor;
            timePassed+=0.02f;
            yield return new WaitForSeconds(0.02f);
        }

        damageB=null;
    }



    public void Heal(float hpToHeal)
    {
        currentHP+=hpToHeal;

        if(currentHP>maxHP)
            currentHP = maxHP;
    }



    public void Die()
    {
        dead = true;
        spriteRenderer.sprite = deadBodySprite;
        StopAttack();
        StopPursuit();
        deadCountdown = StartCoroutine(countdown());
        move=false;
    }



    public void Ressurect()
    {
        dead = false;
        currentHP = maxHP;
        StopCoroutine(deadCountdown);
        spriteRenderer.color = spriteColor;
    }



    //Countdown until object destruction
    private IEnumerator countdown()
    {
        yield return new WaitForSeconds(deadCorpseExists);

        Destroy(gameObject);
    }



    public void StartPursuit()
    {
        timeStuck=0f;
        pursuting = true;
        move=true;
        atTargetDestination=false;
        tempDirection=Vector2.zero;
        timeLeftUntilTeleport=0f;
    }



    public void StopPursuit()
    {
        pursuting = false;
        move=false;
        timeStuck=0f;
        timeLeftUntilTeleport=0f;
    }



    public void StartAttack()
    {
        atTargetDestination=false;
        attacking = StartCoroutine(attackLoop());
    }



    public void StopAttack()
    {
        if(attacking!=null)
        {
            StopCoroutine(attacking);
            attacking=null;
        }
    }



    private IEnumerator attackLoop()
    {
        //Attack player until player is dead or enemy itself
        while(true)
        {
            while(timePassedSinceLastAttack<attackPeriod)
            {
                yield return null;
            }

            timePassedSinceLastAttack=0f;
            playerComponent.TakeDamage(attackDMG);

            if(playerComponent.isDead() || dead)
                StopAttack();
        }
    }
}