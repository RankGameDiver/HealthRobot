using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapSpawn : MonoBehaviour {

    public GameObject bgIndex;
    public GameObject bgObj;
    public GameObject[] patternIndex;

    public void Initialize()
    {
        GameObject obj = Instantiate(bgObj);
        MapSpawn mapSpawnObj = obj.GetComponent<MapSpawn>();

        obj.GetComponent<MoveObject>().player = gameObject.GetComponent<MoveObject>().player;
        mapSpawnObj.bgIndex = bgIndex;
        mapSpawnObj.bgObj = bgObj;
        obj.transform.parent = bgIndex.transform;
        obj.transform.localPosition = new Vector3(transform.position.x + 102, -1.6f, 0);
        obj.name = "bg";
        Destroy(gameObject);

        GameObject obj2 = Instantiate(patternIndex[Random.Range(0, patternIndex.Length)]);
        obj2.transform.parent = transform;
        obj2.transform.localPosition = Vector3.zero;
    }

    // Use this for initialization
    void Start () {
    }

    // Update is called once per frame
    void Update () {
		if(transform.position.x < -25.5f)
        {
            Initialize();
        }
	}
}
