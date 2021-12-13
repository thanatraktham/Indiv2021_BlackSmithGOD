using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    public Button playButton;
    public Button quitButton;
    public Button resumeButton;
    public Button menuButton;

    public GameObject menuCanvas;
    public GameObject pauseCanvas;

    public GameManager gm;
    // Start is called before the first frame update
    void Start()
    {
        // PauseGame();

        playButton.onClick.AddListener(delegate{
            ResumeGame();
            menuCanvas.SetActive(false);
        });
        quitButton.onClick.AddListener(delegate{
            Application.Quit();
        });
        resumeButton.onClick.AddListener(delegate{
            ResumeGame();
        });
        menuButton.onClick.AddListener(delegate{
            Time.timeScale = 0;
            gm.SwapSceen("StartingScene");
        });
    }

    public void PauseGame ()
    {
        pauseCanvas.SetActive(true);
        Time.timeScale = 0;
    }

    public void ResumeGame ()
    {
        pauseCanvas.SetActive(false);
        Time.timeScale = 1;
    }

}
