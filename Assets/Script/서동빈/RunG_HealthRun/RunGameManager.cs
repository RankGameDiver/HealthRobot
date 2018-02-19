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
    public GameObject StartBtn;
    public Image[] hpObj;
    public Sprite on_hpSprite;
    public Sprite off_hpSprite;

    public bool isPlaying = true;

	// Use this for initialization
	void Start () {
		for(int i =0; i < player.maxLife; i++)
        {
            hpObj[i].gameObject.SetActive(true);
            hpObj[i].sprite = on_hpSprite;
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public GameObject GetPattern()
    {
        GameObject obj = pattern[Random.Range(0, pattern.Length - 1)];
        return Instantiate(obj);
    }

    public void StopGame()
    {
        Time.timeScale = 0;
        StartBtn.SetActive(true);
        isPlaying = false;
    }

    public void StartGame()
    {
        Time.timeScale = 1;
        StartBtn.SetActive(false);
        isPlaying = true;
    }

    public void SubHeart()
    {
        for(int i = player.maxLife - 1; i >= 0; i--)
        {
            if (hpObj[i].sprite == on_hpSprite)
            {
                hpObj[i].sprite = off_hpSprite;
                return;
            }
        }
    }

    public void AddHeart()
    {
        for (int i = 0; i < player.maxLife; i++)
        {
            if (hpObj[i].sprite == off_hpSprite)
            {
                hpObj[i].sprite = off_hpSprite;
                return;
            }
        }
    }

}
