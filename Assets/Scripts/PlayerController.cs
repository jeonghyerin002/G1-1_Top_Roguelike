using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    float moveSpeed = 1.5f;



    [SerializeField] Sprite spriteUp;
    [SerializeField] Sprite spriteDown;
    [SerializeField] Sprite spriteLeft;
    [SerializeField] Sprite spriteRight;

    Rigidbody2D rb;
    SpriteRenderer sR;
    BossController bossController;

    public bool isOpenBox;
    public GameObject helpMemo;      //패널
    public bool isOpenDoor = false;
    //public bool isBossDie = false;
    public bool isHelpful;              //트리거 bool 확인
    public bool isNeverDie;
    public bool key;
    //public bool veryNiceKey;
    public bool dyingMessage = false;
    private bool is4RoundMode = false;
    private bool round4BackSpeed = false;

    public GameObject closeHitbox;           //근접공격 히트박스
    public GameObject Boss;
    public Transform AttackPoint;
    public Transform[] AttackPos;

    private float coolTime;
    public float MaxcoolTime = 0.5f;

    Animator animator;
    Vector2 input;
    Vector2 Velocity;

    private bool isDead = false;

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded; // 씬 로드 시 이벤트 등록
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded; // 메모리 누수 방지
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        isDead = false; // 씬이 바뀌면 자동으로 false로 초기화
    }

    private void Start()
    {
        if(SceneManager.GetActiveScene().name == "Stage_4")
        {
            is4RoundMode = true;
            round4BackSpeed = true;
        }

    }

    private void Awake()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        sR = GetComponent<SpriteRenderer>();
        //rb.bodyType = RigidbodyType2D.Kinematic;  
        bossController = GetComponent<BossController>();
        
    }

    private void Update()
    {
        input.x = Input.GetAxisRaw("Horizontal");
        input.y = Input.GetAxisRaw("Vertical");

        if (round4BackSpeed == true)
        {
            // 4라운드 자동 후진
            Velocity = (input.normalized * moveSpeed) + new Vector2(-0.3f, 0f);
        }
        else
        {
            Velocity = input.normalized * moveSpeed;
        }

        //방향에 따라 애니메이션 파라미터 연결
        //animator.SetInteger("MoveX", input.x);
        //animator.SetInteger("MoveY", input.y);

        

        if (input.sqrMagnitude > .01f)
        {
            if (Mathf.Abs(input.x) > Mathf.Abs(input.y))
            {
                if (input.x > 0)
                {
                    animator.SetInteger("Direction", 2);
                    AttackPoint.position = AttackPos[0].position;

                }
                    
                //sR.sprite = spriteRight;
                else
                {
                    animator.SetInteger("Direction", 1);
                    AttackPoint.position = AttackPos[1].position;
                }
                    
                    //sR.sprite = spriteLeft;
            }
            else
            {
                if (input.y > 0)
                {
                    animator.SetInteger("Direction", 3);
                    AttackPoint.position = AttackPos[2].position;
                }
                    
                //sR.sprite = spriteUp;
                else
                {
                    animator.SetInteger("Direction", 0);
                    AttackPoint.position = AttackPos[3].position;
                }
                    
                    //sR.sprite = spriteDown;
            }
        }


        if (coolTime <= 0)
        {
            // 공격
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SpawnSmashHitbox();
                //Animator.SetTrigger(attack);  //스페이스바 누를 때 애니메이션 출력
                coolTime = MaxcoolTime;
            }

        }
        else
        {
            coolTime -= Time.deltaTime;
        }

        
        

        if (Input.GetKeyDown(KeyCode.E))
        {
            if(isHelpful)
            {
                Debug.Log("상자와 상호작용 중 - 도움말 열기");
                //isHelp = true; 뭔지 기억 안남
                helpMemo.SetActive(true);
                //isHelpful = false;

            }
            else
            {
                Debug.Log("근처에 상자가 없어서 도움말을 열 수 없음");
            }

        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            helpMemo.SetActive(false);
        }

        

    }
    
    public void SpawnSmashHitbox()
    {
        Instantiate(closeHitbox, AttackPoint.position, Quaternion.identity);
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {

        if (isDead) return;

        if (collision.CompareTag("Respawn"))
        {
            if (isNeverDie)
            {
                isNeverDie = false;
                Debug.Log("무적 취소");
            }
            if(!isNeverDie)
            {
                Debug.Log("죽음");
                isDead = true;
                string sceneName = SceneManager.GetActiveScene().name;

                // "Stage_1" → 숫자 '1'만 추출
                int stageNumber = 0;
                if (sceneName.StartsWith("Stage_"))
                {
                    string numberPart = sceneName.Replace("Stage_", "");
                    int.TryParse(numberPart, out stageNumber);
                }

                GameDataManager.instance.PlayerDead(stageNumber);
            }

        }

        if (collision.CompareTag("Box"))
        {
            isHelpful = true;
            Debug.Log("상자 발견");
            isOpenBox = true;
        }

        if (collision.CompareTag("NeverDie"))
        {
            isNeverDie = true;
            Debug.Log("목숨 +1");
            Destroy(collision.gameObject);

            //if(collision.gameObject.CompareTag("Respawn")) 
            //{
            //isNeverDie = false;
            //}
        }

        if (collision.CompareTag("Key"))
        {
            key = true;
            Debug.Log("열쇠 획득");
            Destroy(collision.gameObject);
        }

        //if (collision.CompareTag("VeryNiceKey"))
        //{
            //veryNiceKey = true;
            //Debug.Log("베리나이스열쇠 획득");
           // Destroy(collision.gameObject);
         //}


        if (collision.CompareTag("Door"))
        {

            //bool isBossDead = BossController.Instance != null && BossController.Instance.isBossDie ==false;

            if (BossController.Instance != null)
            {
                if (BossController.Instance.isBossDie == false || key == false)
                {
                    Debug.Log("문이 잠겨있다.");
                }


                if ((BossController.Instance.isBossDie == true && key == true) || is4RoundMode)
                {
                    string currentSceneName = SceneManager.GetActiveScene().name;
                    string nextSceneName = "";

                    if (currentSceneName == "TestScene")
                    {
                        nextSceneName = "Stage_1";
                    }
                    else if (currentSceneName.StartsWith("Stage_"))
                    {
                        string numberPart = currentSceneName.Replace("Stage_", "");
                        int stageNumber;

                        if (int.TryParse(numberPart, out stageNumber))
                        {
                            int nextStage = stageNumber + 1;
                            nextSceneName = $"Stage_{nextStage}";
                        }
                    }

                    if (!string.IsNullOrEmpty(nextSceneName))
                    {
                        SceneManager.LoadScene(nextSceneName);
                    }
                    else
                    {
                        Debug.LogWarning("다음 씬 이름이 잘못되었습니다.");
                    }
                }
            }
            else
            {
                if (key == false)
                {
                    Debug.Log("문이 잠겨있다.");
                }


                if (key == true)
                {
                    string currentSceneName = SceneManager.GetActiveScene().name;
                    string nextSceneName = "";

                    if (currentSceneName == "TestScene")
                    {
                        nextSceneName = "Stage_1";
                    }
                    else if (currentSceneName.StartsWith("Stage_"))
                    {
                        string numberPart = currentSceneName.Replace("Stage_", "");
                        int stageNumber;

                        if (int.TryParse(numberPart, out stageNumber))
                        {
                            int nextStage = stageNumber + 1;
                            nextSceneName = $"Stage_{nextStage}";
                        }
                    }

                    if (!string.IsNullOrEmpty(nextSceneName))
                    {
                        SceneManager.LoadScene(nextSceneName);
                    }
                    else
                    {
                        Debug.LogWarning("다음 씬 이름이 잘못되었습니다.");
                    }
                }
            }
            
            

        }

        
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.CompareTag("Box"))
        {
            isHelpful = false;
        }
    }
    void HelpItem()
    {
        isNeverDie = false;

        Debug.Log("무적취소");
    }

    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + Velocity * Time.fixedDeltaTime);
    }
}
