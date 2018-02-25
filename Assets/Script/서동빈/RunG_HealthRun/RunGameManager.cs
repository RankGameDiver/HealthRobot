using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RunGameManager : MonoBehaviour
{
#if UNITY_ANDROID && !UNITY_EDITOR
    private bool Active;
    private AndroidJavaObject camera1;
#endif
    public RunPlayer player;  // 플레이어
    public GameObject[] pattern; // 런게임 패턴
    public GameObject bgIndex; // 배경 목록 오브젝트
    public Text t_scoreTex; // 점수 텍스트 오브젝트
    public GameObject StartBtn; // 시작 버튼
    public Image[] hpObj; // 체력 오브젝트
    public Sprite on_hpSprite; // 체력이 차있는 이미지
    public Sprite off_hpSprite; // 체력이 없는 이미지
    public GameObject[] healthTimeObj; // HEALTH UI 오브젝트
    public RunGameData r_GD; // 미니게임에 넘어갈 때 현재 상황을 저장 해놓을 오브젝트
    public Button[] playerControllBtn; // 0 점프, 1 슬라이드 버튼
    public Button[] actionBtn; // 액션 버튼

    public bool isPlaying = true;  // 현재 게임이 진행 중인가?

    public int treatmentPer; // 치료도
    public Text t_treatmentPer; // 치료도 텍스트 UI
    public Image treatmentImg; // 치료도 단계 UI
    public Sprite[] treatmentSprite; // 치료도 단계 이미지 파일
    public Image[] faceUI; // 얼굴 UI
    public Sprite[] faceOnsprite; //얼굴 UI 이미지
    public Sprite[] faceOffsprite; //얼굴 UI 이미지

    public RunGameResult r_GR;

    public int minigameCnt = 0;
    public bool GameEnd = false;


    // Use this for initialization
    void Start () {
        for (int i =0; i < player.maxLife; i++)
        {
            hpObj[i].gameObject.SetActive(true);
            hpObj[i].sprite = on_hpSprite;
        }

        r_GD = GameObject.Find("RunGameData").GetComponent<RunGameData>();
        if(r_GD.isMapChange) ReSetting();
    }

    // Update is called once per frame
    void Update () {
		
	}
    
    public GameObject GetPattern()
    { 
        int tempPattern = Random.Range(0, pattern.Length - 2);
        GameObject obj = pattern[tempPattern];

        Debug.Log(tempPattern);

        GameObject tempObj = pattern[tempPattern];
        pattern[tempPattern] = pattern[pattern.Length - 1];
        pattern[pattern.Length - 1] = tempObj;

        return Instantiate(obj);
    }

#if UNITY_ANDROID && !UNITY_EDITOR
    public void FL_Start()
    {
        AndroidJavaClass cameraClass = new AndroidJavaClass("android.hardware.Camera");

        int camID = 0;
        camera1 = cameraClass.CallStatic<AndroidJavaObject>("open", camID);

        if (camera1 != null && !Active)
        {
            AndroidJavaObject cameraParameters = camera1.Call<AndroidJavaObject>("getParameters");
            cameraParameters.Call("setFlashMode", "torch");
            camera1.Call("setParameters", cameraParameters);
            camera1.Call("startPreview");
            Active = true;
        }
        else
        {
            //Debug.LogError("[CameraParametersAndroid] Camera not available");
        }
    }

    public void FL_Stop()
    {
        if (camera1 != null && Active)
        {
            camera1.Call("stopPreview");
            Active = false;
        }
        else
        {
            //Debug.LogError("[CameraParametersAndroid] Camera not available");
        }
    }
#endif

    public void StopGame()
    {
        Time.timeScale = 0;
        isPlaying = false;
    }

    public void StartGame()
    {
        Time.timeScale = 1;
        isPlaying = true;
    }

    public void SubHeart()
    {
        for(int i = player.maxLife - 1; i >= 0; i--)
        {
            if (hpObj[i].sprite == on_hpSprite)
            {
                hpObj[i].sprite = off_hpSprite;
                return;
            }
        }
    }

    public void AddHeart()
    {
        for (int i = 0; i < player.maxLife; i++)
        {
            if (hpObj[i].sprite == off_hpSprite)
            {
                hpObj[i].sprite = off_hpSprite;
                return;
            }
        }
    }

    public void OnHealthTimeObj(int num)
    {
        healthTimeObj[num].SetActive(true);
    }

    public void OffHealthTimeObj(int num = 7)
    {
        if(num == 7)
        {
            for (int i = 0; i < 6; i++)
            {
                healthTimeObj[i].SetActive(false);
            }
        }

        else
        {
            healthTimeObj[num].SetActive(false);
        }
    }

    public void OnActionButton()
    {
        for(int i = 0; i < 2; i++)
        {
            playerControllBtn[i].gameObject.SetActive(false);
            actionBtn[i].gameObject.SetActive(true);
        }
    }

    public void OffActionButton()
    {
        for (int i = 0; i < 2; i++)
        {
            playerControllBtn[i].gameObject.SetActive(true);
            actionBtn[i].gameObject.SetActive(false);
        }
    }

    public void SaveScene()
    {
        r_GD = GameObject.Find("RunGameData").GetComponent<RunGameData>();

        for (int i = 0; i < 4; i++)
        {
            r_GD.bgPostionIndex[i] = bgIndex.transform.GetChild(i).transform.position;
            r_GD.patternIndex[i] = Instantiate(bgIndex.transform.GetChild(i).GetChild(0).gameObject);
            r_GD.patternIndex[i].SetActive(false);
            r_GD.patternIndex[i].transform.parent = r_GD.gameObject.transform;
        }
        r_GD.score = System.Convert.ToInt32(t_scoreTex.text);
        r_GD.coin = player.coin;
        r_GD.treatmentPer = treatmentPer;
        r_GD.currentLife = player.currentLife;
        for (int i = 0; i < 6; i++) r_GD.health[i] = player.healthTime[i];
        r_GD.minigameCnt = minigameCnt;

        r_GD.isMapChange = true;
    }

    public void SettingPer()
    {
        t_treatmentPer.text = treatmentPer.ToString() + "%";

        if(treatmentPer >= 20)
        {
            treatmentImg.sprite = treatmentSprite[1];
            for(int i = 0; i< 5; i++)
            {
                faceUI[i].sprite = faceOffsprite[i];
            }

            faceUI[1].sprite = faceOnsprite[1];
        }

        else if (treatmentPer >= 40)
        {
            treatmentImg.sprite = treatmentSprite[2];
            for (int i = 0; i < 5; i++)
            {
                faceUI[i].sprite = faceOffsprite[i];
            }

            faceUI[2].sprite = faceOnsprite[2];

        }

        else if (treatmentPer >= 60)
        {
            treatmentImg.sprite = treatmentSprite[3];
            for (int i = 0; i < 5; i++)
            {
                faceUI[i].sprite = faceOffsprite[i];
            }

            faceUI[3].sprite = faceOnsprite[3];

        }

        else if (treatmentPer >= 80)
        {
            treatmentImg.sprite = treatmentSprite[4];
            for (int i = 0; i < 5; i++)
            {
                faceUI[i].sprite = faceOffsprite[i];
            }

            faceUI[4].sprite = faceOnsprite[4];

        }
    }

    public void ReSetting()
    {
        r_GD.isMapChange = false;
        t_scoreTex.text = r_GD.score.ToString();
        player.healthTime = r_GD.health;
        for (int i = 0; i < player.maxLife - r_GD.currentLife; i++)
        {
            SubHeart();
            player.currentLife--;
        }
        for(int i = 0; i < 4;i++)
        {
            bgIndex.transform.GetChild(i).transform.position = r_GD.bgPostionIndex[i];
            r_GD.patternIndex[i].transform.parent = bgIndex.transform.GetChild(i);
            bgIndex.transform.GetChild(i).GetChild(0).localPosition = Vector3.zero;
            bgIndex.transform.GetChild(i).GetChild(0).gameObject.SetActive(true);
        }
        player.coin = r_GD.coin;
        for(int i = 0; i < 6;i ++)
        {
            player.healthTime[i] = r_GD.health[i];
        }
        treatmentPer = r_GD.treatmentPer;
        SettingPer();
        minigameCnt = r_GD.minigameCnt;
        minigameCnt++;

        if(minigameCnt >= 1)
        {
            GameEnd = true;
            Time.timeScale = 0;
            r_GR.gameObject.SetActive(true);
            r_GR.SetResult(System.Convert.ToInt32(t_scoreTex.text), player.coin, treatmentPer);
#if UNITY_ANDROID && !UNITY_EDITOR
            FL_Start();
#endif
        }
    }

    public void DestroyRGD()
    {
        Destroy(GameObject.Find("RunGameData"));
#if UNITY_ANDROID && !UNITY_EDITOR
        FL_Stop();
        camera1.Call("release");
#endif
    }
}
