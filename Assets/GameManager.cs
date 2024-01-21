using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public HudController hud;
    public float gameSpeed = 1f;
    public GameObject pipePrefab;
    public float spawnDelay = 2f;
    public float pipeDistance = 2f;
    public int score = 0;
    public CanvasGroup gameOverPanel;
    public CanvasGroup startGamePanel;
    public bool isGameRunning = false;
    public PlayerController player;
    public VolumeExpoLerper volume;
    private AudioManager audioManager;
    public Transform background;
    void Start()
    {
        audioManager = AudioManager.GetInstance();
        volume.ChangeFromTo(-30f, 0f, 1f);
        // Start generating pipes
        InvokeRepeating("SpawnPipe", 0f, spawnDelay);
    }

    void Update()
    {
        if (isGameRunning)
        {
            spawnDelay -= 0.05f * Time.deltaTime;
            background.transform.position = new Vector3((background.transform.position.x + -0.5f * Time.deltaTime) % 2.88f,
            background.transform.position.y,
            background.transform.position.z);
            // Move the background (you can use your existing MoveBackground logic)

            // You may add other game-related logic here
        }
    }

    void SpawnPipe()
    {
        if (isGameRunning)
        {
            // Instantiate a new pipe at a specified distance from the previous one
            Vector3 spawnPosition = new Vector3(3f, Random.Range(-1.5f, 1.5f), 0f);
            GameObject pipeInstance = Instantiate(pipePrefab, spawnPosition, Quaternion.identity);

            // Adjust the speed of the pipe based on the game speed
            PipeController pipeController = pipeInstance.GetComponent<PipeController>();
            if (pipeController != null)
            {
                pipeController.SetSpeed(gameSpeed);
                pipeController.gameManager = this;
            }
        }
    }

    public void GameOver()
    {
        if (isGameRunning)
        {

            player.GetComponent<Animator>().enabled = false;

            // Continue animation when the game is running
            // Set game over state
            isGameRunning = false;
            player.rb.bodyType = RigidbodyType2D.Static;

            // Add your game over logic here (e.g., show a game over screen)
            Debug.Log("Game Over!");
            gameOverPanel.alpha = 1f;
            gameOverPanel.blocksRaycasts = true;
            gameOverPanel.interactable = true;

            // Wait for 1.5 seconds before reloading the level
            // Reload the level
            volume.ChangeFromTo(0f, 0f, 1f, () =>
            {
                audioManager.PlaySound("die");
                volume.ChangeFromTo(0f, -30f, 1f, () =>
                {
                    RestartLevel();
                });
            });
        }
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void AddScore()
    {
        score++;
        audioManager.PlaySound("point");
        hud.UpdateScore(score);
    }

    public void StartGame()
    {
        startGamePanel.alpha = 0f;
        startGamePanel.interactable = false;
        startGamePanel.blocksRaycasts = false;
        player.rb.bodyType = RigidbodyType2D.Dynamic;
        isGameRunning = true;
    }

}
