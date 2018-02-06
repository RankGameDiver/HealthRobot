using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mini_S_ObjectManager : MonoBehaviour
{

    public float Speed; // 팔 스피드
    [HideInInspector]
    public bool Collision = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Collision = false;
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        Collision = true;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        Collision = false;
    }
}
