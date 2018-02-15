using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mini_M_ObjectManager : MonoBehaviour
{
    [HideInInspector]
    public string PieceProgress;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == transform.gameObject.name)
        {
            transform.position = collision.gameObject.transform.position;
            PieceProgress = "Locked";
        }
    }
}
