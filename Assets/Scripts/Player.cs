using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    public float maxSpeed = 4;
    public float jumpForce = 400;
    public float minHeight, maxHeight;
    public int maxHealth=10;
    public string playerName;
    public Sprite playerImage;
    public AudioClip collisionSound, jumpSound, healthItem;

    private int currentHealth;
    private float currentSpeed;
    private Rigidbody rb;
    private Animator anim;
    [SerializeField]
    private Transform groundCheck;
    private bool onGround = true;
    private bool isDead = false;
    private bool facingRight = true;
    private bool jump = false;
    private AudioSource audioS;

    void Start()
    {
        this.rb = GetComponent<Rigidbody>();
        this.anim = this.GetComponent<Animator>();
        groundCheck = transform.Find("GroundCheck");
        currentSpeed = maxSpeed;
        currentHealth = maxHealth;
        this.audioS = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        onGround = Physics.Linecast(transform.position, groundCheck.position, 1 << LayerMask.NameToLayer("Ground"));

        anim.SetBool("OnGround", onGround);
        anim.SetBool("Dead", isDead);

        if(Input.GetButtonDown("Jump") && onGround)
        {
            jump = true;
        }

        if (Input.GetButtonDown("Fire1"))
        {
            anim.SetTrigger("Attack");
        }
    }

    private void FixedUpdate()
    {
        if (!isDead)
        {
            float h = Input.GetAxis("Horizontal");
            float z = Input.GetAxis("Vertical");

            if (!onGround)
            {
                z = 0;
            }

            rb.velocity = new Vector3(h * currentSpeed, rb.velocity.y, z* currentSpeed);

            if (onGround)
            {
                anim.SetFloat("Speed", Mathf.Abs(rb.velocity.magnitude));
            }

            if(h>0 && !facingRight)
            {
                Flip();
            }else if(h<0 && facingRight)
            {
                Flip();
            }

            if (jump)
            {
                jump = false;
                rb.AddForce(Vector3.up * jumpForce);

            }

            float minWidth = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 10)).x;
            float maxWidth = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0, 10)).x;
            rb.position = new Vector3(Mathf.Clamp(rb.position.x, minWidth + 1, maxWidth - 1),rb.position.y, Mathf.Clamp(rb.position.z, minHeight + 1, maxHeight - 1));
        }
    }

    private void Flip()
    {
        facingRight = !facingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    private void ZeroSpeed()
    {
        currentSpeed = 0;
    }

    private void ResetSpeed()
    {
        currentSpeed = maxSpeed;
    }

    public void TookDamage(int damage)
    {
        if (!isDead)
        {
            currentHealth -= damage;
            anim.SetTrigger("HitDamage");
            FindObjectOfType<UIManager>().UpdateHealth( currentHealth);
        }
    }

    public void PlaySong(AudioClip clip)
    {
        audioS.clip = clip;
        audioS.Play();
    }
}

