using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzlePiece : MonoBehaviour {

    public Vector2 puzzleNum;
    public float speed;
    public GameObject setpos;

    bool isMouseDown;
    bool remove = false;
    [SerializeField]
    bool clear = false;
    Vector3 startPos;

        

	// Use this for initialization
	void Start () {
        startPos = transform.position;
        Instantiate(setpos).transform.position = startPos;
	}
	
	// Update is called once per frame
	void Update () {
        if (!clear)
        {
            if (isMouseDown)
            {
                Vector3 mousePos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -Camera.main.transform.position.z));
                transform.position = mousePos;
            }

            if(remove)
            {
                transform.Translate((startPos - transform.position).normalized * speed * Time.deltaTime);
            }
        }
    }

    private void OnMouseDown()
    {
        isMouseDown = true;
    }

    private void OnMouseUp()
    {
        isMouseDown = false;
        remove = true;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Board" && !isMouseDown && !clear)
        {
            Debug.Log("dfa");
            PuzzleBoard board = collision.GetComponent<PuzzleBoard>();
            if (board.puzzleNum == puzzleNum)
            {
                clear = true;
                transform.position = collision.transform.position;
            }
        }
        if (collision.gameObject.tag == "Respawn")
        {
            remove = false;
            transform.position = startPos;
        }
    }
}
