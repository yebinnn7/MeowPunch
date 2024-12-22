using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemGenerator : MonoBehaviour
{
    public GameObject rangeItemPrefab;
    public GameObject speedItemPrefab;
    public GameObject bombItemPrefab;

    private List<Vector3> pos = new List<Vector3>();

    // 아이템 생성 주기 범위
    private float rangeMinTime = 30f;
    private float rangeMaxTime = 50f;
    private float bombMinTime = 50f; // 1분 30초
    private float bombMaxTime = 90f; // 2분 30초
    private float speedMinTime = 30f;
    private float speedMaxTime = 50f;

    // Start is called before the first frame update
    void Start()
    {
        // 랜덤 위치 리스트 초기화
        pos.Add(new Vector3(UnityEngine.Random.Range(-7, -4), -0.76f, 7.5f));
        pos.Add(new Vector3(-6.8f, -0.76f, UnityEngine.Random.Range(-5, 7.7f)));
        pos.Add(new Vector3(UnityEngine.Random.Range(-1.8f, 0f), -0.76f, UnityEngine.Random.Range(0.5f, 6f)));
        pos.Add(new Vector3(4f, -0.76f, UnityEngine.Random.Range(2.7f, 6.5f)));
        pos.Add(new Vector3(8f, -0.76f, UnityEngine.Random.Range(2.7f, 6.5f)));
        pos.Add(new Vector3(UnityEngine.Random.Range(1.5f, 9f), -0.76f, -3.2f));
        pos.Add(new Vector3(UnityEngine.Random.Range(2f, 5f), -0.76f, UnityEngine.Random.Range(-10f, -4f)));
        pos.Add(new Vector3(UnityEngine.Random.Range(-6f, -3.5f), -0.76f, UnityEngine.Random.Range(-9.5f, -8f)));
        pos.Add(new Vector3(UnityEngine.Random.Range(-9f, -2.5f), -0.76f, UnityEngine.Random.Range(-4.8f, -1.7f)));

        // 각 아이템의 생성 코루틴을 시작
        StartCoroutine(SpawnRangeItemCoroutine());
        StartCoroutine(SpawnSpeedItemCoroutine());
        StartCoroutine(SpawnBombItemCoroutine());
    }

    // Update는 현재 사용하지 않음
    void Update()
    {

    }

    // Range 아이템 생성 주기
    IEnumerator SpawnRangeItemCoroutine()
    {
        while (true) // 무한 반복
        {
            float spawnTime = UnityEngine.Random.Range(rangeMinTime, rangeMaxTime); // 30초 ~ 50초 사이의 랜덤 시간
            SpawnItem(rangeItemPrefab); // Range 아이템 생성
            yield return new WaitForSeconds(spawnTime); // 랜덤 대기 시간
        }
    }

    // Speed 아이템 생성 주기
    IEnumerator SpawnSpeedItemCoroutine()
    {
        while (true) // 무한 반복
        {
            float spawnTime = UnityEngine.Random.Range(speedMinTime, speedMaxTime); // 30초 ~ 50초 사이의 랜덤 시간
            SpawnItem(speedItemPrefab); // Speed 아이템 생성
            yield return new WaitForSeconds(spawnTime); // 랜덤 대기 시간
        }
    }

    // Bomb 아이템 생성 주기
    IEnumerator SpawnBombItemCoroutine()
    {
        // 20초 기다린 후 첫 번째 Bomb 아이템 생성
        yield return new WaitForSeconds(20f); // 첫 번째 생성까지 20초 대기

        while (true) // 무한 반복
        {
            SpawnItem(bombItemPrefab); // Bomb 아이템 생성

            // 이후에는 랜덤한 시간 간격으로 생성
            float spawnTime = UnityEngine.Random.Range(bombMinTime, bombMaxTime); // 1분 30초 ~ 2분 30초 사이의 랜덤 시간
            yield return new WaitForSeconds(spawnTime); // 랜덤 대기 시간
        }
    }

    void SpawnItem(GameObject itemPrefab)
    {
        Vector3 spawnPosition = GetRandomPosition();
        while (IsPositionOccupied(spawnPosition))
        {
            spawnPosition = GetRandomPosition(); // 겹치는 경우 새 위치 찾기
        }
        Instantiate(itemPrefab, spawnPosition, Quaternion.identity);
    }

    Vector3 GetRandomPosition()
    {
        // 랜덤 위치 반환
        return pos[UnityEngine.Random.Range(0, pos.Count)];
    }

    bool IsPositionOccupied(Vector3 position)
    {
        // 일정 반경 안에 다른 오브젝트가 있는지 확인 (여기선 1f 반경)
        Collider[] colliders = Physics.OverlapSphere(position, 1f);
        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag("Item")) // 아이템 태그를 가진 오브젝트가 있으면 겹친 것
            {
                return true;
            }
        }
        return false;
    }
}
