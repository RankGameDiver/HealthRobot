using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapSpawn : MonoBehaviour {

    [SerializeField]
    RunGameManager rGM;

    public static int mapCnt = 0;

    public void Initialize()
    {
        transform.localPosition = new Vector3(transform.position.x + 102, -1.6f, 0);
        for(int i = 0; i < transform.childCount; i++) Destroy(transform.GetChild(i).gameObject);
        Transform pattern;
        if (mapCnt < 5)
        {
            pattern  = rGM.GetPattern().transform;
            mapCnt++;
        }
        else
        {
            pattern = Instantiate(rGM.doorPattern[Random.Range(0, rGM.doorPattern.Length)]).transform;
            mapCnt = 0;
        }
        Debug.Log(mapCnt);
        pattern.parent = transform;
        pattern.localPosition = new Vector3(0, 0);
        GameObject _gagu = Instantiate(rGM.gagu);
        _gagu.transform.parent = transform;
        _gagu.transform.localPosition = Vector3.zero;
    }

    // Use this for initialization
    void Start () {
        if (transform.childCount <= 0 && mapCnt > 0)
        {
            Transform pattern = rGM.GetPattern().transform;
            pattern.parent = transform;
            pattern.localPosition = new Vector3(0, 0);
        }
        GameObject _gagu = Instantiate(rGM.gagu);
        _gagu.transform.parent = transform;
        _gagu.transform.localPosition = Vector3.zero;
        mapCnt++;
    }

    // Update is called once per frame
    void FixedUpdate () {
		if(transform.position.x < -25.5f)
        {
            Initialize();
        }
	}
}
