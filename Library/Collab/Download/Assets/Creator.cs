using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Creator : MonoBehaviour {

	public GameObject Support, Camera, Cubit_Container;
	public int m, n, ld, lcub;
	public float support_width, support_height;
	// Use this for initialization
	void Start () {
		Support = GameObject.Find ("Support");
		Camera = GameObject.Find ("Main Camera");

		ld = 1;
		lcub = 2;

		m = 3;
		n = 2;

		support_width = (m + 1) * ld + m * lcub;
		support_height = (n + 1) * ld + n * lcub;

		Support.transform.localScale = new Vector3 (support_width, 0, support_height);
		Support.transform.position = new Vector3 ((float)support_width / (float)2, 0, (float)support_height / (float)2);

		Camera.transform.position = new Vector3 ((float)support_width / (float)2, 9, (float)support_height / (float)2);

		int k = 1;

		for (int i = 0; i < n; i++)
			for (int j = 0; j < m; j++)
			{
				Cubit_Container = GameObject.Find (k.ToString());
				Cubit_Container.transform.position = new Vector3 (j * (ld + lcub) + ld + 1, 0, i * (ld + lcub) + ld + 1);
				k++;
			}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
