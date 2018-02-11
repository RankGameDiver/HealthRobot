using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayCheck : MonoBehaviour
{
    private Ray2D ray;
    private RaycastHit2D hit;
    private Vector2 touchPos;
    private Touch touch;

    public void Touch() // 터치나 클릭시에 좌표 저장
    {
        if (Input.GetMouseButtonDown(0)) // 마우스 클릭시 실행
        {
            touchPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            ray = new Ray2D(touchPos, Vector2.zero);
            hit = Physics2D.Raycast(ray.origin, ray.direction);
        }

        if (Input.touchCount > 0) // 터치시 실행
        {
            touch = Input.GetTouch(0);
            touchPos = Camera.main.ScreenToViewportPoint(touch.position);
            ray = new Ray2D(touchPos, Vector2.zero);
            hit = Physics2D.Raycast(ray.origin, ray.direction);
        }
    }

    public bool Ray() // 저장된 좌표에 콜라이더가 있으면 true를 반환
    {
        if (hit.collider != null)
        {
            Debug.Log("hit.collider" + hit.collider);
            return true;
        }
        return false;
    }

    public GameObject Ray(string tag)
    {
        if (hit.collider.gameObject.tag == tag)
        {
            return hit.collider.gameObject;
        }
        return null;
    }

    public void RayReset() // 저장된 좌표 초기화
    {
        ray = new Ray2D();
        hit = new RaycastHit2D();
        touchPos = new Vector2();
        touch = new Touch();
    }
}
