using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UImanager : MonoBehaviour
{
    public GameObject title;
    private Image titleImg { get { return title.GetComponent<Image>(); } }
    private Fade fade;
    public GameObject[] objArr;
    public bool ingame;

    private M_VirusGame game { get { return gameObject.GetComponent<M_VirusGame>(); } }

    private int deltaTime;

    void Start()
    {
        fade = gameObject.AddComponent<Fade>();
        fade.AlphaSet(titleImg, 1.0f);
        ingame = false;
    }

    void Update()
    {
        if (fade.GetActive())
        {
            fade.FadeOut(titleImg);
        }
        else if (ingame) { }
        else
        {
            ingame = true;
            objArr[0].SetActive(true);
            StartCoroutine(game.SpawnVirus());
        }
    }
}
