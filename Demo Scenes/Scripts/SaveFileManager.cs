using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Skytanet.SimpleDatabase;

public class SaveFileManager : MonoBehaviour {

	public GameObject savefileListPrefab;
	public GameObject savefileListPanel;

	public GameObject contentListPrefab;
	public GameObject contentListPanel;
	public Text contentListTitle;

	public GameObject rightBlockImage;
	public GameObject leftBlockImage;
	
    public static SaveFileManager instance;

	public SaveFile saveFile;

	void Awake() {
		if (instance == null) instance = this;
	}

	public void Start() {
		refreshSaveFileList();
	}

	/// <summary>
	/// refreshes the list of savefiles in the scene UI
	/// </summary>
	public void refreshSaveFileList() {
		int childCount = savefileListPanel.transform.childCount;
        for (int i=0; i<childCount; i++) {
            GameObject.Destroy(savefileListPanel.transform.GetChild(i).gameObject);
        }

		string[] dbs = SaveFile.GetSaveFileList();
		for (int i = 0; i < dbs.Length; ++i) {
			GameObject go = GameObject.Instantiate(savefileListPrefab);
			go.transform.Find("Background").transform.Find("Text").GetComponent<Text>().text = dbs[i];
			go.transform.SetParent(savefileListPanel.transform, false);		
		}
	}

	/// <summary>
	/// Deletes a save file and refreshes the scene UI
	/// </summary>
	/// <param name="name">the name of the save file to delete</param>
	public void deleteSavefile(string name) {
		SaveFile.DeleteSaveFile(name);
		refreshSaveFileList();
	}

	/// <summary>
	/// Creates a new save file and refreshes the scene UI
	/// </summary>
	/// <param name="name">name of the save file to create</param>
	public void create(string name) {
		SaveFile sdb = new SaveFile(name);
		sdb.close();
		refreshSaveFileList();
	}

	/// <summary>
	/// Opens a save file and shows its contents on the scene UI
	/// </summary>
	/// <param name="name">name of the save file to open</param>
	public void open(string name) {
		rightBlockImage.SetActive(false);
		leftBlockImage.SetActive(true);
		saveFile = new SaveFile(name);
		contentListTitle.text = name;
		refreshContentList();
	}

	/// <summary>
	/// Refreshes the contents of an openes save file on the UI
	/// </summary>
	public void refreshContentList() {
		int childCount = contentListPanel.transform.childCount;
        for (int i=0; i<childCount; i++) {
            GameObject.Destroy(contentListPanel.transform.GetChild(i).gameObject);
        }

		List<string> keys = saveFile.getKeys();
		for (int i = 0; i < keys.Count; ++i) {
			GameObject go = GameObject.Instantiate(contentListPrefab);
			go.transform.Find("Key").transform.Find("Text").GetComponent<Text>().text = keys[i];
			go.transform.Find("Value").transform.Find("Text").GetComponent<Text>().text = saveFile.get<string>(keys[i]);
			go.transform.SetParent(contentListPanel.transform, false);		
		}
	}

	/// <summary>
	/// Adds a new key-value pair to the save file and refreshes the content UI
	/// </summary>
	/// <param name="key">key of the new pair</param>
	/// <param name="value">value of the new pair</param>
	public void add(string key, string value) {
		saveFile.set(key, value);
		refreshContentList();
	}

	/// <summary>
	/// Closes the currently opened save file
	/// </summary>
	public void close() {
		rightBlockImage.SetActive(true);
		leftBlockImage.SetActive(false);
		saveFile.close();
	}

	/// <summary>
	/// Deletes a key-value pair and refreshes the content UI
	/// </summary>
	/// <param name="key">key of the pair to delete</param>
	public void deleteKey(string key) {
		saveFile.delete(key);
		refreshContentList();
	}

}
