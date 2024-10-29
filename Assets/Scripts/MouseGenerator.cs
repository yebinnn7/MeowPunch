using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseGenerator : MonoBehaviour
{

    public GameObject mousePrefab;
    private Vector3 pos1 = new Vector3(10f, -1.16f, -10f);
    private Vector3 pos2 = new Vector3(-6, -1.16f, 8f);

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("SpawnMouse", 3f, 3f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SpawnMouse()
    {
        Vector3 spawnPosition = Random.Range(0, 2) == 0 ? pos1 : pos2;

        Instantiate(mousePrefab, spawnPosition, Quaternion.identity);
    }
}
