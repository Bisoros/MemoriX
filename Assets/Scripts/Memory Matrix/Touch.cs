using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class Touch : MonoBehaviour
{
    private int nrFound = 0;
    private float waitTime = 1, time;
    public Text avarageTime, level;
    public GameObject endScreen, menu;

    private IEnumerator end()
    {
        Creator.ready = false;

        GameObject[] cubes = GameObject.FindGameObjectsWithTag("1");

        foreach (GameObject cube in cubes)
                cube.GetComponent<MeshRenderer>().material.color = Color.yellow;

        time = Time.timeSinceLevelLoad - Creator.crtTime;

        if (nrFound == Creator.nrActive)
        {
            Debug.Log("win " + time);
            PlayerPrefs.SetFloat("avarageTime", ((PlayerPrefs.GetInt("difficulty") - 1) * PlayerPrefs.GetFloat("avarageTime") + time) / (float)PlayerPrefs.GetInt("difficulty"));

            yield return new WaitForSecondsRealtime(waitTime);

            Application.LoadLevel(Application.loadedLevel);
        }
        else
        {
            Debug.Log("lose" + time);
            menu.SetActive(false);
            avarageTime.text = "Average Time/Completed Puzzle: " + PlayerPrefs.GetFloat("avarageTime").ToString();
            PlayerPrefs.SetInt("highscore", Mathf.Max(PlayerPrefs.GetInt("highscore"), PlayerPrefs.GetInt("difficulty")));
            level.text = "Level Reached: " + PlayerPrefs.GetInt("difficulty").ToString() + " (Highscore: " + PlayerPrefs.GetInt("highscore").ToString() + ")";
            yield return new WaitForSeconds(1);
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

        Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);

		RaycastHit hitInfo;

		if (Physics.Raycast (ray, out hitInfo))
		{
			if (Input.GetMouseButtonDown (0) && Creator.ready)
            {
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
        
        GameObject[] cubes = GameObject.FindGameObjectsWithTag("pressed");

        foreach (GameObject cube in cubes)
            if (cube.GetComponent<MeshRenderer>().material.color != Color.yellow)
                cube.GetComponent<MeshRenderer>().material.color = Color.green;
    }
}﻿