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
    public bool isHelpful;              //트리거 bool 확인
    public bool isNeverDie;
    public bool dyingMessage = false;

    public GameObject closeHitbox;           //근접공격 히트박스
    public Transform AttackPoint;

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
                    animator.SetInteger("Direction", 2);
                //sR.sprite = spriteRight;
                else
                    animator.SetInteger("Direction", 1);
                    //sR.sprite = spriteLeft;
            }
            else
            {
                if (input.y > 0)
                    animator.SetInteger("Direction", 3);
                //sR.sprite = spriteUp;
                else
                    animator.SetInteger("Direction", 0);
                    //sR.sprite = spriteDown;
            }
        }


        if (coolTime <= 0)
        {
            // 공격
            if (Input.GetKeyDown(KeyCode.Space))
            {

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
            Debug.Log("상호작용 확인");
            //isHelp = true; 뭔지 기억 안남
            helpMemo.SetActive(true);
            isHelpful = false;

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

            if (isOpenBox == true)
            {
                SceneManager.LoadScene("Stage_1");
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
