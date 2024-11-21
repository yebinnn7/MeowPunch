using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using UnityEngine;


public class CatController : MonoBehaviour
{
    public float speed = 5f; // 이동 속도
    public Transform cameraTransform; // 카메라 Transform
    public float smoothTime = 0.1f; // Lerp를 위한 부드럽기 조정

    private float hAxis;
    private float vAxis;

    private Vector3 moveVec;
    private Vector3 currentVelocity; // Lerp를 위한 속도
    private Animator anim;

    public static event Action OnRangeIncrease;
    public static event Action OnSpeedIncrease;
    public static event Action OnKillAllMouse;

    private float originalSpeed;

    void Start()
    {
        anim = GetComponent<Animator>();
        GameManager.Instance.OnLevelUp += speedUp;

        originalSpeed = speed;
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
        Vector3 targetMoveVec = (camForward * vAxis + camRight * hAxis).normalized;

        // Lerp로 부드럽게 이동 벡터 계산
        moveVec = Vector3.SmoothDamp(moveVec, targetMoveVec, ref currentVelocity, smoothTime);

        // 이동 처리
        transform.position += moveVec * speed * Time.deltaTime;

        // 이동 방향에 따라 캐릭터 회전
        if (moveVec != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveVec);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * 10f);
            anim.SetBool("isWalk", true);
        }
        else
        {
            anim.SetBool("isWalk", false);
        }

        // 공격 모션
        if (Input.GetMouseButtonDown(0))
        {
            anim.SetTrigger("isAttack");
        }
    }

    void speedUp()
    {
        speed += 0.5f;

        originalSpeed = speed;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Item")) // 태그 확인
        {
            Item item = other.GetComponent<Item>();

            if (item != null)
            {
                // 디버그 메시지와 효과 실행
                switch (item.type)
                {
                    case Item.Type.Range:
                        UnityEngine.Debug.Log("범위 아이템 획득");
                        OnRangeIncrease?.Invoke();
                        break;
                    case Item.Type.Speed:
                        UnityEngine.Debug.Log("스피드 아이템 획득");
                        SpeedUpItem();
                        OnSpeedIncrease?.Invoke();
                        break;
                    case Item.Type.Bomb:
                        UnityEngine.Debug.Log("폭탄 아이템 획득");
                        OnKillAllMouse?.Invoke();
                        break;
                }

                // 아이템 제거
                Destroy(other.gameObject);
            }
        }
    }

    void SpeedUpItem()
    {
        StartCoroutine(IncreaseSpeedTemporarily());
    }

    IEnumerator IncreaseSpeedTemporarily()
    {
        float increasedSpeed = speed;

        increasedSpeed += 2f;
        speed = increasedSpeed;

        // 5초 기다린 후, 원래 크기로 돌아감
        yield return new WaitForSeconds(5f);

        speed = originalSpeed;
    }

}




