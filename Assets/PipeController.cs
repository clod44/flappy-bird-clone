using UnityEngine;

public class PipeController : MonoBehaviour
{
    private float speed;
    public GameManager gameManager;
    public void SetSpeed(float newSpeed)
    {
        speed = newSpeed;
    }

    void Update()
    {
        if (gameManager.isGameRunning)
        {

            // Move the pipe to the left
            transform.Translate(Vector2.left * speed * Time.deltaTime);

            // Destroy the pipe when it moves off-screen
            if (transform.position.x < -10f)
            {
                Destroy(gameObject);
            }
        }
    }
}
