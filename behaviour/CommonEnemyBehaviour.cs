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
    private bool dead;
    private List<CurrentDamagingWeapon> attackingMeeleWeaponsInRange;
    private SpriteRenderer spriteRenderer;
    private Coroutine deadCountdown, attacking, pursuting, damageB;
    private Color spriteColor;


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


    public float getDetectionRange()
    {
        return detectionRange;
    }



    private void Start()
    {
        attackingMeeleWeaponsInRange = new List<CurrentDamagingWeapon>();
        currentHP = maxHP;
        dead=false;

        spriteRenderer = GetComponent<SpriteRenderer>();
        playerTransform = GameObject.FindGameObjectWithTag("player").transform;
        playerComponent = GameObject.FindGameObjectWithTag("player").GetComponent<PlayerComponent>();
    }



    private void Update()
    {
        foreach(CurrentDamagingWeapon weapon in attackingMeeleWeaponsInRange)
        {
            if(weapon.GetHit())
            {
                TakeDamage(weapon.GetDMG());
            }
        }
    }



    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject gameObject = collision.gameObject;
        if(gameObject.TryGetComponent<BulletBehaviour>(out BulletBehaviour bulletBehaviour))
        {
            TakeDamage(bulletBehaviour.getDMG());
            Destroy(gameObject);
        }
        else if(gameObject.tag=="swords")
        {
            attackingMeeleWeaponsInRange.Add(new CurrentDamagingWeapon(gameObject));
        }
        else if (collision.gameObject.name=="Main_character")
        {
            StopPursuit();
            StartAttack();
        }
    }



    private void OnCollisionExit2D(Collision2D collision) 
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
        else if (collision.gameObject.name=="Main_character")
        {
            StopAttack();
            StartPursuit();
        }
    }



    public void TakeDamage(float rawDMG)
    {
        currentHP-=rawDMG;
        if(damageB!=null)
            StopCoroutine(damageB);
        
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
            currentColor.b = Mathf.Clamp(0f,spriteColor.b,timePassed/damageBlinkPeriod);
            currentColor.g = Mathf.Clamp(0f,spriteColor.g,timePassed/damageBlinkPeriod);
            currentColor.r = Mathf.Clamp(1f,spriteColor.r,timePassed/damageBlinkPeriod);
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
        deadCountdown = StartCoroutine(countdown());
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
        }
    }



    private IEnumerator pursuitLoop()
    {
        while(true)
        {
            Vector2 direction = (playerTransform.position - transform.position).normalized;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
            rotationBox.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
            transform.Translate(new Vector3(1,0,0)*speed*Time.deltaTime);

            float distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);
            if(distanceToPlayer>detectionRange)
                StopPursuit();
            
            yield return null;
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
            playerComponent.TakeDamage(attackDMG);

            if(playerComponent.isDead() || dead)
                StopAttack();

            yield return new WaitForSeconds(attackPeriod);
        }
    }
}