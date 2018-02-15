using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    private Text score {get{ return gameObject.GetComponent<Text>(); } }
    private M_VirusGame game { get { return GameObject.Find("Game").GetComponent<M_VirusGame>(); } }

    void Update()
    {
        score.text = "Score : " + game.GetTotalScore();
    }
}
