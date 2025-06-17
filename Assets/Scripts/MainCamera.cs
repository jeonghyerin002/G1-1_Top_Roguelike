using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamera : MonoBehaviour
{
    public float followSpeed = 1.0f;
    public float cameraOffset = -10;
    public float fixedZ = -10f;

    public float minX;
    public float maxX;
    public float minY;
    public float maxY;

    private GameObject player;
    private bool stage3Mode = false;


    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "stage_3")
        {
            stage3Mode = true;
        }
    }
    private void Update()
    {

        if (player == null) return;

        Vector3 targetPos;

        if (stage3Mode)
        {
            //스테이지3 전용 : X,Z값 고정
            targetPos = new Vector3(
                transform.position.x,Mathf.Clamp(player.transform.position.y, minY, maxY), fixedZ);

            transform.position = Vector3.Lerp(transform.position, targetPos, followSpeed * Time.deltaTime);
        }
        else
        {
            targetPos = new Vector3(
                Mathf.Clamp(player.transform.position.x, minX, maxX), Mathf.Clamp(player.transform.position.y, minY, maxY), fixedZ);

            transform.position = Vector3.Lerp(transform.position, targetPos, followSpeed*Time.deltaTime);
        }

        targetPos = new Vector3(player.transform.position.x, player.transform.position.y, cameraOffset);

        targetPos.x = Mathf.Clamp(targetPos.x, minX, maxX);
        targetPos.y = Mathf.Clamp(targetPos.y, minY, maxY);

        transform.position = Vector3.Lerp(transform.position, targetPos, followSpeed * Time.deltaTime);
    }
}
