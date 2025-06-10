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
    public bool isBossDie = false;
    public bool isHelpful;              //트리거 bool 확인
    public bool isNeverDie;
    public bool dyingMessage = false;

    public GameObject closeHitbox;           //근접공격 히트박스
    public Transform AttackPoint;
    public Transform[] AttackPos;

    private float coolTime;
    public float MaxcoolTime = 0.5f;

    Animator animator;
    Vector2 input;
    Vector2 Velocity;


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

        //방향에 따라 애니메이션 파라미터 연결
        //animator.SetInteger("MoveX", input.x);
        //animator.SetInteger("MoveY", input.y);

        Velocity = input.normalized * moveSpeed;

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
        if (collision.CompareTag("Respawn"))
        {
            if (isNeverDie)
            {
                isNeverDie = false;
                Debug.Log("무적 취소");
            }
            else
            {
                GameDataManager.instance.PlayerDead(SceneManager.GetActiveScene().buildIndex);
                collision.GetComponent<LevelObject>().MoveToDyingMessage();
            }

        }

        if (collision.CompareTag("Box"))
        {
            isHelpful = true;
            Debug.Log("상자 발견" + " 열쇠 획득");
            isOpenBox = true;
        }

        if (collision.CompareTag("Door"))
        {
            isHelpful = false;
            Debug.Log("열쇠가 필요합니다.");

            if (isOpenBox == true || isBossDie)
            {
                int CurrentStage = SceneManager.GetActiveScene().buildIndex;
                int NextStage = CurrentStage += 1;
                SceneManager.LoadScene(NextStage);

            }
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
