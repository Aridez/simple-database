using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonScript : MonoBehaviour {

	public Text text;
	public Text text2;

	public void DeleteSavefile() {
		SaveFileManager.instance.DeleteSavefile(text.text);
	}

	public void Create() {
		SaveFileManager.instance.Create(text.text);
	}

	public void Refresh() {
		SaveFileManager.instance.RefreshSaveFileList();
	}

	public void Open() {
		SaveFileManager.instance.Open(text.text);
	}

	public void Add() {
		SaveFileManager.instance.Add(text.text, text2.text);
	}

	public void Close() {
		SaveFileManager.instance.Close();
	}

	public void DeleteKey() {
		SaveFileManager.instance.DeleteKey(text.text);
	}
}
