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

    public GameObject closeHitbox;           //�������� ��Ʈ�ڽ�
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

        //���⿡ ���� �ִϸ��̼� �Ķ���� ����
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
            // ����
            if (Input.GetKeyDown(KeyCode.Space))
            {

                //Animator.SetTrigger(attack);  //�����̽��� ���� �� �ִϸ��̼� ���
                coolTime = MaxcoolTime;
            }

        }
        else
        {
            coolTime -= Time.deltaTime;
        }


        if (Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("��ȣ�ۿ� Ȯ��");
            //isHelp = true; ���� ��� �ȳ�
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
                Debug.Log("���� ���");
            }
            else
            {
                collision.GetComponent<LevelObject>().MoveToDyingMessage();
            }

        }

        if (collision.CompareTag("Box"))
        {
            isHelpful = true;
            Debug.Log("���� �߰�" + " ���� ȹ��");
            isOpenBox = true;
        }

        if (collision.CompareTag("Door"))
        {
            isHelpful = false;
            Debug.Log("���谡 �ʿ��մϴ�.");

            if (isOpenBox == true)
            {
                SceneManager.LoadScene("Stage_1");
            }
        }

        if (collision.CompareTag("NeverDie"))
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
        rb.MovePosition(rb.position + Velocity * Time.fixedDeltaTime);
    }
}
