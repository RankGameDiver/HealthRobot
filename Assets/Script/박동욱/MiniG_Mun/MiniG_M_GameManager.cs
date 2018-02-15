using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniG_M_GameManager : MonoBehaviour
{
    public int TotalPiece;
    public int ht_Times_X;
    public int ht_Times_Y;
    public GameObject[] PuzzlePiece;

    private Mini_M_ObjectManager s_PuzzlePiece;
    // Use this for initialization
    void Awake ()
    {
        PuzzlePiece = new GameObject[TotalPiece];
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (s_PuzzlePiece.PieceProgress != "Locked")
        {
            Vector2 mousePosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            Vector2 objPosition = Camera.main.ScreenToWorldPoint(mousePosition);
            transform.position = objPosition;
        }
        
    }
}
