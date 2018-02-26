using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

enum M_GameProgress   // 게임의 진행상태 확인
{
    Start,
    Over,
    None
}

public class MiniG_M_GameManager : MonoBehaviour
{
    public static int s_Stage = 1;
    public int Stage = 1;
    public float TimeSec;
    private int WhatMultiplication;
    private int pieceCount = 0;
    private bool Timeset = false;
    private float Score = 0;
    private bool GameOver = false;
    private bool ExampleImageOn = false;

    public GameObject PieceParents;
    public GameObject Piece;
    public GameObject PieceBackground;
    public GameObject PieceBackgroundParents;
    public GameObject OnTheMendMun;
    public GameObject Title_Background;
    public GameObject obj_Title;
    public GameObject Text_Background;
    public GameObject ExampleImage;
    public GameObject ExampleText;
    public Text txt_time;

    private M_GameProgress gp_GameProgress = M_GameProgress.None;

    private Sprite[] newPieceSprite;
    public static string[] PieceState;
    public static string GameState = "None";

    private void Awake()
    {
        
    }

    void Start ()
    {
        s_Stage = Stage;

        if (Stage == 1)
        {
            WhatMultiplication = 2;
        }
        else if (Stage == 2)
        {
            WhatMultiplication = 4;
        }
        else if (Stage == 3)
        {
            WhatMultiplication = 8;
        }
        PieceState = new string[WhatMultiplication * WhatMultiplication];

        newPieceSprite = Resources.LoadAll<Sprite>("정지윤(UI & Object)/MiniG_Mun/Object/" + Stage + "_mun");

        PieceParents.GetComponent<RectTransform>().position = new Vector3(-8, 5.4f - newPieceSprite[0].rect.height / 100.0f - 0.5f);
        PieceParents.GetComponent<RectTransform>().sizeDelta = new Vector2(newPieceSprite[0].rect.width / 100.0f, newPieceSprite[0].rect.height / 100.0f);
        //OnTheMendMun.transform.position = new Vector3(-8, 5.4f - newPieceSprite[0].rect.height / 100.0f - 0.5f);
        txt_time.text = TimeSec + "sec";

        StartCoroutine(CreationPiece());
        StartCoroutine(CreationPieceBackground());
        StartCoroutine(TitleTransform());
    }

    void Update ()
    {
        switch (gp_GameProgress)     // 게임의 진행상태에 따라 게임 상태를 변경
        {
            case M_GameProgress.Start:
                {
                    StartCoroutine(TimeCount());

                    for (int i = 0; i < WhatMultiplication * WhatMultiplication; i++)
                    {
                        if (PieceState[i] != "locked")
                        {
                            GameOver = false;
                            break;
                        }
                        else
                        {
                            GameOver = true;
                        }
                    }

                    if (GameOver)
                    {
                        gp_GameProgress = M_GameProgress.Over;
                        GameState = "Over";
                        StartCoroutine(IE_GameOver());
                    }

                    break;
                }
            case M_GameProgress.Over:
                {
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
                    else if (TimeSec < 0.0f)
                    {
                        Score = 0.0f;
                        Text_Background.SetActive(true);
                        Text_Background.GetComponent<MiniGameResult>().SetText((int)Score);
                    }
                    StopCoroutine(TimeCount());
                    ExampleImage.GetComponent<SpriteRenderer>().sprite = OnTheMendMun.GetComponent<SpriteRenderer>().sprite;
                    break;
                }
            case M_GameProgress.None:     // 여기에서 할 일은 Start함수에 넣자
                { break; }
        }
    }

    private IEnumerator TimeCount()
    {
        if (TimeSec > 0.0f && Timeset == false)
        {
            TimeSec -= Time.deltaTime;
            if(TimeSec < 10.0f)
                txt_time.text = "Time : " + TimeSec.ToString().Substring(0, 1);
            else
                txt_time.text = "Time : " + TimeSec.ToString().Substring(0, 2);
        }
        else if (TimeSec < 0.0f && Timeset == false)
        {
            txt_time.text = "Time : " + TimeSec.ToString().Substring(1, 1);
            Score = 0;
            gp_GameProgress = M_GameProgress.Over;
            GameState = "Over";
        }
        yield return null;
    }

    private IEnumerator CreationPiece()
    {
        for (int i = 0; i < WhatMultiplication * WhatMultiplication; i++)    
        {
            float randomX = Random.Range(-210.0f, 210.0f);
            float randomY = Random.Range(-210.0f, 210.0f);
            GameObject newPiece = Instantiate(Piece);
            newPiece.name = newPieceSprite[i].name.Substring(2, newPieceSprite[i].name.Length - 2);
            newPiece.transform.position = new Vector3((450.0f + randomX) / 100.0f, (0f + randomY) / 100.0f, 0f);
            newPiece.GetComponent<Renderer>().sortingOrder = 0;
            newPiece.GetComponent<RectTransform>().sizeDelta = new Vector2(newPieceSprite[i].rect.width / 100.0f, newPieceSprite[i].rect.height / 100.0f);
            newPiece.GetComponent<SpriteRenderer>().sprite = newPieceSprite[i];
            newPiece.transform.SetParent(PieceParents.transform);
            newPiece.GetComponent<MovePiece>().PieceNum = i;
            newPiece.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        }
        yield return null;

    }

    private IEnumerator CreationPieceBackground()
    {
        float Width  = 350.0f / WhatMultiplication * 2; 
        float Height = 350.0f / WhatMultiplication * 2;


        float X = -(Width / 2 * (WhatMultiplication - 1)) - 400.0f;
        float Y = Height / 2 * (WhatMultiplication - 1);

        PieceBackgroundParents.transform.position = new Vector3(0 - 400.0f / 100.0f, 0f, 0.0f);
        OnTheMendMun.transform.position = new Vector3(0 - 400.0f / 100.0f, 0f, 0.0f);
        PieceBackgroundParents.GetComponent<RectTransform>().sizeDelta = new Vector2(350.0f * 2 + 80.0f, 350.0f * 2 + 80.0f);

        for (int i = 0; i < WhatMultiplication; i++)
        {
            for (int j = 0; j < WhatMultiplication; j++)
            {
                GameObject newPieceBackground = Instantiate(PieceBackground);
                newPieceBackground.name = "mun_" + pieceCount;
                newPieceBackground.AddComponent<BoxCollider2D>();
                newPieceBackground.GetComponent<BoxCollider2D>().isTrigger = true;
                newPieceBackground.GetComponent<RectTransform>().sizeDelta = new Vector2(Width, Height);
                newPieceBackground.transform.position = new Vector3(X / 100.0f, Y / 100.0f, 0.0f);
                newPieceBackground.transform.SetParent(PieceBackgroundParents.transform);
                newPieceBackground.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                newPieceBackground.SetActive(true);
                X += Width;
                pieceCount++;
            }
            X = -(Width / 2 * (WhatMultiplication - 1)) - 400.0f;
            Y -= Height;
        }

        

        yield return null;
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
        txt_time.transform.parent.gameObject.SetActive(true);
        ExampleText.SetActive(true);
        gp_GameProgress = M_GameProgress.Start;
        GameState = "Start";
        yield return null;
    }

    private IEnumerator IE_GameOver()
    {
        OnTheMendMun.SetActive(true);
        OnTheMendMun.GetComponent<AudioManager>().PlayEffectSound(0); // 클리어 사운드
        yield return new WaitForSeconds(2.0f);
        for (float i = 0; i <= 255; i += 2)
        {
            OnTheMendMun.GetComponent<SpriteRenderer>().color = new Color(255.0f / 255.0f, 255.0f / 255.0f, 255.0f / 255.0f, i / 255.0f);
            yield return new WaitForSeconds(0.01f);
        }
        yield return new WaitForSeconds(2.0f);
        Text_Background.SetActive(true);
        Text_Background.GetComponent<MiniGameResult>().SetText((int)Score);
        yield return null;
    }

    public void ExampleImageOnOff()
    {
        if (gp_GameProgress == M_GameProgress.Start)
        {
            if (!ExampleImageOn)
            {
                ExampleImage.SetActive(true);
                ExampleImageOn = true;
            }
            else
            {
                ExampleImage.SetActive(false);
                ExampleImageOn = false;
            }
        }
    }
}
