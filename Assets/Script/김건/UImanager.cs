using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UImanager : MonoBehaviour
{
    public GameObject title;
    private Image titleImg { get { return title.GetComponent<Image>(); } }
    private Fade fade;
    public bool ingame;

    private Game game { get { return gameObject.GetComponent<Game>(); } }

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
        else
        {
            ingame = true;
            game.tempTime = 0;
        }
    }
}
