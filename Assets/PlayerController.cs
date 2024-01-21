using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float jumpForce = 3f;
    public Rigidbody2D rb;
    public GameManager gameManager;

    private AudioManager audioManager;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        audioManager = AudioManager.GetInstance();

    }

    void Update()
    {
        if (!gameManager.isGameRunning)
        {
            return;
        }
        // Check if the player is no longer visible in the camera's view
        if (!IsVisibleInCamera())
        {
            // Player is out of the screen, trigger game over
            gameManager.GameOver();
        }
        // Check for keyboard, mouse, or touch input to jump
        if (Input.GetButtonDown("Jump") || Input.GetMouseButtonDown(0) || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began))
        {
            Jump();
        }

        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.identity, Time.deltaTime * 5f);

    }

    void Jump()
    {
        audioManager.PlaySound("wing");
        // Rotate the player when jumping
        transform.rotation = Quaternion.Euler(0f, 0f, 30f);

        // Apply jump force
        rb.velocity = Vector2.up * jumpForce;
    }


    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Pipe"))
        {
            audioManager.PlaySound("hit");
            gameManager.GameOver();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("ScoreField"))
        {
            gameManager.AddScore();
            Debug.Log("added score");
        }
    }

    bool IsVisibleInCamera()
    {
        Vector3 viewportPos = Camera.main.WorldToViewportPoint(transform.position);
        return viewportPos.x > 0 && viewportPos.x < 1 && viewportPos.y > 0 && viewportPos.y < 1;
    }
}
