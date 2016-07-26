using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class AudioDebug : MonoBehaviour {

	public Text[] bgmNameList = new Text[8]; 

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(AudioManagerScript.instance != null)
		{
			AudioManagerScript audio = AudioManagerScript.instance;
			for(int i =0; i < 8; i++)
			{
				if(audio.ChannelManager[i] != null)
				{
					bgmNameList[i].text = audio.ChannelManager[i].fileName;
                }
				else
				{
					bgmNameList[i].text = "-";
                }
			}
		}
	
	}
}
