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
    //public bool isBossDie = false;
    public bool isHelpful;              //Ʈ���� bool Ȯ��
    public bool isNeverDie;
    public bool key;
    //public bool veryNiceKey;
    public bool dyingMessage = false;
    private bool is4RoundMode = false;
    private bool round4BackSpeed = false;

    public GameObject closeHitbox;           //�������� ��Ʈ�ڽ�
    public GameObject Boss;
    public Transform AttackPoint;
    public Transform[] AttackPos;

    private float coolTime;
    public float MaxcoolTime = 0.5f;

    Animator animator;
    Vector2 input;
    Vector2 Velocity;

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
            // 4���� �ڵ� ����
            Velocity = (input.normalized * moveSpeed) + new Vector2(-0.3f, 0f);
        }
        else
        {
            Velocity = input.normalized * moveSpeed;
        }

        //���⿡ ���� �ִϸ��̼� �Ķ���� ����
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
            // ����
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SpawnSmashHitbox();
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
            if(isHelpful)
            {
                Debug.Log("���ڿ� ��ȣ�ۿ� �� - ���� ����");
                //isHelp = true; ���� ��� �ȳ�
                helpMemo.SetActive(true);
                //isHelpful = false;

            }
            else
            {
                Debug.Log("��ó�� ���ڰ� ��� ������ �� �� ����");
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
                Debug.Log("���� ���");
            }
            if(!isNeverDie)
            {
                Debug.Log("����");
                GameDataManager.instance.PlayerDead(SceneManager.GetActiveScene().buildIndex);
                collision.GetComponent<LevelObject>().MoveToDyingMessage();
            }

        }

        if (collision.CompareTag("Box"))
        {
            isHelpful = true;
            Debug.Log("���� �߰�");
            isOpenBox = true;
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

        if (collision.CompareTag("Key"))
        {
            key = true;
            Debug.Log("���� ȹ��");
            Destroy(collision.gameObject);
        }

        //if (collision.CompareTag("VeryNiceKey"))
        //{
            //veryNiceKey = true;
            //Debug.Log("�������̽����� ȹ��");
           // Destroy(collision.gameObject);
        //}


        if (collision.CompareTag("Door"))
        {

            //bool isBossDead = BossController.Instance != null && BossController.Instance.isBossDie ==false;

            if ( BossController.Instance.isBossDie == false || key == false )
            {
                Debug.Log("���� ����ִ�.");
            }
            

            if ((BossController.Instance.isBossDie == true && key == true) || is4RoundMode )
            {
                int CurrentStage = SceneManager.GetActiveScene().buildIndex;
                int NextStage = CurrentStage += 1;
                SceneManager.LoadScene(NextStage);
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

        Debug.Log("�������");
    }

    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + Velocity * Time.fixedDeltaTime);
    }
}
