using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextInput : MonoBehaviour
{
    public GameObject inputField;
    public Text inputText; // 텍스트 입력 창
    public Text boxText; // 텍스트 창
    private bool active = false;

    public void MsgBox()
    {
        if (active)
            SetText();
        else
            GetText();   
    }

    private void SetText()
    {
        boxText.text = inputText.text;
        inputField.SetActive(false);
        active = false;
    }

    private void GetText()
    {
        inputField.SetActive(true);
        active = true;
    }
}
