using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TouchMP : MonoBehaviour
{
	private Color last = Color.clear, crt;
    private int nrFound = 0;
    private float time;
    public GameObject endScreen, menu;
    public Text avarageTime, level;

	private IEnumerator end()
    {
        CreatorMP.ready = false;
        GameObject[] cubes = GameObject.FindGameObjectsWithTag("1");

        foreach (GameObject cube in cubes)
            cube.GetComponent<MeshRenderer>().material.color = CreatorMP.colours[System.Int32.Parse(cube.name)];

        time = Time.timeSinceLevelLoad - CreatorMP.crtTime;

        if (nrFound == CreatorMP.nrPairs * 2)
        {
            PlayerPrefs.SetFloat("avarageTimeMP", ((PlayerPrefs.GetInt("difficultyMP") - 1) * PlayerPrefs.GetFloat("avarageTimeMP") + time) / (float)PlayerPrefs.GetInt("difficultyMP"));
            Debug.Log("win " + time);
            yield return new WaitForSeconds(1);
            Application.LoadLevel(Application.loadedLevel);
        }
        else
        {
            Debug.Log("lose" + time);
            menu.SetActive(false);
			avarageTime.text = "Average Time/Completed Puzzle: " + PlayerPrefs.GetFloat("avarageTimeMP").ToString();
            PlayerPrefs.SetInt("highscoreMP", Mathf.Max(PlayerPrefs.GetInt("highscoreMP"), PlayerPrefs.GetInt("difficultyMP")));
            level.text = "Level Reached: " + PlayerPrefs.GetInt("difficultyMP").ToString() + " (Highscore: " + PlayerPrefs.GetInt("highscoreMP").ToString() + ")";
            yield return new WaitForSeconds(1);
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

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hitInfo;

        if (Physics.Raycast(ray, out hitInfo))
        {
            if (Input.GetMouseButtonDown(0) && CreatorMP.ready && hitInfo.collider.CompareTag("1") && hitInfo.collider.GetComponent<MeshRenderer>().material.color == Color.white)
            {
                crt = hitInfo.collider.GetComponent<MeshRenderer>().material.color = CreatorMP.colours[Int32.Parse(hitInfo.collider.name)];

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
