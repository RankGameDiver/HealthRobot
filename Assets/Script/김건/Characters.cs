using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Characters
{
    [SerializeField]
    private bool gender; // true = 남, false = 여

    public void SetGender(bool temp) { gender = temp; }
    public bool GetGender() { return gender; }

    public Characters(bool _gender)
    {
        gender = _gender;
    }
}
