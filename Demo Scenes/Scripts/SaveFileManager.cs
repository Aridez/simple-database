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
		RefreshSaveFileList();
	}

	/// <summary>
	/// refreshes the list of savefiles in the scene UI
	/// </summary>
	public void RefreshSaveFileList() {
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
	public void DeleteSavefile(string name) {
		SaveFile.DeleteSaveFile(name);
		RefreshSaveFileList();
	}

	/// <summary>
	/// Creates a new save file and refreshes the scene UI
	/// </summary>
	/// <param name="name">name of the save file to create</param>
	public void Create(string name) {
		SaveFile sdb = new SaveFile(name);
		sdb.Close();
		RefreshSaveFileList();
	}

	/// <summary>
	/// Opens a save file and shows its contents on the scene UI
	/// </summary>
	/// <param name="name">name of the save file to open</param>
	public void Open(string name) {
		rightBlockImage.SetActive(false);
		leftBlockImage.SetActive(true);
		saveFile = new SaveFile(name);
		contentListTitle.text = name;
		RefreshContentList();
	}

	/// <summary>
	/// Refreshes the contents of an openes save file on the UI
	/// </summary>
	public void RefreshContentList() {
		int childCount = contentListPanel.transform.childCount;
        for (int i=0; i<childCount; i++) {
            GameObject.Destroy(contentListPanel.transform.GetChild(i).gameObject);
        }

		List<string> keys = saveFile.GetKeys();
		for (int i = 0; i < keys.Count; ++i) {
			GameObject go = GameObject.Instantiate(contentListPrefab);
			go.transform.Find("Key").transform.Find("Text").GetComponent<Text>().text = keys[i];
			go.transform.Find("Value").transform.Find("Text").GetComponent<Text>().text = saveFile.Get<string>(keys[i]);
			go.transform.SetParent(contentListPanel.transform, false);		
		}
	}

	/// <summary>
	/// Adds a new key-value pair to the save file and refreshes the content UI
	/// </summary>
	/// <param name="key">key of the new pair</param>
	/// <param name="value">value of the new pair</param>
	public void Add(string key, string value) {
		saveFile.Set(key, value);
		RefreshContentList();
	}

	/// <summary>
	/// Closes the currently opened save file
	/// </summary>
	public void Close() {
		rightBlockImage.SetActive(true);
		leftBlockImage.SetActive(false);
		saveFile.Close();
	}

	/// <summary>
	/// Deletes a key-value pair and refreshes the content UI
	/// </summary>
	/// <param name="key">key of the pair to delete</param>
	public void DeleteKey(string key) {
		saveFile.Delete(key);
		RefreshContentList();
	}

}
