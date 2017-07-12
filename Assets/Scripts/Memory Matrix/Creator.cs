using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Creator : MonoBehaviour
{
    public GameObject Support, Camera, cubitContainer;
    private int lines, columns, lenMargin = 1, lenCube = 2, modifier, rotationSpeed=100, direction, dif;
    public static int nrActive;
    private float supportWidth, supportHeight, waitTime = 2, waitTime2 = .5f;
    public static bool ready = false, modify = false;
    private float rotation = 90;
    public static float crtTime;
    public Text level;

    private struct Matrix
    {
        public bool active;
        public GameObject cube;

        public Matrix(GameObject a, bool b = false)
        {
            active = false;
            cube = a;
        }
    };

    private IEnumerator hide(Matrix[][] Matrix)
    {
        yield return new WaitForSecondsRealtime(waitTime);

        for (int i = 0; i < lines; i++)
            for (int j = 0; j < columns; j++)
                if (Matrix[i][j].active)
                    Matrix[i][j].cube.GetComponent<MeshRenderer>().material.color = Color.white;

        yield return new WaitForSecondsRealtime(waitTime2);

        modify = true;
    }

    private IEnumerator modifier6(Matrix[][] Matrix)
    {     
        yield return new WaitForSecondsRealtime(waitTime+waitTime2);

        int a = Random.Range(0, lines), b=Random.Range(0, columns);

        Matrix[a][b].cube.GetComponent<MeshRenderer>().material.color = Color.red;
        if (Matrix[a][b].cube.CompareTag("0"))
        {
            Matrix[a][b].cube.tag = "1";
            nrActive++;
        }
        else
        {
            Matrix[a][b].cube.tag = "0";
            nrActive--;
        }

        yield return new WaitForSecondsRealtime(1);
        Matrix[a][b].cube.GetComponent<MeshRenderer>().material.color = Color.white;
        rotation = 0;

    }

    private void Start()
    {
        Time.timeScale = 1;
        modify = false;
        ready = false;
        crtTime = 0;

        int c= Random.Range(-1, 1);

        columns = 2 + (int)Mathf.Round(1.5f*Mathf.Log(PlayerPrefs.GetInt("difficulty"))) + c;
        if (PlayerPrefs.GetInt("difficulty") == 0)
            columns = 2 + Random.Range(-1, 1);

        //print((int)Mathf.Round(1.5f * Mathf.Log(PlayerPrefs.GetInt("difficulty"))) + " " + c + " " + columns);

        lines = columns + Random.Range(-1, 1);

        if (columns < 2)
            columns = 2;

        if (lines < 2)
            lines = 2;

        PlayerPrefs.SetInt("difficulty", PlayerPrefs.GetInt("difficulty") + 1);

        level.text = "Level: " + PlayerPrefs.GetInt("difficulty").ToString();

        nrActive = Random.Range(lines < columns ? lines : columns, (int)(lines * columns / PlayerPrefs.GetInt("difficulty")));

        supportWidth = (lines + 1) * lenMargin + lines * lenCube;
        supportHeight = (columns + 1) * lenMargin + columns * lenCube;

        Support.transform.localScale = new Vector3(supportWidth, .8f, supportHeight);
        Support.transform.position = new Vector3(supportWidth / 2f, 0, supportHeight / 2f);
        Camera.transform.position = new Vector3(supportWidth / 2f, (supportWidth > supportHeight) ? supportWidth * 3 / 2f : supportHeight * 3 / 2f, supportHeight / 2f);

        Matrix[][] Matrix = new Matrix[lines][];

        int i, j;

        for (i = 0; i < lines; i++)
        {
            Matrix[i] = new Matrix[columns];

            for (j = 0; j < columns; j++)
            {
                Matrix[i][j].cube = Instantiate(cubitContainer);
                Matrix[i][j].cube.transform.position = new Vector3(i * (lenMargin + lenCube) + lenMargin + 1, 0, j * (lenMargin + lenCube) + lenMargin + 1);
                Matrix[i][j].cube.transform.SetParent(Support.transform);
                Matrix[i][j].cube.tag = "0";
            }
        }

        for (i = 0; i < nrActive; i++)
        {
            int lin = Random.Range(0, lines);
            int col = Random.Range(0, columns);

            if (!Matrix[lin][col].active)
                Matrix[lin][col].active = true;
            else
                i--;
        }

        for (i = 0; i < lines; i++)
            for (j = 0; j < columns; j++)
                if (Matrix[i][j].active)
                {
                    Matrix[i][j].cube.GetComponent<MeshRenderer>().material.color = Color.green;
                    Matrix[i][j].cube.tag = "1";
                }

        StartCoroutine(hide(Matrix));

        modifier = Random.Range(1, 7);

        if (Random.Range(0, 2) == 1)
            direction = -1;
        else
            direction = 1;

        if (modifier == 6)
           StartCoroutine (modifier6(Matrix));
    }

    private void Update()
    {
        if (Time.timeScale == 0)
            return;

        if (modify)
        {
            if (rotation <= 0)
            {
                modifier = 0;
                ready = true;
                if (crtTime==0)
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
                Support.transform.Rotate(new Vector3(0, direction*rotationSpeed, 0) * Time.deltaTime);
                rotation -= rotationSpeed/2f * Time.deltaTime;

                if (rotation <= 0)
                    Support.transform.Rotate(0, direction * rotation * 2, 0);
            }

            if (modifier == 4)
            {
                Support.transform.Rotate(new Vector3(0, 0, direction*rotationSpeed) * Time.deltaTime);
                rotation -= rotationSpeed / 2f * Time.deltaTime;

                if (rotation <= 0)
                    Support.transform.Rotate(0, 0, direction * rotation * 2);
            }

            if (modifier == 5)
            {
                Support.transform.Rotate(new Vector3(direction*rotationSpeed, 0, 0) * Time.deltaTime);
                rotation -= rotationSpeed / 2f * Time.deltaTime;

                if (rotation <= 0)
                    Support.transform.Rotate(direction * rotation * 2, 0, 0);
            }
        }
    }
}

