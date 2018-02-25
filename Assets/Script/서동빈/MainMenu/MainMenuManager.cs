using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour {

    public Text moneyTxt;
    public GameObject[] gobj_char;

	// Use this for initialization
	void Start () {
        if (PlayerData.gender) Instantiate(gobj_char[0]);
        else Instantiate(gobj_char[1]);
    }

    // Update is called once per frame
    void Update () {
        Debug.Log(PlayerData.money);
		if(moneyTxt.text != PlayerData.money.ToString())
        {
            moneyTxt.text = PlayerData.money.ToString();
        }
	}
}
