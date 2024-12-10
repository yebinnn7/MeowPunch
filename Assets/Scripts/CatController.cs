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
    // 속도 증가가 진행 중인지 확인하는 변수
    private bool isSpeedIncreased = false;
    private float speedIncreaseEndTime = 0f; // 속도 증가 종료 시간


    void Start()
    {
        anim = GetComponent<Animator>();
        GameManager.Instance.OnLevelUp += speedUp;
        GameManager.Instance.GameReStart += ResetSpeed;

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
                        SoundManager.Instance.PlaySound("item");
                        OnRangeIncrease?.Invoke();
                        break;
                    case Item.Type.Speed:
                        UnityEngine.Debug.Log("스피드 아이템 획득");
                        SoundManager.Instance.PlaySound("item");
                        SpeedUpItem();
                        OnSpeedIncrease?.Invoke();
                        break;
                    case Item.Type.Bomb:
                        UnityEngine.Debug.Log("폭탄 아이템 획득");
                        SoundManager.Instance.PlaySound("bomb");
                        OnKillAllMouse?.Invoke();
                        break;
                }

                // 아이템 제거
                Destroy(other.gameObject);
            }
        }
    }

    // 아이템 사용 시 속도 증가 (5초 동안만)
    void SpeedUpItem()
    {
        if (!isSpeedIncreased)
        {
            // 속도 증가 시작
            StartCoroutine(IncreaseSpeedTemporarily());
        }
        else
        {
            // 속도가 이미 증가 중이면 5초를 추가
            speedIncreaseEndTime += 5f;
        }
    }

    // 5초 동안 속도 증가
    IEnumerator IncreaseSpeedTemporarily()
    {
        isSpeedIncreased = true;
        speedIncreaseEndTime = Time.time + 5f; // 5초 후 종료 시간을 설정

        float increasedSpeed = speed;
        increasedSpeed += 2f;
        speed = increasedSpeed;

        // 5초 기다린 후, 원래 속도로 돌아감
        while (Time.time < speedIncreaseEndTime)
        {
            yield return null; // 속도 증가가 끝날 때까지 기다림
        }

        speed = originalSpeed;
        isSpeedIncreased = false; // 속도 증가 상태 해제
    }

    void ResetSpeed()
    {
        speed = 3f;
        transform.position = new Vector3(-3f, -1.16f, 0.66f);
        transform.rotation = Quaternion.Euler(0, 0, 0); // Quaternion으로 회전 설정
    }
}




