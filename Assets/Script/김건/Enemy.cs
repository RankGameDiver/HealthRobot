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
    private SoundM sound { get { return gameObject.GetComponent<SoundM>(); } }

    public void ColliderReset(int temp)
    {
        switch (temp)
        {
            case 0:
                boxCol.size = new Vector2(2, 2);
                life = 1;              
                break;
            case 1:
                boxCol.size = new Vector2(2.5f, 2.5f);
                life = 2;
                break;
            case 2:
                boxCol.size = new Vector2(3.5f, 4);
                life = 3;
                break;
            case 3:
                boxCol.size = new Vector2(5, 5);
                life = 4;
                break;
        }
        score = life * 100;
    }

    public SoundM GetSound()
    {
        return sound;
    }

    public IEnumerator VirusDead()
    {
        gameObject.GetComponent<SpriteRenderer>().sprite = game.virusDieImg[enemyKind];
        //sound.PlaySound();
        yield return new WaitForSeconds(0.2f);
        gameObject.SetActive(false);
        yield break;
    }
}
