using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Hitbox : MonoBehaviour
{
    public int damage = 10;
    public float duration = 0.1f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Respawn"))
        {
            BossController boss = collision.GetComponent<BossController>();
            Debug.Log("ÆÜ");
            if(boss != null)
            {
                //boss.bossHP -= damage;
                boss.TakeDemage(damage);
            }
            
        }
    }

    private void Start()
    {
        Destroy(gameObject, duration);
    }

}
