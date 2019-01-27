using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using Skytanet.SimpleDatabase;
using Newtonsoft.Json;

public class JsonConvertersTest {

	public SaveFile saveFile = null;

	[SetUp] public void Init()
    { 
		//creates "testFolder" if it doesn't exists and a save file inside it
		System.IO.DirectoryInfo info = System.IO.Directory.CreateDirectory(System.IO.Path.Combine(Application.persistentDataPath, "testFolder"));
		saveFile = new SaveFile("testing", info.FullName); 
	}

    [TearDown] public void Dispose()
    { 
		//deletes the testing save files
		saveFile.close(); 
		SaveFile.DeleteSaveFile("testing", saveFile.getPath());
	}


	[Test]
	public void Vector3_test() {
		//arrange
		Vector3 original = new Vector3(1, 2, 3);

		//act
		saveFile.set("vector3", original);
		Vector3 deserialized = saveFile.get<Vector3>("vector3");
		
		//assert
		Assert.IsTrue(original.Equals(deserialized));
	}

	[Test]
	public void Vector3Int_test() {
		//arrange
		Vector3Int original = new Vector3Int(1, 2, 3);

		//act
		saveFile.set("vector3int", original);
		Vector3Int deserialized = saveFile.get<Vector3Int>("vector3int");

		//assert
		Assert.IsTrue(original.Equals(deserialized));
	}

		[Test]
	public void Vector2_test() {
		//arrange
		Vector2 original = new Vector2(1, 2);

		//act
		saveFile.set("Vector2", original);
		Vector2 deserialized = saveFile.get<Vector2>("Vector2");

		//assert
		Assert.IsTrue(original.Equals(deserialized));
	}

	[Test]
	public void Vector2Int_test() {
		//arrange
		Vector2Int original = new Vector2Int(1, 2);

		//act
		saveFile.set("Vector2Int", original);
		Vector2Int deserialized = saveFile.get<Vector2Int>("Vector2Int");

		//assert
		Assert.IsTrue(original.Equals(deserialized));
	}

		[Test]
	public void Vector4_test() {
		//arrange
		Vector4 original = new Vector4(1, 2, 3, 4);

		//act
		saveFile.set("Vector4", original);
		Vector4 deserialized = saveFile.get<Vector4>("Vector4");

		//assert
		Assert.IsTrue(original.Equals(deserialized));
	}

	[Test]
	public void List_references_test() {
		//arrange
		SimpleTestObject first = new SimpleTestObject("first");
		SimpleTestObject second = new SimpleTestObject("second");
		IList<SimpleTestObject> list = new List<SimpleTestObject>();
		list.Add(first);
		list.Add(second);
		list.Add(first);

		//act
		saveFile.set("list", list);
		IList<SimpleTestObject> deserialized = saveFile.get<List<SimpleTestObject>>("list");

		//assert
		Assert.IsTrue(Object.ReferenceEquals(deserialized[0], deserialized[2]));
		Assert.IsTrue(deserialized[0].getPrivateString() == "first");
		Assert.IsTrue(deserialized[1].getPrivateString() == "second");
	}

	[Test]
	public void Dictionary_references_test() {
		//arrange
		SimpleTestObject first = new SimpleTestObject("first");
		SimpleTestObject second = new SimpleTestObject("second");
		Dictionary<string, SimpleTestObject> dictionary = new Dictionary<string, SimpleTestObject>();
        dictionary.Add("key1", first);
		dictionary.Add("key2", second);
		dictionary.Add("key3", first);

		//act
		saveFile.set("dictionary", dictionary);
		Dictionary<string, SimpleTestObject> deserialized = saveFile.get<Dictionary<string, SimpleTestObject>>("dictionary");

		//assert
		Assert.IsTrue(Object.ReferenceEquals(deserialized["key1"], deserialized["key3"]));
		Assert.IsTrue(deserialized["key1"].getPrivateString() == "first");
		Assert.IsTrue(deserialized["key2"].getPrivateString() == "second");
	}

	[Test]
	public void Color_test() {
		//arrange
		Color color = new Color(0.1f, 0.2f, 0.3f, 0.4f);
		
		//act
		saveFile.set("color", color);
		Color deserialized = saveFile.get<Color>("color");
		//assert
		Assert.IsTrue(color.Equals(deserialized));
	}

	[Test]
	public void Bounds_test() {
		//arrange
		Bounds original = new Bounds(Vector3.zero, Vector3.one);

		//act
		saveFile.set("Bounds", original);
		Bounds deserialized = saveFile.get<Bounds>("Bounds");

		//assert
		Assert.IsTrue(original.Equals(deserialized));
	}

	[Test]
	public void Matrix4x4_test() {
		//arrange
		Matrix4x4 original = new Matrix4x4(Vector4.zero, Vector4.one, Vector4.zero, Vector4.one);

		//act
		saveFile.set("Matrix4x4", original);
		Matrix4x4 deserialized = saveFile.get<Matrix4x4>("Matrix4x4");

		//assert
		Assert.IsTrue(original.Equals(deserialized));
	}

	[Test]
	public void Quaternion_test() {
		//arrange
		Quaternion original = new Quaternion(1f,2f,3f,4f);

		//act
		saveFile.set("Quaternion", original);
		Quaternion deserialized = saveFile.get<Quaternion>("Quaternion");

		//assert
		Assert.IsTrue(original.Equals(deserialized));
	}

	[Test]
	public void Rect_test() {
		//arrange
		Rect original = new Rect(1f,2f,3f,4f);

		//act
		saveFile.set("Rect", original);
		Rect deserialized = saveFile.get<Rect>("Rect");

		//assert
		Assert.IsTrue(original.Equals(deserialized));
	}

	[Test]
	public void RectOffset_test() {
		//arrange
		RectOffset original = new RectOffset(1,2,3,4);

		//act
		saveFile.set("RectOffset", original);
		RectOffset deserialized = saveFile.get<RectOffset>("RectOffset");
		
		//assert
		Assert.IsTrue(original.left == deserialized.left);
		Assert.IsTrue(original.right == deserialized.right);
		Assert.IsTrue(original.top == deserialized.top);
		Assert.IsTrue(original.bottom == deserialized.bottom);

	}
}
