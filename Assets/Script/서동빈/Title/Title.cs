using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;

public class Title : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetMouseButtonDown(0))
        {
            if (File.Exists(Application.dataPath + "/Data/SaveData.json"))
            {
                SaveLoad temp = new SaveLoad();
                temp.Load();
                SceneManager.LoadScene("MainMenu");
            }
            else SceneManager.LoadScene("CharacterCreate");
        }
    }
}
