using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleBoard : MonoBehaviour {

    public Vector2 puzzleNum;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if (collision.gameObject.tag == "Puzzle")
    //    {
    //        PuzzlePiece piece = collision.GetComponent<PuzzlePiece>();
    //        if (piece.puzzleNum == puzzleNum)
    //        {
    //            piece.clear = true;
    //            collision.transform.position = transform.position;
    //        }
    //    }
    //}
}
