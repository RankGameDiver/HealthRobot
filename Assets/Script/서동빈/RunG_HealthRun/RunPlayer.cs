using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunPlayer : MonoBehaviour {

    Rigidbody2D m_rigid;
    Animator m_animator;

    private int jumpCnt;
    private bool isUnHitTime = false;

    public float jump;
    public float speed;
    public GameObject bodyImage;

	// Use this for initialization
	void Start () {
        m_rigid = GetComponent<Rigidbody2D>();
        m_animator = GetComponent<Animator>();
        jumpCnt = 2;
    }

    // Update is called once per frame
    void Update () {
		
	}

    public void Jump()
    {
        if (jumpCnt > 0)
        {
            m_rigid.velocity = Vector2.zero;
            m_rigid.AddForce(new Vector2(0, jump));
            m_animator.PlayInFixedTime("Jump", 0, 0);
            jumpCnt--;
        }
    }

    public void Slide()
    {
        if (jumpCnt == 2)
        {
            m_animator.PlayInFixedTime("Slide");
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
            isUnHitTime = true;
            StartCoroutine(UnHitTime());
        }

        if (collision.gameObject.tag == "Score")
        {
            Destroy(collision.gameObject);
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
}
