using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapSpawn : MonoBehaviour {

    [SerializeField]
    RunGameManager rGM;

    public void Initialize()
    {
        transform.localPosition = new Vector3(transform.position.x + 102, -1.6f, 0);
        Destroy(transform.GetChild(0).gameObject);
        Transform pattern = rGM.GetPattern().transform;
        pattern.parent = transform;
        pattern.localPosition = new Vector3(0, 0);
    }

    // Use this for initialization
    void Start () {
        Transform pattern = rGM.GetPattern().transform;
        pattern.parent = transform;
        pattern.localPosition = new Vector3(0, 0);
    }

    // Update is called once per frame
    void FixedUpdate () {
		if(transform.position.x < -25.5f)
        {
            Initialize();
        }
	}
}
