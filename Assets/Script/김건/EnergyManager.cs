using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnergyManager : MonoBehaviour
{
    private Times Time = new Times();
    public Image[] energy;
    public Sprite[] energyType;
    public Text energyT;
    private int nTime = 0; // 현재 시간
    private int sec = 0;
    private int chargeTime = 0; // 에너지 충전 시간

    void Start()
    {
        sec = System.DateTime.Now.Second;
        nTime = NowTime();
        ImageInit();
    }

    void Update()
    {
        if (sec != System.DateTime.Now.Second)
        {
            nTime = NowTime();
            if (PlayerData.sTime != 0)
                EnergeTimer();
            energyT.text = EnergyT();
        }
        sec = System.DateTime.Now.Second;
    }

    private string EnergyT()
    {
        int tempTime = chargeTime - nTime;

        if (chargeTime == 0)
            return " ";
        return tempTime / Time.min + ":" + tempTime % Time.min;
    }

    private int NowTime()
    {
        return System.DateTime.Now.Hour * Time.hour + System.DateTime.Now.Minute * Time.min
                + System.DateTime.Now.Second * Time.sec;
    }

    private void EnergyCharge()
    {
        energy[PlayerData.energy].sprite = energyType[0];
        PlayerData.energy++;
    }

    public void EnergyUse()
    {
        energy[PlayerData.energy - 1].sprite = energyType[1];
        PlayerData.energy--;
        if (PlayerData.sTime == 0)
            PlayerData.sTime = nTime;
    }

    private void ImageInit()
    {
        for (int i = 0; i < 5; i++)
        {
            if (PlayerData.energy > i)
                energy[i].sprite = energyType[0];
            else
                energy[i].sprite = energyType[1];
        }
    }

    private void EnergeTimer()
    {
        if (nTime < PlayerData.sTime)
            nTime += (Time.hour * 24);

        chargeTime = PlayerData.sTime + Time.min * 5;
        if (nTime >= chargeTime)
        {
            EnergyCharge();
            if (PlayerData.energy < 5)
                PlayerData.sTime = nTime;
            else
            {
                PlayerData.sTime = 0;
                chargeTime = 0;
            }
        }
    }
}