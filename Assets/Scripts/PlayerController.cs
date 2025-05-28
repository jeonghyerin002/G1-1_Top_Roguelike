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

    Vector2 input;
    Vector2 Velocity;


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sR = GetComponent<SpriteRenderer>();
        //rb.bodyType = RigidbodyType2D.Kinematic;  
        bossController = GetComponent<BossController>();
    }

    private void Update()
    {
        input.x = Input.GetAxisRaw("Horizontal");
        input.y = Input.GetAxisRaw("Vertical");

        Velocity = input.normalized * moveSpeed;

        if (input.sqrMagnitude > .01f)
        {
            if(Mathf.Abs(input.x) > Mathf.Abs(input.y))
            {
                if (input.x > 0)
                    sR.sprite = spriteRight;
                else
                    sR.sprite = spriteLeft;
            }
            else
            {
                if (input.y > 0)
                    sR.sprite = spriteUp;
                else
                    sR.sprite = spriteDown;
            }
        }

       if (Input.GetKeyDown(KeyCode.E))
       {
            Debug.Log("상호작용 확인");
            //isHelp = true; 뭔지 기억 안남
            helpMemo.SetActive(true);
            isHelpful = false;

        }
       if(Input.GetKeyDown(KeyCode.Q))
        {
            helpMemo.SetActive(false);
        }

 
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.collider.gameObject.CompareTag("Respawn"))
        {
            if (isNeverDie)
            {
                isNeverDie = false;
                Debug.Log("무적 취소");
            }
            else
            {
                collision.collider.GetComponent<LevelObject>().MoveToDyingMessage();
            }
            
        }

        if (collision.gameObject.CompareTag("Box"))
        {
            isHelpful = true;
            Debug.Log("상자 발견" + " 열쇠 획득");
            isOpenBox = true;
        }

        if (collision.gameObject.CompareTag("Door"))
        {
            isHelpful = false;
            Debug.Log("열쇠가 필요합니다.");

            if (isOpenBox == true)
            {
                SceneManager.LoadScene("Stage_1");
            }
        }

        if (collision.gameObject.CompareTag("NeverDie"))
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
        rb.MovePosition(rb.position + Velocity *  Time.fixedDeltaTime);
    }
}
