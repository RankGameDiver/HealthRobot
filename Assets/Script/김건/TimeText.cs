using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeText : MonoBehaviour
{
    private Text score { get { return gameObject.GetComponent<Text>(); } }
    private M_VirusGame game { get { return GameObject.Find("Game").GetComponent<M_VirusGame>(); } }

    void Update()
    {
        score.text = "Time : " + game.GetTimer();
    }
}