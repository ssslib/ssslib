using UnityEngine;
using System.Collections;

public class DebugBtnScript : MonoBehaviour {

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {
	
	}

	/// <summary>
	/// オーディオプレイヤーのTriggerメソッド実行
	/// </summary>
	/// <param name="id"></param>
	public void TriggerCall(int id)
	{
		AudioManagerScript.Trigger(id);
	}

	public void TriggerPlay()
	{
        AudioManagerScript.instance.Play(0,"BGM002");
	}

	public void TriggerSet()
	{
		AudioManagerScript.instance.setMixerAndSource(0, AudioManagerScript.mixerType.BGM, this.GetComponent<AudioSource>());
	}
}
