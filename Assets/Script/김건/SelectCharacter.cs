using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectCharacter : MonoBehaviour
{
    private Characters characters = new Characters(false);
    public bool gender;

    public void SetChar()
    {
        characters.SetGender(gender);
        // 이 밑으로 캐릭터 세부 사항 추가
    }

    public void SetGender(bool temp) { gender = temp; }
}
