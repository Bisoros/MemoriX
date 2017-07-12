using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Buttons : MonoBehaviour
{
    public GameObject helpImg, canvas;

    public void memoryMatrix ()
    {
        SceneManager.LoadScene("Memory Matrix");
    }

    public void simon ()
    {
        SceneManager.LoadScene("Simon");
    }

    public void matchPairs ()
    {
        SceneManager.LoadScene("Match Pairs");
    }

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
