using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MiniG_S_GameManager : MonoBehaviour
{
    private bool isMoving = false;        // 주사기가 움직이고 있는지 확인하는 변수

    private string GameState = "none";    // 게임의 진행 상태를 알려주는 변수

    private int count = 0;      // 팔이 좌우를 몇번 왔다갔다 했는지 카운트하는 변수

    private float ArmSpeed;     // 팔의 스피드를 조절하는 변수
    private float SyringeSpeed; // 주사기의 상하 이동속도를 조절하는 변수


    public int HP = 1;      // 체력을 조절하는 변수 변수

    public Sprite DeadHP;    // 죽은 HP 이미지를 저장하는 변수
    public Transform LiveHP;    // 살아있는 HP 이미지를 저장하는 변수

    public Transform Arm;       // 팔 오브젝트를 저장하고 위치나 크기를 바꾸어주는 변수
    public Transform Vessel;    // 혈관 오브젝트를 저장하고 위치나 크기를 바꾸어주는 변수
    public Transform Syringe;   // 주사기 오브젝트를 저장하고 위치나 크기를 바꾸어주는 변수

    public GameObject Band;     // 밴드 오브젝트를 저장하는 변수
    public GameObject Result;   // 결과창 오브젝트를 저장하는 변수

    // 타이틀과 타이틀 배경 오브젝트를 저장하는 변수;
    public GameObject Title;
    public GameObject TitleBg;


    public Animator animSyringe;
    public Animator animEffect;

    // 오브젝트마다 가지고있는 오브젝트매니저를 받아오는 변수
    private Mini_S_ObjectManager s_Arm;
    private Mini_S_ObjectManager s_Vessel;
    private Mini_S_ObjectManager s_Syringe;

    void Start()
    {
        s_Arm = Arm.GetComponent<Mini_S_ObjectManager>();
        s_Vessel = Vessel.GetComponent<Mini_S_ObjectManager>();
        s_Syringe = Syringe.GetComponent<Mini_S_ObjectManager>();

        ArmSpeed = s_Arm.Speed;
        SyringeSpeed = s_Syringe.Speed;
        Result.SetActive(false);

        StartCoroutine(CreationHP());       // HP오브젝트 설정해놓은 HP만큼 생성
        StartCoroutine(TitleTransform());   // 타이틀 조정
    }

    void Update()
    {
        // 게임이 끝나지 않았다면
        if (GameState.Equals("start"))
        {
            int RandomSpeed = Random.Range(2, 5);

            if (Arm.position.x <= -3.1f || Arm.position.x >= 3.75f)   // 팔 움직임 좌우 조작
            {
                ArmSpeed *= -1;
                count++;

                if (count % 2 == 0)     // 팔이 좌우로 한번 왔다갔다하면 스피드 증가
                    ArmSpeed = RandomSpeed;
            }

            Arm.Translate(new Vector3(ArmSpeed * Time.deltaTime, 0, 0));  // 팔 이동

            if (Input.GetMouseButtonDown(0) && !isMoving) // 화면 클릭시 주사기가 내려옴
            {
                isMoving = true;
                StartCoroutine(SyringsMove());
            }
        }
    }

    // 주사기를 움직이게 하고 주사기 판정을 확인하는 함수
    private IEnumerator SyringsMove()
    {
        float Rand = Random.Range(-1.0f, 1.0f);

        // 주사기가 Y좌표까지 내려갈때까지 실행
        while (Syringe.position.y >= 2.5 + Rand)
        {
            Syringe.position += new Vector3(0, -SyringeSpeed * Time.deltaTime);
            yield return null;
        }

        // 주사기와 혈관이 충돌했을시
        if (s_Vessel.Collision)
        {
            ArmSpeed = 0;
            animSyringe.Play("Syringes");
            Syringe.GetComponent<AudioManager>().PlayEffectSound(1);

            yield return new WaitForSeconds(1.0f);

            Result.SetActive(true);
            Result.GetComponent<AudioManager>().PlayEffectSound();
            Result.GetComponent<MiniGameResult>().SetText(HP * 5);
            GameState = "over";
        }
        // 주사기와 팔이 충돌했을시
        else if (s_Arm.Collision)
        {
            LiveHP.parent.GetChild(HP - 1).GetComponent<Image>().sprite = DeadHP;
            HP--;

            GameObject newGameObject = Instantiate(Band);
            Transform newTransform = newGameObject.transform;

            newTransform.SetParent(Vessel.transform);
            newTransform.position = new Vector3(Syringe.position.x - 0.2f, Syringe.position.y - 2.3f, 1.0f);

            Syringe.GetComponent<AudioManager>().PlayEffectSound(2);

            yield return new WaitForSeconds(1.5f);


        }
        // 주사기가 아무것도 충돌 안했을 시
        else
        {
            LiveHP.parent.GetChild(HP - 1).GetComponent<Image>().sprite = DeadHP;
            HP--;

            animSyringe.Play("Syringes");
            animEffect.Play("Effects");

            Syringe.GetComponent<AudioManager>().PlayEffectSound(0);

            yield return new WaitForSeconds(1.0f);
        }

        // 체력이 0이라면 게임 종료
        if (HP == 0)
        {
            Result.SetActive(true);
            Result.GetComponent<AudioManager>().PlayEffectSound();
            Result.GetComponent<MiniGameResult>().SetText(HP * 5);
            GameState = "over";
        }

        while (Syringe.position.y <= 8.0f)      // 주사기가 Y좌표까지 올라갈때까지 실행
        {
            Syringe.position += new Vector3(0, SyringeSpeed * Time.deltaTime);
            yield return null;
        }

        isMoving = false;
    }

    // 타이틀과 타이틀의 배경 알파값을 조절하는 함수
    private IEnumerator TitleTransform()
    {
        yield return new WaitForSeconds(2.0f);

        // 맨 처음 타이틀과 하얀색 배경 투명하게 만들어줌
        for (float i = 255; i >= 0; i -= 2)
        {
            Title.GetComponent<Image>().color = new Color(255.0f / 255.0f, 255.0f / 255.0f, 255.0f / 255.0f, i / 255.0f);
            TitleBg.GetComponent<Image>().color = new Color(255.0f / 255.0f, 255.0f / 255.0f, 255.0f / 255.0f, i / 255.0f);
            yield return new WaitForSeconds(0.01f);
        }

        TitleBg.SetActive(false);
        Title.SetActive(false);

        GameState = "start";
    }

    // HP오브젝트 설정해놓은 HP만큼 생성하는 함수
    private IEnumerator CreationHP()
    {
        // 새로운 HP오브젝트를 생성하고 부모를 하나로 만들어 줌.
        for (int i = 1; i < HP; i++)
        {
            GameObject newGameObject = Instantiate(LiveHP.gameObject);
            Transform newTransform = newGameObject.transform;
            newTransform.SetParent(LiveHP.parent);
            newTransform.localScale = new Vector3(1.5f, 1.5f, 1.0f);
        }
        yield return null;
    }
}
