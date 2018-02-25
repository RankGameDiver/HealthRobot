using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using LitJson;

public class SaveLoad : MonoBehaviour
{
    [SerializeField]
    private List<Data> S_Data = new List<Data>();

    void Start()
    {
        Load();
    }

    public void Save()
    {
        S_Data.Clear();
        S_Data.Add(new Data(PlayerData.money, PlayerData.gender));
        JsonData charData = JsonMapper.ToJson(S_Data);
        File.WriteAllText(Application.dataPath + "/Data/SaveData.json", charData.ToString());
    }

    public void Load()
    {
        string L_sData = null;
        if (File.Exists(Application.dataPath + "/Data/SaveData.json"))
        {
            L_sData = File.ReadAllText(Application.dataPath + "/Data/SaveData.json");
            JsonData charData = JsonMapper.ToObject(L_sData);
            GetData(charData);
        }
        else
        {
            Save();
        }

    }

    public void GetData(JsonData data)
    {
        PlayerData.money = (int)data[0]["money"];
        PlayerData.gender = (bool)data[0]["gender"];
    }
}
