using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class HamsterController : MonoBehaviour
{
    public float speed = 1f;                    // 이동 속도
    public float changeDirectionInterval = 2f;  // 방향 변경 주기
    public float fixedYPosition = -1.16f;         // 고정 y 좌표 값
    public float maxLifeTime = 10f;
    public float currentLifeTime = 0;

    private Vector3 moveVec;                    // 이동 벡터
    private Animator anim;                      // 애니메이터


    void Start()
    {
        anim = GetComponent<Animator>();
        StartCoroutine(ChangeDirectionRoutine()); // 방향 변경 코루틴 시작
    }

    void Update()
    {
        // 이동 처리
        transform.position += moveVec * speed * Time.deltaTime;

        // y 좌표를 고정하여 위치 업데이트
        transform.position = new Vector3(transform.position.x, fixedYPosition, transform.position.z);

        // 이동 방향에 따라 캐릭터 회전 및 애니메이션
        if (moveVec != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(moveVec);
            // anim.SetBool("isWalk", true);
        }
        else
        {
            // anim.SetBool("isWalk", false);
        }

        currentLifeTime += Time.deltaTime;
        if (currentLifeTime > maxLifeTime)
        {
            Destroy(this.gameObject);
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
        float randomX = Random.Range(-1f, 1f);
        float randomZ = Random.Range(-1f, 1f);
        moveVec = new Vector3(randomX, 0, randomZ).normalized;
    }

    void OnCollisionEnter(Collision collision)
    {
        // 다른 오브젝트와 충돌하면 새로운 방향으로 전환
        SetRandomDirection();
    }
}