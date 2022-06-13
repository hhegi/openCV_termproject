using System.Collections;
using UnityEngine;

public class MobController : MonoBehaviour
{
    private Rigidbody2D rigidbody2D;
    private Animator animator;
    private Vector2 range;
    private Transform target;
    private readonly float speed = .4f;
    private readonly int m_facingDirection = 1;
    private readonly float m_delayToIdle = 0.0f;
    private bool isAttack = false;
    private Coroutine coroutine;
    private SpriteRenderer render;
    private Animator animator2;
    private CharController charcontorller = new CharController();
    private int currentHealth = 10;
    private BoxCollider2D boxcollider2D;

    private void Start()
    {
        target = GameObject.Find("Warrior").GetComponent<Transform>();
        rigidbody2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        animator2 = target.GetComponent<Animator>();
        boxcollider2D = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        Target();
        Dead();
    }

    private void Target()
    {
        if (Vector2.Distance(transform.position, target.position) <= 1.5f && Vector2.Distance(transform.position, target.position) > 1.3f)
        {
            transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
            animator.SetInteger("AnimState", 1);
        }
        else if (Vector2.Distance(transform.position, target.position) <= 1.3f && Vector2.Distance(transform.position, target.position) > 1.0f)
        {
            rigidbody2D.velocity = Vector2.zero;
            animator.SetInteger("AnimState", 0);
        }
        else if (Vector2.Distance(transform.position, target.position) <= 1.0f && coroutine == null)
        {
            rigidbody2D.velocity = Vector2.zero;
            isAttack = true;
            animator.SetInteger("AnimState", 0);

            animator.SetTrigger("Attack" + Random.Range(1, 3));
            coroutine = StartCoroutine(Delay(5f));
        }
        else if (Vector2.Distance(transform.position, target.position) >= 1.5f)
        {
            rigidbody2D.velocity = Vector2.zero;
            animator.SetInteger("AnimState", 0);
        }
    }

    private IEnumerator Delay(float x)
    {
        yield return new WaitForSeconds(x);
        isAttack = false;
        coroutine = null;
    }

    public void TakeDamage(int damage)
    {
        currentHealth = currentHealth - damage;
        animator.SetTrigger("Hit");
        coroutine = StartCoroutine(Delay(1f));
    }

    private void Dead()
    {
        if (currentHealth <= 0)
        {
            animator.SetTrigger("Dead");
            coroutine = StartCoroutine(DeadDelay(1f));
        }
    }

    private IEnumerator DeadDelay(float x)
    {
        yield return new WaitForSeconds(x);
        Destroy(gameObject);
    }
}