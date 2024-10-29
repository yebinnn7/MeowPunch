using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Security.Cryptography;
using UnityEngine;


public class CameraController : MonoBehaviour
{
    public Transform target; // 고양이 타겟 Transform
    public Vector3 offset = new Vector3(0, 2, -2); // 고양이와 카메라 간 기본 거리
    public float mouseSensitivity = 200f; // 마우스 감도

    private float mouseY;
    private float mouseX;

    // 시점 이동 제한 각도
    public float minYAngle;
    public float maxYAngle;
    public float minXAngle;
    public float maxXAngle;

    public Vector3 rotationOffset = new Vector3(20f, 0f, 0f); // 기본 회전 각도

    void LateUpdate()
    {
        Rotate(); // 카메라 회전 처리
    }

    private void Rotate()
    {
        // 마우스를 이용해 카메라 회전
        mouseX += Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        mouseY -= Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // 각도 제한
        mouseY = Mathf.Clamp(mouseY, minYAngle, maxYAngle); // 위아래 시야 제한
        mouseX = Mathf.Clamp(mouseX, minXAngle, maxXAngle); // 좌우 시야 제한

        // 기본 회전 각도와 마우스 이동 각도를 더하여 최종 회전 계산
        Quaternion rotation = Quaternion.Euler(mouseY + rotationOffset.x, mouseX + rotationOffset.y, rotationOffset.z);
        Vector3 rotatedOffset = rotation * offset; // 회전된 오프셋 적용
        transform.position = target.position + rotatedOffset;

        // 카메라가 항상 고양이를 바라보도록 설정
        transform.LookAt(target.position);
    }
}