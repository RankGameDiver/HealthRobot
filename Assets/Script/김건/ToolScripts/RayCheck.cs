using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayCheck : MonoBehaviour
{
    private Ray2D[] ray;
    private RaycastHit2D[] hit;
    private Vector2 clickPos;
    private Touch[] touch;
    private Vector2[] touchPos;

    private int touchCount;

    private void Start()
    {
        ray = new Ray2D[10];
        hit = new RaycastHit2D[10];
        touch = new Touch[10];
        touchPos = new Vector2[10];
    }

    public void Touch() // 터치나 클릭시에 좌표 저장
    {
        if (Input.GetMouseButtonDown(0)) // 마우스 클릭시 실행
        {
            clickPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            ray[0] = new Ray2D(clickPos, Vector2.zero);
            hit[0] = Physics2D.Raycast(ray[0].origin, ray[0].direction);
            touchCount = 1;
        }

        if (Input.touchCount > 0) // 터치시 실행
        {
            touchCount = Input.touchCount;
            for (int i = 0; i < Input.touchCount; i++)
            {
                touch[i] = Input.GetTouch(i + 1);
                touchPos[i] = Camera.main.ScreenToViewportPoint(touch[i].position);
                ray[i] = new Ray2D(touchPos[i], Vector2.zero);
                hit[i] = Physics2D.Raycast(ray[i].origin, ray[i].direction);
            }     
        }
    }

    public bool Ray(int temp) // 저장된 좌표에 콜라이더가 있으면 true를 반환
    {
        if (hit[temp].collider != null)
        {
            return true;
        }
        return false;
    }

    public GameObject Ray(int temp, string tag)
    {
        if (hit[temp].collider.gameObject.tag == tag)
        {
            return hit[temp].collider.gameObject;
        }
        return null;
    }

    public void RayReset() // 저장된 좌표 초기화
    {
        System.Array.Clear(ray, 0, ray.Length);
        System.Array.Clear(hit, 0, hit.Length);
        System.Array.Clear(touchPos, 0, touchPos.Length);
        System.Array.Clear(touch, 0, touch.Length);
        touchCount = 0;
    }

    public int GetTouchCount()
    {
        return touchCount;
    }
}
