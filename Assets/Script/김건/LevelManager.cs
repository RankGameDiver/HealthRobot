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
        LvText.text = level.ToString();
        ExpBar.value = exp / 50 * level;
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

        LvText.text = level.ToString();
        ExpBar.value = exp / maxValue;

    }

}