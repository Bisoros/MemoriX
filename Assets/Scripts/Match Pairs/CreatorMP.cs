using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CreatorMP : MonoBehaviour
{
    public GameObject Support, Camera, cubitContainer;
    private int lines=3, columns=4, lenMargin = 1, lenCube = 2, modifier, rotationSpeed = 100, direction, supportWidth, supportHeight;
    public static int nrPairs;
    private float waitTime = 1;
    public static bool ready = false, modify = false;
    private float rotation = 90;
    private Color[] colourPallette = new Color[] {Color.red, Color.red, Color.blue, Color.blue, Color.green, Color.green, Color.yellow, Color.yellow, Color.magenta, Color.magenta, Color.cyan, Color.cyan};
    public static Color[] colours;
    public static float crtTime;
    public Text level;

    //Fisher–Yates shuffle
    private void reshuffle(Color[] colours)
    {
        for (int t = 0; t < nrPairs*2; t++)
        {
            Color tmp = colours[t];
            int r = Random.Range(t, nrPairs*2);
            colours[t] = colours[r];
            colours[r] = tmp;
        }
    }
    
    //hiding pattern
    private IEnumerator hide()
    {
        yield return new WaitForSeconds(waitTime);

        GameObject[] cubes = GameObject.FindGameObjectsWithTag("1");

        foreach (GameObject cube in cubes)
            cube.GetComponent<MeshRenderer>().material.color = Color.white;

        modify = true;
    }

    private void Start()
    {
        //initialising level
        Time.timeScale = 1;

        modify = false;
        ready = false;
        crtTime = 0;
        colours = colourPallette;

        int dif = PlayerPrefs.GetInt("difficultyMP");

        if (dif < 10)
            nrPairs = 3;
        else if (dif < 20)
            nrPairs = 4;
        else
            nrPairs = 5;

        PlayerPrefs.SetInt("difficultyMP", dif + 1);

        level.text = "Level: " + PlayerPrefs.GetInt("difficultyMP").ToString();

        waitTime += nrPairs; 
        reshuffle(colours);

        columns = 2;
        lines = nrPairs;

        if (nrPairs == 6)
        {
            columns = 3;
            lines = 4;
        }

        supportWidth = (lines + 1) * lenMargin + lines * lenCube;
        supportHeight = (columns + 1) * lenMargin + columns * lenCube;

        Support.transform.localScale = new Vector3(supportWidth, .8f, supportHeight);
        Support.transform.position = new Vector3(supportWidth / 2f, 0, supportHeight / 2f);
        Camera.transform.position = new Vector3(supportWidth / 2f, (supportWidth > supportHeight) ? supportWidth * 3 / 2f : supportHeight * 3 / 2f, supportHeight / 2f);

        //instantiating the matrix
        int i, j, k=0;

        GameObject aux;

        for (i = 0; i < lines; i++)
            for (j = 0; j < columns; j++)
            {
                aux = Instantiate(cubitContainer);
                aux.transform.position = new Vector3(i * (lenMargin + lenCube) + lenMargin + 1, 0, j * (lenMargin + lenCube) + lenMargin + 1);
                aux.transform.SetParent(Support.transform);
                aux.tag = "1";
                aux.name = k++.ToString();
            }

        GameObject[] cubes = GameObject.FindGameObjectsWithTag("1");

        foreach (GameObject cube in cubes)
            cube.GetComponent<MeshRenderer>().material.color = colours[System.Int32.Parse(cube.name)];

        StartCoroutine(hide());

        //choosing event
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
