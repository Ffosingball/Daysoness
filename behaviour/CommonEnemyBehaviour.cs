using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System;
using Unity.VisualScripting;


public class CurrentDamagingWeapon
{
    private GameObject damagingMeeleWeapon = null;
    private int attackRowAbsorbed = -1;

    public CurrentDamagingWeapon(GameObject meeleWeapon)
    {
        damagingMeeleWeapon = meeleWeapon;
    }

    public bool GetHit()
    {
        int currentAttackRow = damagingMeeleWeapon.GetComponent<MeeleWeapon>().getAttackRowNum();

        if(currentAttackRow==attackRowAbsorbed)
            return false;

        attackRowAbsorbed = currentAttackRow;
        return true;
    }

    public bool IsEqual(GameObject gameObject)
    {
        return damagingMeeleWeapon==gameObject;
    }

    public float GetDMG()
    {
        return damagingMeeleWeapon.GetComponent<MeeleWeapon>().getDMG();
    }
}


public class CommonEnemyBehaviour : MonoBehaviour
{
    [SerializeField] private float maxHP;
    [SerializeField] private float deadCorpseExists=120f;
    [SerializeField] private Sprite deadBodySprite;
    [SerializeField] private float attackDMG;
    [SerializeField] private float attackPeriod;
    [SerializeField] private float damageBlinkPeriod=0.3f;
    [SerializeField] private float speed;
    [SerializeField] private float detectionRange;
    [SerializeField] private float targetDestinationRange=0.5f;
    [SerializeField] private float distanceToBarrierToChangeDirection=1f;
    [SerializeField] private float checkIfTargetVisibleWithInterval=1f;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private float periodToLosePlayer=5f;
    [SerializeField] private float timeUntilTeleport=200f;

    private PlayerComponent playerComponent;
    private Transform playerTransform;
    private float currentHP;
    private bool dead=false;
    private List<CurrentDamagingWeapon> attackingMeeleWeaponsInRange;
    private Coroutine deadCountdown, attacking, damageB;
    private Color spriteColor;
    private float timePassedSinceLastAttack=0f;
    private Rigidbody2D rigidbody2d;
    private bool move=false;
    private Vector2 movementDirection;
    private Vector2 targetDestination;
    private bool pursuting;
    private bool atTargetDestination=false;
    private Vector2 lastPlayerPosition;
    private Vector2 tempDirection;
    private float timePassed, timeStuck=0f, timeLeftUntilTeleport=0f;
    //private bool countUntilPlayerLost=false;
    private Vector3 previousPosition;
    private bool followPlayer;


    public bool IsDead()
    {
        return dead;
    }


    public bool IsMoving()
    {
        return move;
    }


    public void setTargetDestination(Vector2 _destination)
    {
        targetDestination = _destination;
    }


    public void setAtTargetDestination(bool _atTargetDestination)
    {
        atTargetDestination = _atTargetDestination;
    }


    public void setFollowPlayer(bool _followPlayer)
    {
        followPlayer = _followPlayer;
    }


    public bool getAtTargetDestination()
    {
        return atTargetDestination;
    }



    private void Start()
    {
        attackingMeeleWeaponsInRange = new List<CurrentDamagingWeapon>();
        currentHP = maxHP;
        dead=false;
        move=false;

        rigidbody2d = GetComponent<Rigidbody2D>();
        playerTransform = GameObject.FindGameObjectWithTag("player").transform;
        playerComponent = GameObject.FindGameObjectWithTag("player").GetComponent<PlayerComponent>();
    
        tempDirection=Vector2.zero;
        previousPosition = rigidbody2d.position;
    }



    private void Update()
    {
        timePassedSinceLastAttack+=Time.deltaTime;

        foreach(CurrentDamagingWeapon weapon in attackingMeeleWeaponsInRange)
        {
            if(weapon.GetHit())
            {
                TakeDamage(weapon.GetDMG());
            }
        }
    }



    private void FixedUpdate()
    {
        if(!dead)
        {
            if(attacking==null)
            {
                if(pursuting)
                    PursuitPlayer();
                else
                {
                    CheckIfPlayerVisible();

                    float distanceToTarget = Vector2.Distance(transform.position, targetDestination);
                    //Debug.Log("Distance: "+distanceToTarget+"; range: "+targetDestinationRange);
                    if(distanceToTarget>targetDestinationRange)
                        MoveToTargetPosition();
                    else
                    {
                        move=false;
                        atTargetDestination=true;
                    }
                }
            }

            if(move)
            {
                if(tempDirection!=Vector2.zero)
                    rigidbody2d.linearVelocity = tempDirection * speed;
                else
                    rigidbody2d.linearVelocity = movementDirection * speed;

                if(Vector2.Distance(previousPosition,rigidbody2d.position)< 0.001f)
                {
                    //Debug.Log("Do not move "+timeStuck);
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



    private void PursuitPlayer()
    {
        //Debug.Log(gameObject.name+" pursuiting");
        move=true;
        float distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);

        if(distanceToPlayer<=detectionRange && timeStuck<periodToLosePlayer)
        {
            if(followPlayer)
            {
                Vector2 direction = (playerTransform.position - transform.position).normalized;
                LayerMask hitMask = LayerMask.GetMask("Player", "Barrier");
                RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, detectionRange, hitMask);

                bool playerInFront=false;
                if(hit.collider!=null)
                {
                    if(hit.collider.gameObject.tag=="player")
                    {
                        //Debug.Log("I see player");
                        movementDirection = direction;
                        lastPlayerPosition = playerTransform.position;
                        playerInFront=true;
                        timeStuck=0f;
                    }
                }

                if(!playerInFront)
                {
                    //Debug.Log("I do NOT see player");

                    float distanceToRememberedPoint = Vector2.Distance(transform.position, lastPlayerPosition);

                    if(distanceToRememberedPoint<targetDestinationRange)
                        StopPursuit();
                    else
                    {
                        Vector2 newDirection = (lastPlayerPosition - (Vector2)transform.position).normalized;
                        movementDirection = newDirection;
                    }
                }
            }
            else
                move=false;
        }
        else
            StopPursuit();
    }



    private void CheckIfPlayerVisible()
    {
        //Debug.Log("Check player!");
        float distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);

        if(distanceToPlayer<=detectionRange && followPlayer)
        {
            //Debug.Log("Player in range!");
            Vector2 direction = (playerTransform.position - transform.position).normalized;
            LayerMask hitMask = LayerMask.GetMask("Player", "Barrier");
            RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, detectionRange, hitMask);

            if(hit.collider!=null)
            {
                //Debug.Log("Tag: "+hit.collider.gameObject.tag);
                if(hit.collider.gameObject.tag=="player")
                    StartPursuit();
            }
        }
    }



    private void MoveToTargetPosition()
    {
        move=true;
        timeLeftUntilTeleport+=Time.fixedDeltaTime;
        //Debug.Log(gameObject.name+" moving back");

        if(tempDirection!=Vector2.zero)
        {
            timePassed+=Time.fixedDeltaTime;

            if(timeStuck>1f)
                SelectNewTempDirection(tempDirection);

            if(timePassed>checkIfTargetVisibleWithInterval)
            {
                timePassed-=checkIfTargetVisibleWithInterval;

                if(timeStuck-0f<0.01f)
                    SelectNewTempDirection(tempDirection);

                Vector2 direction = (targetDestination - (Vector2)transform.position).normalized;
                LayerMask hitMask = LayerMask.GetMask("Barrier");
                RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, detectionRange, hitMask);

                float distanceToTarget = Vector2.Distance(transform.position, targetDestination);
                float distanceToBarrier;
                if(hit.collider==null)
                    distanceToBarrier = distanceToTarget+1f;
                else
                    distanceToBarrier = Vector2.Distance(transform.position, hit.collider.gameObject.transform.position);
                
                if(distanceToTarget<distanceToBarrier)
                    tempDirection = Vector2.zero;
            }

            if(tempDirection!=Vector2.zero)
            {
                //Debug.Log("Moving at temp direction: "+tempDirection.x+"; "+tempDirection.y);
                LayerMask hitMask = LayerMask.GetMask("Barrier");
                RaycastHit2D hit = Physics2D.Raycast(transform.position, tempDirection, detectionRange, hitMask);
                
                float distanceToBarrierInDirection;
                if(hit.collider==null)
                    distanceToBarrierInDirection = distanceToBarrierToChangeDirection+1f;
                else
                    distanceToBarrierInDirection = Vector2.Distance(transform.position, hit.collider.gameObject.transform.position);

                if(distanceToBarrierInDirection<distanceToBarrierToChangeDirection)
                    SelectNewTempDirection(tempDirection);
            }
        }
        else
        {
            timePassed=0f;

            //Debug.Log("Moving to final destination");
            Vector2 direction = (targetDestination - (Vector2)transform.position).normalized;
            LayerMask hitMask = LayerMask.GetMask("Barrier");
            RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, detectionRange, hitMask);

            float distanceToTarget = Vector2.Distance(transform.position, targetDestination);
            float distanceToBarrier;
            if(hit.collider==null)
                distanceToBarrier = distanceToTarget+1f;
            else
                distanceToBarrier = Vector2.Distance(transform.position, hit.collider.gameObject.transform.position);
            
            if(distanceToTarget<distanceToBarrier)
                movementDirection=direction;
            else
            {
                if(distanceToBarrier<distanceToBarrierToChangeDirection)
                    SelectNewTempDirection(direction);
                else
                    movementDirection=direction;
            }

            if(timeStuck>1f)
                SelectNewTempDirection(direction);
        }

        if(timeLeftUntilTeleport>timeUntilTeleport)
        {
            timeLeftUntilTeleport=0f;
            move=false;
            rigidbody2d.position = targetDestination;
        }
    }



    private void SelectNewTempDirection(Vector2 previousDirection)
    {
        //.Log("Selecting new temp direction");
        timeStuck=0f;
        if(previousDirection!=tempDirection)
        {
            previousDirection.x = (float)Math.Round(previousDirection.x, MidpointRounding.AwayFromZero);
            previousDirection.y = (float)Math.Round(previousDirection.y, MidpointRounding.AwayFromZero);
        }

        previousDirection = -previousDirection;

        //string posDir="Possible dir: ";
        List<Vector2> listOfPossibleDirections = new List<Vector2>();
        if(IsThisDirectionPossible(previousDirection, Vector2.up))
        {
            //posDir = posDir+" "+Vector2.up.x+", "+Vector2.up.y+";";
            listOfPossibleDirections.Add(Vector2.up);
        }
        
        if(IsThisDirectionPossible(previousDirection, Vector2.down))
        {
            //posDir = posDir+" "+Vector2.down.x+", "+Vector2.down.y+";";
            listOfPossibleDirections.Add(Vector2.down);
        }
        
        if(IsThisDirectionPossible(previousDirection, Vector2.left))
        {
            //posDir = posDir+" "+Vector2.left.x+", "+Vector2.left.y+";";
            listOfPossibleDirections.Add(Vector2.left);
        }
        
        if(IsThisDirectionPossible(previousDirection, Vector2.right))
        {
            //posDir = posDir+" "+Vector2.right.x+", "+Vector2.right.y+";";
            listOfPossibleDirections.Add(Vector2.right);
        }
        //Debug.Log(posDir);

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



    private bool IsThisDirectionPossible(Vector2 previousDirection, Vector2 targetDirection)
    {
        if(previousDirection!=targetDirection)
        {
            LayerMask hitMask = LayerMask.GetMask("Barrier");
            RaycastHit2D hit = Physics2D.Raycast(transform.position, targetDirection, detectionRange, hitMask);
            if(hit.collider==null)
                return true;

            float distanceToBarrierInDirection = Vector2.Distance(transform.position, hit.collider.gameObject.transform.position);
            //Debug.Log("Why skip? "+distanceToBarrierInDirection);
            if(distanceToBarrierInDirection>distanceToBarrierToChangeDirection*2f)
                return true;
        }

        return false;
    }



    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject gameObject = collision.gameObject;

        if(gameObject.tag=="swords")
        {
            attackingMeeleWeaponsInRange.Add(new CurrentDamagingWeapon(gameObject));
        }
    }



    private void OnTriggerExit2D(Collider2D collision) 
    {
        GameObject gameObject = collision.gameObject;

        if(gameObject.tag=="swords")
        {
            int index=0;
            int target=-1;
            foreach(CurrentDamagingWeapon weapon in attackingMeeleWeaponsInRange)
            {
                if(weapon.IsEqual(gameObject))
                {
                    target = index;
                    break;
                }
                
                index++;
            }

            attackingMeeleWeaponsInRange.RemoveAt(target);
        }
    }



    public void TakeDamage(float rawDMG)
    {
        currentHP-=rawDMG;
        if(damageB!=null)
        {
            StopCoroutine(damageB);
            spriteRenderer.color = spriteColor;
        }
        
        if(!dead)
            damageB = StartCoroutine(DamageBlink());

        if(currentHP<0)
            Die();
    }



    private IEnumerator DamageBlink()
    {
        spriteColor = spriteRenderer.color;
        Color currentColor = new Color(1f,0f,0f,spriteColor.a);

        float timePassed=0f;
        while(timePassed<damageBlinkPeriod)
        {
            //Debug.Log("B: "+currentColor.b);
            currentColor.b = Mathf.Clamp(timePassed/damageBlinkPeriod,0f,spriteColor.b);
            //Debug.Log("G: "+currentColor.g);
            currentColor.g = Mathf.Clamp(timePassed/damageBlinkPeriod,0f,spriteColor.g);
            //Debug.Log("R: "+currentColor.r);
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



    private IEnumerator countdown()
    {
        yield return new WaitForSeconds(deadCorpseExists);

        Destroy(gameObject);
    }



    public void StartPursuit()
    {
        //Debug.Log("Started pursuit");
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