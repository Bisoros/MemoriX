using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TouchMP : MonoBehaviour
{
	private Color last = Color.clear, crt;
    private int nrFound = 0;
    private float time;
    public GameObject endScreen, menu, crtLevel;
    public Text avarageTime, level;

	private IEnumerator end()
    {
        //giving end feedback
        CreatorMP.ready = false;
        GameObject[] cubes = GameObject.FindGameObjectsWithTag("1");

        foreach (GameObject cube in cubes)
            cube.GetComponent<MeshRenderer>().material.color = CreatorMP.colours[System.Int32.Parse(cube.name)];

        time = Time.timeSinceLevelLoad - CreatorMP.crtTime;

        //win
        if (nrFound == CreatorMP.nrPairs * 2)
        {
            PlayerPrefs.SetFloat("avarageTimeMP", ((PlayerPrefs.GetInt("difficultyMP") - 1) * PlayerPrefs.GetFloat("avarageTimeMP") + time) / (float)PlayerPrefs.GetInt("difficultyMP"));
            yield return new WaitForSeconds(1);
            Application.LoadLevel(Application.loadedLevel);
        }
        //lose
        else
        {
            level.text = "";
			avarageTime.text = "Average Time/Completed Puzzle: " + (PlayerPrefs.GetFloat("avarageTimeMP") == 0 ? "N/A" : PlayerPrefs.GetFloat("avarageTimeMP").ToString("0.00"));
            PlayerPrefs.SetInt("highscoreMP", Mathf.Max(PlayerPrefs.GetInt("highscoreMP"), PlayerPrefs.GetInt("difficultyMP")));
            level.text = "Level Reached: " + PlayerPrefs.GetInt("difficultyMP").ToString() + " (Highscore: " + PlayerPrefs.GetInt("highscoreMP").ToString() + ")";
            yield return new WaitForSeconds(1);
            menu.SetActive(false);
            crtLevel.SetActive(false);
            Time.timeScale = 0;
            endScreen.SetActive(true);
            PlayerPrefs.SetInt("difficultyMP", 0);
            PlayerPrefs.SetFloat("avarageTimeMP", 0);
            PlayerPrefs.Save();
        }
    }

	private void Update ()
    {
        if (Time.timeScale == 0)
            return;

        //getting input
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hitInfo;

        if (Physics.Raycast(ray, out hitInfo))
        {
            if (Input.GetMouseButtonDown(0) && CreatorMP.ready && hitInfo.collider.CompareTag("1") && hitInfo.collider.GetComponent<MeshRenderer>().material.color == Color.white)
            {
                crt = hitInfo.collider.GetComponent<MeshRenderer>().material.color = CreatorMP.colours[Int32.Parse(hitInfo.collider.name)];

                //processing input
                if (last != Color.clear)
                {
                    if (crt != last)
                        StartCoroutine(end());
                    else
                        last = Color.clear;
                }
                else
                    last = crt;

                nrFound++;

                if (nrFound == CreatorMP.nrPairs * 2)
                    StartCoroutine(end());
            }
        }
    }

}
