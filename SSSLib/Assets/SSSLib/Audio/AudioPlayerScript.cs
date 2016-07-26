using UnityEngine;
using System.Collections;

public class AudioPlayerScript : MonoBehaviour
{
    /// <summary>
    /// オーディオソース
    /// </summary>
    public AudioSource Source;

	/// <summary>
	/// 呼び出し用ID、設定しなくても良い
	/// </summary>
	public int id;

    /// <summary>
    /// 自動停止時フェードアウトを行うかどうか
    /// </summary>
    public bool IsAutoFadeOut = false;

	/// <summary>
	/// フェードインする時の時間
	/// </summary>
	public double FadeInSeconds = 1.0;

	/// <summary>
	/// フェードアウトする時の時間
	/// </summary>
	public double FadeOutSeconds = 1.0;

	/// <summary>
	/// フェードイン再生中かどうか
	/// </summary>
	bool IsFadeInPlaying = false;

    /// <summary>
    /// フェードアウト再生中かどうか
    /// </summary>
    bool IsFadeOutPlaying = false;

	

    /// <summary>
    /// フェードイン/アウト経過時間
    /// </summary>
    double FadeDeltaTime = 0;

	/// <summary>
	/// 使用するミキサーの種類
	/// </summary>
	public AudioManagerScript.mixerType useMixerType = AudioManagerScript.mixerType.NONE;

    /// <summary>
    /// 一時停止中かどうか
    /// </summary>
    [SerializeField]
    private bool isPause = false;
    public bool IsPause
    {
        get{ return isPause; }
        private set { isPause = value; }
    }

    /// <summary>
    /// 基本ボリューム。
    /// </summary>
    public float BaseVolume = 1;
    public float InitBaseVolume = 1;

	public string fileName;

    public int channel = -1;

    public string FileName
    {
        set { fileName = value; }
        get { return fileName; }
    }
    public AudioManagerScript AudioManagerScriptData;

    /// <summary>
    /// AudioManagerのパス
    /// </summary>
    public string findPath = "AudioManager";

    /// <summary>
    /// 
    /// </summary>
    void Start()
    {
		SetManager();
		Init();
	}

	void SetManager()
	{
		AudioManagerScriptData = GameObject.Find(findPath).GetComponent<AudioManagerScript>();
		AudioManagerScriptData.RegistPlayer(this);
	}

	public void Init()
	{
		Debug.Log(gameObject.name);
		Debug.Log(Source);
		Debug.Log(AudioManagerScriptData);
		if(AudioManagerScriptData == null)
		{
			SetManager();
        }
		Debug.Log(AudioManagerScriptData);
		if (Source == null)
		{
			return;
		}
		switch (useMixerType)
		{
			case AudioManagerScript.mixerType.NONE:
				break;
			case AudioManagerScript.mixerType.BGM:
				Source.outputAudioMixerGroup = AudioManagerScriptData.MixerBgmGroup;
				break;
			case AudioManagerScript.mixerType.SE:
				Source.outputAudioMixerGroup = AudioManagerScriptData.MixerSeGroup;
				break;
			case AudioManagerScript.mixerType.VOICE:
				Source.outputAudioMixerGroup = AudioManagerScriptData.MixerVoiceGroup;
				break;
		}
	}

    /// <summary>
    /// 
    /// </summary>
    public void OnDisable()
    {
		if(Source != null)
		{
			Source.Stop();
		}
        AudioManagerScriptData.UnregistPlayer(this);
    }

    /// <summary>
    /// フレーム毎処理。
    /// </summary>
    void Update()
    {
        if(!IsPause)
        {
            // フェードイン
            if (IsFadeInPlaying)
            {
                FadeDeltaTime += Time.deltaTime;
                if (FadeDeltaTime >= FadeInSeconds)
                {
                    FadeDeltaTime = FadeInSeconds;
                    IsFadeInPlaying = false;
                }
                Source.volume = (float)(FadeDeltaTime / FadeInSeconds) * BaseVolume;
            }

            // フェードアウト
            if (IsFadeOutPlaying)
            {
                FadeDeltaTime += Time.deltaTime;
                if (FadeDeltaTime >= FadeOutSeconds)
                {
                    FadeDeltaTime = FadeOutSeconds;
                    IsFadeOutPlaying = false;
                    Source.Stop();
                }
                Source.volume = (float)(1.0 - FadeDeltaTime / FadeOutSeconds) * BaseVolume;
			}
        }
       
    }

    /// <summary>
    /// 再生
    /// </summary>
    public void Play(string newFileName = "")
    {
        if (newFileName != "")
        {
            FileName = newFileName;
        }
        AudioManagerScriptData.Play(id);
    }

	/// <summary>
	/// 停止
	/// </summary>
	public void Stop()
	{
		AudioManagerScriptData.StopChannel(channel);
		
	}
	

	/// <summary>
	/// 一時停止を行います。
	/// </summary>
	public void Pause()
	{
		if (Source.isPlaying)
		{
			AudioManagerScriptData.PauseChannel(channel);
		}
	}

	/// <summary>
	/// フェードアウトして停止
	/// <param name="fadeSec">フェードアウト完了までの秒数。</param>
	/// </summary>
	public void StopFadeOut()
	{
        AudioManagerScriptData.StopFadeOutChannel(channel);
	}

	/// <summary>
	/// オーディオクリップをセットする。
	/// 通常時は再生するときに自動で読み込んでくれるので
	/// 利用する必要は無し。
	/// </summary>
	/// <param name="newFileName"></param>
	public void SetClip(string newFileName = "")
    {
        if (newFileName != "")
        {
            FileName = newFileName;
        }
        Source.clip = AudioManagerScriptData.GetClip(this);
    }


    /// <summary>
    /// 実際に再生処理を実行する関数、AudioManagerScriptからのみ呼んでよい
    /// </summary>
    public void ExecPlay()
    {
        
        Debug.Log(gameObject.name);
        Source.clip = AudioManagerScriptData.GetClip(this);
        Source.Play();
        Debug.Log(fileName + "再生開始");
        if (true) //フェードアウト中だった場合は設定を上書きしない
        {
			BaseVolume = InitBaseVolume;
			FadeDeltaTime = 0;
            Source.volume = 0;

            if (FadeInSeconds == 0)
            {
                Source.volume = BaseVolume;
                IsFadeInPlaying = false;
				IsFadeOutPlaying = false;
            }
            else
            {
                IsFadeInPlaying = true;
				IsFadeOutPlaying = false;

			}
		}
        IsPause = false;

    }


    /// <summary>
    /// 実際に停止処理を実行する関数、AudioManagerScriptからのみ呼んでよい
    /// </summary>
    public void ExecStop()
    {
        Source.Stop();
        IsPause = false;

    }

   

    /// <summary>
    /// 実際にフェードアウト処理を実行する関数、AudioManagerScriptからのみ呼んでよい
    /// </summary>
    public void ExecStopFadeOut()
    {
        FadeDeltaTime = 0;
        IsFadeOutPlaying = true;
        IsFadeInPlaying = false;
		BaseVolume = Source.volume;

	}

	/// <summary>
	/// 実際に一時停止処理を実行する関数、AudioManagerScriptからのみ呼んでよい
	/// </summary>
	public void ExecPause()
    {
        Source.Pause();
        IsPause = true;
    }

    /// <summary>
    /// 実際に一時停止解除を実行する関数、AudioManagerScriptからのみ呼んでよい
    /// </summary>
    public void ExecResume()
    {

        if (IsPause)
        {
            Debug.Log(gameObject.name);
            Source.Play();
            IsPause = false;
        }   
        

    }
}
