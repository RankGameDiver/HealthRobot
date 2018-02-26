using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveObject : MonoBehaviour {

    private RunPlayer player;
    public int percent = 100;

	// Use this for initialization
	void Start () {
        player = GameObject.Find("Player").GetComponent<RunPlayer>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        transform.Translate(new Vector3(-player.speed * percent * Time.deltaTime / 100, 0));
    }
}
