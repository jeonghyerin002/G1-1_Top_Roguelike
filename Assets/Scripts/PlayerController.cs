using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    float moveSpeed = 2f;


    [SerializeField] Sprite spriteUp;
    [SerializeField] Sprite spriteDown;
    [SerializeField] Sprite spriteLeft;
    [SerializeField] Sprite spriteRight;

    Rigidbody2D rb;
    SpriteRenderer sR;


    public bool isOpenBox;
    public GameObject HelpMemo;      //�г�
    public GameObject OpenDoor;
    public bool isHelpful;              //Ʈ���� bool Ȯ��

    Vector2 input;
    Vector2 Velocity;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sR = GetComponent<SpriteRenderer>();
        //rb.bodyType = RigidbodyType2D.Kinematic;  
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

       if (Input.GetKey(KeyCode.E) && isHelpful == true)
       {
            Debug.Log("��ȣ�ۿ� Ȯ��");
            //isHelp = true; ���� ��� �ȳ�
            HelpMemo.SetActive(true);
            isHelpful = false;

        }
       if(Input.GetKeyUp(KeyCode.E))
        {
            HelpMemo.SetActive(false);
        }

 
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Box"))
        {
            isHelpful = true;
            Debug.Log("���� �߰�");
        }

        if (collision.gameObject.CompareTag("Door"))
        {
            isHelpful = true;
            SceneManager.LoadScene("Stage_1");
        }
    }

    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + Velocity *  Time.fixedDeltaTime);
    }
}
