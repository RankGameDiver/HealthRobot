using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TrGame : MonoBehaviour {
    
    private float ArmSpeed;
    private float SyringsSpeed;
    private int count = 0;

    private bool GameStart = false;
    private bool Moving = false;
    
    public int HP = 1;

    public GameObject Background;
    public GameObject obj_Title;
    public GameObject obj_HP;
    public GameObject obj_Arm;
    public GameObject obj_Syrings;
    
    private TrObject s_Arm;
    private TrObject s_Syrings;
    
	// Use this for initialization
	void Start ()
    {
        StartCoroutine(TitleTransform());
        StartCoroutine(CreationHP());
        s_Arm = obj_Arm.GetComponent<TrObject>();
        s_Syrings = obj_Syrings.GetComponent<TrObject>();
        ArmSpeed = s_Arm.Speed;
        SyringsSpeed = s_Syrings.Speed;
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (GameStart)
        {
            if (s_Arm.transform.position.x <= -6.9f || obj_Arm.transform.position.x >= 6.7f)
            {
                ArmSpeed *= -1;
                count++;
                if (count % 2 == 0)
                    ArmSpeed += 1;
            }

            s_Arm.transform.Translate(new Vector3(ArmSpeed * Time.deltaTime, 0, 0));  // 팔 이동

            if (Input.GetMouseButtonDown(0) && !Moving) // 화면 클릭시 주사기가 내려옴
            {
                Moving = true;
                StartCoroutine(SyringsMove());
            }
        }
    }

    private IEnumerator SyringsMove()
    {
        while (s_Syrings.transform.position.y >= 2.5)
        {
            s_Syrings.transform.position = s_Syrings.transform.position + new Vector3(0, -SyringsSpeed * Time.deltaTime);
            yield return null;
        }

        if (s_Arm.Collision) // 주사기와 팔이 충돌 했을 시
        {
            ArmSpeed = 0;
            yield return new WaitForSeconds(2.0f);
        }
        else
        {
            Destroy(obj_HP.transform.parent.transform.GetChild(HP - 1).gameObject);
            HP--;
        }

        while (s_Syrings.transform.position.y <= 7.5f)
        {
            s_Syrings.transform.position = s_Syrings.transform.position + new Vector3(0, SyringsSpeed * Time.deltaTime);
            yield return null;
        }
        Moving = false;

    }

    private IEnumerator TitleTransform()
    {
        yield return new WaitForSeconds(2.0f);
        for (float i = 255; i >= 0; i -= 2)
        {
            Background.transform.GetComponent<Image>().color = new Color(255.0f / 255.0f, 255.0f / 255.0f, 255.0f / 255.0f, i / 255.0f);
            obj_Title.transform.GetComponent<Image>().color = new Color(255.0f / 255.0f, 255.0f / 255.0f, 255.0f / 255.0f, i / 255.0f);
            yield return new WaitForSeconds(0.01f);
        }
        Background.SetActive(false);
        obj_Title.SetActive(false);
        GameStart = true;
    }

    private IEnumerator CreationHP()
    {
        for (int i = 1; i < HP; i++)
        {
            GameObject newHp = Instantiate(obj_HP);
            newHp.transform.SetParent(obj_HP.transform.parent.transform);
            newHp.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            yield return null;
        }
    }
}
