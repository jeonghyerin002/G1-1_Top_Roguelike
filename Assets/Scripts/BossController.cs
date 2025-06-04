using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class BossController : MonoBehaviour
{
    [SerializeField] private BossSO bossData;

    public int bossHP = 0;
    public int bossHPMax;
    public int bossAttackCount;
    public GameObject warningEffect;
    public GameObject attackPrefabs;

    public Vector2 min;
    public Vector2 max;
    
    void Start()
    {
        bossHP = bossHPMax;

        switch (bossData.bossType)
        {
            case BossType.Round1:

                StartCoroutine(Round1BossAttackPattern());
                Debug.Log("라운드 1");
                    break;

            case BossType.Round2:
                Debug.Log("라운드 2");
                break;

            case BossType.Round3:
                Debug.Log("라운드 3");
                break;

            case BossType.Round4:
                Debug.Log("라운드 4");
                break;

            case BossType.Round5:
                Debug.Log("라운드 ");
                break;
        }
    }

    IEnumerator Round1BossAttackPattern()
    {
        while (true)
        {

            yield return StartCoroutine(RoundBoss());
            yield return new WaitForSeconds(2f);

            Debug.Log("반복 공격");
        }
    }
    IEnumerator RoundBoss()
    {

        Vector2[] attackPos = new Vector2[bossAttackCount];
        
        for(int i = 0; i < attackPos.Length; i++)
        {
            float x = Random.Range(min.x, max.x);
            float y = Random.Range(min.y, max.y);

            attackPos[i] = new Vector2(x, y);
        }

        foreach(var position  in attackPos)
        {
            Instantiate(warningEffect, position, Quaternion.identity);
        }

        yield return new WaitForSeconds(2f);

        foreach (var position in attackPos)
        {
            Instantiate(attackPrefabs, position, Quaternion.identity);
        }
        
    }

    
    void Update()
    {
        
    }


}
