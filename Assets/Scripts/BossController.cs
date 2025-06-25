using System.Collections;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BossController : MonoBehaviour
{
    public static BossController Instance;
    [SerializeField] private BossSO bossData;

    public float bossHP = 0;
    public float bossHPMax;
    public int bossAttackCount;
    public GameObject warningEffect;
    public GameObject attackPrefabs;
    public GameObject[] round3BossAttack;
    public GameObject[] round3BossAttackReal;
    public Transform BossAttackPos;
    public GameObject Boss;

    public Vector2 min;
    public Vector2 max;

    //private bool inSubwayStageMode = false;
    public bool inElevatorStageMode = false;
    public bool isBossDie = false;

    //5라운드 보스 HP 회복용
    private float lastDemageTime;       //마지막 데미지 타임
    public float noDemageTime = 13f;   //데미지가 없을 시 회복 대기 시간
    public float returnBossHP = 5;    //회복량
    private void Awake()
    {
        Instance = this;
    }

    public void TakeDemage (int demage)
    {
        bossHP -= demage;
        lastDemageTime = Time.time;
    }
    void Start()
    {


        bossHP = bossHPMax;

        switch (bossData.bossType)
        {
            //case BossType.TestScene:
                //isBossDie = true;
               // break;


            case BossType.Round1:
                Debug.Log("라운드 1");
                StartCoroutine(Round1BossAttackPattern());
                break;

            case BossType.Round2:
                Debug.Log("라운드 2");
                StartCoroutine(Round1BossAttackPattern());
                break;

            case BossType.Round3:
                Debug.Log("라운드 3");
                StartCoroutine(Round3BossAttackPattern());
                break;

            case BossType.Round4:
                Debug.Log("라운드 4");
                break;

            case BossType.Round5:
                Debug.Log("라운드 ");
                StartCoroutine(Round3BossAttackPattern());
                StartCoroutine(Round5BossHealthPattern());
                break;
        }

        
    }

    IEnumerator Round1BossAttackPattern() //1, 2라운드 보스 공격 패턴
    {

        while (true)
        {

            yield return StartCoroutine(RoundSubwayBoss());
            yield return new WaitForSeconds(2f);

            Debug.Log("반복 공격");
            if (isBossDie == true)
            {
                break;
            }

        }


    }

    IEnumerator RoundSubwayBoss()    //1, 2라운드 보스 공격 소환
    {

        Vector2[] subwayAttackPos = new Vector2[bossAttackCount];

        for (int i = 0; i < subwayAttackPos.Length; i++)
        {
            float x = Random.Range(min.x, max.x);
            float y = Random.Range(min.y, max.y);

            subwayAttackPos[i] = new Vector2(x, y);
        }

        foreach (var position in subwayAttackPos)
        {
            Instantiate(warningEffect, position, Quaternion.identity);
        }

        yield return new WaitForSeconds(2f);

        foreach (var position in subwayAttackPos)
        {
            Instantiate(attackPrefabs, position, Quaternion.identity);
        }

    }

    IEnumerator Round3BossAttackPattern()     // 3라운드 보스 공격 패턴
    {

        while (true)
        {
            yield return StartCoroutine(RoundElevatorBoss());
            yield return new WaitForSeconds(4.0f);


            Debug.Log(" 쿵 ");
        }


    }

    IEnumerator RoundElevatorBoss()         //3라운드 보스 공격 소환
    {
        if (inElevatorStageMode)
            yield return new WaitForSeconds(0f);

        inElevatorStageMode = true;
        int randomIndex = Random.Range(0, round3BossAttack.Length);
        yield return new WaitForSeconds(0f);

        Instantiate(round3BossAttack[randomIndex], BossAttackPos.position, Quaternion.identity);

        yield return new WaitForSeconds(2f);

        Instantiate(round3BossAttackReal[randomIndex], BossAttackPos.position, Quaternion.identity);

    }

    IEnumerator Round5BossHealthPattern()     // 5라운드 보스 HP회복
    {

        while (!isBossDie)
        {
            if (Time.time - lastDemageTime > noDemageTime && bossHP < bossHPMax)
            {
                bossHP += returnBossHP;
                bossHP = Mathf.Min(bossHP, bossHPMax);
                Debug.Log($"보스가 체력을 회복했습니다. 현재 체력 : {bossHP}");
            }

            yield return new WaitForSeconds(1f);
        }


    }




    void Update()
    {
        if (bossHP <= 0)
        {
            isBossDie = true;
            Destroy(Boss);
        }
    }


}
