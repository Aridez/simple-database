using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using Skytanet.SimpleDatabase;
using Newtonsoft.Json;
using System.Text.RegularExpressions;
using System.Reflection;

public class SaveFileTest {

	public SaveFile saveFile = null;

	[SetUp] public void Init()
    { 
		//creates "testFolder" if it doesn't exists
		System.IO.DirectoryInfo info = System.IO.Directory.CreateDirectory(System.IO.Path.Combine(Application.persistentDataPath, "testFolder"));
		saveFile = new SaveFile("testing", info.FullName); 
	}

    [TearDown] public void Dispose()
    { 
		//deletes the testing save files
		if (saveFile.IsOpen()) saveFile.Close(); 
		SaveFile.DeleteSaveFile("testing", saveFile.GetPath());
		//clear the console
		/*/*/	
		var assembly = Assembly.GetAssembly(typeof(UnityEditor.ActiveEditorTracker));
        var type = assembly.GetType("UnityEditor.LogEntries");
        var method = type.GetMethod("Clear");
        method.Invoke(new object(), null);
		//*/
	}


	[Test]
	public void OpenOrCreate() {
		//arrange
		SaveFile saveFile2 = null;
		Regex r = new Regex("(Error while opening the save file)");

		//act
		saveFile2 = new SaveFile("testing", saveFile.GetPath());
		
		//assert
		LogAssert.Expect(LogType.Error, r);
		Assert.IsTrue(!saveFile2.IsOpen());
	}

	[Test]
	public void Close() {
		//arrange
		Regex r = new Regex("(You are tring to close a save file that is not open)");

		//close correctly a savefile
		saveFile.Close();
		Assert.IsTrue(!saveFile.IsOpen());

		//throw error when trying to close a closed savefile
		saveFile.Close();
		LogAssert.Expect(LogType.Error, r);
	}

	[Test]
	public void Set() {
		//arrange
		Regex r = new Regex("(You are tring to use \"set\" on a save file that is not open)");

		//checks that a key is set correctly
		saveFile.Set("key", "value");
		Assert.IsTrue(saveFile.HasKey("key"));

		//checks that keys are overriden
		saveFile.Set("key", "value override");
		Assert.IsTrue(saveFile.Get<string>("key").Equals("value override"));

		//throws error when trying to set on a closed save file
		saveFile.Close();
		saveFile.Set("key", "value");
		LogAssert.Expect(LogType.Error, r);
	}

	[Test]
	public void Get() {
		//arrange
		Regex r = new Regex("(You are tring to use \"get\" on a save file that is not open)");

		//gets an unexisting key without default value
		string result = saveFile.Get<string>("key");
		Assert.IsTrue(result == null);

		//gets an unexisting key with default value
		result = saveFile.Get<string>("key", "default");
		Assert.IsTrue(result.Equals("default"));

		//throws error when trying to get on a closed save file
		saveFile.Close();
		saveFile.Get<string>("key");
		LogAssert.Expect(LogType.Error, r);
	}

	[Test]
	public void Delete() {
		//arrange
		Regex r = new Regex("(You are tring to use \"delete\" on a save file that is not open)");

		//checks that a key is deleted correctly
		saveFile.Set("key", "value");
		Assert.IsTrue(saveFile.HasKey("key"));
		saveFile.Delete("key");
		Assert.IsTrue(!saveFile.HasKey("key"));

		//deletes an unexisting key (shouldn't do anything)
		saveFile.Delete("key");
		
		//throws error when trying to delete on a closed save file
		saveFile.Close();
		saveFile.Delete("key");
		LogAssert.Expect(LogType.Error, r);
	}

	[Test]
	public void ReOpen() {
		//arrange
		Regex r = new Regex("(You are tring to open an already opened save file)");
		saveFile.Set("key", "persistent value");

		//tries to reopen an opened save file
		saveFile.ReOpen();
		LogAssert.Expect(LogType.Error, r);

		//checks that data persists between closing and opening a save file
		saveFile.Close();
		saveFile.ReOpen();
		Assert.IsTrue(saveFile.Get<string>("key").Equals("persistent value"));		
	}

	[Test]
	public void DeleteSaveFile() {
		//arrange
		Regex r = new Regex("(Error while deleting the save file)");

		//tries to delete an already opened saveFile
		SaveFile.DeleteSaveFile("testing", saveFile.GetPath());
		LogAssert.Expect(LogType.Error, r);

		//tries to delete an unexisting SaveFile (should do nothing)
		SaveFile.DeleteSaveFile("unexisting", saveFile.GetPath());
		
		//deletes a save file
		saveFile.Close();
		SaveFile.DeleteSaveFile("testing", saveFile.GetPath());
		string[] list = SaveFile.GetSaveFileList(saveFile.GetPath());
		Assert.IsTrue(list.Length == 0);		
	}
}