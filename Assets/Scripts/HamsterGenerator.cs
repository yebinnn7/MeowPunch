using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using Random = UnityEngine.Random;

public class HamsterGenerator : MonoBehaviour
{
    public GameObject hamsterPrefab;
    private List<Vector3> pos = new List<Vector3>();


    public float maxSpawnTime = 30f;
    public float minSpawnTime = 40f;


    void Start()
    {
        pos.Add(new Vector3(10f, -1.16f, -10f));
        pos.Add(new Vector3(-6f, -1.16f, 8f));
        pos.Add(new Vector3(7.9f, -1.16f, 6.7f));
        pos.Add(new Vector3(-5f, -1.16f, -3.3f));
        pos.Add(new Vector3(-4f, -1.16f, -9f));

        // Debug.Log("햄스터 생성기 시작");
        StartCoroutine(SpawnHamsterCoroutine()); // 코루틴 시작
    }

    IEnumerator SpawnHamsterCoroutine()
    {

        yield return new WaitForSeconds(10f); // 처음 생성 대기 시간

        while (true) // 계속해서 햄스터 생성
        {
            Vector3 spawnPosition = pos[Random.Range(0, 5)];
            GameObject obj = Instantiate(hamsterPrefab, spawnPosition, Quaternion.identity);
            // Debug.Log("햄스터 생성: " + obj.name + " 위치: " + spawnPosition);

            float spawnTime = Random.Range(minSpawnTime, maxSpawnTime); // 랜덤으로 생성 대기 시간 설정

            yield return new WaitForSeconds(spawnTime); // 다음 햄스터 생성 대기 시간
        }
    }



    /*
    

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("SpawnHamster", 5f, 5f);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void SpawnHamster()
    {
        Vector3 spawnPosition = Random.Range(0, 2) == 0 ? pos1 : pos2;

        GameObject obj = Instantiate(hamsterPrefab, spawnPosition, Quaternion.identity);
        HamsterController hamster = obj.GetComponent<HamsterController>();

        
    }
    */
}
