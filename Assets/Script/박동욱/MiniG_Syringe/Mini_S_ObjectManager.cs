using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mini_S_ObjectManager : MonoBehaviour
{
    // 오브젝트 스피드 조정 변수
    public float Speed;

    // 오브젝트의 충돌 판정을 확인하는 변수
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
