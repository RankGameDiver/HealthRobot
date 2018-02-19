using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour {

    public Text moneyTxt;

	// Use this for initialization
	void Start () {
		
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
