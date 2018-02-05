using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    [SerializeField]
    private int totalScore; // 총 점수
    [SerializeField]
    private float timer; // 미니게임의 타이머
    public GameObject[] enemy;

    private UImanager uiManager { get { return gameObject.GetComponent<UImanager>(); } }

    public Sprite[] virusImg;

    private Vector2 touchPos;
    private Touch touch;

    private int deltaTime;
    public int coolTime; // Enemy 생성 대기시간
    public int tempTime = 0;

    void Update()
    {
        if (deltaTime != System.DateTime.Now.Second)
        {
            deltaTime = System.DateTime.Now.Second;
            tempTime++;
        }

        if (tempTime >= coolTime && uiManager.ingame)
        {
            CreateEnemy();
            tempTime = 0;
        }

        if (Input.GetMouseButtonDown(0)) // 마우스 클릭시 실행
        {
//            Debug.Log("mouse Click");
            touchPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Ray(touchPos);
        }

        if (Input.touchCount > 0) // 터치시 실행
        {
            touch = Input.GetTouch(0);
            touchPos = Camera.main.ScreenToViewportPoint(touch.position);
            Ray(touchPos);
        }
    }

    public void Ray(Vector2 pos)
    {
        Ray2D ray = new Ray2D(pos, Vector2.zero);
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);
        if (hit.collider != null)
        {
            Debug.Log(hit.collider.gameObject);
            if (hit.collider.gameObject.tag == "Enemy") // Enemy 터치시 실행
            {
                Enemy s_enemy = hit.collider.gameObject.GetComponent<Enemy>();
                GameObject g_enemy = hit.collider.gameObject;
                if (s_enemy.life <= 1)
                {
                    totalScore += s_enemy.score;
                    g_enemy.SetActive(false);
                }
                else
                {
                    s_enemy.life--;
                }
            }
        }
    }

    public void CreateEnemy()
    {
        for (int i = 0; i < 8; i++)
        {
            if (enemy[i].activeInHierarchy)
            { }
            else
            {
                Enemy s_enemy = enemy[i].GetComponent<Enemy>();
                s_enemy.life = 1;
                s_enemy.score = 100;
                s_enemy.enemyKind = Random.Range(0, 4);
                enemy[i].GetComponent<SpriteRenderer>().sprite = virusImg[s_enemy.enemyKind];
                s_enemy.ColliderReset(s_enemy.enemyKind);
                enemy[i].transform.position = new Vector2(Random.Range(-8.0f, 8.0f), Random.Range(-4.0f, 4.0f));
                enemy[i].SetActive(true);
                i = 10;
            }
        }
    }

}
