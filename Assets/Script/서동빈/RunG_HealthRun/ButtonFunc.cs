using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonFunc : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public enum BUTTON_KIND
    {
        SLIDE_BTN, STOP_BTN, REWARD_BTN
    }


    public BUTTON_KIND buttonKind;
    public Text serialnumTxt;
    public RunPlayer player;

    bool buttonDown = false;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        switch(buttonKind)
        {
            case BUTTON_KIND.SLIDE_BTN:
                if (buttonDown) player.Slide();
                else
                {
                    if(player.m_animator.GetInteger("PlayerState") != (int)PlayerState.DIE) player.Stand();
                }
                break;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        buttonDown = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        buttonDown = false;
    }

    public void ChangeScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void CheckSerial()
    {
        if (serialnumTxt.text == "HealthRun" || serialnumTxt.text == "Hospital" || serialnumTxt.text == "Out Of The Hospital Tomorrow")
        {
            PlayerData.money += 1000;
        }
    }
    
    public void CreateObj(GameObject obj)
    {
        Instantiate(obj);
    }

    public void ChangeMiniGameScene()
    {
        switch(player.doorNum)
        {
            case 1:
                SceneManager.LoadScene("MiniG_Syringe");
                break;

            case 2:
                SceneManager.LoadScene("MiniG_Mun");
                break;

            case 3:
                SceneManager.LoadScene("MiniG_Virus");
                break;
        }

        player.r_GM.SaveScene();
    }
}
