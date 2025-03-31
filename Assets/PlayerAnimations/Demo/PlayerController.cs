using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour {

    public Level1Controller level1Controller;
    [SerializeField] float      m_speed = 4.0f;
    [SerializeField] float      m_jumpForce = 7.5f;
    [SerializeField] float      m_rollForce = 6.0f;
    [SerializeField] int        m_attackForce = 30;
    [SerializeField] int        m_maxHealth = 100;
    [SerializeField] bool       m_noBlood = false;
    [SerializeField] GameObject m_slideDust;
    [SerializeField] int        m_totalJumps;

    private Animator            m_animator;
    private Rigidbody2D         m_body2d;
    private Sensor_HeroKnight   m_groundSensor;
    private Sensor_HeroKnight   m_wallSensorR1;
    private Sensor_HeroKnight   m_wallSensorR2;
    private Sensor_HeroKnight   m_wallSensorL1;
    private Sensor_HeroKnight   m_wallSensorL2;
    private bool                m_isWallSliding = false;
    private bool                m_grounded = false;
    private bool                m_rolling = false;
    private int                 m_facingDirection = 1;
    private int                 m_currentAttack = 0;
    private float               m_timeSinceAttack = 0.0f;
    private float               m_delayToIdle = 0.0f;
    private float               m_rollDuration = 8.0f / 14.0f;
    private float               m_rollCurrentTime;
    private int                 m_availableJumps;
    private bool                m_multipleJump;
    private bool                m_isDead;
    private bool                m_isBlocking = false;
    
    // Use this for initialization
    void Start ()
    {
        m_animator = GetComponent<Animator>();
        m_body2d = GetComponent<Rigidbody2D>();
        m_groundSensor = transform.Find("GroundSensor").GetComponent<Sensor_HeroKnight>();
        m_wallSensorR1 = transform.Find("WallSensor_R1").GetComponent<Sensor_HeroKnight>();
        m_wallSensorR2 = transform.Find("WallSensor_R2").GetComponent<Sensor_HeroKnight>();
        m_wallSensorL1 = transform.Find("WallSensor_L1").GetComponent<Sensor_HeroKnight>();
        m_wallSensorL2 = transform.Find("WallSensor_L2").GetComponent<Sensor_HeroKnight>();
        m_availableJumps = m_totalJumps;
    }

    // Update is called once per frame
    void Update ()
    {
        // Increase timer that controls attack combo
        m_timeSinceAttack += Time.deltaTime;
        // Increase timer that checks roll duration
        if (m_rolling)
        {
            m_rollCurrentTime += Time.deltaTime;
            GetComponent<HealthBar>().isPlayerAttcking = false;
        }

        // Disable rolling if timer extends duration
        if (m_rollCurrentTime > m_rollDuration)
        {
            m_rolling = false;
            GetComponent<HealthBar>().isPlayerAttcking = false;
        }

        //Check if character just landed on the ground
        if (!m_grounded && m_groundSensor.State())
        {
            m_grounded = true;
            m_animator.SetBool("Grounded", m_grounded);
            GetComponent<HealthBar>().isPlayerAttcking = false;
        }
        //Check if character just started falling
        if (m_grounded && !m_groundSensor.State())
        {
            m_grounded = false;
            m_animator.SetBool("Grounded", m_grounded);
            GetComponent<HealthBar>().isPlayerAttcking = false;
        }

        // -- Handle input and movement --
        float inputX = ControlFreak2.CF2Input.GetAxis("Horizontal");

        if(CanMove())
        {
            // Swap direction of sprite depending on walk direction
            if (inputX > 0)
            {
                GetComponent<SpriteRenderer>().flipX = false;
                m_facingDirection = 1;
                GetComponent<HealthBar>().isPlayerAttcking = false;
            }
            else if (inputX < 0)
            {
                GetComponent<SpriteRenderer>().flipX = true;
                m_facingDirection = -1;
                GetComponent<HealthBar>().isPlayerAttcking = false;
            }
            // Move left or right
            if (!m_rolling)
            {
                m_body2d.velocity = new Vector2(inputX * m_speed, m_body2d.velocity.y);
                GetComponent<HealthBar>().isPlayerAttcking = false;
            }

            //Set AirSpeed in animator
            m_animator.SetFloat("AirSpeedY", m_body2d.velocity.y);

            // -- Handle Animations --
            //Wall Slide
            m_isWallSliding = (m_wallSensorR1.State() && m_wallSensorR2.State()) || (m_wallSensorL1.State() && m_wallSensorL2.State());
            m_animator.SetBool("WallSlide", m_isWallSliding);

            //Death
            if (ControlFreak2.CF2Input.GetKeyDown("x") && !m_rolling)
            {
                m_animator.SetBool("noBlood", m_noBlood);
                m_animator.SetTrigger("Death");
                GetComponent<HealthBar>().isPlayerAttcking = false;
            }
            //Hurt
            else if (ControlFreak2.CF2Input.GetKeyDown("z") && !m_rolling)
            {
                m_animator.SetTrigger("Hurt");
                GetComponent<HealthBar>().isPlayerAttcking = false;
            }

            //Attack
            else if(ControlFreak2.CF2Input.GetMouseButtonDown(0) && m_timeSinceAttack > 0.25f && !m_rolling)
            {
                m_currentAttack++;
                GetComponent<HealthBar>().isPlayerAttcking = true;
                // Loop back to one after third attack
                if (m_currentAttack > 3)
                    m_currentAttack = 1;
                // Reset Attack combo if time since last attack is too large
                if (m_timeSinceAttack > 1.0f)
                    m_currentAttack = 1;

                // Call one of three attack animations "Attack1", "Attack2", "Attack3"
                m_animator.SetTrigger("Attack" + m_currentAttack);
                // Reset timer
                m_timeSinceAttack = 0.0f;
            }

            else if (ControlFreak2.CF2Input.GetMouseButtonUp(1))
            {
                m_isBlocking = false;
                GetComponent<HealthBar>().isPlayerAttcking = false;
                m_animator.SetBool("IdleBlock", false);
            } 
            // Roll
            else if (ControlFreak2.CF2Input.GetKeyDown("space") && !m_rolling && !m_isWallSliding)
            {
                GetComponent<HealthBar>().isPlayerAttcking = false;
                m_rolling = true;
                m_animator.SetTrigger("Roll");
                m_body2d.velocity = new Vector2(m_facingDirection * m_rollForce, m_body2d.velocity.y);
            }
            //Jump
            else if (ControlFreak2.CF2Input.GetKeyDown("w"))
            {
                GetComponent<HealthBar>().isPlayerAttcking = false;
                Jump();
            }
            //Run
            else if (Mathf.Abs(inputX) > Mathf.Epsilon)
            {
                // Reset timer
                GetComponent<HealthBar>().isPlayerAttcking = false;
                m_delayToIdle = 0.05f;
                m_animator.SetInteger("AnimState", 1);
            }
            //Idle
            else
            {
                GetComponent<HealthBar>().isPlayerAttcking = false;
                // Prevents flickering transitions to idle
                m_delayToIdle -= Time.deltaTime;
                    if(m_delayToIdle < 0)
                        m_animator.SetInteger("AnimState", 0);
            }
        }
        // Block
        if (ControlFreak2.CF2Input.GetMouseButtonDown(1) && !m_rolling && CanMove())
        {
            GetComponent<HealthBar>().isPlayerAttcking = false;
            m_isBlocking = true;
            m_animator.SetTrigger("Block");
            m_animator.SetBool("IdleBlock", true);
        }
        if(ControlFreak2.CF2Input.GetKeyDown(KeyCode.Escape))
            SceneManager.LoadScene(0);

        if(ControlFreak2.CF2Input.GetKeyDown(KeyCode.R))
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // Animation Events
    // Called in slide animation.
    void AE_SlideDust()
    {
        Vector3 spawnPosition;
        if (m_facingDirection == 1)
            spawnPosition = m_wallSensorR2.transform.position;
        else
            spawnPosition = m_wallSensorL2.transform.position;

        if (m_slideDust != null)
        {
            // Set correct arrow spawn position
            GameObject dust = Instantiate(m_slideDust, spawnPosition, gameObject.transform.localRotation) as GameObject;
            // Turn arrow in correct direction
            dust.transform.localScale = new Vector3(m_facingDirection, 1, 1);
        }
    }

    public bool IsRolling()
    {
        return m_rolling;
    }
    public bool IsBlocking()
    {
        return m_isBlocking;
    }
    void Jump()
    {
        if(m_grounded && !m_rolling)    // On ground
        {
            m_multipleJump = true;
            m_availableJumps--;

            m_animator.SetTrigger("Jump");
            m_animator.SetBool("Grounded", m_grounded);
            m_body2d.velocity = Vector2.up * m_jumpForce;//new Vector2(m_body2d.velocity.x, m_jumpForce);
            m_groundSensor.Disable(0.2f);
        }
        else    // Mid air
        {
            if(m_multipleJump && m_availableJumps > 0)
            {
                m_availableJumps--;

                m_animator.SetTrigger("Jump");
                m_animator.SetBool("Grounded", m_grounded);
                m_body2d.velocity = Vector2.up * m_jumpForce;//new Vector2(m_body2d.velocity.x, m_jumpForce);
                m_groundSensor.Disable(0.2f);

                m_multipleJump = false;
            }
            m_availableJumps = m_totalJumps;
        }
    }

    bool CanMove()
    {
        bool can = true;
        if(FindObjectOfType<InteractionSystem>().isExamining)
        {
            can = false;
        }

        if(m_isDead)
            return false;

        return can;
    }

    public void Die()
    {
        m_isDead = true;
        m_animator.SetBool("noBlood", m_noBlood);
        m_animator.SetTrigger("Death");   
    }
    public void Hurt()
    {
        m_animator.SetTrigger("Hurt");
    }
    public int GetMaxHP()
    {
        return m_maxHealth;
    }
    public int GetATKforce()
    {
        return m_attackForce;
    }
    public float GetJMPforce()
    {
        return m_jumpForce;
    }
    public float GetSpeed()
    {
        return m_speed;
    }
    public void SetMaxHealth(int value)
    {
        m_maxHealth =- value;
    }
    public void SetAttackForce(int value)
    {
        m_attackForce = value;
    }
    public void SetJumpForce(float value)
    {
        m_jumpForce = value;
    }
    public void SetSpeed(float value)
    {
        m_speed = value;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("StartTrigger"))
            level1Controller.isGameStarted = true;
    }

}