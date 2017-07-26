using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Touch : MonoBehaviour
{
    private int nrFound = 0;
    private float waitTime = 1, time;
    public Text avarageTime, level;
    public GameObject endScreen, menu, crtLevel;

    private IEnumerator end()
    {
        //giving end feedback
        Creator.ready = false;

        GameObject[] cubes = GameObject.FindGameObjectsWithTag("1");

        foreach (GameObject cube in cubes)
                cube.GetComponent<MeshRenderer>().material.color = Color.yellow;

        time = Time.timeSinceLevelLoad - Creator.crtTime;

        //win
        if (nrFound == Creator.nrActive)
        {
            PlayerPrefs.SetFloat("avarageTime", ((PlayerPrefs.GetInt("difficulty") - 1) * PlayerPrefs.GetFloat("avarageTime") + time) / (float)PlayerPrefs.GetInt("difficulty"));

            yield return new WaitForSeconds(waitTime);

            Application.LoadLevel(Application.loadedLevel);
        }
        //lose
        else
        {
            avarageTime.text = "Average Time/Completed Puzzle: " + (PlayerPrefs.GetFloat("avarageTime") == 0 ? "N/A" : PlayerPrefs.GetFloat("avarageTime").ToString("0.00"));
            PlayerPrefs.SetInt("highscore", Mathf.Max(PlayerPrefs.GetInt("highscore"), PlayerPrefs.GetInt("difficulty")));
            level.text = "Level Reached: " + PlayerPrefs.GetInt("difficulty").ToString() + " (Highscore: " + PlayerPrefs.GetInt("highscore").ToString() + ")";
            yield return new WaitForSeconds(1);
            menu.SetActive(false);
            crtLevel.SetActive(false);
            Time.timeScale = 0;
            endScreen.SetActive(true);
            PlayerPrefs.SetInt("difficulty", 0);
            PlayerPrefs.SetFloat("avarageTime", 0);
            PlayerPrefs.Save();
        }

    }

	private void Update () 
	{
        if (Time.timeScale == 0)
            return;

        //getting user input
        Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);

		RaycastHit hitInfo;

		if (Physics.Raycast (ray, out hitInfo))
		{
			if (Input.GetMouseButtonDown (0) && Creator.ready)
            {
                //processing input
                if (hitInfo.collider.CompareTag("0"))
                    StartCoroutine(end());

                if (hitInfo.collider.CompareTag("1"))
                {
                    hitInfo.collider.tag = "pressed";
                    nrFound++;

                    if (nrFound == Creator.nrActive)
                        StartCoroutine(end());
                }
			}
		}
        
        //coloring pressed cubes
        GameObject[] cubes = GameObject.FindGameObjectsWithTag("pressed");

        foreach (GameObject cube in cubes)
            if (cube.GetComponent<MeshRenderer>().material.color != Color.yellow)
                cube.GetComponent<MeshRenderer>().material.color = Color.green;
    }
}﻿
