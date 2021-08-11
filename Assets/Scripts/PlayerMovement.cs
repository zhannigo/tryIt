using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    protected Joystick joystick;
    protected JoyButton joyButton;

    private Rigidbody2D m_Rigidbody2D;
    public Animator m_Animator;

    public HealthBar healthBar;

    public Transform attackPoint;
    public float _attackRange;
    public int _attackDamage;
    public LayerMask enemyLayer;
    private readonly float attackRate = 2f;
    private float nextAttackTime = 0f;
    
    public float runSpeed = 60f;
    private float horizontalMove;
    private bool m_FacingRight = true;
    public float m_JumpForce;
    private bool _jump = false;
    private Vector2 m_Velocity = Vector2.zero;
    private readonly float m_MovementSmoothing = .1f;

    //находится ли персонаж на земле или в прыжке?
    private bool isGrounded;
    //ссылка на компонент Transform объекта для определения соприкосновения с землей
    public Transform groundCheck;
    //ссылка на слой, представляющий землю
    public LayerMask whatIsGround;
    //радиус коллайдера
    public float groundRadius;

    public int _maxPlayerHP;


    public int _currentPlayerHP;

    public int CurrentPlayerHP
    {
        get 
        { 
            return _currentPlayerHP; 
        }

        set
        {
            Debug.Log($"{value}, {_currentPlayerHP}");
            _currentPlayerHP = _currentPlayerHP >= 0 ? value : 0;
        }
    }

    bool _isDie = false;

    private void Awake()
    {
        //joystick = FindObjectOfType<Joystick>();
        //joyButton = FindObjectOfType<JoyButton>();

        m_Rigidbody2D = GetComponent<Rigidbody2D>();
        m_Animator = GetComponent<Animator>();
        healthBar.SetMaxHealth(_maxPlayerHP);
        CurrentPlayerHP = _maxPlayerHP;
    }
    void Update()
    {
       horizontalMove = Input.GetAxis("Horizontal") * runSpeed;

        if(Time.time >= nextAttackTime)
        {
            if (Input.GetButtonDown("Hit"))
            {
                Attack();
                nextAttackTime = Time.time + 1f / attackRate;
            }
        }
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
                _jump = true;
        }

    }
    private void FixedUpdate()
    {
        // определяем, на земле ли персонаж
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundRadius, whatIsGround);
        m_Animator.SetBool("isJumping", !isGrounded);

        if (_jump == true)
        {
            Jump();
        }
        //Move(joystick.Horizontal + horizontalMove);
        Move(horizontalMove);

        /*if (!isGrounded)
           {
               return;
           }*/
    }
    public void Move(float move)
    {

            //m_Rigidbody2D.AddForce(new Vector2(move, 0), ForceMode2D.Impulse);
            //m_Rigidbody2D.AddForce(new Vector2(move, m_Rigidbody2D.velocity.y), ForceMode2D.Impulse);
            Vector2 targetVelocity = new Vector2(move * 10, m_Rigidbody2D.velocity.y);
            m_Rigidbody2D.velocity = Vector2.SmoothDamp(m_Rigidbody2D.velocity, targetVelocity, ref m_Velocity, m_MovementSmoothing);

            //m_Animator.SetFloat("Speed", Mathf.Abs(horizontalMove+ joystick.Horizontal));
            m_Animator.SetFloat("Speed", Mathf.Abs(horizontalMove));


        if (move > 0 && !m_FacingRight)
        {
            Flip();
        }
        else if (move < 0 && m_FacingRight)
        {
            Flip();
        }
    }
    private void Jump()
    {
            //m_Rigidbody2D.AddForce(new Vector2(horizontalMove, m_JumpForce), ForceMode2D.Impulse);
            m_Rigidbody2D.AddForce(Vector2.up * m_JumpForce, ForceMode2D.Impulse);
            //m_Rigidbody2D.AddForce(new Vector2(0, verticalMove * m_JumpForce), ForceMode2D.Impulse);
            _jump = false;
    }
    public void Flip()
    {
        m_FacingRight = !m_FacingRight;

        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
    private void Attack()
    {
        m_Animator.SetTrigger("Attack");

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, _attackRange, enemyLayer);
        foreach(Collider2D enemy in hitEnemies)
        {
            //Debug.Log("We hit"+ enemy.name);
            enemy.GetComponent<EnemyMovement>().TakeDamage(_attackDamage);
        }
    }
    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
        {
            return;
        }
        Gizmos.DrawWireSphere(attackPoint.position, _attackRange);
    }
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Enemy")
        {
           int enemy_damage = other.gameObject.GetComponent<EnemyMovement>().e_attackDamage;
            other.gameObject.GetComponent<EnemyMovement>().EnemyAttack();
            TakeDamage(enemy_damage);
        }
    }
    public void TakeDamage(int damage)
    {
        if(CurrentPlayerHP > 0)
        {
            CurrentPlayerHP = CurrentPlayerHP - damage;
            m_Animator.SetTrigger("Damage");
            healthBar.SetHealth(CurrentPlayerHP);
            Debug.Log("Damage" + damage);
        }

        if (CurrentPlayerHP <= 0 && !_isDie)
        {
            _isDie = true;
            StartCoroutine("Die");
        }
    }
    IEnumerator Die()
    {
        yield return new WaitForSeconds(1.5f);
        m_Animator.SetTrigger("Dead");
        Debug.Log("гг умирает");
        FinMenu();
    }
    public void FinMenu()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 2);
    }
}
