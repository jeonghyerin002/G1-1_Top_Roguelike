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

    //5���� ���� HP ȸ����
    private float lastDemageTime;       //������ ������ Ÿ��
    public float noDemageTime = 13f;   //�������� ���� �� ȸ�� ��� �ð�
    public float returnBossHP = 5;    //ȸ����
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
                Debug.Log("���� 1");
                StartCoroutine(Round1BossAttackPattern());
                break;

            case BossType.Round2:
                Debug.Log("���� 2");
                StartCoroutine(Round1BossAttackPattern());
                break;

            case BossType.Round3:
                Debug.Log("���� 3");
                StartCoroutine(Round3BossAttackPattern());
                break;

            case BossType.Round4:
                Debug.Log("���� 4");
                break;

            case BossType.Round5:
                Debug.Log("���� ");
                StartCoroutine(Round3BossAttackPattern());
                StartCoroutine(Round5BossHealthPattern());
                break;
        }

        
    }

    IEnumerator Round1BossAttackPattern() //1, 2���� ���� ���� ����
    {

        while (true)
        {

            yield return StartCoroutine(RoundSubwayBoss());
            yield return new WaitForSeconds(2f);

            Debug.Log("�ݺ� ����");
            if (isBossDie == true)
            {
                break;
            }

        }


    }

    IEnumerator RoundSubwayBoss()    //1, 2���� ���� ���� ��ȯ
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

    IEnumerator Round3BossAttackPattern()     // 3���� ���� ���� ����
    {

        while (true)
        {
            yield return StartCoroutine(RoundElevatorBoss());
            yield return new WaitForSeconds(4.0f);


            Debug.Log(" �� ");
        }


    }

    IEnumerator RoundElevatorBoss()         //3���� ���� ���� ��ȯ
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

    IEnumerator Round5BossHealthPattern()     // 5���� ���� HPȸ��
    {

        while (!isBossDie)
        {
            if (Time.time - lastDemageTime > noDemageTime && bossHP < bossHPMax)
            {
                bossHP += returnBossHP;
                bossHP = Mathf.Min(bossHP, bossHPMax);
                Debug.Log($"������ ü���� ȸ���߽��ϴ�. ���� ü�� : {bossHP}");
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
