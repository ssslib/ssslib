using UnityEngine;
using System.Collections;
using System.Collections.Generic;
/// <summary>
/// シングルトンかつシーンに依存しないオブジェクトに使用する
/// </summary>
public class DontDestroyScript : MonoBehaviour {

    public string obejct_name = "";
    public static Dictionary<string, bool> instances = null;
    public static Dictionary<string, int> call_cnt = null;

    void Awake()
    {
		Debug.Log("Awake実行");
        obejct_name = this.gameObject.name;
        if (instances == null)
        {
            instances = new Dictionary<string, bool>();
            call_cnt = new Dictionary<string, int>();
        }
        if (call_cnt.ContainsKey(obejct_name))
        {
            call_cnt[obejct_name]++;

        }
        else
        {
            call_cnt[obejct_name] = 1;

        }

        Debug.Log("[" + obejct_name + "] 呼び出し回数= " + call_cnt[obejct_name].ToString());

        if (instances.ContainsKey(name))
        {
            Debug.Log("[" + obejct_name + "] は既に存在しているので破棄 " + call_cnt[obejct_name].ToString());
            // 既に存在しているなら削除
            Destroy(this.gameObject);
        }
        else
        {
            Debug.Log("[" + obejct_name + "] 新規生成 ");
            instances[obejct_name] = true;
            // シーン遷移では破棄させない
            DontDestroyOnLoad(this);
        }
    }

	public void Init()
	{
		Awake();
	}

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
