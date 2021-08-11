using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    private SpriteRenderer render_Enemy;
    Animator e_Animator;
    public float _speed;
    public Rigidbody2D rb_Enemy;
    private Vector2 _movement;
    public int _maxHP;
    private int _currentHP;
    private bool _isDie=false;

    public Transform targetGameObject;
    public float targetDistance = -1;
    public LayerMask targetLayer;

    public int e_attackDamage;
    private bool e_isAttack = false;

    private void Start()
    {
        render_Enemy = GetComponent<SpriteRenderer>();
        e_Animator = GetComponent<Animator>();
        rb_Enemy = GetComponent<Rigidbody2D>();
    }
    void Awake()
    {
        _movement = new Vector2(_speed, 0);
        _currentHP = _maxHP;
    }
    private void FixedUpdate()
    {
        if (!_isDie)
        {
            if (!e_isAttack)
            {
                Move();
            }

            Vector2 direction = targetGameObject.position;

            /*RaycastHit2D hit = Physics2D.Raycast(rb_Enemy.position, direction, targetDistance, targetLayer);
            if (hit.collider != null)
            {
                //Debug.Log("Attack is" + hit.collider.name);
            }*/
            RaycastHit2D hit_back = Physics2D.Raycast(rb_Enemy.position, direction, -targetDistance, targetLayer);

            if(hit_back.collider != null)
            {
                Flip();
            }

            Debug.DrawRay(rb_Enemy.position, direction, Color.red);
        }
    }
        private void Move()
    {
        rb_Enemy.AddForce(_movement);
        e_Animator.SetBool("IsWalking",true);
    }
    public void Flip()
    {
        rb_Enemy.velocity = Vector2.zero;
        /*Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;*/
        _movement = -_movement;
        render_Enemy.flipX = ! render_Enemy.flipX;
        targetDistance = -targetDistance;
    }
    public void EnemyAttack()
    {
            rb_Enemy.velocity = Vector2.zero;
            e_Animator.SetBool("IsWalking", false);
            e_Animator.SetTrigger("Attack");
    }
    public void TakeDamage(int damage)
    {
        e_Animator.SetTrigger("Hurt");
        _currentHP -= damage;

        if (_currentHP <= 0 && !_isDie)
        {
            rb_Enemy.velocity = Vector2.zero;
            e_Animator.SetTrigger("Dead");
            _isDie = true;
            StartCoroutine("Die");
        }
    }
    /*private void Die()
    {
        
    }*/
    IEnumerator Die()
    {
        yield return new WaitForSeconds(1.5f);

        this.enabled = false;
        //etComponent<Collider2D>().enabled = false;
        Destroy(gameObject,1.1f);
    }
}
