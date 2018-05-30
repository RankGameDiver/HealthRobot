using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class MovePiece : MonoBehaviour
{
    public int PieceNum;
    public string pieceStatus = "idle";

    private Transform edgeParticles;

    void Start()
    {
        edgeParticles = Resources.Load<Transform>("prefabs/MiniG_Mun/Particle_" + MiniG_M_GameManager.s_Stage);
        gameObject.AddComponent<BoxCollider2D>();
    }

    void Update()
    {
        if (MiniG_M_GameManager.GameState.Equals("start"))
        {
            // 퍼즐 조각의 상태가 눌려있는 것으로 되어 있다면 퍼즐 조각을 이동
            if (pieceStatus.Equals("pickedup"))
            {
                Vector2 mousePosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
                Vector2 objPosition = Camera.main.ScreenToWorldPoint(mousePosition);
                transform.position = objPosition;
            }

            // 퍼즐 조각의 상태가 눌려있는 것으로 되어 있고 마우스를 뗐다면 퍼즐 조각의 상태를 안눌린 상태로 변경
            if (Input.GetMouseButtonUp(0) && pieceStatus.Equals("pickedup"))
            {
                pieceStatus = "pickeddown";
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (MiniG_M_GameManager.GameState == "start")
        {
            // 퍼즐 조각의 이름과 퍼즐 조각 배경의 이름이 같고 안 누르고 있다면 퍼즐 상태를 맞추었다고 변경
            if (collision.gameObject.name.Equals(gameObject.name) && pieceStatus.Equals("pickeddown"))
            {
                transform.position = collision.transform.position;

                collision.GetComponent<BoxCollider2D>().enabled = false;
                GetComponent<BoxCollider2D>().enabled = false;
                GetComponent<SpriteRenderer>().sortingOrder = 2;
                GetComponent<AudioManager>().PlayEffectSound();
                GetComponent<SpriteRenderer>().color = new Color(255.0f / 255.0f, 255.0f / 255.0f, 255.0f / 255.0f, 255.0f / 255.0f);

                pieceStatus = "locked";
                MiniG_M_GameManager.PieceState[PieceNum] = pieceStatus;

                Instantiate(edgeParticles, collision.gameObject.transform.position, edgeParticles.rotation);
            }
            // 퍼즐 조각의 이름과 퍼즐 조각 배경의 이름이 같지 않은데 안 누르고 있다면 퍼즐의 알파값을 변경
            if (collision.gameObject.name != gameObject.name && pieceStatus.Equals("pickeddown"))
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
        if (MiniG_M_GameManager.GameState.Equals("start"))
        {
            pieceStatus = "pickedup";
            GetComponent<SpriteRenderer>().sortingOrder = 10;
        }
    }
}
