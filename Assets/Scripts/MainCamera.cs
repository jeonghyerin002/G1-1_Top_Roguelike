using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamera : MonoBehaviour
{
    public float followSpeed = 1.0f;
    public float cameraOffset;

    public float minX;
    public float maxX;
    public float minY;
    public float maxY;

    private void Update()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        Vector3 targetPos = new Vector3(player.transform.position.x, player.transform.position.y, cameraOffset);

        targetPos.x = Mathf.Clamp(targetPos.x, minX, maxX);
        targetPos.y = Mathf.Clamp(targetPos.y, minY, maxY);

        transform.position = Vector3.Lerp(transform.position, targetPos, followSpeed * Time.deltaTime);
    }
}
