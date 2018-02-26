using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class M_VirusGame: MonoBehaviour
{
    [SerializeField]
    private int totalScore; // 총 점수
    public int timer; // 미니게임의 타이머
    private bool gameover = false;
    private bool checkResult = false;

    public GameObject[] enemy;
    public int virusLife;

    private UImanager uiManager { get { return gameObject.GetComponent<UImanager>(); } }
    private RayCheck rayCheck { get { return gameObject.GetComponent<RayCheck>(); } }

    public Sprite[] virusImg;
    public Sprite[] virusDieImg;

    private int deltaTime;
    public float coolTime; // Enemy 생성 대기시간

    public GameObject result;
    public GameObject particle;

    void Update()
    {
        Timer();
        TouchCheck();
    }

    private void TouchCheck()
    {
        rayCheck.Touch();
        if (rayCheck.Ray() && !gameover)
        {
            Enemy s_enemy = rayCheck.Ray("Enemy").GetComponent<Enemy>();
            GameObject g_enemy = rayCheck.Ray("Enemy");
            if (s_enemy.life <= 1)
            {
                totalScore += s_enemy.score;
                StartCoroutine(s_enemy.VirusDead());
            }
            else
            {
                s_enemy.life--;           
            }
            s_enemy.GetSound().PlaySound();
            rayCheck.RayReset();
            Instantiate(particle).transform.position = g_enemy.transform.position + new Vector3((float)(Random.Range(0,11) - 5) / 10, (float)(Random.Range(0, 11) - 5) / 10);
        }
    }

    public IEnumerator SpawnVirus()
    {
        while (!gameover)
        {
            StartCoroutine(CreateEnemy());
            yield return new WaitForSeconds(coolTime);
        }
    }

    public IEnumerator CreateEnemy()
    {
        for (int i = 0; i < 8; i++)
        {
            if (enemy[i].activeInHierarchy)
            { }
            else
            {
                Enemy s_enemy = enemy[i].GetComponent<Enemy>();
                s_enemy.enemyKind = Random.Range(0, 4);
                enemy[i].GetComponent<SpriteRenderer>().sprite = virusImg[s_enemy.enemyKind];
                s_enemy.ColliderReset(s_enemy.enemyKind);
                enemy[i].transform.position = new Vector2(Random.Range(-7.0f, 7.0f), Random.Range(-4.0f, 4.0f));
                enemy[i].SetActive(true);
                i = 10;
                yield return null;
            }
        }
    }

    public void Timer()
    {
        if (deltaTime != System.DateTime.Now.Second && timer > 0)
        {
            deltaTime = System.DateTime.Now.Second;
            timer--;
        }

        if (!gameover && uiManager.ingame)
        {
            if (timer <= 0)
            {
                gameover = true;
            }
        }
        else if(gameover && !checkResult) // 타임 아웃시 여기에 추가
        {
            result.SetActive(true);
            result.GetComponent<MiniGameResult>().SetText(GetCureBar());
            checkResult = true;
        }
    }

    public int GetTotalScore()
    {
        return totalScore;
    }

    public int GetTimer()
    {
        return timer;
    }

    public int GetCureBar()
    {
        if (totalScore / 500 > 25)
            return 25;
        else
            return totalScore / 500;
    }

}
