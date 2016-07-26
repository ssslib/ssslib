using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SampleSceneScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
		List<Hashtable> data = CSVLoarder.LoadCSV("test_data");
		AudioManagerScript.instance.Init(data);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
