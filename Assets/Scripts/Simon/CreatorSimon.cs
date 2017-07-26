using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CreatorSimon : MonoBehaviour
{
    public static int nrClicks;
    public GameObject Support;
    private int rotationSpeed = 100, modifier, direction;
    private float rotation = 90;
    public static int[] order = new int[10];
    public static bool ready = false;
    private bool modify = false;
    public static float crtTime;
    public Text level;

    //showing the pattern
    private IEnumerator Show ()
    {
        int colour;

        for (int i = 0; i < nrClicks; ++i)
        {
            colour = order[i];
            yield return new WaitForSeconds(.5f);

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

        modify = true;
    }

	private void Start ()
    {
        //initialising the level
        Time.timeScale = 1;
        modify = false;
        ready = false;
        crtTime = 0;

        nrClicks = 3 + (int)Mathf.Round(1.5f * Mathf.Log(PlayerPrefs.GetInt("difficultySimon"))) + Random.Range(-1, 1);

        if (nrClicks < 3)
            nrClicks = 3;

        PlayerPrefs.SetInt("difficultySimon", PlayerPrefs.GetInt("difficultySimon") + 1);

        level.text = "Level: " + PlayerPrefs.GetInt("difficultySimon").ToString();

        order[0] = Random.Range(0, 4);

        for (int i = 1; i < nrClicks; ++i)
        {
            do
                order[i] = Random.Range(0, 4);
            while (order[i] == order[i - 1]);
        }

        StartCoroutine(Show());

        //choosing the event
        modifier = Random.Range(1, 6);

        if (Random.Range(0, 2) == 1)
            direction = -1;
        else
            direction = 1;
    }

    private void Update()
    {
        if (Time.timeScale == 0)
            return;

        //events
        if (modify)
        {
            if (rotation <= 0)
            {
                modifier = 0;
                ready = true;

                if (crtTime == 0)
                    crtTime = Time.timeSinceLevelLoad;
            }

            if (modifier == 1)
            {
                Support.transform.Rotate(new Vector3(0, -rotationSpeed, 0) * Time.deltaTime);
                rotation -= rotationSpeed * Time.deltaTime;

                if (rotation <= 0)
                    Support.transform.Rotate(0, -rotation, 0);
            }

            if (modifier == 2)
            {
                Support.transform.Rotate(new Vector3(0, rotationSpeed, 0) * Time.deltaTime);
                rotation -= rotationSpeed * Time.deltaTime;

                if (rotation <= 0)
                    Support.transform.Rotate(0, rotation, 0);
            }

            if (modifier == 3)
            {
                Support.transform.Rotate(new Vector3(0, direction * rotationSpeed, 0) * Time.deltaTime);
                rotation -= rotationSpeed / 2f * Time.deltaTime;

                if (rotation <= 0)
                    Support.transform.Rotate(0, direction * rotation * 2, 0);
            }

            if (modifier == 4)
            {
                Support.transform.Rotate(new Vector3(0, 0, direction * rotationSpeed) * Time.deltaTime);
                rotation -= rotationSpeed / 2f * Time.deltaTime;

                if (rotation <= 0)
                    Support.transform.Rotate(0, 0, direction * rotation * 2);
            }

            if (modifier == 5)
            {
                Support.transform.Rotate(new Vector3(direction * rotationSpeed, 0, 0) * Time.deltaTime);
                rotation -= rotationSpeed / 2f * Time.deltaTime;

                if (rotation <= 0)
                    Support.transform.Rotate(direction * rotation * 2, 0, 0);
            }
        }
    }
}
