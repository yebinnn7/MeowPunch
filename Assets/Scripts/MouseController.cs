using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseController : MonoBehaviour
{
    public GameObject gameManagerObj; // GameManager 오브젝트 참조
    public GameManager gameManager; // GameManager 스크립트 참조

    public float speed = 1f;                    // 이동 속도
    public float changeDirectionInterval = 2f;  // 방향 변경 주기
    public float fixedYPosition = -1.16f;       // 고정 y 좌표 값

    private Vector3 moveVec;                    // 이동 벡터
    private Animator anim;                      // 애니메이터
    private Rigidbody rb;                       // Rigidbody 참조

    void Awake()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();  // Rigidbody 컴포넌트 참조

        rb.isKinematic = false; // Rigidbody를 물리 엔진에서 제어하도록 설정

        StartCoroutine(ChangeDirectionRoutine()); // 방향 변경 코루틴 시작
    }

    void Update()
    {
        // 이동 처리
        rb.velocity = moveVec * speed;  // Rigidbody의 속도 값으로 이동

        // y 좌표를 고정하여 위치 업데이트
        transform.position = new Vector3(transform.position.x, fixedYPosition, transform.position.z);

        // 이동 방향에 따라 캐릭터 회전 및 애니메이션
        if (moveVec != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(moveVec);
        }
    }

    IEnumerator ChangeDirectionRoutine()
    {
        while (true)
        {
            // 일정 시간마다 방향 변경
            yield return new WaitForSeconds(changeDirectionInterval);
            SetRandomDirection();
        }
    }

    void SetRandomDirection()
    {
        // 랜덤으로 새로운 이동 방향 설정
        float randomX = UnityEngine.Random.Range(-1f, 1f);
        float randomZ = UnityEngine.Random.Range(-1f, 1f);
        moveVec = new Vector3(randomX, 0, randomZ).normalized;
    }

    void OnCollisionEnter(Collision collision)
    {
        // 다른 오브젝트와 충돌하면 새로운 방향으로 전환
        SetRandomDirection();
    }

    public void MouseDead()
    {
        anim.SetTrigger("Death");
        StartCoroutine(MouseDestroy(0.5f));
        GameManager.Instance.AddMouseCount();
    }

    IEnumerator MouseDestroy(float delay)
    {
        // Wait for the specified delay (1 second in this case)
        yield return new WaitForSeconds(delay);

        // Destroy the GameObject after the delay
        Destroy(gameObject);
    }
}
