using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    public float maxSpeed;
    public float minHeight, maxHeight;
    public float damageTime = 0.5f;
    public int maxHealth = 10;
    public float attackRate = 1f;
    public string enemyName;
    public Sprite enemyImage;

    private int currentHealth;
    private float currentSpeed;
    private Rigidbody rb;
    protected Animator anim;
    private Transform groundCheck;
    private bool onGround;
    protected bool facingRight = false;
    private Transform target;
    protected bool isDead = false;
    private float zForce;
    private float walkTimer;
    private bool damaged = false;
    private float damageTimer ;
    private float nextAttack;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        groundCheck = transform.Find("GroundCheck");
        target = FindObjectOfType<Player>().transform;
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {

        onGround = Physics.Linecast(transform.position, groundCheck.position, 1 << LayerMask.NameToLayer("Ground"));
        anim.SetBool("Grounded", onGround);
        anim.SetBool("Dead", isDead);

        facingRight = (target.position.x < transform.position.x) ? false : true;
        if (facingRight)
        {
            transform.eulerAngles = new Vector3(0, 180, 0);
        }
        else
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
        }

        if (damaged && !isDead)
        {
            damageTimer += Time.deltaTime;
            if(damageTimer>= damageTime)
            {
                damaged = false;
                damageTimer = 0;
            }
        }

        walkTimer += Time.deltaTime;
    }

    private void FixedUpdate()
    {
        if (!isDead)
        {
            Vector3 targetDistance = target.position - transform.position;
            float hForce = targetDistance.x / Mathf.Abs(targetDistance.x);

            if(walkTimer>= Random.Range(1f,2f))
            {
                zForce = Random.Range(-1, 2);
                walkTimer = 0;
            }

            if(Mathf.Abs(targetDistance.x) < 1.5f)
            {
                hForce = 0;
            }

            if (!damaged)
            {
                rb.velocity = new Vector3(hForce * currentSpeed, 0, zForce * currentSpeed);
            }

            anim.SetFloat("Speed", Mathf.Abs(currentSpeed));

            if (Mathf.Abs(targetDistance.x) < 1.5f && Mathf.Abs(targetDistance.z) < 1.5 && Time.time > nextAttack)
            {
                anim.SetTrigger("Attack");
                currentSpeed = 0;
                nextAttack = Time.time + attackRate;
            }
        }

        rb.position = new Vector3(
            rb.position.x, 
            rb.position.y, 
            Mathf.Clamp(rb.position.z, minHeight, maxHeight)
        );

    }

    public void TookDamage(int damage)
    {
        if (!isDead)
        {
            damaged = true;
            currentHealth -= damage;
            anim.SetTrigger("HitDamage");
            FindObjectOfType<UIManager>().UpdateEnemyUI(maxHealth, currentHealth, enemyName, enemyImage);
            if(currentHealth <= 0)
            {
                isDead = true;
                rb.AddRelativeForce(new Vector3(3,5,0),ForceMode.Impulse);
            }
        }
    }

    public void DisableEnemy()
    {
        gameObject.SetActive(false);
    }

    private void ZeroSpeed()
    {
        currentSpeed = 0;
    }

    private void ResetSpeed()
    {
        currentSpeed = maxSpeed;
    }
}
