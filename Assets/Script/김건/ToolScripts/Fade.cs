using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Fade : MonoBehaviour
{
    private bool active;

    public void AlphaSet(Image img, float alpha)
    {
        img.color = new Color(img.color.r, img.color.g, img.color.b, alpha);
        active = true;
    }

    public void FadeIn(Image img)
    {
        if (img.color.a < 1)
        {
            img.color = new Color(img.color.r, img.color.g, img.color.b, img.color.a + 0.01f);
        }
        else
            active = false;
    }

    public void FadeOut(Image img)
    {
        if (img.color.a > 0)
        {
            img.color = new Color(img.color.r, img.color.g, img.color.b, img.color.a - 0.01f);
        }
        else
            active = false;
    }

    public bool GetActive()
    {
        return active;
    }
}
