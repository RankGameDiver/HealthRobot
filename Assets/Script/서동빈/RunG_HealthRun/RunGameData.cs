using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunGameData : MonoBehaviour {

    public Vector3[] bgPostionIndex;
    public GameObject[] patternIndex; 
    public int score = 0;
    public int coin = 0;
    public int treatmentPer = 0;
    public bool[] health;
    public int currentLife = 0;
    public int minigameCnt = 0;

    public bool isMapChange = false;

    // Use this for initialization
    void Start () {
        bgPostionIndex = new Vector3[4];
        patternIndex = new GameObject[4];
        health = new bool[6];
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
