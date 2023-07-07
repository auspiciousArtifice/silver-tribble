using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float maxJumpSpeed;
    [SerializeField] private float maxFallSpeed;
    //[SerializeField] private float jumpHeight;
    [SerializeField] private float jumpForce;
    [SerializeField] private float jumpTime;
    private float jumpTimeCounter;
    private bool grounded = false;
    private bool stoppedJumping;
    private Rigidbody2D body;
    private Animator animator;


    private void Awake()
    {
        // Get refs for rigidbody and animator
        body = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }
    // Start is called before the first frame update
    void Start()
    {
        jumpTimeCounter = jumpTime;
    }

    // Update is called once per frame
    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        body.velocity = new Vector2(horizontalInput * speed, body.velocity.y);

        // Flip player sprite depending on input
        if (horizontalInput > 0.01f)
        {
            transform.localScale = new Vector3(4, 4, 1);
        }
        else if (horizontalInput < -0.01f)
        {
            transform.localScale = new Vector3(-4, 4, 1);
        }

        if (grounded)
        {
            jumpTimeCounter = jumpTime;
        }

        // Set animator params
        animator.SetBool("Running", horizontalInput != 0);
        animator.SetBool("Grounded", grounded);
    }

    void FixedUpdate()
    {
        // If spacebar is let go
        if (!Input.GetKey(KeyCode.Space))
        {
            // Stop jumping and set jump counter to zero. Counter will reset once grounded.
            jumpTimeCounter = 0;
            stoppedJumping = true;
        }

        // If pressing space and grounded
        if (Input.GetKey(KeyCode.Space) && grounded)
        {
            body.velocity = new Vector2(body.velocity.x, jumpForce);
            stoppedJumping = false;
            animator.SetTrigger("Jump");
        }

        // If holding space while jumping and jumping time not yet reached
        if (Input.GetKey(KeyCode.Space) && !stoppedJumping && jumpTimeCounter > 0) 
        {
            body.velocity = new Vector2(body.velocity.x, jumpForce);
            jumpTimeCounter -= Time.deltaTime;
        }

        body.velocity = new Vector2(body.velocity.x, Mathf.Clamp(body.velocity.y, -maxFallSpeed, maxJumpSpeed));
    }

    //private void Jump()
    //{
        
    //    grounded = false;
    //    animator.SetTrigger("Jump");
    //}

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            grounded = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            grounded = false;
        }
    }
}
