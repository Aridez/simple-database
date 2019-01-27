using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

public class SimpleTestObject {

    //serialized private property
    [JsonProperty]
    private string privateString = "private";

    public SimpleTestObject() {}

    public SimpleTestObject(string privateString) {
        this.privateString = privateString;
    }

    public string getPrivateString() {
        return privateString;
    }

}
