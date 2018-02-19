using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MovePiece : MonoBehaviour
{
    public string pieceStatus = "idle";

    public Transform edgeParticles;

    public KeyCode placePiece;

    public string checkPlacement = "";

    public float yDiff;

	void Start ()
    {
        gameObject.AddComponent<BoxCollider2D>();
    }
	
	void Update ()
    {
        InvControl();

        if (pieceStatus == "pickedup")
        {
            Vector2 mousePosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            Vector2 objPosition = Camera.main.ScreenToWorldPoint(mousePosition);
            transform.position = objPosition;
        }

        if(Input.GetKeyDown(placePiece) && (pieceStatus == "pickedup"))
        {
            checkPlacement = "y";
        }
	}

    private void OnTriggerStay2D(Collider2D collision)
    {
        if ((collision.gameObject.name == gameObject.name) && (checkPlacement == "y"))
        {
            collision.GetComponent<BoxCollider2D>().enabled = false;
            GetComponent<BoxCollider2D>().enabled = false;
            GetComponent<Renderer>().sortingOrder = 0;
            transform.position = collision.gameObject.transform.position;
            pieceStatus = "locked";
            Instantiate(edgeParticles, collision.gameObject.transform.position, edgeParticles.rotation);
            checkPlacement = "n";
            GetComponent<SpriteRenderer>().color = new Color(255.0f / 255.0f, 255.0f / 255.0f, 255.0f / 255.0f, 255.0f / 255.0f);
        }

        if ((collision.gameObject.name != gameObject.name) && (checkPlacement == "y"))
        {
            checkPlacement = "n";
            GetComponent<SpriteRenderer>().color = new Color(255.0f / 255.0f, 255.0f / 255.0f, 255.0f / 255.0f, 127.0f / 255.0f);
        }
    }

    private void OnMouseDown()
    {
        checkPlacement = "n";
        pieceStatus = "pickedup";
        GetComponent<Renderer>().sortingOrder = 10;
    }

    private void InvControl()
    {
        if(Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            transform.position = new Vector2(transform.position.x, transform.position.y - 2.4f);
        }
    }
}
