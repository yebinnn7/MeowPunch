using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseGenerator : MonoBehaviour
{
    public GameObject mousePrefab;
    private List<Vector3> pos = new List<Vector3>();

    private float timer = 0f;  // 타이머 값
    private float spawnInterval = 3f;  // 기본 생성 간격
    private float reductionFactor = 0.7f; // 생성 간격 감소 비율
    private float reductionPeriod = 10f; // 감소 주기

    // Start is called before the first frame update
    void Start()
    {
        pos.Add(new Vector3(10f, -1.16f, -10f));
        pos.Add(new Vector3(-6f, -1.16f, 8f));
        pos.Add(new Vector3(7.9f, -1.16f, 6.7f));
        pos.Add(new Vector3(-5f, -1.16f, -3.3f));
        pos.Add(new Vector3(-4f, -1.16f, -9f));

        StartCoroutine(SpawnMouseCoroutine());  // 마우스 생성 코루틴 시작
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;  // 매 프레임마다 타이머 증가

        // 생성 간격 업데이트
        SetSpawnInterval(timer);
    }

    // 타이머 값에 맞는 생성 간격을 설정하는 함수
    void SetSpawnInterval(float currentTimer)
    {
        // 감소 주기마다 생성 간격 감소 계산
        int reductionCount = Mathf.FloorToInt(currentTimer / reductionPeriod);
        spawnInterval = 3f * Mathf.Pow(reductionFactor, reductionCount);

        // 생성 간격의 하한선을 설정 (옵션)
        spawnInterval = Mathf.Max(spawnInterval, 0.2f); 
    }

    // 마우스를 생성하는 코루틴
    IEnumerator SpawnMouseCoroutine()
    {
        while (true) // 무한 반복
        {
            SpawnMouse(); // 마우스 생성
            yield return new WaitForSeconds(spawnInterval); // spawnInterval만큼 기다림
        }
    }

    // 마우스 생성 함수
    void SpawnMouse()
    {
        Vector3 spawnPosition = pos[Random.Range(0, pos.Count)];
        Instantiate(mousePrefab, spawnPosition, Quaternion.identity);
    }
}
