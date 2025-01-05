using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class HamsterController : MonoBehaviour
{
    public float speed = 1f;                    // 이동 속도
    public float changeDirectionInterval = 2f;  // 방향 변경 주기
    public float fixedYPosition = -1.16f;       // 고정 y 좌표 값
    public float maxLifeTime = 10f;
    public float currentLifeTime;

    private Vector3 moveVec;                    // 이동 벡터
    private Animator anim;                      // 애니메이터
    private Rigidbody rb;                       // Rigidbody 참조

    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();          // Rigidbody 컴포넌트 참조
        currentLifeTime = 0f;                    // 타이머 초기화
        StartCoroutine(ChangeDirectionRoutine()); // 방향 변경 코루틴 시작

        rb.isKinematic = false; // Rigidbody가 물리 엔진에 의해 제어되도록 설정
    }

    void Update()
    {
        if (currentLifeTime > maxLifeTime)
        {
            Destroy(gameObject);
            return;
        }

        // 이동 처리 (Rigidbody의 속도를 사용하여 이동)
        rb.velocity = moveVec * speed;  // Rigidbody의 속도 값으로 이동, deltaTime을 이용해 프레임 독립적으로 처리

        // y 좌표를 고정하여 위치 업데이트
        transform.position = new Vector3(transform.position.x, fixedYPosition, transform.position.z);

        // 이동 방향에 따라 캐릭터 회전
        if (moveVec != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(moveVec);
        }

        currentLifeTime += Time.deltaTime;

        
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
        float randomX = Random.Range(-1f, 1f);
        float randomZ = Random.Range(-1f, 1f);
        moveVec = new Vector3(randomX, 0, randomZ).normalized; // 벡터를 정규화하여 이동 방향 설정
    }

    void OnCollisionEnter(Collision collision)
    {
        // 다른 오브젝트와 충돌하면 새로운 방향으로 전환
        SetRandomDirection();
    }

    public void HamsterDead()
    {
        anim.SetTrigger("Death");
        StartCoroutine(HamsterDestroy(0.5f));
    }

    IEnumerator HamsterDestroy(float delay)
    {
        // 지정된 시간 후에 객체 삭제
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
    }
}