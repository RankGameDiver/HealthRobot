using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UImanager : MonoBehaviour
{
    public GameObject title;
    private Image titleImg { get { return title.GetComponent<Image>(); } }
    public bool ingame;

    private Game game { get { return gameObject.GetComponent<Game>(); } }

    private int deltaTime;

    void Start()
    {
        titleImg.color = new Color(titleImg.color.r, titleImg.color.g, titleImg.color.b, 1.0f);
        ingame = false;
    }

    void Update()
    {
        if (titleImg.color.a > 0)
            titleImg.color = new Color(titleImg.color.r, titleImg.color.g, titleImg.color.b, titleImg.color.a - 0.01f);
        else
        {
            ingame = true;
            game.tempTime = 0;
        }
    }
}
