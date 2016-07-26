using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public static class CSVLoarder
{
	/// <summary>
	/// 
	/// </summary>
	/// <param name="path"></param>
	/// <returns></returns>
	public static List<Hashtable> LoadCSV(string path)
	{
		List<Hashtable> output = new List<Hashtable>();
		// csvの読み込み.
		string text = (Resources.Load(path) as TextAsset).text;
		// テキストを改行ごとに分割し, 複数の行として取得.
		List<string> rows = new List<string>(GetTextLines(text));
		// 各項目名の取得.
		string[] itemNames = rows[0].Split(',');
		// 各項目名の削除.
		rows.RemoveAt(0);
		// 各項目名をキーに各項目の値を格納しているディクショナリを生成して追加.
		foreach (string row in rows)
		{
			// 各項目の値へと分割.
			string[] itemValues = row.Split(',');
			Hashtable csvDictionary = new Hashtable();
			// 各項目名をキーに各項目の値をディクショナリへ格納.
			for (int i = 0; i < itemNames.Length; ++i)
			{
				if(itemValues[i] == "TRUE" || itemValues[i] == "FALSE")
				{
					itemValues[i] = itemValues[i].ToLower();
				}
				csvDictionary.Add(itemNames[i], itemValues[i]);
			}
			output.Add(csvDictionary);
		}
		return output;
	}

	private static string[] GetTextLines(string t_text)
	{
		if (t_text != null)
		{
			string text = t_text.TrimEnd();
			// テキストデータの前後から\rを取り除く
			text = text.Trim('\r');
			// \rを区切り文字として分割して配列に変換
			string[] textLines = text.Split('\r');
			return textLines;
		}
		else
		{
			Debug.Log("text is null.");
			return null;
		}
	}
}
