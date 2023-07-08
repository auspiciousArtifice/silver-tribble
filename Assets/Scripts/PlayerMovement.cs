using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float maxJumpSpeed;
    [SerializeField] private float maxFallSpeed;
    [SerializeField] private float jumpForce;
    [SerializeField] private float jumpTime;
    [SerializeField] private int jumpCount;
    [SerializeField] private bool autoJump;
    private float jumpTimeCounter;
    private int jumpCounter;
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
        jumpCounter = jumpCount;
        stoppedJumping = true;
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

        // Set animator params
        animator.SetBool("Running", horizontalInput != 0);
        animator.SetBool("Grounded", grounded);

        // Reset jump stats when grounded
        if (grounded && (!Input.GetKey(KeyCode.Space) || autoJump))
        {
            jumpCounter = jumpCount;
            jumpTimeCounter = jumpTime;
        }

        // Jump when either grounded or if you have a double jump stored
        if (Input.GetKeyDown(KeyCode.Space) && stoppedJumping && jumpCounter > 0)
        {
            stoppedJumping = false;
            jumpTimeCounter = jumpTime;
            jumpCounter -= grounded ? 0 : 1;
        }
        else if (Input.GetKeyUp(KeyCode.Space))
        {
            // Stop jumping force
            jumpTimeCounter = 0;
            stoppedJumping = true;
        }
    }

    void FixedUpdate()
    {
        // Apply jumping force so long as timer is greater than 0 and space is held down
        if (!stoppedJumping && jumpTimeCounter > 0)
        {
            body.velocity = new Vector2(body.velocity.x, jumpForce);
            jumpTimeCounter -= Time.deltaTime;
        }

        // Limit vertical velocity according to parameters
        body.velocity = new Vector2(body.velocity.x, Mathf.Clamp(body.velocity.y, -maxFallSpeed, maxJumpSpeed));
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            grounded = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            grounded = false;
        }
    }
}
