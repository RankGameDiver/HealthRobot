using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageManager : MonoBehaviour
{
    public int Stage = 1;

    public GameObject PieceParents;
    public GameObject Piece;

    private Sprite[] newPieceSprite;
    void Start ()
    {
        newPieceSprite = Resources.LoadAll<Sprite>("정지윤(UI & Object)/MiniG_Mun/Object/" + Stage + "_mun");
        PieceParents.GetComponent<RectTransform>().position = new Vector3(-8, 5.4f - newPieceSprite[0].rect.height / 100.0f - 0.5f);
        PieceParents.GetComponent<RectTransform>().sizeDelta = new Vector2(newPieceSprite[0].rect.width / 100.0f, newPieceSprite[0].rect.height / 100.0f);
        Piece.GetComponent<RectTransform>().sizeDelta = new Vector2(newPieceSprite[0].rect.width / 100.0f, newPieceSprite[0].rect.height / 100.0f);
        Piece.GetComponent<SpriteRenderer>().sprite = newPieceSprite[0];
        PieceParents.GetComponent<VerticalLayoutGroup>().spacing = Piece.GetComponent<RectTransform>().rect.height + 0.5f;
        StartCoroutine(CreationPiece());
    }

    void Update ()
    {
		
	}

    private IEnumerator CreationPiece()
    {
        for (int i = 1; i < (Stage + 1) * (Stage + 1); i++)    
        {
            GameObject newPiece = Instantiate(Piece);
            newPiece.name = "mun_" + i;
            newPiece.GetComponent<RectTransform>().sizeDelta = new Vector2(newPieceSprite[i].rect.width / 100.0f, newPieceSprite[i].rect.height / 100.0f);
            newPiece.GetComponent<SpriteRenderer>().sprite = newPieceSprite[i];
            newPiece.transform.SetParent(PieceParents.transform);
            newPiece.transform.localScale = new Vector3(2.0f, 2.0f, 1.0f);
            yield return null;
        }
    }
}
