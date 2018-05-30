using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MiniG_M_GameManager : MonoBehaviour
{
    public static List<string> PieceState;  // 퍼즐 조각의 상태를 저장하는 변수
    public static string GameState = "none";    // 게임의 상태를 저장하는 변수
    public static int s_Stage;  // 난이도를 다른 변수의 넘기는 변수

    public int Stage;       // 게임의 난이도를 설정하는 변수
    public float TimeSec;   // 게임 시간을 저장하는 변수

    private int whatMultiplication; // 퍼즐이 몇 곱하기 몇인지 알려주는 변수
    private int StartPieceNum;      // 퍼즐 조각 검사 시작 번호를 저장하는 변수 
    private float Score = 0;        // 시간의 따라 점수를 다르게 하여 저장하는 변수 

    private bool isExampleImageOn = false;  // 예시 이미지가 켜져있는지 확인하는 변수

    // 퍼즐 조각과 퍼즐 조각의 배경을 한 곳에 모아놓은 부모를 담는 변수
    public Transform PieceParents;
    public RectTransform PieceBgParents;

    // 퍼즐 조각과 퍼즐 조각의 배경 오브젝트를 담는 변수
    public GameObject Piece;
    public GameObject PieceBg;

    // 타이틀과 타이틀 배경 오브젝트를 담는 변수
    public GameObject Title;
    public GameObject TitleBg;

    public GameObject FixMun;   // 고쳐진 Mun 오브젝트를 담는 변수
    public GameObject Result;   // 결과창 오브젝트를 담는 변수
    public GameObject ExampleImage;     // 예시 이미지 오브젝트를 담는 변수

    public Text txtTime;    // 인게임에 표현되는 시간 텍스트를 담는 변수

    private Sprite[] PieceSprite;   // 퍼즐 조각들의 이미지를 담는 변수

    void Start()
    {
        s_Stage = Stage;

        // 게임의 난이도에 따라 몇 곱하기 몇인지 정해준다.
        if (Stage == 1)
            whatMultiplication = 2;
        else if (Stage == 2)
            whatMultiplication = 4;
        else if (Stage == 3)
            whatMultiplication = 8;

        PieceState = new List<string>(new string[whatMultiplication * whatMultiplication]);   // 퍼즐 조각의 개수만큼 배열을 할당

        PieceSprite = Resources.LoadAll<Sprite>("정지윤(UI & Object)/MiniG_Mun/Object/" + Stage + "_mun");     // 이미지 로드

        // 오브젝트의 위치를 조정
        PieceParents.localPosition = new Vector3(-8, 5.4f - PieceSprite[0].rect.height / 100.0f - 0.5f);
        PieceBgParents.localPosition = new Vector3(-400.0f, 0f, 0.0f);
        PieceBgParents.sizeDelta = new Vector2(350.0f * 2 + 80.0f, 350.0f * 2 + 80.0f);
        FixMun.transform.localPosition = new Vector3(-400.0f / 100.0f, 0f, 0.0f);

        // 시간 텍스트의 시작 텍스트를 정해줌
        txtTime.text = "Time : " + TimeSec;

        // 퍼즐 조각과 퍼즐 조각의 배경를 생성 후 조정
        StartCoroutine(CreationPiece());
        StartCoroutine(CreationPieceBg());

        StartCoroutine(TitleTransform());
    }

    void Update()
    {
        // 게임 상태가 start라면 시간 카운트 시작
        if (GameState.Equals("start"))
        {
            StartCoroutine(TimeCount());
            StartCoroutine(PieceCheck());
        }
    }

    private IEnumerator PieceCheck()
    {
        // 퍼즐 개수만큼 반복
        for (int i = StartPieceNum; i < whatMultiplication * whatMultiplication; i++)
        {
            // 퍼즐이 다 맞추어 졌을 때만 게임이 끝나야하므로 하나라도 안 맞춰져있다면 반복문을 정지
            if (PieceState[i] != "locked")
                break;
            else if (PieceState[i] == "locked")
                StartPieceNum = i;
        }
        if (PieceState.TrueForAll(check => check == "locked"))
        {
            GameState = "over";
            StartCoroutine(IE_isGameOver());
        }
        yield return null;
    }

    // 시간을 카운트하는 함수
    private IEnumerator TimeCount()
    {
        if (TimeSec > 0.0f)
        {
            TimeSec -= Time.deltaTime;

            if (TimeSec < 10.0f)
                txtTime.text = "Time : " + TimeSec.ToString().Substring(0, 1);
            else
                txtTime.text = "Time : " + TimeSec.ToString().Substring(0, 2);
        }
        else if (TimeSec < 0.0f)
        {
            txtTime.text = "Time : " + TimeSec.ToString().Substring(1, 1);

            StartCoroutine(IE_isGameOver());
            GameState = "over";
        }
        yield return null;
    }

    // 퍼즐 조각 생성 후 조정하는 함수
    private IEnumerator CreationPiece()
    {
        for (int i = 0; i < whatMultiplication * whatMultiplication; i++)   // 퍼즐 개수만큼 반복
        {
            float randomX = Random.Range(-210.0f, 210.0f);
            float randomY = Random.Range(-210.0f, 210.0f);

            GameObject newGameobject = Instantiate(Piece);
            Transform newTransform = newGameobject.transform;

            newTransform.name = PieceSprite[i].name.Substring(2, PieceSprite[i].name.Length - 2);
            newTransform.position = new Vector3((450.0f + randomX) / 100.0f, (0f + randomY) / 100.0f, 0f);
            newTransform.GetComponent<SpriteRenderer>().sortingOrder = 0;
            newTransform.GetComponent<SpriteRenderer>().sprite = PieceSprite[i];
            newTransform.GetComponent<MovePiece>().PieceNum = i;
            newTransform.SetParent(PieceParents);
        }
        yield return null;

    }

    // 퍼즐 조각의 배경 생성 및 조정하는 함수
    private IEnumerator CreationPieceBg()
    {
        float Width = 350.0f / whatMultiplication * 2;
        float Height = 350.0f / whatMultiplication * 2;

        float X = -(Width / 2 * (whatMultiplication - 1));
        float Y = Height / 2 * (whatMultiplication - 1);

        int pieceCount = 0;

        // 퍼즐 조각의 배경은 행과 열마다 X, Y가 바뀌어서 이중 반복문 사용
        for (int i = 0; i < whatMultiplication; i++)
        {
            for (int j = 0; j < whatMultiplication; j++)
            {
                GameObject newGameObject = Instantiate(PieceBg);
                RectTransform newRectTransform = newGameObject.GetComponent<RectTransform>();

                newRectTransform.name = "mun_" + pieceCount;
                newRectTransform.gameObject.AddComponent<BoxCollider2D>();
                newRectTransform.GetComponent<BoxCollider2D>().isTrigger = true;
                newRectTransform.sizeDelta = new Vector2(Width, Height);
                newRectTransform.SetParent(PieceBgParents);
                newRectTransform.localPosition = new Vector3(X, Y, 0.0f);
                newRectTransform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                newRectTransform.gameObject.SetActive(true);

                X += Width;
                pieceCount++;
            }
            X = -(Width / 2 * (whatMultiplication - 1));
            Y -= Height;
        }
        yield return null;
    }

    private IEnumerator TitleTransform()
    {
        yield return new WaitForSeconds(2.0f);
        // 타이틀과 하얀색 배경 투명하게 만들어줌
        for (float i = 255; i >= 0; i -= 2)
        {
            TitleBg.transform.GetComponent<Image>().color = new Color(255.0f / 255.0f, 255.0f / 255.0f, 255.0f / 255.0f, i / 255.0f);
            Title.transform.GetComponent<Image>().color = new Color(255.0f / 255.0f, 255.0f / 255.0f, 255.0f / 255.0f, i / 255.0f);
            yield return new WaitForSeconds(0.01f);
        }

        // 더이상 사용하지 않으므로 비활성화
        TitleBg.SetActive(false);
        Title.SetActive(false);

        GameState = "start";

        yield return null;
    }

    // 게임이 끝났을 때 실행하는 함수
    private IEnumerator IE_isGameOver()
    {
        // 시간마다 스코어를 다르게 해줌
        if (TimeSec > 48.0f)
            Score = 25.0f;
        else if (TimeSec > 36.0f)
            Score = 20.0f;
        else if (TimeSec > 24.0f)
            Score = 15.0f;
        else if (TimeSec > 12.0f)
            Score = 10.0f;
        else if (TimeSec > 0.0f)
            Score = 5.0f;
        else
            Score = 0.0f;

        StopCoroutine(TimeCount());

        FixMun.GetComponent<AudioManager>().PlayEffectSound(0);

        yield return new WaitForSeconds(2.0f);

        for (float i = 0; i <= 255; i += 2)
        {
            FixMun.GetComponent<SpriteRenderer>().color = new Color(255.0f / 255.0f, 255.0f / 255.0f, 255.0f / 255.0f, i / 255.0f);
            yield return new WaitForSeconds(0.01f);
        }

        yield return new WaitForSeconds(2.0f);

        Result.SetActive(true);
        Result.GetComponent<MiniGameResult>().SetText((int)Score);

        yield return null;
    }

    // 예시 이미지 온오프 함수 (버튼으로 실행)
    public void ExampleImageOnOff()
    {
        if (GameState.Equals("start"))
        {
            // 예시 이미지가 안켜져 있다면 이미지를 활성화 아니면 비활성화
            if (!isExampleImageOn)
            {
                ExampleImage.SetActive(true);
                isExampleImageOn = true;
            }
            else
            {
                ExampleImage.SetActive(false);
                isExampleImageOn = false;
            }
        }
    }
}
