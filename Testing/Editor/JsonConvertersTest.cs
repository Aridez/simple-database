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
		saveFile.Close(); 
		SaveFile.DeleteSaveFile("testing", saveFile.GetPath());
	}


	[Test]
	public void Vector3Test() {
		//arrange
		Vector3 original = new Vector3(1, 2, 3);

		//act
		saveFile.Set("vector3", original);
		Vector3 deserialized = saveFile.Get<Vector3>("vector3");
		
		//assert
		Assert.IsTrue(original.Equals(deserialized));
	}

	[Test]
	public void Vector3IntTest() {
		//arrange
		Vector3Int original = new Vector3Int(1, 2, 3);

		//act
		saveFile.Set("vector3int", original);
		Vector3Int deserialized = saveFile.Get<Vector3Int>("vector3int");

		//assert
		Assert.IsTrue(original.Equals(deserialized));
	}

		[Test]
	public void Vector2Test() {
		//arrange
		Vector2 original = new Vector2(1, 2);

		//act
		saveFile.Set("Vector2", original);
		Vector2 deserialized = saveFile.Get<Vector2>("Vector2");

		//assert
		Assert.IsTrue(original.Equals(deserialized));
	}

	[Test]
	public void Vector2IntTest() {
		//arrange
		Vector2Int original = new Vector2Int(1, 2);

		//act
		saveFile.Set("Vector2Int", original);
		Vector2Int deserialized = saveFile.Get<Vector2Int>("Vector2Int");

		//assert
		Assert.IsTrue(original.Equals(deserialized));
	}

		[Test]
	public void Vector4Test() {
		//arrange
		Vector4 original = new Vector4(1, 2, 3, 4);

		//act
		saveFile.Set("Vector4", original);
		Vector4 deserialized = saveFile.Get<Vector4>("Vector4");

		//assert
		Assert.IsTrue(original.Equals(deserialized));
	}

	[Test]
	public void ListReferencesTest() {
		//arrange
		SimpleTestObject first = new SimpleTestObject("first");
		SimpleTestObject second = new SimpleTestObject("second");
		IList<SimpleTestObject> list = new List<SimpleTestObject>();
		list.Add(first);
		list.Add(second);
		list.Add(first);

		//act
		saveFile.Set("list", list);
		IList<SimpleTestObject> deserialized = saveFile.Get<List<SimpleTestObject>>("list");

		//assert
		Assert.IsTrue(Object.ReferenceEquals(deserialized[0], deserialized[2]));
		Assert.IsTrue(deserialized[0].getPrivateString() == "first");
		Assert.IsTrue(deserialized[1].getPrivateString() == "second");
	}

	[Test]
	public void DictionaryReferencesTest() {
		//arrange
		SimpleTestObject first = new SimpleTestObject("first");
		SimpleTestObject second = new SimpleTestObject("second");
		Dictionary<string, SimpleTestObject> dictionary = new Dictionary<string, SimpleTestObject>();
        dictionary.Add("key1", first);
		dictionary.Add("key2", second);
		dictionary.Add("key3", first);

		//act
		saveFile.Set("dictionary", dictionary);
		Dictionary<string, SimpleTestObject> deserialized = saveFile.Get<Dictionary<string, SimpleTestObject>>("dictionary");

		//assert
		Assert.IsTrue(Object.ReferenceEquals(deserialized["key1"], deserialized["key3"]));
		Assert.IsTrue(deserialized["key1"].getPrivateString() == "first");
		Assert.IsTrue(deserialized["key2"].getPrivateString() == "second");
	}

	[Test]
	public void ColorTest() {
		//arrange
		Color color = new Color(0.1f, 0.2f, 0.3f, 0.4f);
		
		//act
		saveFile.Set("color", color);
		Color deserialized = saveFile.Get<Color>("color");
		//assert
		Assert.IsTrue(color.Equals(deserialized));
	}

	[Test]
	public void BoundsTest() {
		//arrange
		Bounds original = new Bounds(Vector3.zero, Vector3.one);

		//act
		saveFile.Set("Bounds", original);
		Bounds deserialized = saveFile.Get<Bounds>("Bounds");

		//assert
		Assert.IsTrue(original.Equals(deserialized));
	}

	[Test]
	public void Matrix4x4Test() {
		//arrange
		Matrix4x4 original = new Matrix4x4(Vector4.zero, Vector4.one, Vector4.zero, Vector4.one);

		//act
		saveFile.Set("Matrix4x4", original);
		Matrix4x4 deserialized = saveFile.Get<Matrix4x4>("Matrix4x4");

		//assert
		Assert.IsTrue(original.Equals(deserialized));
	}

	[Test]
	public void QuaternionTest() {
		//arrange
		Quaternion original = new Quaternion(1f,2f,3f,4f);

		//act
		saveFile.Set("Quaternion", original);
		Quaternion deserialized = saveFile.Get<Quaternion>("Quaternion");

		//assert
		Assert.IsTrue(original.Equals(deserialized));
	}

	[Test]
	public void RectTest() {
		//arrange
		Rect original = new Rect(1f,2f,3f,4f);

		//act
		saveFile.Set("Rect", original);
		Rect deserialized = saveFile.Get<Rect>("Rect");

		//assert
		Assert.IsTrue(original.Equals(deserialized));
	}

	[Test]
	public void RectOffsetTest() {
		//arrange
		RectOffset original = new RectOffset(1,2,3,5);

		//act
		saveFile.Set("RectOffset", original);
		RectOffset deserialized = saveFile.Get<RectOffset>("RectOffset");
		
		//assert
		Assert.IsTrue(original.left == deserialized.left);
		Assert.IsTrue(original.right == deserialized.right);
		Assert.IsTrue(original.top == deserialized.top);
		Assert.IsTrue(original.bottom == deserialized.bottom);

	}
}
