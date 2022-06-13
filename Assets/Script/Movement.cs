using UnityEngine;

public class Movement : MonoBehaviour
{
    private Vector2 footPosition;
    private Rigidbody2D rigidbody2d;
    private BoxCollider2D boxCollider2D;
    private Vector3 Direction;

    private readonly float Speed = 2.0f;
    private readonly float jumpforce = 3.0f;
    public bool isGrounded;

    public bool isLongJump = false;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        isGrounded = true;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        isGrounded = false;
    }

    private void Awake()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        boxCollider2D = GetComponent<BoxCollider2D>();
    }

    private void FixedUpdate()
    {
        Bounds bounds = boxCollider2D.bounds;
        footPosition = new Vector2(bounds.center.x, bounds.min.y);

        if (isLongJump && rigidbody2d.velocity.y > 0)
        {
            rigidbody2d.gravityScale = 1.0f;
        }
        else
        {
            rigidbody2d.gravityScale = 2.5f;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(footPosition, 0.2f);
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
}