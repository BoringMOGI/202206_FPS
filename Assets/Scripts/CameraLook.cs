using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLook : MonoBehaviour
{
    [SerializeField] Transform playerBody;
    [SerializeField] Transform playerHead;
    [SerializeField] Vector2 sencitivity;       // �ΰ���.

    float xRotation;

    // Update is called once per frame
    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * sencitivity.x;
        float mouseY = Input.GetAxis("Mouse Y") * sencitivity.y;

        playerBody.Rotate(Vector3.up * mouseX);     // ���� ȸ��. (��,��)

        // ���� ȸ��. (��,�Ʒ�)
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -60.0f, 60.0f);
        playerHead.localRotation = Quaternion.Euler(xRotation, 0, 0);
    }
}
