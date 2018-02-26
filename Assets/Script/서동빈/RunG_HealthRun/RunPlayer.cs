using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Anima2D;

public class RunPlayer : MonoBehaviour {

    Rigidbody2D m_rigid;
    public Animator m_animator;

    private int jumpCnt;
    private bool isUnHitTime = false;

    public RunGameManager r_GM;
    public float jump;
    public float speed;
    public GameObject bodyImage;
    public int gettingScore;

    public int maxLife;
    public int currentLife;
    public int coin = 0;

    public bool[] healthTime = new bool[6];
    public bool feverTime = false;
    public int doorNum = 0; // 0 null 1 처치실 2 수술실 3 검사실

    public GameObject[] faceObj; // 0 기본 1 웃음 2 슬픔
    public SpriteMeshInstance[] bodyMesh;
    public GameObject[] genderFace;
    public GameObject[] genderHair;
    public GameObject particle;


    public AudioSource[] sound;
    // 0 jump, 1 damage, 3 pong, 4 coin


    // Use this for initialization
    void Start()
    {
        m_rigid = GetComponent<Rigidbody2D>();
        m_animator = GetComponent<Animator>();
        jumpCnt = 2;

        if (PlayerData.gender)
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

    public void Jump()
    {
        if (r_GM.isPlaying)
        {
            if (jumpCnt > 0)
            {
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
            if (jumpCnt == 2)
            {
                m_animator.PlayInFixedTime("Slide");
            }
        }
    }

    public void Stand()
    {
        if(m_animator.GetInteger("PlayerState") == (int)PlayerState.SLIDE)
        {
            m_animator.PlayInFixedTime("Run");
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Ground")
        {
            if (jumpCnt != 2 && m_animator.GetInteger("PlayerState") != (int)PlayerState.DIE)
            {
                m_animator.PlayInFixedTime("Run", 0, 0);
                jumpCnt = 2;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Door")
        {
            r_GM.OnActionButton();
            r_GM.minigameCnt++;

            switch (collision.gameObject.name)
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

        if(collision.gameObject.tag == "Enemy" && !isUnHitTime)
        {
            if (feverTime) collision.GetComponent<Rigidbody2D>().AddForce(new Vector2(3000, 3000));
            else
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

        if (collision.gameObject.tag == "Score")
        {
            switch(collision.gameObject.name)
            {
                case "H1":
                    healthTime[0] = true;
                    r_GM.OnHealthTimeObj(0);
                    break;

                case "E":
                    healthTime[1] = true;
                    r_GM.OnHealthTimeObj(1);
                    break;

                case "A":
                    healthTime[2] = true;
                    r_GM.OnHealthTimeObj(2);
                    break;

                case "L":
                    healthTime[3] = true;
                    r_GM.OnHealthTimeObj(3);
                    break;

                case "T":
                    healthTime[4] = true;
                    r_GM.OnHealthTimeObj(4);
                    break;

                case "H2":
                    healthTime[5] = true;
                    r_GM.OnHealthTimeObj(5);
                    break;
            }

            if (ChecAllHealth() && !feverTime)
            {
                feverTime = true;
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

        if (collision.gameObject.tag == "Heart")
        {
            if (currentLife < maxLife)
            {
                currentLife++;
                r_GM.AddHeart();
                Destroy(collision.gameObject);

            }
            sound[2].PlayScheduled(0);
            Instantiate(particle).transform.position = collision.transform.position;
        }

        if (collision.gameObject.tag =="Coin")
        {
            coin += 10;
            r_GM.t_Coin.text = (System.Convert.ToInt32(r_GM.t_Coin.text) + 10).ToString();
            sound[3].PlayScheduled(0);
            Instantiate(particle).transform.position = collision.transform.position;
            Destroy(collision.gameObject);
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Door")
        {
            r_GM.OffActionButton();
            doorNum = 0;
        }
    }

    IEnumerator UnHitTime()
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


    IEnumerator PlayerGetBigger()
    {
        yield return new WaitForSeconds(0.01f);

        transform.localScale += new Vector3(0.1f, 0.1f);

        if (transform.localScale.x >= 1.5f)
        {
            transform.localScale = new Vector3(1.5f, 1.5f);
            yield return new WaitForSeconds(10f);
            StartCoroutine(PlayerGetSmaller());
        }
        else
        {
            StartCoroutine(PlayerGetBigger());
        }


        yield return false;
    }

    IEnumerator PlayerGetSmaller()
    {
        yield return new WaitForSeconds(0.01f);

        transform.localScale -= new Vector3(0.1f, 0.1f);

        if (transform.localScale.x <= 0.7f)
        {
            transform.localScale = new Vector3(0.7f, 0.7f);
            feverTime = false;
            for (int i = 0; i < healthTime.Length; i++)
            {
                healthTime[i] = false;
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

    public bool ChecAllHealth()
    {
        for(int i = 0; i < healthTime.Length; i++)
        {
            if (!healthTime[i]) return false;
        }

        return true;
    }

    public void Die()
    {
        r_GM.r_GR.SetResult(false, System.Convert.ToInt32(r_GM.t_scoreTex.text), coin, r_GM.treatmentPer);
        r_GM.r_GR.gameObject.SetActive(true);
    }
}
