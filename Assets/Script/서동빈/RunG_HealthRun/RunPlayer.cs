using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Anima2D;

public class RunPlayer : MonoBehaviour {

    Rigidbody2D m_rigid; // Player의 RigidBody2D
    public Animator m_animator; // Player의 Animator

    int jumpCnt; // 점프 횟수
    bool isUnHitTime = false; // 무적 시간

    public RunGameManager r_GM; // 인 게임 매니저
    public float jump; // 점프력
    public float speed; // 이동속도
    public int gettingScore; // 획득한 점수

    public int maxLife; // 최대 생명
    public int currentLife; // 현재 생명
    public int gettingCoin = 0; // 획득한 게임 머니

    public bool[] isGetHealthTime = new bool[6]; // Health Time Item들을 먹었는가
    public bool healthTime = false; // 현재 Health Time인가
    public int doorNum = 0; // 0 null 1 처치실 2 수술실 3 검사실 // 현재 충돌한 문의 넘버

    public GameObject[] faceObj; // 0 기본 1 웃음 2 슬픔
    public SpriteMeshInstance[] bodyMesh; // 신체 Mesh Sprite
    public GameObject[] genderFace; // 각 성별의 얼굴
    public GameObject[] genderHair; // 각 성별의 머리카락
    public GameObject particle; // 아이템 획득 Particle


    public AudioSource[] sound;
    // 0 jump, 1 damage, 3 pong, 4 coin


    // Use this for initialization
    void Start()
    {
        m_rigid = GetComponent<Rigidbody2D>();
        m_animator = GetComponent<Animator>();
        jumpCnt = 2;

        if (PlayerData.gender) // Player의 성별에 따라 다른 얼굴과 머리카락
        {
            faceObj[0] = genderFace[0];
            genderFace[0].SetActive(true);
            genderHair[0].SetActive(true);
        }
        else
        {
            faceObj[0] = genderFace[1];
            genderFace[1].SetActive(true);
            genderHair[1].SetActive(true);
        }


        if (currentLife == 0) currentLife = maxLife;
    }

    // Update is called once per frame
    void Update () {

	}

    public void Jump() // 점프 함수
    {
        if (r_GM.isPlaying) // 현재 게임중이라면
        {
            if (jumpCnt > 0) // 점프 횟수가 남아 있다면
            {
                // 점프
                sound[0].PlayScheduled(0);
                m_rigid.velocity = Vector2.zero;
                m_rigid.AddForce(new Vector2(0, jump));
                m_animator.PlayInFixedTime("Jump", 0, 0);
                jumpCnt--;
            }
        }
    }

    public void Slide()
    {
        if (r_GM.isPlaying)
        {
            if (jumpCnt == 2) // 점프 횟수가 2번이라면 현재 지상에 있으므로 점프 횟수 체크
            {
                m_animator.PlayInFixedTime("Slide");
            }
        }
    }

    public void Stand()
    {
        if(m_animator.GetInteger("PlayerState") == (int)PlayerState.SLIDE) // 현재 Player가 슬라이딩 중이라면
        {
            m_animator.PlayInFixedTime("Run");
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Ground") // Ground와 충돌했을 때에
        {
            if (jumpCnt != 2 && m_animator.GetInteger("PlayerState") != (int)PlayerState.DIE) // Die상태가 아니라면
            {
                // Animation을 Run으로 변환
                m_animator.PlayInFixedTime("Run", 0, 0);
                jumpCnt = 2;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Door") // 문과 충돌 했을때
        {
            r_GM.OnActionButton(); // Action Button의 Active를 On으로 바꿈
            r_GM.minigameCnt++; // 현재 미니게임 횟수를 증가

            switch (collision.gameObject.name) // 충돌한 collision의 Object의 이름에 따라
            {
                case "Door1":
                    doorNum = 1;
                    break;

                case "Door2":
                    doorNum = 2;
                    break;

                case "Door3":
                    doorNum = 3;
                    break;
            }
        }

        if(collision.gameObject.tag == "Enemy" && !isUnHitTime) // 무적 상태가 아니며 장애물과 충돌 했을때
        {
            if (healthTime) collision.GetComponent<Rigidbody2D>().AddForce(new Vector2(3000, 3000)); // 무적 상태일 때에는 장애물을 튕겨냄
            else // 아니면 피격
            {
                isUnHitTime = true;
                currentLife--;
                r_GM.SubHeart();
                faceObj[0].SetActive(false);
                faceObj[1].SetActive(false);
                faceObj[2].SetActive(true);

                if(currentLife <= 0)
                {
                    r_GM.isPlaying = false;
                    m_animator.Play("DIE");
                    speed = 0;
                }
                else StartCoroutine(UnHitTime());
            }
            sound[1].PlayScheduled(0);
        }

        if (collision.gameObject.tag == "Score") // 점수 아이템과 충돌 했을때
        {
            switch(collision.gameObject.name) // 만약 Object의 이름이 HEALTH RUN일 경우에는 각 아이템을 획득 한 것 처리
            {
                case "H1":
                    isGetHealthTime[0] = true;
                    r_GM.OnHealthTimeObj(0);
                    break;

                case "E":
                    isGetHealthTime[1] = true;
                    r_GM.OnHealthTimeObj(1);
                    break;

                case "A":
                    isGetHealthTime[2] = true;
                    r_GM.OnHealthTimeObj(2);
                    break;

                case "L":
                    isGetHealthTime[3] = true;
                    r_GM.OnHealthTimeObj(3);
                    break;

                case "T":
                    isGetHealthTime[4] = true;
                    r_GM.OnHealthTimeObj(4);
                    break;

                case "H2":
                    isGetHealthTime[5] = true;
                    r_GM.OnHealthTimeObj(5);
                    break;
            }

            if (ChecAllHealth() && !healthTime) // 만약 모든 HEALTH RUN 아이템을 획득했을 경우에
            {
                // HEALTH TIME으로 변환
                healthTime = true;
                faceObj[0].SetActive(false);
                faceObj[1].SetActive(true);
                faceObj[2].SetActive(false);
                StartCoroutine(PlayerGetBigger());
            }

            int temp_Score = System.Convert.ToInt32(r_GM.t_scoreTex.text) + gettingScore;
            r_GM.t_scoreTex.text = temp_Score.ToString();
            Destroy(collision.gameObject);

            sound[2].PlayScheduled(0);
            Instantiate(particle).transform.position = collision.transform.position ;
        }

        if (collision.gameObject.tag == "Heart") // 회복 아이템을 먹었을 때
        {
            if (currentLife < maxLife) // 현재 체력이 최대 체력보다 낮다면
            {
                currentLife++;
                r_GM.AddHeart();
                Destroy(collision.gameObject);

            }
            sound[2].PlayScheduled(0);
            Instantiate(particle).transform.position = collision.transform.position;
        }

        if (collision.gameObject.tag =="Coin") // 게임 머니 아이템을 먹었을 때
        {
            gettingCoin += 10;
            r_GM.t_Coin.text = (System.Convert.ToInt32(r_GM.t_Coin.text) + 10).ToString();
            sound[3].PlayScheduled(0);
            Instantiate(particle).transform.position = collision.transform.position;
            Destroy(collision.gameObject);
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Door") // 문 Obect에서 벗어났을 때
        {
            if (r_GM.minigameCnt >= 4) // 미니게임 횟수가 4번을 넘겼을 때
            {
                r_GM.EndGame(); // 게임 종료
            }

            r_GM.OffActionButton(); // Action Button의 Active를 Off 해줌
            doorNum = 0; // doorNum 초기화
        }
    }

    IEnumerator UnHitTime() // 무적 시간 Coroutine Player에게 페이드 인 아웃 효과와 isUnHitTime관리
    {
        int countTime = 0;

        while (countTime < 10)
        {

            for (int i = 0; i < bodyMesh.Length; i++)
            {

                if (countTime % 2 == 0)
                    bodyMesh[i].color = new Color32(255, 255, 255, 255);
                else
                    bodyMesh[i].color = new Color32(150, 150, 150, 255);


            }

            yield return new WaitForSeconds(0.2f);

            countTime++;

        }

        for (int i = 0; i < bodyMesh.Length; i++)
            bodyMesh[i].color = new Color32(255, 255, 255, 255);

        faceObj[0].SetActive(true);
        faceObj[1].SetActive(false);
        faceObj[2].SetActive(false);

        isUnHitTime = false;

        yield return false;
    }


    IEnumerator PlayerGetBigger() // Player의 크기가 커지는 Coroutine
    {
        yield return new WaitForSeconds(0.01f);

        transform.localScale += new Vector3(0.1f, 0.1f);

        if (transform.localScale.x >= 1.5f)
        {
            transform.localScale = new Vector3(1.5f, 1.5f);
            yield return new WaitForSeconds(10f);
            isUnHitTime = true;
            StartCoroutine(PlayerGetSmaller());
        }
        else
        {
            StartCoroutine(PlayerGetBigger());
        }


        yield return false;
    }

    IEnumerator PlayerGetSmaller() // Player의 크기가 작아지는 Coroutine
    {
        yield return new WaitForSeconds(0.01f);

        transform.localScale -= new Vector3(0.1f, 0.1f);

        if (transform.localScale.x <= 0.7f)
        {
            transform.localScale = new Vector3(0.7f, 0.7f);
            healthTime = false;
            for (int i = 0; i < isGetHealthTime.Length; i++)
            {
                isGetHealthTime[i] = false;
            }
            r_GM.OffHealthTimeObj();
            faceObj[0].SetActive(true);
            faceObj[1].SetActive(false);
            faceObj[2].SetActive(false);
            StartCoroutine(UnHitTime());
        }
        else
        {
            StartCoroutine(PlayerGetSmaller());
        }


        yield return false;
    }

    public bool ChecAllHealth() // HEALTH TIME 아이템을 모두 획득했는지 체크하는 함수
    {
        for(int i = 0; i < isGetHealthTime.Length; i++)
        {
            if (!isGetHealthTime[i]) return false;
        }

        return true;
    }

    public void Die() // Player가 죽었을때의 함수
    {
        r_GM.r_GR.SetResult(false, System.Convert.ToInt32(r_GM.t_scoreTex.text), gettingCoin, r_GM.treatmentPer);
        r_GM.r_GR.gameObject.SetActive(true);
    }
}
