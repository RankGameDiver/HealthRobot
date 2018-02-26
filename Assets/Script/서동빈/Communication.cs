using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class Communication : MonoBehaviour {

    public int page = 0;
    public string[] communication;
    public Text t_commu;
    public GameObject charSelec;

    // Use this for initialization
    void Start () {
        t_commu.text = communication[0];
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetMouseButtonDown(0))
        {
            page++;
            if(page == 4)
            {
                charSelec.SetActive(true);
                gameObject.SetActive(false);
            }
            if (page >= communication.Length)
            {
                SceneManager.LoadScene("MainMenu");
                SaveLoad saveLoad = new SaveLoad();
                saveLoad.Save();
            }
            else t_commu.text = communication[page];
        }
	}
}
