using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStart : MonoBehaviour {



	// Use this for initialization
	void Start () {
        gameObject.SetActive(false);
        transform.parent = GameObject.Find("RunGameData(Clone)").transform;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void ReadyGame()
    {
        RunGameManager rgm = GameObject.Find("RunGameManager").GetComponent<RunGameManager>();

        rgm.isPlaying = false;
        rgm.player.speed = 0;
    }

    public void StartGame()
    {
        RunGameManager rgm = GameObject.Find("RunGameManager").GetComponent<RunGameManager>();

        rgm.isPlaying = true;
        rgm.player.speed = 6f;
        rgm.player.m_animator.Play("Run");
        Destroy(gameObject);
    }
}
