using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

enum S_GameProgress   // 게임의 진행상태 확인
{
    Start,
    Over,
    None
}

public class MiniG_S_GameManager : MonoBehaviour {

    private float VesselSpeed;     // 팔의 좌우 이동속도를 조절하는 변수
    private float ArmSpeed;
    private float SyringeSpeed; // 주사기의 상하 이동속도를 조절하는 변수
    private int count = 0;      // 팔이 좌우를 몇번 왔다갔다 했는지 카운트하는 변수
    private float Rand;

    private S_GameProgress gp_GameProgress = S_GameProgress.None;     // 타이틀이 사라지고 게임 시작했는지 확인하는 변수
    private bool Moving = false;        // 주사기가 움직이고 있는지 확인하는 변수
    
    public int HP = 1;              // 체력 변수
    public int PerHPScore = 30;     // 체력당 스코우 변수
    private int Score = 0;          // 게임 스코어

    public Sprite DistroyHp;
    public GameObject Title_Background;
    public GameObject obj_Title;
    public GameObject obj_HP;
    public GameObject obj_Vessel;
    public GameObject obj_Arm;
    public GameObject obj_Syringe;
    public GameObject obj_Band;
    public GameObject Text_Background;
    public Text txt_Score;
    public Animator anim_Syringe;
    public Animator anim_Effect;

    private Mini_S_ObjectManager s_Vessel;
    private Mini_S_ObjectManager s_Arm;
    private Mini_S_ObjectManager s_Syringe;

    // Use this for initialization
    void Start()
    {
        StartCoroutine(CreationHP());       // HP오브젝트 설정해놓은 HP만큼 생성
        StartCoroutine(TitleTransform());   // 타이틀 조정
        s_Vessel = obj_Vessel.GetComponent<Mini_S_ObjectManager>();
        s_Arm = obj_Arm.GetComponent<Mini_S_ObjectManager>();
        s_Syringe = obj_Syringe.GetComponent<Mini_S_ObjectManager>();
        VesselSpeed = s_Vessel.Speed;
        ArmSpeed = s_Arm.Speed;
        SyringeSpeed = s_Syringe.Speed;
    }

    // Update is called once per frame
    void Update()
    {
        switch(gp_GameProgress)     // 게임의 진행상태에 따라 게임 상태를 변경
        {
            case S_GameProgress.Start:
                {
                    if (s_Vessel.transform.position.x <= -3.1f || s_Vessel.transform.position.x >= 3.75f)   // 팔 움직임 좌우 조작
                    {
                        VesselSpeed *= -1;
                        count++;
                        if (count % 2 == 0)     // 팔이 좌우로 한번 왔다갔다하면 스피드 증가
                            VesselSpeed += 1;
                    }

                    s_Vessel.transform.Translate(new Vector3(VesselSpeed * Time.deltaTime, 0, 0));  // 팔 이동

                    if (s_Arm.transform.position.x <= -3.1f || s_Arm.transform.position.x >= 3.75f)   // 팔 움직임 좌우 조작
                    {
                        ArmSpeed *= -1;
                        if (count % 2 == 0)     // 팔이 좌우로 한번 왔다갔다하면 스피드 증가
                            ArmSpeed += 1;
                    }

                    s_Arm.transform.Translate(new Vector3(VesselSpeed * Time.deltaTime, 0, 0));  // 팔 이동

                    if (Input.GetMouseButtonDown(0) && !Moving) // 화면 클릭시 주사기가 내려옴
                    {
                        Moving = true;
                        StartCoroutine(SyringsMove());
                    }
                    break;
                }
            case S_GameProgress.Over:
                {
                    txt_Score.text = Score.ToString();
                    Text_Background.SetActive(true);
                    break;
                }
            case S_GameProgress.None:     // 여기에서 할 일은 Start함수에 넣자
                {   break;  }
        }
    }

    private IEnumerator SyringsMove()
    {
        Rand = Random.Range(-1.0f, 1.0f);
        while (s_Syringe.transform.position.y >= 2.5 + Rand)       // 주사기가 Y좌표까지 내려갈때까지 실행
        {
            s_Syringe.transform.position = s_Syringe.transform.position + new Vector3(0, -SyringeSpeed * Time.deltaTime);
            yield return null;
        }

        if (s_Vessel.Collision) // 주사기와 혈관이 충돌했을시
        {
            VesselSpeed = 0;
            Score = HP * PerHPScore;
            anim_Syringe.Play("Syringes");
            yield return new WaitForSeconds(1.0f);
            gp_GameProgress = S_GameProgress.Over;
        }
        else if (s_Arm.Collision) // 주사기와 팔이 충돌했을시
        {
            obj_HP.transform.parent.transform.GetChild(HP - 1).GetComponent<Image>().sprite = DistroyHp;
            HP--;
            //anim_Syringe.Play("Syringes");
            GameObject newBand = Instantiate(obj_Band);
            newBand.transform.SetParent(obj_Vessel.transform);
            newBand.transform.position = new Vector3(obj_Syringe.transform.position.x + 0.2f, obj_Syringe.transform.position.y - 2.3f, 1.0f);
            yield return new WaitForSeconds(1.5f);
            if (HP == 0)     // 체력이 0이라면 게임 종료
            {
                Score = 0;
                gp_GameProgress = S_GameProgress.Over;
            }
        }
        else                 // 주사기가 아무것도 충돌 안했을 시
        {
            obj_HP.transform.parent.transform.GetChild(HP - 1).GetComponent<Image>().sprite = DistroyHp;
            HP--;
            Debug.Log(s_Syringe.transform.position.y);
            anim_Syringe.Play("Syringes");
            anim_Effect.Play("Effects");
            yield return new WaitForSeconds(1.0f);
            if (HP == 0)     // 체력이 0이라면 게임 종료
            {
                Score = 0;
                gp_GameProgress = S_GameProgress.Over;
            }
        }

        while (s_Syringe.transform.position.y <= 8.0f)      // 주사기가 Y좌표까지 올라갈때까지 실행
        {
            s_Syringe.transform.position = s_Syringe.transform.position + new Vector3(0, SyringeSpeed * Time.deltaTime);
            yield return null;
        }
        Moving = false;

    }

    private IEnumerator TitleTransform()
    {
        yield return new WaitForSeconds(2.0f);  // 맨 처음 타이틀을 2초간 보여준다.
        for (float i = 255; i >= 0; i -= 2)     // 맨 처음 타이틀과 하얀색 배경 투명하게 만들어줌
        {
            Title_Background.transform.GetComponent<Image>().color = new Color(255.0f / 255.0f, 255.0f / 255.0f, 255.0f / 255.0f, i / 255.0f);
            obj_Title.transform.GetComponent<Image>().color = new Color(255.0f / 255.0f, 255.0f / 255.0f, 255.0f / 255.0f, i / 255.0f);
            yield return new WaitForSeconds(0.01f);
        }
        Title_Background.SetActive(false);  // 있어도 그만 없어도 그만인 코드
        obj_Title.SetActive(false);         // 있어도 그만 없어도 그만인 코드
        gp_GameProgress = S_GameProgress.Start;   // 게임이 시작됬다는걸 알려줌
    }

    private IEnumerator CreationHP()
    {
        for (int i = 1; i < HP; i++)    // 새로운 HP오브젝트를 생성하고 부모를 하나로 만들어 줌.
        {
            GameObject newHp = Instantiate(obj_HP);
            newHp.transform.SetParent(obj_HP.transform.parent.transform);
            newHp.transform.localScale = new Vector3(1.5f, 1.5f, 1.0f);
        }
        yield return null;
    }
}
