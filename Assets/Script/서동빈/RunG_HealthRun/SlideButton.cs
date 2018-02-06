using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SlideButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler{

    public RunPlayer player;

    bool buttonDown = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (buttonDown) player.Slide();
        else player.Stand();
	}

    public void OnPointerDown(PointerEventData eventData)
    {
        buttonDown = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        buttonDown = false;
    }
}
