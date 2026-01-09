using System.Collections.Generic;
using System.Collections;
using UnityEngine;


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
    private PlayerComponent playerComponent;
    private Transform playerTransform;
    [SerializeField] private Transform rotationBox;
    [SerializeField] private float deadCorpseExists=120f;
    [SerializeField] private Sprite deadBodySprite;
    [SerializeField] private float attackDMG;
    [SerializeField] private float attackPeriod;
    [SerializeField] private float damageBlinkPeriod=0.3f;
    [SerializeField] private float speed;
    [SerializeField] private float detectionRange;
    //[SerializeField] private float meeleWeaponResistance=0f;
    //[SerializeField] private float firearmWeaponResistance=0f;

    private float currentHP;
    [SerializeField] private bool dead;
    private List<CurrentDamagingWeapon> attackingMeeleWeaponsInRange;
    [SerializeField] private SpriteRenderer spriteRenderer;
    private Coroutine deadCountdown, attacking, pursuting, damageB;
    private Color spriteColor;
    private float timePassedSinceLastAttack=0f;
    private Rigidbody2D rigidbody2d;
    private bool move=false;
    private Vector2 movementDirection;


    public bool IsDead()
    {
        return dead;
    }


    public bool IsAttacking()
    {
        return !(attacking==null);
    }


    public bool IsPursuiting()
    {
        return !(pursuting==null);
    }


    public float getSpeed()
    {
        return speed;
    }


    public bool IsMoving()
    {
        //Debug.Log("Changed movement");
        return move;
    }


    public void setIsMoving(bool _move)
    {
        //Debug.Log("Changed movement");
        move = _move;
    }


    public void setMovementDirection(Vector2 _direction)
    {
        movementDirection = _direction;
    }


    public float getDetectionRange()
    {
        return detectionRange;
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
        if(move && !dead)
            rigidbody2d.linearVelocity = movementDirection * speed;
        else if(!dead)
            rigidbody2d.linearVelocity = movementDirection * 0f;
    }



    private void OnTriggerEnter2D(Collider2D collision)
    {
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
        pursuting = StartCoroutine(pursuitLoop());
    }



    public void StopPursuit()
    {
        if(pursuting!=null)
        {
            StopCoroutine(pursuting);
            pursuting=null;
            move=false;
        }
    }



    private IEnumerator pursuitLoop()
    {
        //Debug.Log("Started pursuit!");
        move=true;
        var wait = new WaitForFixedUpdate();

        while(true)
        {
            movementDirection = (playerTransform.position - transform.position).normalized;

            float distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);
            if(distanceToPlayer>detectionRange)
                StopPursuit();
            
            yield return wait;
        }
    }



    public void StartAttack()
    {
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