using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RunGameManager : MonoBehaviour {

    public RunPlayer player;    
    public GameObject[] pattern;
    public GameObject bgIndex;
    public GameObject bgObj;
    public Text t_scoreTex;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public GameObject GetPattern()
    {
        GameObject obj = pattern[Random.Range(0, pattern.Length - 1)];
        return Instantiate(obj);
    }
}
