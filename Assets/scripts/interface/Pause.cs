using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Pause : MonoBehaviour
{
    // Has the game been paused?
    private bool isPaused = true;
    // Has the game gone through it's pause transition?
    private bool hasPaused = true;

    public Text text;

    public void Start()
    {
        // Default to not paused.
        this.isPaused = true;
        text = GetComponentInChildren<Text>();
    }

    public void ClickOn()
    {
        changePauseState();
        // If you're paused
        if (this.isPaused)
        {
            Debug.Log("Click on pause");
            text.text = "Start";
            Camera.main.GetComponent<AudioSource>().Pause();
            // And nothing has initialized
            if (!this.hasPaused)
            {
                // Pause the game
                this.pauseGame();
                this.hasPaused = true;
            }
            // GUI changes
        }
        else
        {
            if (this.hasPaused)
            {
                Debug.Log("Click on start");
                text.text = "Pause";
                Camera.main.GetComponent<AudioSource>().Play();
                this.resumeGame();
                this.hasPaused = false;
            }
        }
    }

    public void pauseGame()
    {
        // Pause the game
        Time.timeScale = 0;
        // Any other logic
    }

    public void resumeGame()
    {
        // Resume the game
        Time.timeScale = 1;
        // Any other logic
    }

    public void changePauseState()
    {
        // Alternate bool value per button click
        this.isPaused = !this.isPaused;
    }
}