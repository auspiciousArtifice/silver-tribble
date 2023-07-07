using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] private float speed;
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
        
    }

    // Update is called once per frame
    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        body.velocity = new Vector2(horizontalInput * speed, body.velocity.y);

        // Flip player sprite depending on input
        transform.localScale = horizontalInput > 0.01f ? new Vector3(4, 4, 1) : new Vector3(-4, 4, 1);

        if (Input.GetKey(KeyCode.Space))
        {
            body.velocity = new Vector2(body.velocity.x, 1);
        }

        // Set animator params
        animator.SetBool("Running", horizontalInput != 0);
    }
}
