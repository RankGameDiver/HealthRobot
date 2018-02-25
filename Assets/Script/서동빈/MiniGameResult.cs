using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class MiniGameResult : MonoBehaviour {

    public Text currentPer;
    public Text getPer;
    public Image resultImg;
    public Sprite[] resultSprite;

    int i_get = 0;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SetText(int get)
    {
        RunGameData r_GD = GameObject.Find("RunGameData").GetComponent<RunGameData>();
        getPer.text = get.ToString() + "%";
        currentPer.text = (r_GD.treatmentPer + get).ToString() + "%";

        if (get >= 20) resultImg.sprite = resultSprite[4];
        else if (get >= 15) resultImg.sprite = resultSprite[3];
        else if (get >= 10) resultImg.sprite = resultSprite[2];
        else if (get >= 5) resultImg.sprite = resultSprite[1];
        else resultImg.sprite = resultSprite[0];

        i_get = get;
    }

    public void AddPer()
    {
        RunGameData r_GD = GameObject.Find("RunGameData").GetComponent<RunGameData>();
        r_GD.treatmentPer = r_GD.treatmentPer + i_get;
    }
}
