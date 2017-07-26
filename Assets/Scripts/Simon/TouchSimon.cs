using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TouchSimon : MonoBehaviour
{
    private int i = 0;
    private float time;
    private bool lost = false;
    public Text avarageTime, level;
    public GameObject endScreen, menu, crtLevel;

    //colouring pressed cube
    private IEnumerator Show(int colour)
    {
        switch (colour)
        {
            case 0:
                GameObject.FindGameObjectWithTag("0").GetComponent<MeshRenderer>().material.color = Color.white;
                yield return new WaitForSeconds(.5f);
                GameObject.FindGameObjectWithTag("0").GetComponent<MeshRenderer>().material.color = Color.yellow;
                break;

            case 1:
                GameObject.FindGameObjectWithTag("1").GetComponent<MeshRenderer>().material.color = Color.white;
                yield return new WaitForSeconds(.5f);
                GameObject.FindGameObjectWithTag("1").GetComponent<MeshRenderer>().material.color = Color.green;
                break;

            case 2:
                GameObject.FindGameObjectWithTag("2").GetComponent<MeshRenderer>().material.color = Color.white;
                yield return new WaitForSeconds(.5f);
                GameObject.FindGameObjectWithTag("2").GetComponent<MeshRenderer>().material.color = Color.blue;
                break;

            case 3:
                GameObject.FindGameObjectWithTag("3").GetComponent<MeshRenderer>().material.color = Color.white;
                yield return new WaitForSeconds(.5f);
                GameObject.FindGameObjectWithTag("3").GetComponent<MeshRenderer>().material.color = Color.red;
                break;
        }
    }

    //lose
    private IEnumerator ShowFinal(int colour)
    {
        //end feedback
        for (int i = colour; i < CreatorSimon.nrClicks; i++)
        {
            switch (CreatorSimon.order[i])
            {
                case 0:
                    GameObject.FindGameObjectWithTag("0").GetComponent<MeshRenderer>().material.color = Color.white;
                    yield return new WaitForSeconds(.5f);
                    GameObject.FindGameObjectWithTag("0").GetComponent<MeshRenderer>().material.color = Color.yellow;
                    break;

                case 1:
                    GameObject.FindGameObjectWithTag("1").GetComponent<MeshRenderer>().material.color = Color.white;
                    yield return new WaitForSeconds(.5f);
                    GameObject.FindGameObjectWithTag("1").GetComponent<MeshRenderer>().material.color = Color.green;
                    break;

                case 2:
                    GameObject.FindGameObjectWithTag("2").GetComponent<MeshRenderer>().material.color = Color.white;
                    yield return new WaitForSeconds(.5f);
                    GameObject.FindGameObjectWithTag("2").GetComponent<MeshRenderer>().material.color = Color.blue;
                    break;

                case 3:
                    GameObject.FindGameObjectWithTag("3").GetComponent<MeshRenderer>().material.color = Color.white;
                    yield return new WaitForSeconds(.5f);
                    GameObject.FindGameObjectWithTag("3").GetComponent<MeshRenderer>().material.color = Color.red;
                    break;
            }

            yield return new WaitForSeconds(.5f);
        }

		avarageTime.text = "Average Time/Completed Puzzle: " + (PlayerPrefs.GetFloat("avarageTimeSimon")==0? "N/A" : PlayerPrefs.GetFloat("avarageTimeSimon").ToString("0.00"));
        PlayerPrefs.SetInt("highscoreSimon", Mathf.Max(PlayerPrefs.GetInt("highscoreSimon"), PlayerPrefs.GetInt("difficultySimon")));
        level.text = "Level Reached: " + PlayerPrefs.GetInt("difficultySimon").ToString() + " (Highscore: " + PlayerPrefs.GetInt("highscoreSimon").ToString() + ")";
        yield return new WaitForSeconds(1);
        Time.timeScale = 0;
        menu.SetActive(false);
        crtLevel.SetActive(false);
        endScreen.SetActive(true);
        PlayerPrefs.SetInt("difficultySimon", 0);
        PlayerPrefs.SetFloat("avarageTimeSimon", 0);
        PlayerPrefs.Save();
    }

    private void Start()
    {
        lost = false;
    }

    //win
    private IEnumerator end()
    {
        PlayerPrefs.SetFloat("avarageTimeSimon", ((PlayerPrefs.GetInt("difficultySimon") - 1) * PlayerPrefs.GetFloat("avarageTimeSimon") + time) / (float)PlayerPrefs.GetInt("difficultySimon"));

        yield return new WaitForSeconds(3);

        Application.LoadLevel(Application.loadedLevel);
    }

    private void Update ()
    {
        if (Time.timeScale == 0)
            return;

        //getting user input
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        
        RaycastHit hitInfo;

        if (Physics.Raycast(ray, out hitInfo))
            if (Input.GetMouseButtonDown(0) && CreatorSimon.ready)
            {
                //processing user input
                if (hitInfo.collider.CompareTag(CreatorSimon.order[i].ToString()))
                {
                    StartCoroutine(Show(CreatorSimon.order[i]));
                    i++;
                }
              
                else if (!lost && (hitInfo.collider.CompareTag("0") || hitInfo.collider.CompareTag("1") || hitInfo.collider.CompareTag("2") || hitInfo.collider.CompareTag("3")))
                {
                    time = Time.timeSinceLevelLoad - CreatorSimon.crtTime;
                    lost = true;
                    StartCoroutine(ShowFinal(i));
                }

                if (i == CreatorSimon.nrClicks && !lost)
                {
                    time = Time.timeSinceLevelLoad - CreatorSimon.crtTime;
                    StartCoroutine(end());
                }
            }
    }
}
