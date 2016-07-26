using UnityEngine;
using System.Collections;
using System.IO;

public class CleanSlateStartup : MonoBehaviour {

	void Awake () {
        PlayerPrefs.DeleteAll();
        File.Delete(Application.persistentDataPath + "/scores.dat");
    }
}
