using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceBackground : MonoBehaviour
{
    public GameObject piece;

	void Start ()
    {
        transform.localScale = new Vector3(piece.GetComponent<RectTransform>().rect.width / 2, piece.GetComponent<RectTransform>().rect.height / 2);

    }
	
	void Update ()
    {
		
	}
}
