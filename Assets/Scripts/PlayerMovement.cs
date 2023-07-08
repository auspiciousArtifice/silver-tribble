using System;
using System.Net.NetworkInformation;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public int jumpCount;
    public int dashCount;

    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask wallLayer;
    [SerializeField] private float speed;
    [SerializeField] private float maxJumpSpeed;
    [SerializeField] private float maxFallSpeed;
    [SerializeField] private float dashSpeed;
    [SerializeField] private float jumpForce;
    [SerializeField] private float jumpTime;
    [SerializeField] private float dashTime;
    [SerializeField] private bool autoJump;
    private float jumpTimeCounter;
    private float dashTimeCounter;
    private float gravScale;
    private int jumpCounter;
    private int dashCounter;
    private bool grounded = false;
    private bool stoppedJumping;
    private bool stoppedDashing;
    private Rigidbody2D body;
    private Animator animator;
    private BoxCollider2D collider;


    private void Awake()
    {
        // Get refs for rigidbody and animator
        body = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        collider = GetComponent<BoxCollider2D>();
        // Save gravity scale, so it isn't hardcoded everywhere!
        gravScale = body.gravityScale;
    }
    // Start is called before the first frame update
    void Start()
    {
        jumpTimeCounter = jumpTime;
        dashTimeCounter = dashTime;
        jumpCounter = jumpCount;
        dashCounter = dashCount;
        stoppedJumping = true;
        stoppedDashing = true;
    }

    // Update is called once per frame
    void Update()
    {
        // Flip player sprite depending on horizontal velocity
        if (body.velocity.x > 0.01f)
        {
            transform.localScale = new Vector3(4, 4, 1);
        }
        else if (body.velocity.x < -0.01f)
        {
            transform.localScale = new Vector3(-4, 4, 1);
        }

        // Set animator params
        animator.SetBool("Running", Input.GetAxis("Horizontal") != 0);
        animator.SetBool("Grounded", isGrounded());
        animator.SetBool("Rising", body.velocity.y > 0);
        animator.SetBool("Falling", body.velocity.y < 0);

        JumpLogic();
        DashLogic();
    }

    void FixedUpdate()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float inputBasedSpeed = horizontalInput * speed;
        int xDirection = transform.localScale.x > 0 ? 1 : -1;   // Which direction the player is facing
        // Input is opposite of whichever direction the player is facing
        bool opposingDirections = xDirection > 0 && horizontalInput < 0 || xDirection < 0 && horizontalInput > 0;

        // Dashing prevents horizontal movement
        if (stoppedDashing && horizontalInput != 0)
        {
            // When dashing, you don't want to slow down when pressing the same direction as the dash
            // This value prevents unexpected behavior while keeping the snappy movement if you decide to move in the opposite direction after a dash
            float biggerVelocity = MaximumMagnitude(inputBasedSpeed, body.velocity.x);

            // Cancel dash momentum if moving in the opposite direction, else keep momentum
            if (opposingDirections)
            {
                body.velocity = new Vector2(inputBasedSpeed, body.velocity.y);
            }
            else
            {
                body.velocity = new Vector2(biggerVelocity, body.velocity.y);
            }
        }

        // Apply jumping force so long as timer is greater than 0 and space is held down
        if (!stoppedJumping && jumpTimeCounter > 0)
        {
            body.velocity = new Vector2(body.velocity.x, jumpForce);
            jumpTimeCounter -= Time.deltaTime;
        }

        if (!stoppedDashing && dashTimeCounter > 0)
        {
            body.velocity = new Vector2(dashSpeed * xDirection, 0);
            dashTimeCounter -= Time.deltaTime;
        }

        //// Limit horizontal velocity if stopped dashing and trying to move horizontally
        
        //if (stoppedDashing && opposingDirections)
        //{
        //    body.AddForce(new Vector2(horizontalInput * speed / 4, 0));
        //}
        
        // Limit vertical velocity according to parameters
        body.velocity = new Vector2(body.velocity.x, Mathf.Clamp(body.velocity.y, -maxFallSpeed, maxJumpSpeed));
    }

    private void JumpLogic()
    {
        // Reset jump stats when grounded
        if (isGrounded() && (!Input.GetKey(KeyCode.Space) || autoJump))
        {
            jumpCounter = jumpCount;
            jumpTimeCounter = jumpTime;
        }

        // Jump when either grounded or if you have a double jump stored
        if (Input.GetKeyDown(KeyCode.Space) && stoppedJumping && (isGrounded() || jumpCounter > 0))
        {
            stoppedJumping = false;
            jumpTimeCounter = jumpTime;
            jumpCounter -= isGrounded() ? 0 : 1;    // Jump from the ground doesn't decrement jumpCounter since you want to keep a jump for when you walk off a ledge
            if (!isGrounded())
            {
                animator.SetTrigger("DoubleJump");
            }
            StopDash();                         // Re-engage gravity but keep dash momentum
        }
        else if (Input.GetKeyUp(KeyCode.Space) || !stoppedJumping && jumpTimeCounter <= 0)
        {
            // Stop jumping force
            StopJump();
        }
    }

    private void DashLogic()
    {
        // Reset dash when touching ground
        if (isGrounded())
        {
            dashTimeCounter = dashTime;
            dashCounter = dashCount;
        }

        // Dash when in the air, not currently dashing, and dash is available
        if (Input.GetKeyDown(KeyCode.LeftShift) && !isGrounded() && stoppedDashing && dashCounter > 0)
        {
            stoppedDashing = false;
            dashCounter--;
            dashTimeCounter = dashTime;
            body.gravityScale = 0;  // Kill gravity
            StopJump();             // Stop jumping force from being applied
        }

        if (!stoppedDashing && dashTimeCounter <= 0)
        {
            StopDash();
        }
    }

    private void StopJump()
    {
        jumpTimeCounter = 0;
        stoppedJumping = true;
    }

    private void StopDash()
    {
        dashTimeCounter = 0;
        stoppedDashing = true;
        body.gravityScale = gravScale;
    }

    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //    if (collision.gameObject.CompareTag("Ground"))
    //    {
    //        grounded = true;
    //    }
    //}

    //private void OnCollisionExit2D(Collision2D collision)
    //{
    //    if (collision.gameObject.CompareTag("Ground"))
    //    {
    //        grounded = false;
    //    }
    //}

    private bool isGrounded()
    {
        RaycastHit2D hit = Physics2D.BoxCast(collider.bounds.center, collider.bounds.size, 0, Vector2.down, 0.1f, groundLayer);
        return hit.collider != null;
    }

    private bool onWall()
    {
        RaycastHit2D hit = Physics2D.BoxCast(collider.bounds.center, collider.bounds.size, 0, new Vector2(transform.localScale.x, 0), 0.1f, wallLayer);
        return hit.collider != null;
    }

    private float MaximumMagnitude(float a, float b)
    {
        if (Mathf.Abs(a) > Mathf.Abs(b))
        {
            return a;
        }

        return b;
    }
}
