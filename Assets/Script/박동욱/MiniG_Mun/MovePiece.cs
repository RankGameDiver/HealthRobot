using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class MovePiece : MonoBehaviour
{
    private Transform edgeParticles;
    public string pieceStatus = "idle";
    public int PieceNum;

    void Start ()
    {
        edgeParticles = Resources.Load<Transform>("prefabs/MiniG_Mun/Particle_" + MiniG_M_GameManager.s_Stage);
        gameObject.AddComponent<BoxCollider2D>();
    }
	
	void Update ()
    {
        if (MiniG_M_GameManager.GameState == "Start" && MiniG_M_GameManager.GameState != "Over")
        {
            if (pieceStatus == "pickedup")
            {
                Vector2 mousePosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
                Vector2 objPosition = Camera.main.ScreenToWorldPoint(mousePosition);
                transform.position = objPosition;
            }

            if (Input.GetMouseButtonUp(0) && pieceStatus == "pickedup")
            {
                pieceStatus = "pickeddown";
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (MiniG_M_GameManager.GameState == "Start" && MiniG_M_GameManager.GameState != "Over")
        {
            if (collision.gameObject.name == gameObject.name && pieceStatus == "pickeddown")
            {
                collision.GetComponent<BoxCollider2D>().enabled = false;
                GetComponent<BoxCollider2D>().enabled = false;
                GetComponent<Renderer>().sortingOrder = 2;
                transform.position = collision.gameObject.transform.position;
                pieceStatus = "locked";
                MiniG_M_GameManager.PieceState[PieceNum] = "locked";
                GetComponent<AudioManager>().PlayEffectSound();
                Instantiate(edgeParticles, collision.gameObject.transform.position, edgeParticles.rotation);
                GetComponent<SpriteRenderer>().color = new Color(255.0f / 255.0f, 255.0f / 255.0f, 255.0f / 255.0f, 255.0f / 255.0f);
            }

            if (collision.gameObject.name != gameObject.name && pieceStatus == "pickeddown")
            {
                GetComponent<SpriteRenderer>().color = new Color(255.0f / 255.0f, 255.0f / 255.0f, 255.0f / 255.0f, 127.0f / 255.0f);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        GetComponent<SpriteRenderer>().color = new Color(255.0f / 255.0f, 255.0f / 255.0f, 255.0f / 255.0f, 255.0f / 255.0f);
    }

    private void OnMouseDown()
    {
        if (MiniG_M_GameManager.GameState == "Start" && MiniG_M_GameManager.GameState != "Over")
        {
            pieceStatus = "pickedup";
            GetComponent<Renderer>().sortingOrder = 10;
        }
    }
}
