using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonScript : MonoBehaviour {

	public Text text;
	public Text text2;

	public void deleteSavefile() {
		SaveFileManager.instance.deleteSavefile(text.text);
	}

	public void create() {
		SaveFileManager.instance.create(text.text);
	}

	public void refresh() {
		SaveFileManager.instance.refreshSaveFileList();
	}

	public void open() {
		SaveFileManager.instance.open(text.text);
	}

	public void add() {
		SaveFileManager.instance.add(text.text, text2.text);
	}

	public void close() {
		SaveFileManager.instance.close();
	}

	public void deleteKey() {
		SaveFileManager.instance.deleteKey(text.text);
	}
}
