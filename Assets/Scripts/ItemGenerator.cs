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
    private float bombMinTime = 90f; // 1분 30초
    private float bombMaxTime = 150f; // 2분 30초
    private float speedMinTime = 30f;
    private float speedMaxTime = 50f;

    // Start is called before the first frame update
    void Start()
    {
        // 랜덤 위치 리스트 초기화
        pos.Add(new Vector3(UnityEngine.Random.Range(-7, -4), -0.76f, 7.5f));
        pos.Add(new Vector3(-7.3f, -0.76f, UnityEngine.Random.Range(1, 5)));
        pos.Add(new Vector3(1.3f, -0.76f, UnityEngine.Random.Range(1.3f, 6.3f)));
        pos.Add(new Vector3(4.8f, -0.76f, UnityEngine.Random.Range(4, 6)));
        pos.Add(new Vector3(UnityEngine.Random.Range(0.6f, 9.4f), -0.76f, UnityEngine.Random.Range(-10, -2.8f)));
        pos.Add(new Vector3(UnityEngine.Random.Range(-6, -4.5f), -0.76f, UnityEngine.Random.Range(-9, -8)));
        pos.Add(new Vector3(UnityEngine.Random.Range(-9, -4), -0.76f, UnityEngine.Random.Range(-5, -1.5f)));

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
        while (true) // 무한 반복
        {
            float spawnTime = UnityEngine.Random.Range(bombMinTime, bombMaxTime); // 1분 30초 ~ 2분 30초 사이의 랜덤 시간
            SpawnItem(bombItemPrefab); // Bomb 아이템 생성
            yield return new WaitForSeconds(spawnTime); // 랜덤 대기 시간
        }
    }

    // 아이템 생성 함수 (공통으로 사용)
    void SpawnItem(GameObject itemPrefab)
    {
        Vector3 spawnPosition = pos[UnityEngine.Random.Range(0, pos.Count)];
        Instantiate(itemPrefab, spawnPosition, Quaternion.identity);
    }
}
