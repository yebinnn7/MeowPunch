using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Security.Cryptography;
using UnityEngine;


public class CameraController : MonoBehaviour
{
    public Transform target; // ����� Ÿ�� Transform
    public Vector3 offset = new Vector3(0, 2, -2); // ����̿� ī�޶� �� �⺻ �Ÿ�
    public float mouseSensitivity = 200f; // ���콺 ����

    private float mouseY;
    private float mouseX;

    // ���� �̵� ���� ����
    public float minYAngle;
    public float maxYAngle;
    public float minXAngle;
    public float maxXAngle;

    public Vector3 rotationOffset = new Vector3(20f, 0f, 0f); // �⺻ ȸ�� ����

    void LateUpdate()
    {
        Rotate(); // ī�޶� ȸ�� ó��
    }

    private void Rotate()
    {
        // ���콺�� �̿��� ī�޶� ȸ��
        mouseX += Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        mouseY -= Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // ���� ����
        mouseY = Mathf.Clamp(mouseY, minYAngle, maxYAngle); // ���Ʒ� �þ� ����
        mouseX = Mathf.Clamp(mouseX, minXAngle, maxXAngle); // �¿� �þ� ����

        // �⺻ ȸ�� ������ ���콺 �̵� ������ ���Ͽ� ���� ȸ�� ���
        Quaternion rotation = Quaternion.Euler(mouseY + rotationOffset.x, mouseX + rotationOffset.y, rotationOffset.z);
        Vector3 rotatedOffset = rotation * offset; // ȸ���� ������ ����
        transform.position = target.position + rotatedOffset;

        // ī�޶� �׻� ����̸� �ٶ󺸵��� ����
        transform.LookAt(target.position);
    }
}