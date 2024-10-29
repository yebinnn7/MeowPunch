using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Security.Cryptography;
using System.Threading;
using UnityEngine;

public class CatController : MonoBehaviour
{
    public float speed = 5f; // 이동 속도
    public Transform cameraTransform; // 카메라 Transform

    float hAxis;
    float vAxis;

    Vector3 moveVec;
    Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        // 입력값 받기
        hAxis = Input.GetAxisRaw("Horizontal"); // A, D 키
        vAxis = Input.GetAxisRaw("Vertical");   // W, S 키

        // 카메라의 방향을 기준으로 이동 벡터 계산
        Vector3 camForward = cameraTransform.forward;
        Vector3 camRight = cameraTransform.right;

        // 수평, 수직 벡터의 y 값 제거해 평면 이동 설정
        camForward.y = 0;
        camRight.y = 0;

        // 입력 방향으로 이동 벡터 설정
        moveVec = (camForward * vAxis + camRight * hAxis).normalized;

        // 이동 처리
        transform.position += moveVec * speed * Time.deltaTime;

        // 이동 방향에 따라 캐릭터 회전
        if (moveVec != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(moveVec);
            anim.SetBool("isWalk", true);
        }
        else
        {
            anim.SetBool("isWalk", false);
        }
    }
}