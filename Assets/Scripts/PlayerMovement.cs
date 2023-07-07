using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float maxJumpSpeed;
    [SerializeField] private float maxFallSpeed;
    //[SerializeField] private float jumpHeight;
    [SerializeField] private float jumpForce;
    [SerializeField] private float jumpTime;
    [SerializeField] private int jumpCount;
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

        if (grounded)
        {
            jumpCounter = jumpCount;
            jumpTimeCounter = jumpTime;
        }

        if (Input.GetKeyDown(KeyCode.Space) && stoppedJumping && jumpCounter > 0)
        {
            stoppedJumping = false;
            jumpTimeCounter = jumpTime;
            jumpCounter -= grounded ? 0 : 1;
        }
        else if (Input.GetKeyUp(KeyCode.Space))
        {
            jumpTimeCounter = 0;
            stoppedJumping = true;
        }
    }

    void FixedUpdate()
    {
        //// If spacebar is let go
        //if (Input.GetKeyUp(KeyCode.Space))
        //{
        //    // Stop jumping and set jump counter to zero. Counter will reset once grounded.
        //    jumpTimeCounter = 0;
        //    stoppedJumping = true;
        //}

        //// If pressing space and have jumps left
        //if (Input.GetKeyDown(KeyCode.Space) && stoppedJumping && jumpCounter > 0)
        //{
        //    body.velocity = new Vector2(body.velocity.x, jumpForce);
        //    stoppedJumping = false;
        //    jumpTimeCounter = jumpTime;
        //    jumpCounter--;
        //    animator.SetTrigger("Jump");
        //}

        //// If holding space while jumping and jumping time not yet reached
        //if (Input.GetKey(KeyCode.Space) && !stoppedJumping && jumpTimeCounter > 0) 
        //{
        //    body.velocity = new Vector2(body.velocity.x, jumpForce);
        //    jumpTimeCounter -= Time.deltaTime;
        //}

        if (!stoppedJumping && jumpTimeCounter > 0)
        {
            body.velocity = new Vector2(body.velocity.x, jumpForce);
            jumpTimeCounter -= Time.deltaTime;
        }

        body.velocity = new Vector2(body.velocity.x, Mathf.Clamp(body.velocity.y, -maxFallSpeed, maxJumpSpeed));
    }

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
