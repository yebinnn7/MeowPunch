using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using Random = UnityEngine.Random;

public class HamsterGenerator : MonoBehaviour
{
    public GameObject hamsterPrefab;

    private Vector3 pos1 = new Vector3(10f, -1.16f, -10f);
    private Vector3 pos2 = new Vector3(-6, -1.16f, 8f);

    void Start()
    {
        // Debug.Log("햄스터 생성기 시작");
        StartCoroutine(SpawnHamsterCoroutine()); // 코루틴 시작
    }

    IEnumerator SpawnHamsterCoroutine()
    {
        yield return new WaitForSeconds(5f); // 처음 생성 대기 시간

        while (true) // 계속해서 햄스터 생성
        {
            Vector3 spawnPosition = Random.Range(0, 2) == 0 ? pos1 : pos2;
            GameObject obj = Instantiate(hamsterPrefab, spawnPosition, Quaternion.identity);
            // Debug.Log("햄스터 생성: " + obj.name + " 위치: " + spawnPosition);

            yield return new WaitForSeconds(5f); // 다음 햄스터 생성 대기 시간
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
