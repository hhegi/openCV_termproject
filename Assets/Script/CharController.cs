using System.Collections;
using UnityEngine;

public class CharController : MonoBehaviour
{
    private readonly KeyCode jumpKeyCode = KeyCode.C;
    private readonly KeyCode attackKeyCode = KeyCode.X;
    private readonly KeyCode blockKeyCode = KeyCode.Z;
    private readonly Movement movement;
    private Animator animator;
    private int m_facingDirection = 1;
    private float m_delayToIdle = 0.0f;
    private Vector2 footPosition;
    private Rigidbody2D rigidbody2d;
    private BoxCollider2D boxCollider2D;
    private Vector3 Direction;
    private readonly float Speed = 1.0f;
    private readonly float jumpforce = 3.0f;
    public int CurrentAttack;
    public bool isAttack = false;
    public bool isGrounded = false;
    public bool isLongJump = false;
    private int maxHealth = 5, currentHealth = 5;
    private GameObject atkrange;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        isGrounded = true;
    }

    private void OnCollisionStay(Collision collision)
    {
        isGrounded = true;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        isGrounded = false;
    }

    private void Start()
    {
        animator = GetComponent<Animator>();
        rigidbody2d = GetComponent<Rigidbody2D>();
        boxCollider2D = GetComponent<BoxCollider2D>();
        atkrange = transform.Find("AtkRange").gameObject;
        atkrange.SetActive(false);
    }

    private void Update()
    {
        MoveByKeyInput();
        JumpByKeyInput();
        AttackByKeyInput();
        BlockByKeyInput();
        Bounds bounds = boxCollider2D.bounds;

        if (isLongJump && rigidbody2d.velocity.y > 0)
        {
            rigidbody2d.gravityScale = 1.0f;
        }
        else
        {
            rigidbody2d.gravityScale = 2.5f;
        }
    }

    public void Move(float x)
    {
        rigidbody2d.velocity = new Vector2(x * Speed, rigidbody2d.velocity.y);
    }

    public bool Jump()
    {
        if (isGrounded == true)
        {
            rigidbody2d.velocity = Vector2.up * jumpforce;

            return true;
        }
        return false;
    }

    private void MoveByKeyInput()
    {
        float x = Input.GetAxisRaw("Horizontal");
        if (!isAttack)
            Move(x);
        if (x > 0)
        {
            GetComponent<SpriteRenderer>().flipX = false;
            m_facingDirection = 1;
        }
        else if (x < 0)
        {
            GetComponent<SpriteRenderer>().flipX = true;
            m_facingDirection = -1;
        }
        if (Mathf.Abs(x) > Mathf.Epsilon)
        {
            m_delayToIdle = 0.05f;
            animator.SetInteger("AnimState", 1);
        }
        else
        {
            m_delayToIdle -= Time.deltaTime;
            if (m_delayToIdle < 0)
            {
                animator.SetInteger("AnimState", 0);
            }
        }
    }

    private void JumpByKeyInput()
    {
        if (Input.GetKeyDown(jumpKeyCode))
        {
            animator.SetTrigger("Jump");
            bool isJump = Jump();

            if (isJump == true)
            {
                isGrounded = false;
            }
        }
        if (Input.GetKey(jumpKeyCode))
        {
            isLongJump = true;
            rigidbody2d.gravityScale = 1.5f;
        }
        else if (Input.GetKeyUp(jumpKeyCode))
        {
            isLongJump = false;
            rigidbody2d.gravityScale = 2.5f;
        }
    }

    private void AttackByKeyInput()
    {
        if (Input.GetKeyDown(attackKeyCode) && isAttack == false)
        {
            isAttack = true;
            animator.SetTrigger("Attack" + Random.Range(1, 4));
            atkrange.SetActive(true);
            StartCoroutine(Delay());
        }
    }

    private IEnumerator Delay()
    {
        yield return new WaitForSeconds(1f);
        isAttack = false;
        atkrange.SetActive(false);
    }

    private void BlockByKeyInput()
    {
        if (Input.GetKeyDown(blockKeyCode) && isAttack == false)
        {
            animator.SetTrigger("Block");
            animator.SetBool("IdleBlock", true);
        }
        else if (Input.GetKeyUp(blockKeyCode))
            animator.SetBool("IdleBlock", false);
    }

    public void TakeDamage(int damage)
    {
        currentHealth = currentHealth - damage;
        animator.SetTrigger("Hurt");
    }
}