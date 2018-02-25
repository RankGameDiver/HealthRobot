using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RunGameResult : MonoBehaviour {

    public Image gradeImg;
    public Sprite[] gradeSprite;

    public Text score;
    public Text coin;
    public Text treatment;
    public Text totalScore;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SetResult(int score, int coin, int treatment)
    {
        this.score.text = score.ToString();
        this.coin.text = coin.ToString();
        this.treatment.text = treatment.ToString() + "%";

        totalScore.text = (score + ((float)score * (float)((float)treatment / 100))).ToString();

        if (treatment >= 80) gradeImg.sprite = gradeSprite[4];
        else if (treatment >= 60) gradeImg.sprite = gradeSprite[3];
        else if (treatment >= 40) gradeImg.sprite = gradeSprite[2];
        else if (treatment >= 20) gradeImg.sprite = gradeSprite[1];
        else gradeImg.sprite = gradeSprite[0];
    }
}
