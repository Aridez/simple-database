using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using UnityEngine;
using Newtonsoft.Json;
using BplusDotNet;
using System.IO;

namespace Skytanet.SimpleDatabase {

    public class SaveFile {

        private BplusTreeBytes tree = null;
        private string filename;
        private string path;
        private bool open = false;

        JsonSerializerSettings defaultSettings = new JsonSerializerSettings { 
                                                        PreserveReferencesHandling = PreserveReferencesHandling.All,
                                                        };
        /// <summary>
        /// Checks if the save file is open.
        /// </summary>
        public bool IsOpen() {
            return open;
        }

        /// <summary>
        /// Gets the name of the current save file. Returns null if the save file wasn't initialized.
        /// </summary>
        public string GetName() {
            return filename;
        }

        /// <summary>
        /// Gets the path where the current save file is being saved. Returns null if the save file wasn't initialized.
        /// </summary>
        public string GetPath() {
            return path;
        }

        /// <summary>
        /// Initializes a new instance of the class. Save files will be saved on the Application.persistentDataPath with the specified filename.
        /// </summary>
        /// <param name="filename">Name of the save file without file extension</param>
        public SaveFile(String filename) : this(filename, Application.persistentDataPath) { }

        /// <summary>
        /// Initializes a new instance of the class. Save files will be saved on the specified path with the specified filename.
        /// </summary>
        /// <param name="filename">Name of the save file without file extension.</param>
        /// <param name="path">Path to the folder that will contain the save files</param>
        public SaveFile(String filename, String path) {
            this.OpenSaveFile(filename, path);
        }

        /// <summary>
        /// Opens again a closed saveFile
        /// </summary>
        public void ReOpen() {
            this.OpenSaveFile(this.GetName(), this.GetPath());
        }

        private void OpenSaveFile(String filename, String path) {
            if (this.IsOpen()) {
                Debug.LogError("You are tring to open an already opened save file (" + "\""+filename+ "\")");
                return;
            }
            try {
                tree = BplusTreeBytes.Initialize(System.IO.Path.Combine(path, filename+".save"), System.IO.Path.Combine(path, filename + ".block"), 255);
            } catch (System.IO.IOException) {
                try {
                    tree = BplusTreeBytes.ReOpen(System.IO.Path.Combine(path, filename + ".save"), System.IO.Path.Combine(path, filename + ".block"));
                }  catch (System.IO.DirectoryNotFoundException e) {
                    Debug.LogError("Error while opening the save file, check that the specified directory exists\n" + e);
                    return;
                } catch (System.IO.IOException e) {
                    Debug.LogError("Error while opening the save file, check that save file" + " \""+filename+ "\" " + "is not already open\n" + e);
                    return;
                }
            }
            this.open = true;
            this.filename = filename;
            this.path = path;
        }

        /// <summary>
        /// Inserts a new key-value pair into the database and commits the changes.
        /// </summary>
        /// <param name="key">Key that identifies the value</param>
        /// <param name="value">Object to be stored</param>
        public void Set(string key, object value) {
            if (!this.IsOpen()) {
                Debug.LogError("You are tring to use \"set\" on a save file that is not open. (" + "\""+filename+ "\")");
                return;
            }
            string json = JsonConvert.SerializeObject(value, defaultSettings);
            byte[] bytes = Encoding.ASCII.GetBytes(json);
            tree[key] = bytes;
            tree.Commit();
        }

        /// <summary>
        /// Gets an object stored in the database identified by a key.
        /// </summary>
        /// <param name="key">Key that identified the object</param>
        /// <param name="defaultValue">(optional) Value to use if the key was not found</param>
        /// <returns>T</returns>
        public T Get<T>(string key, T defaultValue = default(T)) {
            if (!this.IsOpen()) {
                Debug.LogError("You are tring to use \"get\" on a save file that is not open. (" + "\""+filename+ "\")");
                return default(T);
            }
            if (tree.ContainsKey(key)) {
                byte[] bytes = tree[key];
                string json = Encoding.ASCII.GetString(bytes);
                return JsonConvert.DeserializeObject<T>(json, defaultSettings);
            }
            return defaultValue;
        }

        /// <summary>
        /// Deletes a key and its value from the database and commits the changes. Does nothing if the key doesn't exist.
        /// </summary>
        /// <param name="key">Key to delete</param>
        public void Delete(string key) {
            if (!this.IsOpen()) {
                Debug.LogError("You are tring to use \"delete\" on a save file that is not open. (" + "\""+filename+ "\")");
                return;
            }
            try {
                tree.RemoveKey(key);
                tree.Commit();
            } catch(BplusDotNet.BplusTreeKeyMissing) {
                //ignore when trying to delete an unexisting key
            }            
        }

        /// <summary>
        /// Checks if a key exists in the database.
        /// </summary>
        /// <param name="key">Key to check</param>
        /// <returns>True if it exists, false otherwise.</returns>
        public bool HasKey(string key) {
            if (!this.IsOpen()) {
                Debug.LogError("You are tring to use \"hasKey\" on a save file that is not open. (" + "\""+filename+ "\")");
                return false;
            }
            return tree.ContainsKey(key);
        }

        /// <summary>
        /// Gets a list of all the keys in the database. This operation may be slow.
        /// </summary>
        /// <returns>A list of keys in the database.</returns>
        public List<string> GetKeys() {
            if (!this.IsOpen()) {
                Debug.LogError("You are tring to use \"getKeys\" on a save file that is not open. (" + "\""+filename+ "\")");
                return null;
            }
            List<string> keys = new List<string>();
            string key = tree.FirstKey();
            while (key != null) {
                keys.Add(key);
                key = tree.NextKey(key);
            }
            return keys;
        }

        /// <summary>
        /// Shuts down the database, closing the file streams.
        /// </summary>
        public void Close() {
            if (!this.IsOpen()) {
                Debug.LogError("You are tring to close a save file that is not open. (" + "\""+filename+ "\")");
                return;
            }
            tree.Abort();
            tree.Shutdown();
            this.open = false;
        }

        /// <summary>
        /// Tries to delete a save file in the Application.persistentDataPath with the specified filename.
        /// </summary>
        /// <param name="filename">Name of the save file without file extension.</param>
        public static void DeleteSaveFile(string filename) {
            DeleteSaveFile(filename, Application.persistentDataPath);
        }

        /// <summary>
        /// Tries to delete a save file in the specified path
        /// </summary>
        /// <param name="filename">Name of the save file without file extension.</param>
        /// <param name="path">Path where the save file is located</param>
        public static void DeleteSaveFile(string filename, string path) {
            try {
                File.Delete(System.IO.Path.Combine(path, filename + ".save"));
                File.Delete(System.IO.Path.Combine(path, filename + ".block"));
            } catch (System.IO.IOException e) {
                Debug.LogError("Error while deleting the save file, check that save file" + " \"" + filename + "\" " + "is not open and that it exists.\n" + e);
            }
        }

        /// <summary>
        /// Tries to get a list of save files in the Application.persistentDataPath
        /// </summary>
        /// <returns>A list containing the save file names</returns>
        public static String[] GetSaveFileList() {
            return GetSaveFileList(Application.persistentDataPath);
        }

        /// <summary>
        /// Tries to get a list of save files in the specified path
        /// </summary>
        /// <param name="path">Path of the folder</param>
        /// <returns>A list containing the save file names</returns>
        public static String[] GetSaveFileList(string path) {
            DirectoryInfo info = new DirectoryInfo(path);
            FileInfo[] fileInfo = info.GetFiles("*.save");
            String[] result = new String[fileInfo.Length];
            for (int i = 0; i < fileInfo.Length; ++i) {
                result[i] = Path.GetFileNameWithoutExtension(fileInfo[i].Name);
            }
            return result;
        }
    }
}