using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyObject : MonoBehaviour
{
    public float destroyTime;
    void Start()
    {
        Destroy(gameObject, destroyTime);
        Invoke("h333", destroyTime);
    }

    void h333()     //�� ������ ������ ���� ������ �̾�����
    {
        BossController.Instance.inElevatorStageMode = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
