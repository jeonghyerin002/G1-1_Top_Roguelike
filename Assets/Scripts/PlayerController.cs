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
    public GameObject helpMemo;      //�г�
    public bool isOpenDoor = false;
    public bool isHelpful;              //Ʈ���� bool Ȯ��
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
            Debug.Log("��ȣ�ۿ� Ȯ��");
            //isHelp = true; ���� ��� �ȳ�
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
                Debug.Log("���� ���");
            }
            else
            {
                collision.collider.GetComponent<LevelObject>().MoveToDyingMessage();
            }
            
        }

        if (collision.gameObject.CompareTag("Box"))
        {
            isHelpful = true;
            Debug.Log("���� �߰�" + " ���� ȹ��");
            isOpenBox = true;
        }

        if (collision.gameObject.CompareTag("Door"))
        {
            isHelpful = false;
            Debug.Log("���谡 �ʿ��մϴ�.");

            if (isOpenBox == true)
            {
                SceneManager.LoadScene("Stage_1");
            }
        }

        if (collision.gameObject.CompareTag("NeverDie"))
        {
            isNeverDie = true;
            Debug.Log("��� +1");
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

        Debug.Log("�������");
    }

    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + Velocity *  Time.fixedDeltaTime);
    }
}
