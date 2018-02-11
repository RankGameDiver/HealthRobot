using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Game : MonoBehaviour
{
    [SerializeField]
    private int totalScore; // 총 점수
    [SerializeField]
    private float timer; // 미니게임의 타이머
    public GameObject[] enemy;

    private UImanager uiManager { get { return gameObject.GetComponent<UImanager>(); } }
    private RayCheck rayCheck { get { return gameObject.GetComponent<RayCheck>(); } }

    public Sprite[] virusImg;

    private Vector2 touchPos;
    private Touch touch;

    private int deltaTime;
    public int coolTime; // Enemy 생성 대기시간
    public int tempTime = 0;

    void Update()
    {
        Timer();
        rayCheck.Touch();
        if (rayCheck.Ray())
        {
            Enemy s_enemy = rayCheck.Ray("Enemy").GetComponent<Enemy>();
            GameObject g_enemy = rayCheck.Ray("Enemy");
            if (s_enemy.life <= 1)
            {
                totalScore += s_enemy.score;
                g_enemy.SetActive(false);
            }
            else
            {
                s_enemy.life--;
            }
            rayCheck.RayReset();
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
                enemy[i].transform.position = new Vector2(Random.Range(-7.0f, 7.0f), Random.Range(-4.0f, 4.0f));
                enemy[i].SetActive(true);
                i = 10;
            }
        }
    }

    public void Timer()
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
    }

    public int GetTotalScore()
    {
        return totalScore;
    }

}
