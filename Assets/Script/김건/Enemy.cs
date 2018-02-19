using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int score; // Enemy가 사라질때 얻는 점수
    public int life; // Enemy 체력
    public int enemyKind; // 바이러스 종류

    private BoxCollider2D boxCol { get{ return gameObject.GetComponent<BoxCollider2D>(); } }
    private M_VirusGame game { get { return FindObjectOfType<M_VirusGame>(); } }

    public void ColliderReset(int temp)
    {
        switch (temp)
        {
            case 0:
                boxCol.size = new Vector2(3, 3);
                break;
            case 1:
                boxCol.size = new Vector2(3, 3);
                break;
            case 2:
                boxCol.size = new Vector2(4, 4);
                break;
            case 3:
                boxCol.size = new Vector2(4, 4);
                break;
        }       
    }

    public IEnumerator VirusDead()
    {
        gameObject.GetComponent<SpriteRenderer>().sprite = game.virusDieImg[enemyKind];
        yield return new WaitForSeconds(0.2f);
        gameObject.SetActive(false);
        yield break;
    }
}
