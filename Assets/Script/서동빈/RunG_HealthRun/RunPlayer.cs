using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class RunPlayer : MonoBehaviour {

    Rigidbody2D m_rigid;
    Animator m_animator;

    private int jumpCnt;
    private bool isUnHitTime = false;

    public RunGameManager r_GM;
    public float jump;
    public float speed;
    public GameObject bodyImage;
    public int gettingScore;

    public int maxLife;
    public int currentLife;

    public bool[] healthTime = new bool[6];
    public bool feverTime = false;

	// Use this for initialization
	void Start () {
        m_rigid = GetComponent<Rigidbody2D>();
        m_animator = GetComponent<Animator>();
        jumpCnt = 2;

        currentLife = maxLife;
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
            if (jumpCnt != 2)
            {
                m_animator.PlayInFixedTime("Run", 0, 0);
                jumpCnt = 2;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Enemy" && !isUnHitTime)
        {
            if (feverTime) collision.GetComponent<Rigidbody2D>().AddForce(new Vector2(3000, 3000));
            else
            {
                isUnHitTime = true;
                r_GM.SubHeart();
            }
            //StartCoroutine(UnHitTime());
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
                StartCoroutine(PlayerGetBigger());
            }

            int temp_Score = System.Convert.ToInt32(r_GM.t_scoreTex.text) + gettingScore;
            r_GM.t_scoreTex.text = temp_Score.ToString();
            Destroy(collision.gameObject);
        }

        if (collision.gameObject.tag == "Score")
        {
            if (currentLife < maxLife)
            {
                currentLife++;
                r_GM.AddHeart();
            }
        }
    }

    IEnumerator UnHitTime()
    {
        int countTime = 0;

        //for(int)
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();

        while(countTime < 10)
        {
            if (countTime % 2 == 0)
                spriteRenderer.color = new Color32(255, 255, 255, 90);
            else
                spriteRenderer.color = new Color32(255, 255, 255, 180);

            yield return new WaitForSeconds(0.2f);

            countTime++;
        }

        spriteRenderer.color = new Color32(255, 255, 255, 255);

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
            yield return new WaitForSeconds(5f);
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
}
