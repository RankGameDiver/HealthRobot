using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    private float calwidth;
    private float calheight;

    public SpriteRenderer[] sprite;
    public Canvas[] canvas;

	void Start ()
    {
        calwidth = Screen.width / 100.0f;
        calheight = Screen.height / 100.0f;
        Camera.main.orthographicSize = Screen.height / 100.0f / 2.0f;

        for(int i = 0; i < canvas.Length; i++)
        {
            canvas[i].GetComponent<CanvasScaler>().referenceResolution = new Vector2(Screen.width, Screen.height);
        }

        for(int i = 0; i < sprite.Length; i++)
        {
            Vector3 objscale = sprite[i].transform.localScale;

            sprite[i].transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            sprite[i].transform.localScale = new Vector3(calwidth / 19.2f * objscale.x, calheight / 10.8f * objscale.y, 1.0f);
        }
	}
}
