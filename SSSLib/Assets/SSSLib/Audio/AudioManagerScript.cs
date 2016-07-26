using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;
using System;
using System.Linq;
/// <summary>
/// BGM、SE、VOICEの管理マネージャ、シングルトンでのみ活用
/// </summary>
public class AudioManagerScript : MonoBehaviour
{
	public static AudioManagerScript instance;

	Coroutine waitFade = null;

	// AssetBundle
	public AssetBundle AssetBundleData = null;
	/// <summary>
	/// 使用するオーディオミキサー。
	/// </summary>
	public AudioMixer Mixer;

	/// <summary>
	/// 0～15チャンネル
	/// </summary>
	[SerializeField]
	private AudioPlayerScript[] mChannelManager;
	public AudioPlayerScript[] ChannelManager
	{
		get { return mChannelManager; }
		private set { mChannelManager = value; }
	}

	public GameObject[] channelObjManager;

	/// <summary>
	/// チャンネル数0～15
	/// </summary>
	[SerializeField]
	private readonly int CHANNEL = 16;

	//////////////////////////////////////
	//Trigger用特殊ID
	//////////////////////////////////////
	//Triggerの引数に90000～90015が設定された場合は下二ケタ値のチャンネルを停止する
	public static readonly int TRIGGER_STOP_CH_MIN = 90000;
	public static readonly int TRIGGER_STOP_CH_MAX = 90015;
	public static readonly int TRIGGER_STOP_CH_00 = 90000;  //チャンネル0
	public static readonly int TRIGGER_STOP_CH_01 = 90001;  //チャンネル1
	public static readonly int TRIGGER_STOP_CH_02 = 90002;  //チャンネル2
	public static readonly int TRIGGER_STOP_CH_03 = 90003;  //チャンネル3
	public static readonly int TRIGGER_STOP_CH_04 = 90004;  //チャンネル4
	public static readonly int TRIGGER_STOP_CH_05 = 90005;  //チャンネル5
	public static readonly int TRIGGER_STOP_CH_06 = 90006;  //チャンネル6
	public static readonly int TRIGGER_STOP_CH_07 = 90007;  //チャンネル7
	public static readonly int TRIGGER_STOP_CH_08 = 90008;  //チャンネル8
	public static readonly int TRIGGER_STOP_CH_09 = 90009;  //チャンネル9
	public static readonly int TRIGGER_STOP_CH_10 = 90010;  //チャンネル10
	public static readonly int TRIGGER_STOP_CH_11 = 90011;  //チャンネル11
	public static readonly int TRIGGER_STOP_CH_12 = 90012;  //チャンネル12
	public static readonly int TRIGGER_STOP_CH_13 = 90013;  //チャンネル13
	public static readonly int TRIGGER_STOP_CH_14 = 90014;  //チャンネル14
	public static readonly int TRIGGER_STOP_CH_15 = 90015;  //チャンネル15

	//Triggerの引数に90016が設定された場合は全チャンネルを停止する
	public static readonly int TRIGGER_STOP_CH_ALL = 90016;
	//Triggerの引数に100000～100015が設定された場合は下二ケタ値のチャンネルをフェードアウトする
	public static readonly int TRIGGER_FADE_CH_MIN = 100000;
	public static readonly int TRIGGER_FADE_CH_MAX = 100015;
	public static readonly int TRIGGER_FADE_CH_00 = 100000;  //チャンネル0
	public static readonly int TRIGGER_FADE_CH_01 = 100001;  //チャンネル1
	public static readonly int TRIGGER_FADE_CH_02 = 100002;  //チャンネル2
	public static readonly int TRIGGER_FADE_CH_03 = 100003;  //チャンネル3
	public static readonly int TRIGGER_FADE_CH_04 = 100004;  //チャンネル4
	public static readonly int TRIGGER_FADE_CH_05 = 100005;  //チャンネル5
	public static readonly int TRIGGER_FADE_CH_06 = 100006;  //チャンネル6
	public static readonly int TRIGGER_FADE_CH_07 = 100007;  //チャンネル7
	public static readonly int TRIGGER_FADE_CH_08 = 100008;  //チャンネル8
	public static readonly int TRIGGER_FADE_CH_09 = 100009;  //チャンネル9
	public static readonly int TRIGGER_FADE_CH_10 = 100010;  //チャンネル10
	public static readonly int TRIGGER_FADE_CH_11 = 100011;  //チャンネル11
	public static readonly int TRIGGER_FADE_CH_12 = 100012;  //チャンネル12
	public static readonly int TRIGGER_FADE_CH_13 = 100013;  //チャンネル13
	public static readonly int TRIGGER_FADE_CH_14 = 100014;  //チャンネル14
	public static readonly int TRIGGER_FADE_CH_15 = 100015;  //チャンネル15

	//Triggerの引数に90016が設定された場合は全チャンネルをフェードアウトする
	public static readonly int TRIGGER_FADE_CH_ALL = 100016;
	//Triggerの引数に110000～110015が設定された場合は下二ケタ値のチャンネルを一時停止する
	public static readonly int TRIGGER_PAUSE_CH_MIN = 110000;
	public static readonly int TRIGGER_PAUSE_CH_MAX = 110015;
	public static readonly int TRIGGER_PAUSE_CH_00 = 110000;  //チャンネル0
	public static readonly int TRIGGER_PAUSE_CH_01 = 110001;  //チャンネル1
	public static readonly int TRIGGER_PAUSE_CH_02 = 110002;  //チャンネル2
	public static readonly int TRIGGER_PAUSE_CH_03 = 110003;  //チャンネル3
	public static readonly int TRIGGER_PAUSE_CH_04 = 110004;  //チャンネル4
	public static readonly int TRIGGER_PAUSE_CH_05 = 110005;  //チャンネル5
	public static readonly int TRIGGER_PAUSE_CH_06 = 110006;  //チャンネル6
	public static readonly int TRIGGER_PAUSE_CH_07 = 110007;  //チャンネル7
	public static readonly int TRIGGER_PAUSE_CH_08 = 110008;  //チャンネル8
	public static readonly int TRIGGER_PAUSE_CH_09 = 110009;  //チャンネル9
	public static readonly int TRIGGER_PAUSE_CH_10 = 110010;  //チャンネル10
	public static readonly int TRIGGER_PAUSE_CH_11 = 110011;  //チャンネル11
	public static readonly int TRIGGER_PAUSE_CH_12 = 110012;  //チャンネル12
	public static readonly int TRIGGER_PAUSE_CH_13 = 110013;  //チャンネル13
	public static readonly int TRIGGER_PAUSE_CH_14 = 110014;  //チャンネル14
	public static readonly int TRIGGER_PAUSE_CH_15 = 110015;  //チャンネル15

	//Triggerの引数に110016が設定された場合は全チャンネルを一時停止する
	public static readonly int TRIGGER_PAUSE_CH_ALL = 110016;
	//Triggerの引数に120000～120015が設定された場合は下二ケタ値のチャンネルを一時停止解除する
	public static readonly int TRIGGER_RESUME_CH_MIN = 120000;
	public static readonly int TRIGGER_RESUME_CH_MAX = 120015;
	public static readonly int TRIGGER_RESUME_CH_00 = 120000;  //チャンネル0
	public static readonly int TRIGGER_RESUME_CH_01 = 120001;  //チャンネル1
	public static readonly int TRIGGER_RESUME_CH_02 = 120002;  //チャンネル2
	public static readonly int TRIGGER_RESUME_CH_03 = 120003;  //チャンネル3
	public static readonly int TRIGGER_RESUME_CH_04 = 120004;  //チャンネル4
	public static readonly int TRIGGER_RESUME_CH_05 = 120005;  //チャンネル5
	public static readonly int TRIGGER_RESUME_CH_06 = 120006;  //チャンネル6
	public static readonly int TRIGGER_RESUME_CH_07 = 120007;  //チャンネル7
	public static readonly int TRIGGER_RESUME_CH_08 = 120008;  //チャンネル8
	public static readonly int TRIGGER_RESUME_CH_09 = 120009;  //チャンネル9
	public static readonly int TRIGGER_RESUME_CH_10 = 120010;  //チャンネル10
	public static readonly int TRIGGER_RESUME_CH_11 = 120011;  //チャンネル11
	public static readonly int TRIGGER_RESUME_CH_12 = 120012;  //チャンネル12
	public static readonly int TRIGGER_RESUME_CH_13 = 120013;  //チャンネル13
	public static readonly int TRIGGER_RESUME_CH_14 = 120014;  //チャンネル14
	public static readonly int TRIGGER_RESUME_CH_15 = 120015;  //チャンネル15
	//Triggerの引数に120016が設定された場合は全チャンネルを一時停止解除する
	public static readonly int TRIGGER_RESUME_CH_ALL = 120016;

	/// <summary>
	/// 使用するオーディオグループ。
	/// </summary>
	public AudioMixerGroup MixerBgmGroup;
	public AudioMixerGroup MixerSeGroup;
	public AudioMixerGroup MixerVoiceGroup;

	/// <summary>
	/// 管理しているオーディオ情報
	/// </summary>
	/// 
	public List<AudioPlayerScript> AudioPlayers { get; private set; }

	/// <summary>
	/// 読み込まれたBGM
	/// </summary>
	public Dictionary<string, AudioClip> BgmAudioClip { get; private set; }

	/// <summary>
	/// 読み込まれたSE
	/// </summary>
	public Dictionary<string, AudioClip> SeAudioClip { get; private set; }

	/// <summary>
	/// 読み込まれたVOICE
	/// </summary>
	public Dictionary<string, AudioClip> VoiceAudioClip { get; private set; }

	/// <summary>
	/// BGM音量
	/// </summary>
	public float BGMVolume
	{
		set
		{
			Mixer.SetFloat("BGMVolume", Mathf.Lerp(-80, 0, value));
			bgmVol = value;
			Debug.Log("BGMボリュームが変更されました " + Mathf.Lerp(-80, 0, value));
		}
		get { return bgmVol; }
	}
	[SerializeField]
	private float bgmVol;
	/// <summary>
	/// SE音量
	/// </summary>
	public float SEVolume
	{
		set
		{
			Mixer.SetFloat("SEVolume", Mathf.Lerp(-80, 0, value));
			seVol = value;
			Debug.Log("SEボリュームが変更されました " + Mathf.Lerp(-80, 0, value));

		}
		get { return seVol; }

	}
	[SerializeField]
	private float seVol;

	/// <summary>
	/// VOICE音量
	/// </summary>
	public float VOICEVolume
	{
		set
		{
			Mixer.SetFloat("VOICEVolume", Mathf.Lerp(-80, 0, value));
			voiceVol = value;
			Debug.Log("VOICEボリュームが変更されました " + Mathf.Lerp(-80, 0, value));

		}
		get { return voiceVol; }

	}
	[SerializeField]
	private float voiceVol;
	/// <summary>
	/// 全ての音量
	/// </summary>
	public float MasterVolume
	{
		set
		{
			Mixer.SetFloat("MASTERVolume", Mathf.Lerp(-80, 0, value));
			masterVol = value;
			Debug.Log("MASTERボリュームが変更されました " + Mathf.Lerp(-80, 0, value));
		}
		get { return masterVol; }

	}
	[SerializeField]
	private float masterVol;

	/// <summary>
	/// リソース
	/// </summary>
	[SerializeField]
	private string bgmPath = "Audio/BGM";
	[SerializeField]
	private string sePath = "Audio/SE";
	[SerializeField]
	private string voicePath = "Audio/VOICE";

	/// <summary>
	/// ミキサーグループのタイプ
	/// </summary>
	public Hashtable mixerGroupType = new Hashtable()
	{
		{"BGM",mixerType.BGM },
		{"SE",mixerType.SE },
		{"VOICE",mixerType.VOICE },
	};

	/// <summary>
	/// ミキサーの定義
	/// </summary>
	public enum mixerType
	{
		NONE,
		VOICE,
		SE,
		BGM
	}

	/// <summary>
	/// コンストラクタ
	/// </summary>
	public AudioManagerScript()
	{
		AudioPlayers = new List<AudioPlayerScript>();
	}

	/// <summary>
	/// 起動時の初期化処理
	/// 
	/// </summary>
	void Awake()
	{
		instance = this;

		ChannelManager = new AudioPlayerScript[CHANNEL];
		BgmAudioClip = new Dictionary<string, AudioClip>();
		SeAudioClip = new Dictionary<string, AudioClip>();
		VoiceAudioClip = new Dictionary<string, AudioClip>();
		GameObject prefab = (GameObject)Resources.Load("Prefab/Ch");
		channelObjManager = new GameObject[CHANNEL];
		for (int i = 0; i < CHANNEL; i++)
		{
			channelObjManager[i] = (GameObject)Instantiate(prefab, transform.position, Quaternion.identity);
			channelObjManager[i].transform.SetParent(transform, false);
			channelObjManager[i].name = string.Format("Ch{0:D2}", i);
			DontDestroyScript dontDestory = channelObjManager[i].AddComponent<DontDestroyScript>();
			channelObjManager[i].GetComponent<AudioPlayerScript>().channel = i;
			//channelObjManager[i].GetComponent<DontDestroyScript>().Init();
		}
	}

	/// <summary>
	/// 必ず音量を最大値に設定
	/// </summary>
	void Start()
	{
		BGMVolume = 1;
		SEVolume = 1;
		VOICEVolume = 1;
		MasterVolume = 1;
		
		StartCoroutine(checkPlaying());
	}

	public Dictionary<int, Hashtable> audioConfig;

	/// <summary>
	/// 引数に指定された情報を現在のシーンにセットする
	/// 指定されたゲームオブジェクトにAudioSourceとAudioPlayerScriptをアタッチして情報をセット
	/// 既にAudioScriptかAudioSourceが設定されているオブジェクトを指定した場合はアタッチはせず情報をセット
	/// </summary>
	/// <param name="configList"></param>
	public void Init(List<Hashtable> configList)
	{
		audioConfig = new Dictionary<int, Hashtable>();

		List<string> option = new List<string>()
		{
			"channel",					//【必須】 再生するチャンネル
			"audio",				    //【必須】【string型】設定するミキサーグループ, 「BGM」「SE」「VOICE」のどれかを設定
			"source",					//【string型】AudioSourceをセットするゲームオブジェクト
			"isAutoFadeOut",			//【bool型】自動でフェードアウトを行うか 
			"fadeInSeconds",			//【double型】フェードインが完了するまでの時間、フェードインが不要ならば0が自動でセットされる
			"fadeOutSeconds",			//【double型】フェードアウトが完了するまでの時間、フェードアウトが不要ならば0が自動でセットされる
			"fileName",					//【string型】再生する音声ファイル名
			"findPath",					//【string型】AudioManagerScriptのアタッチされているゲームオブジェクトのパス。一番上の階層かつプレハブ名を変更していなければ設定不要
			"bypassEffects",			//【bool型】 AudioSourceに設定
			"bypassListenerEffects",	//【bool型】 AudioSourceに設定
            "bypassReverbZones",		//【bool型】 AudioSourceに設定
			"dopplerLevel",				//【float型】AudioSourceに設定
			"loop",						//【bool型】 AudioSourceに設定
			"maxDistance",				//【float型】AudioSourceに設定
			"minDistance",				//【float型】AudioSourceに設定
			"panStereo",				//【float型】AudioSourceに設定
			"pitch",					//【float型】AudioSourceに設定
			"priority",					//【int型】  AudioSourceに設定
			"reverbZoneMix",			//【float型】AudioSourceに設定
			"spatialBlend",				//【float型】AudioSourceに設定
			"spatialize",				//【bool型】 AudioSourceに設定
			//"mute",					// AudioManager側で管理するので設定不可 
			//"clip",					// AudioManager側で管理するので設定不可 
			//"ignoreListenerPause",	// AudioManager側で管理するので設定不可
			//"ignoreListenerVolume",	// AudioManager側で管理するので設定不可
			//"mute",					// AudioManager側で管理するので設定不可 
			//"outputAudioMixerGroup",	// AudioPlayer側で管理するので設定不可 
			//"playOnAwake",			// AudioManager側で管理するので設定不可 
			//"rolloffMode",			//非対応
			//"time",					//非対応
			//"timeSamples",			//非対応
			//"velocityUpdateMode",		//非対応
			//"volume"					//非対応

		};
		List<string> validation = new List<string>()
		{
			"channel",
			"audio"
		};

		foreach (Hashtable config in configList)
		{
			Debug.Log("<color=red>【AudioManagerScript】id=" + config["id"]+ "設定開始</color>");
			//空白行を除外する
			Hashtable data = (Hashtable)config.Clone();
			foreach (DictionaryEntry column in config)
			{

				if (column.Value.ToString() == "")
				{
					data.Remove(column.Key);
				}
			}
			audioConfig[Convert.ToInt32(config["id"])] = data;
			foreach (string valid in validation)
			{
				if (!data.ContainsKey(valid))
				{
					Debug.LogError("<color=red>【AudioManagerScript】" + valid + "が設定されておりません。</color>");
				}
			}
			
			
		}
	}

	void SetConfig(int id)
	{
		
		Hashtable data = audioConfig[id];
		GameObject obj = channelObjManager[Convert.ToInt32(data["channel"])];
		mChannelManager[Convert.ToInt32(data["channel"])] = obj.GetComponent<AudioPlayerScript>();
		GameObject audioSourceObj;
		if (data.ContainsKey("source"))
		{
			audioSourceObj = GameObject.Find(data["source"].ToString());
			if (audioSourceObj == null)
			{
				Debug.LogError("【AudioManagerScript】data[\"source\"] = " + data["source"].ToString() + "が存在しません。");
			}
		}
		else
		{
			audioSourceObj = obj;
		}


		AudioSource source = audioSourceObj.GetComponent<AudioSource>();
		if (source == null)
		{
			source = audioSourceObj.AddComponent<AudioSource>();
		}
		AudioPlayerScript audioPlayer = obj.GetComponent<AudioPlayerScript>();
		if (audioPlayer == null)
		{
			audioPlayer = obj.AddComponent<AudioPlayerScript>();
		}
		audioPlayer.Source = source;

		if (data.ContainsKey("id"))
		{
			audioPlayer.id = Convert.ToInt32(data["id"]);
		}

		if (data.ContainsKey("isAutoFadeOut"))
		{
			audioPlayer.IsAutoFadeOut = Convert.ToBoolean(data["isAutoFadeOut"]);
		}

		if (data.ContainsKey("fadeInSeconds"))
		{
			audioPlayer.FadeInSeconds = Convert.ToDouble(data["fadeInSeconds"]);
		}

		if (data.ContainsKey("fadeOutSeconds"))
		{
			audioPlayer.FadeOutSeconds = Convert.ToDouble(data["fadeOutSeconds"]);
		}

		if (data.ContainsKey("fileName"))
		{
			audioPlayer.FileName = data["fileName"].ToString();
		}

		if (data.ContainsKey("channel"))
		{
			audioPlayer.channel = Convert.ToInt32(data["channel"]);
		}

		if (data.ContainsKey("findPath"))
		{
			audioPlayer.findPath = data["findPath"].ToString();
		}

		if (data.ContainsKey("audio"))
		{
			audioPlayer.useMixerType = (mixerType)mixerGroupType[data["audio"].ToString()];
		}

		if (data.ContainsKey("bypassEffects"))
		{
			audioPlayer.Source.bypassEffects = Convert.ToBoolean(data["bypassEffects"]);
		}

		if (data.ContainsKey("bypassListenerEffects"))
		{
			audioPlayer.Source.bypassListenerEffects = Convert.ToBoolean(data["bypassListenerEffects"]);
		}

		if (data.ContainsKey("bypassReverbZones"))
		{
			audioPlayer.Source.bypassReverbZones = Convert.ToBoolean(data["bypassReverbZones"]);
		}

		if (data.ContainsKey("dopplerLevel"))
		{
			audioPlayer.Source.dopplerLevel = Convert.ToSingle(data["dopplerLevel"]);
		}

		if (data.ContainsKey("loop"))
		{
			Debug.Log(Convert.ToBoolean(data["loop"]));
			audioPlayer.Source.loop = Convert.ToBoolean(data["loop"]);
		}

		if (data.ContainsKey("maxDistance"))
		{
			audioPlayer.Source.maxDistance = Convert.ToSingle(data["maxDistance"]);
		}

		if (data.ContainsKey("minDistance"))
		{
			audioPlayer.Source.minDistance = Convert.ToSingle(data["minDistance"]);
		}

		if (data.ContainsKey("panStereo"))
		{
			audioPlayer.Source.panStereo = Convert.ToSingle(data["panStereo"]);
		}
		if (data.ContainsKey("pitch"))
		{
			audioPlayer.Source.pitch = Convert.ToSingle(data["pitch"]);
		}

		if (data.ContainsKey("priority"))
		{
			audioPlayer.Source.priority = Convert.ToInt32(data["priority"]);
		}

		if (data.ContainsKey("reverbZoneMix"))
		{
			audioPlayer.Source.reverbZoneMix = Convert.ToSingle(data["reverbZoneMix"]);
		}

		if (data.ContainsKey("spatialBlend"))
		{
			audioPlayer.Source.spatialBlend = Convert.ToSingle(data["spatialBlend"]);
		}

		if (data.ContainsKey("spatialize"))
		{
			audioPlayer.Source.spatialize = Convert.ToBoolean(data["spatialize"]);
		}

		if (data.ContainsKey("volume"))
		{
			audioPlayer.Source.volume = Convert.ToSingle(data["volume"]);
			audioPlayer.BaseVolume = Convert.ToSingle(data["volume"]);
			audioPlayer.InitBaseVolume = Convert.ToSingle(data["volume"]);
		}
		audioPlayer.Source.mute = false;
		audioPlayer.Source.playOnAwake = false;
		audioPlayer.Init();

	}


	

	IEnumerator checkPlaying()
	{
		while (true)
		{
			for (int i = 0; i < ChannelManager.Length; i++)
			{
				AudioPlayerScript value = ChannelManager[i];
				if (value != null && !value.IsPause && value.Source != null && !value.Source.isPlaying)
				{
					ChannelManager[i] = null;
				}
			}
			yield return new WaitForSeconds(1f);
		}
		
	}

	/// <summary>
	/// アセットバンドルからBGMを読み込む
	/// </summary>
	/// <param name="fileName"></param>
	private void AssetLoadBGM(string fileName)
	{
		AudioClip audio = AssetBundleData.LoadAsset<AudioClip>(fileName);
		if (audio != null)
		{
			BgmAudioClip[fileName] = audio;
			Debug.Log(fileName + "アセットから読み込み");
		}
	}

	/// <summary>
	/// アセットバンドルからSEを読み込む
	/// </summary>
	/// <param name="fileName"></param>
	private void AssetLoadSE(string fileName)
	{
		AudioClip audio = AssetBundleData.LoadAsset<AudioClip>(fileName);
		if (audio != null)
		{
			SeAudioClip[fileName] = audio;
			Debug.Log(fileName + "アセットから読み込み");
		}
	}

	/// <summary>
	/// アセットバンドルからVOICEを読み込む
	/// </summary>
	/// <param name="fileName"></param>
	private void AssetLoadVOICE(string fileName)
	{
		AudioClip audio = AssetBundleData.LoadAsset<AudioClip>(fileName);
		if (audio != null)
		{
			VoiceAudioClip[fileName] = audio;
			Debug.Log(fileName + "アセットから読み込み");
		}
	}

	/// <summary>
	/// リソースフォルダに保存されているすべてのデータを保存する
	/// </summary>
	public void ResourceLoadAll()
	{
		ResourceLoadAllBGM();
		ResourceLoadAllSE();
		ResourceLoadAllVOICE();
	}

	/// <summary>
	/// 指定したフォルダに保存されているすべてのBGMを読み込む
	/// </summary>
	public void ResourceLoadAllBGM(string addPath = "")
	{
		string path = bgmPath;
		if (addPath != "")
		{
			path = bgmPath + "/" + addPath;
		}
		Debug.Log("BGM読み込み開始");




		object[] bgmList = Resources.LoadAll(path);
		int i = 0;
		foreach (AudioClip bgm in bgmList)
		{
			Debug.Log(bgm.name);
			Debug.Log(bgm);
			BgmAudioClip[bgm.name] = bgm;
			i++;
		}
		Debug.Log("BGMファイル" + i + "個読み込み完了");
	}

	/// <summary>
	/// リソースフォルダに保存されているすべてのSEを読み込む
	/// </summary>
	public void ResourceLoadAllSE(string addPath = "")
	{
		string path = sePath;
		if (addPath != "")
		{
			path = path + "/" + addPath;
		}
		Debug.Log("SE読み込み開始");
		object[] seList = Resources.LoadAll(path);
		int i = 0;
		foreach (AudioClip se in seList)
		{
			Debug.Log(se.name);
			Debug.Log(se);
			BgmAudioClip[se.name] = se;
			i++;
		}
		Debug.Log("SEファイル" + i + "個読み込み完了");
	}

	/// <summary>
	/// リソースフォルダに保存されているすべてのVOICEを読み込む
	/// </summary>
	public void ResourceLoadAllVOICE(string addPath = "")
	{
		string path = voicePath;
		if (addPath != "")
		{
			path = path + "/" + addPath;
		}
		Debug.Log("VOICE読み込み開始");
		object[] audioList = Resources.LoadAll(path);
		int i = 0;
		foreach (AudioClip audio in audioList)
		{
			Debug.Log(audio.name);
			Debug.Log(audio);
			VoiceAudioClip[audio.name] = audio;
			i++;
		}
		Debug.Log("VOICEファイル" + i + "個読み込み完了");
	}

	/// <summary>
	/// 指定したパスに保存されているBGMを読み込む
	/// </summary>
	public void ResourceLoadBGM(string addPath)
	{
		string path = bgmPath;
		if (addPath != "")
		{
			path = path + "/" + addPath;
		}
		Debug.Log("BGM個別読み込み開始");
		AudioClip resorceData = (AudioClip)Resources.Load(path);
		Debug.Log(resorceData.name);
		Debug.Log(resorceData);
		BgmAudioClip[resorceData.name] = resorceData;
		Debug.Log("BGMファイル個別読み込み完了");
	}

	/// <summary>
	/// 指定したパスに保存されているSEを読み込む
	/// </summary>
	public void ResourceLoadSE(string addPath)
	{
		string path = sePath;
		if (addPath != "")
		{
			path = path + "/" + addPath;
		}
		Debug.Log("SE個別読み込み開始");
		AudioClip resorceData = (AudioClip)Resources.Load(path);
		Debug.Log(resorceData.name);
		Debug.Log(resorceData);
		SeAudioClip[resorceData.name] = resorceData;
		Debug.Log("ファイル個別読み込み完了");
	}

	/// <summary>
	/// 指定したパスに保存されているVOICEを読み込む
	/// </summary>
	public void ResourceLoadVOICE(string addPath)
	{
		string path = voicePath;
		if (addPath != "")
		{
			path = path + "/" + addPath;
		}
		Debug.Log("VOICE個別読み込み開始");
		AudioClip resorceData = (AudioClip)Resources.Load(path);
		Debug.Log(resorceData.name);
		Debug.Log(resorceData);
		VoiceAudioClip[resorceData.name] = resorceData;
		Debug.Log("VOICE個別読み込み完了");
	}

	/// <summary>
	/// 読み込んだ全てのAudioClipを破棄する
	/// </summary>
	public void ResourceDestructionAll()
	{
		ResourceDestructionBGM();
		ResourceDestructionSE();
		ResourceDestructionVOICE();
	}
	/// <summary>
	/// 読み込んだ全てのBGMを破棄する
	/// </summary>
	public void ResourceDestructionBGM()
	{
		BgmAudioClip = new Dictionary<string, AudioClip>();
		Resources.UnloadUnusedAssets();
		Debug.Log("BGM破棄");
	}

	/// <summary>
	/// 読み込んだ全てのSEを破棄する
	/// </summary>
	public void ResourceDestructionSE()
	{
		SeAudioClip = new Dictionary<string, AudioClip>();
		Resources.UnloadUnusedAssets();
		Debug.Log("SE破棄");
	}

	/// <summary>
	/// 読み込んだ全てのVOICEを破棄する
	/// </summary>
	public void ResourceDestructionVOICE()
	{
		VoiceAudioClip = new Dictionary<string, AudioClip>();
		Resources.UnloadUnusedAssets();
		Debug.Log("VOICE破棄");
	}




	/// <summary>
	/// 管理しているBgm情報を取得します。
	/// </summary>
	public List<AudioPlayerScript> AudioBgmPlayers
	{
		get { return AudioPlayers.FindAll(ap => ap.useMixerType == mixerType.BGM); }
	}

	/// <summary>
	/// 管理しているSe情報を取得します。
	/// </summary>
	public List<AudioPlayerScript> AudioSePlayers
	{
		get { return AudioPlayers.FindAll(ap => ap.useMixerType == mixerType.SE); }
	}

	/// <summary>
	/// 管理しているVoice情報を取得します。
	/// </summary>
	public List<AudioPlayerScript> AudioVoicePlayers
	{
		get { return AudioPlayers.FindAll(ap => ap.useMixerType == mixerType.VOICE); }
	}

	/// <summary>
	/// プレイヤーの登録を行います。
	/// </summary>
	/// <param name="player"></param>
	public void RegistPlayer(AudioPlayerScript player)
	{
		Debug.Log(player.name + "登録");
		AudioPlayers.Add(player);
	}

	/// <summary>
	/// プレイヤーの登録解除を行います。
	/// </summary>
	/// <param name="player"></param>
	public void UnregistPlayer(AudioPlayerScript player)
	{
		AudioPlayers.Remove(player);
	}

	/// <summary>
	/// 音声ファイルを取得する
	/// ロードされていないAudioクリップを指定した場合はロードする
	/// </summary>
	public AudioClip GetClip(AudioPlayerScript audioPlayerScriptData)
	{
		var bgm = AudioBgmPlayers;
		var se = AudioSePlayers;
		var voice = AudioVoicePlayers;

		AudioClip audioClip = null;
		if (bgm.Contains(audioPlayerScriptData))
		{
			Debug.Log("BGM");
			if (!BgmAudioClip.ContainsKey(audioPlayerScriptData.FileName))
			{
				ResourceLoadBGM(audioPlayerScriptData.FileName);
				if (!BgmAudioClip.ContainsKey(audioPlayerScriptData.FileName))
				{
					if (AssetBundleData != null)
					{
						AssetLoadBGM(audioPlayerScriptData.FileName);
					}

				}
			}
			if (BgmAudioClip.ContainsKey(audioPlayerScriptData.FileName))
			{
				audioClip = BgmAudioClip[audioPlayerScriptData.FileName];
			}
			else
			{
				Debug.LogError("ファイルが存在しません");
			}
		}
		else if (se.Contains(audioPlayerScriptData))
		{
			Debug.Log("SE");
			if (!SeAudioClip.ContainsKey(audioPlayerScriptData.FileName))
			{
				ResourceLoadSE(audioPlayerScriptData.FileName);
				if (!SeAudioClip.ContainsKey(audioPlayerScriptData.FileName))
				{
					if (AssetBundleData != null)
					{
						AssetLoadSE(audioPlayerScriptData.FileName);
					}
				}
			}
			if (SeAudioClip.ContainsKey(audioPlayerScriptData.FileName))
			{
				audioClip = SeAudioClip[audioPlayerScriptData.FileName];
			}
			else
			{
				Debug.LogError("ファイルが存在しません");
			}
		}
		else if (voice.Contains(audioPlayerScriptData))
		{
			Debug.Log("VOICE");
			if (!VoiceAudioClip.ContainsKey(audioPlayerScriptData.FileName))
			{
				ResourceLoadVOICE(audioPlayerScriptData.FileName);
				if (!VoiceAudioClip.ContainsKey(audioPlayerScriptData.FileName))
				{
					if (AssetBundleData != null)
					{
						AssetLoadVOICE(audioPlayerScriptData.FileName);
					}
				}
			}
			if (VoiceAudioClip.ContainsKey(audioPlayerScriptData.FileName))
			{
				audioClip = VoiceAudioClip[audioPlayerScriptData.FileName];
			}
			else
			{
				Debug.LogError("ファイルが存在しません");
			}
		}
		else
		{
			Debug.Log("ミキサーグループに所属していません");
		}
		return audioClip;
	}


	/// <summary>
	/// 再生
	/// </summary>
	/// <param name="audioPlayer">再生するプレイヤー</param>
	/// <param name="isFade">再生中の音楽停止時にフェードアウトするか否か</param>
	public void Play(int id)
	{
		Hashtable config =  audioConfig[id];
		int ch = Convert.ToInt32(config["channel"]);
		double waitSeconds = 0;
		if (ChannelManager[ch] != null)
		{
			//自動フェードアウトがONだった場合はFadeOutSecondsに設定された秒数を使ってフェードアウトする
			if (ChannelManager[ch].IsAutoFadeOut)
			{
				StopFadeOutChannel(ch);
				if (ChannelManager[ch].Source.isPlaying)
				{
					waitSeconds = ChannelManager[ch].FadeOutSeconds;
				}
			}
			else
			{
				ChannelManager[ch].Stop();
			}
		}
		
		if(waitFade != null) //連続実行防止用
		{
			StopCoroutine(waitFade);
		}
		waitFade =  StartCoroutine(WaitFade(waitSeconds, ch, id));
	}


	/// <summary>
	/// 再生(ファイル名手動設定バージョン)
	/// ※注意！CSVで対象chに設定を行っていない場合はsetMixerAndSourceを実行してから行ってください
	/// </summary>
	/// <param name="ch">再生するチャンネル</param>
	/// <param name="fileName">再生するファイル名</param>
	public void Play(int ch, string fileName)
	{
		double waitSeconds = 0;
		if (ChannelManager[ch] != null)
		{
			//自動フェードアウトがONだった場合はFadeOutSecondsに設定された秒数を使ってフェードアウトする
			if (ChannelManager[ch].IsAutoFadeOut)
			{
				StopFadeOutChannel(ch);
				if (ChannelManager[ch].Source.isPlaying)
				{
					waitSeconds = ChannelManager[ch].FadeOutSeconds;
				}
			}
			else
			{
				ChannelManager[ch].Stop();
			}
		}

		if (waitFade != null) //連続実行防止用
		{
			StopCoroutine(waitFade);
		}
		waitFade = StartCoroutine(WaitFade(waitSeconds, ch, fileName));
	}

	/// <summary>
	/// CSVを使用しない場合にchにミキサーの種類とAudioSourceを設定する。
	/// </summary>
	/// <param name="mixer"></param>
	/// <param name="source"></param>
	public void setMixerAndSource(int ch,mixerType mixer, AudioSource source)
	{
		ChannelManager[ch] = channelObjManager[ch].GetComponent<AudioPlayerScript>();
		ChannelManager[ch].Source = source;
		ChannelManager[ch].useMixerType = mixer;
	}

	IEnumerator WaitFade(double seconds, int ch, string fileName)
	{
		yield return new WaitForSeconds((float)seconds);
		if (fileName != "")
		{
			ChannelManager[ch] = channelObjManager[ch].GetComponent<AudioPlayerScript>();
            ChannelManager[ch].fileName = fileName;
        }
		ChannelManager[ch].ExecPlay();
	}


	IEnumerator WaitFade(double seconds, int ch, int id)
	{
		yield return new WaitForSeconds((float)seconds);
		SetConfig(id);
		ChannelManager[ch].ExecPlay();
	}



	/// <summary>
	/// チャンネルすべて再生(一時停止解除用)
	/// </summary>
	public void ResumeAll()
	{
		for (int i = 0; i < ChannelManager.Length; i++)
		{
			ResumeChannel(i);
		}
	}

	/// <summary>
	/// チャンネルすべて停止
	/// </summary>
	public void StopAll()
	{
		for (int i = 0; i < ChannelManager.Length; i++)
		{
			StopChannel(i);
		}
	}

	/// <summary>
	/// チャンネルすべて一時停止
	/// </summary>
	public void PauseAll()
	{
		for (int i = 0; i < ChannelManager.Length; i++)
		{
			if (ChannelManager[i] != null)
			{
				PauseChannel(i);
			}
		}
	}



	/// <summary>
	/// チャンネルすべて
	/// </summary>
	public void StopFadeOutAll()
	{
		for (int i = 0; i < ChannelManager.Length; i++)
		{
			StopFadeOutChannel(i);
		}
	}

	/// <summary>
	/// 指定したチャンネルを再生(一時停止解除用)
	/// </summary>
	public void ResumeChannel(int ch)
	{
		if (ChannelManager[ch] != null)
		{
			if (ChannelManager[ch].IsPause)
			{
				ChannelManager[ch].ExecResume();
			}
		}
		else
		{
			Debug.Log("【ResumeChannel】チャンネル" + ch + "番にはAudioPlayerScriptが設定されていません");
		}
	}
	

	/// <summary>
	/// 指定したチャンネルを停止
	/// </summary>
	public void StopChannel(int ch)
	{
		if (ChannelManager[ch] != null)
		{
			ChannelManager[ch].ExecStop();
		}
		else
		{
			Debug.Log("【StopChannel】チャンネル" + ch + "番にはAudioPlayerScriptが設定されていません");
		}
	}

	/// <summary>
	/// チャンネル一時停止
	/// </summary>
	public void PauseChannel(int ch)
	{
		if (ChannelManager[ch] != null && ChannelManager[ch].Source.isPlaying)
		{
			ChannelManager[ch].ExecPause();
		}
		else
		{
			Debug.Log("【PauseChannel】チャンネル" + ch + "番にはAudioPlayerScriptが設定されていません");
		}
	}

	/// <summary>
	/// 指定したチャンネルをフェードアウト
	/// </summary>
	/// <param name="ch"></param>
	public void StopFadeOutChannel(int ch)
	{
		if (ChannelManager[ch] != null)
		{
			ChannelManager[ch].ExecStopFadeOut();
		}
		else
		{
			Debug.Log("【StopChannel】チャンネル" + ch + "番にはAudioPlayerScriptが設定されていません");
		}
	}

	

	

	

	/// <summary>
	/// 指定したidを保持するAudioPlayerScriptを再生する
	/// 特殊なidを設定するとチャンネル単位orチャンネル全てを
	/// 停止・一時停止・一時停止解除・フェードアウトして停止を行う
	/// </summary>
	/// <param name="audioPlayer">再生するプレイヤー</param>
	/// <param name="ch">チャンネル</param>
	/// <param name="isFade">再生中の音楽停止時にフェードアウトするか否か</param>
	public static void Trigger(int id)
	{
		Debug.Log(id);
		List<AudioPlayerScript> AudioPlayers = instance.AudioPlayers;
		AudioPlayerScript[] ChannelManager = instance.ChannelManager;

		int ch;
		if (TRIGGER_STOP_CH_MIN <= id && id <= TRIGGER_STOP_CH_MAX) //特定のチャンネルを停止する
		{

			//自動フェードアウトがONだった場合はFadeOutSecondsに設定された秒数を使ってフェードアウトする
			ch = id - TRIGGER_STOP_CH_MIN;
			instance.StopChannel(ch);
		}
		else if(id == TRIGGER_STOP_CH_ALL) //全てのチャンネルを停止する
		{
			instance.StopAll();
		}
		else if(TRIGGER_FADE_CH_MIN <= id && id <= TRIGGER_FADE_CH_MAX) //特定のチャンネルをフェードアウトする
		{
			ch = id - TRIGGER_FADE_CH_MIN;
			instance.StopFadeOutChannel(ch);
		}
		else if(id == TRIGGER_FADE_CH_ALL)
		{
			instance.StopFadeOutAll();
        }
		else if (TRIGGER_PAUSE_CH_MIN <= id && id <= TRIGGER_PAUSE_CH_MAX) //特定のチャンネルを一時停止する
		{
			ch = id - TRIGGER_PAUSE_CH_MIN;
			instance.PauseChannel(ch);
		}
		else if (id == TRIGGER_PAUSE_CH_ALL)//全てのチャンネルを一時停止する
		{
			instance.PauseAll();
		}
		else if (TRIGGER_RESUME_CH_MIN <= id && id <= TRIGGER_RESUME_CH_MAX) //特定のチャンネルを一時停止解除する
		{
			ch = id - TRIGGER_RESUME_CH_MIN;
			instance.ResumeChannel(ch);
		}
		else if (id == TRIGGER_RESUME_CH_ALL)//全てのチャンネルを一時停止解除する
		{
			instance.ResumeAll();
		}
		else
		{
			instance.Play(id);			
		}
		
	}

}
