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

        // Instantiate(hamsterPrefab, spawnPosition, Quaternion.identity);
    }
}
