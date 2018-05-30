using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    public Text LvText;
    public Text ExpPercent;
    private Slider ExpBar { get { return GetComponent<Slider>(); } }
    private int level = 1;
    private float exp = 0;

    void Start()
    {
        ExpText(50 * level);
    }

    public void ExpText(float _maxValue)
    {
        LvText.text = level.ToString();
        ExpBar.value = exp / _maxValue;
        string percent = string.Format("{0:f2}", exp / _maxValue * 100);
        ExpPercent.text = percent + "%";
        //ExpPercent.text = (exp / _maxValue * 100).ToString() + "%";
    }

    public void UpExp()
    {
        exp += 20;
        float maxValue = 50 * level;
        if (exp >= maxValue)
        {
            level += 1;
            exp -= maxValue;
        }
        ExpText(maxValue);
    }

}