using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseGenerator : MonoBehaviour
{
    public GameObject mousePrefab;
    private Vector3 pos1 = new Vector3(10f, -1.16f, -10f);
    private Vector3 pos2 = new Vector3(-6f, -1.16f, 8f);

    private float timer = 0f;  // 타이머 값
    private float spawnInterval = 3f;  // 기본 생성 간격

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnMouseCoroutine());  // 마우스 생성 코루틴 시작
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;  // 매 프레임마다 타이머 증가

        // 타이머가 특정 값에 도달하면 생성 간격을 업데이트
        if (timer >= 10f && timer < 20f)
        {
            SetSpawnInterval(10f);  // 10초에 맞는 생성 간격 설정
        }
        else if (timer >= 20f && timer < 30f)
        {
            SetSpawnInterval(20f);  // 20초에 맞는 생성 간격 설정
        }
        else if (timer >= 30f)
        {
            SetSpawnInterval(30f);  // 30초 이상일 때 생성 간격 설정
        }
    }

    // 타이머 값에 맞는 생성 간격을 설정하는 함수
    void SetSpawnInterval(float currentTimer)
    {
        if (currentTimer == 10f)
        {
            spawnInterval = 2f;  
        }
        else if (currentTimer == 20f)
        {
            spawnInterval = 1.5f; 
        }
        else if (currentTimer >= 30f)
        {
            spawnInterval = 1f; 
        }
    }

    // 마우스를 생성하는 코루틴
    IEnumerator SpawnMouseCoroutine()
    {
        while (true)  // 무한 반복
        {
            SpawnMouse();  // 마우스 생성
            yield return new WaitForSeconds(spawnInterval);  // spawnInterval만큼 기다림
        }
    }

    // 마우스 생성 함수
    void SpawnMouse()
    {
        Vector3 spawnPosition = Random.Range(0, 2) == 0 ? pos1 : pos2;
        Instantiate(mousePrefab, spawnPosition, Quaternion.identity);
    }
}
