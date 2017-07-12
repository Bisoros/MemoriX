using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Pause : MonoBehaviour
{
    public GameObject overlay, menuButton;

	public void resume()
    {
        Time.timeScale = 1;
        overlay.SetActive(false);
        menuButton.SetActive(true);
    }

    public void restart()
    {
        PlayerPrefs.SetInt("difficulty", 0);
        PlayerPrefs.SetInt("difficultySimon", 0);
        PlayerPrefs.SetInt("difficultyMP", 0);
        PlayerPrefs.Save();
        Application.LoadLevel(Application.loadedLevel);
    }

    public void menu()
    {
        PlayerPrefs.SetInt("difficulty", 0);
        PlayerPrefs.SetInt("difficultySimon", 0);
        PlayerPrefs.SetInt("difficultyMP", 0);
        PlayerPrefs.Save();
        SceneManager.LoadScene(0);
    }

    public void pause()
    {
        if (Time.timeScale == 1)
        {
            overlay.SetActive(true);
            menuButton.SetActive(false);
            Time.timeScale = 0;
        }
    }
}
