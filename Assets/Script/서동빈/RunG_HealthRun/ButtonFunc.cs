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
                else player.Stand();
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
        if (serialnumTxt.text == "HealthRun")
        {
            PlayerData.money += 1000;
        }
    }
}
