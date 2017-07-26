using UnityEngine;
using UnityEngine.SceneManagement;

public class Buttons : MonoBehaviour
{
    public GameObject helpImg, canvas;

    //gamemode selection
    public void memoryMatrix ()
    {
        SceneManager.LoadScene("Memory Matrix");
        PlayerPrefs.SetInt("difficulty", 0);
    }

    public void simon ()
    {
        SceneManager.LoadScene("Simon");
        PlayerPrefs.SetInt("difficultySimon", 0);
    }

    public void matchPairs ()
    {
        SceneManager.LoadScene("Match Pairs");
        PlayerPrefs.SetInt("difficultyMP", 0);
    }

    //menu buttons
    public void quit ()
    {
        Application.Quit();
    }

    public void help ()
    {
        helpImg.SetActive(true);
        canvas.SetActive(false);
    }

    public void close ()
    {
        helpImg.SetActive(false);
        canvas.SetActive(true);
    }
}
