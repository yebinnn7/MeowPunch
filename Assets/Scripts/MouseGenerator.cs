using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseGenerator : MonoBehaviour
{
    public GameObject mousePrefab;
    private List<Vector3> pos = new List<Vector3>();


    private float timer = 0f;  // Ÿ�̸� ��
    private float spawnInterval = 3f;  // �⺻ ���� ����

    // Start is called before the first frame update
    void Start()
    {
        pos.Add(new Vector3(10f, -1.16f, -10f));
        pos.Add(new Vector3(-6f, -1.16f, 8f));
        pos.Add(new Vector3(7.9f, -1.16f, 6.7f));
        pos.Add(new Vector3(-5f, -1.16f, -3.3f));
        pos.Add(new Vector3(-4f, -1.16f, -9f));

        StartCoroutine(SpawnMouseCoroutine());  // ���콺 ���� �ڷ�ƾ ����
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;  // �� �����Ӹ��� Ÿ�̸� ����

        // Ÿ�̸Ӱ� Ư�� ���� �����ϸ� ���� ������ ������Ʈ
        if (timer >= 10f && timer < 20f)
        {
            SetSpawnInterval(10f);  // 10�ʿ� �´� ���� ���� ����
        }
        else if (timer >= 20f && timer < 30f)
        {
            SetSpawnInterval(20f);  // 20�ʿ� �´� ���� ���� ����
        }
        else if (timer >= 30f)
        {
            SetSpawnInterval(30f);  // 30�� �̻��� �� ���� ���� ����
        }
    }

    // Ÿ�̸� ���� �´� ���� ������ �����ϴ� �Լ�
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

    // ���콺�� �����ϴ� �ڷ�ƾ
    IEnumerator SpawnMouseCoroutine()
    {
        while (true)  // ���� �ݺ�
        {
            SpawnMouse();  // ���콺 ����
            yield return new WaitForSeconds(spawnInterval);  // spawnInterval��ŭ ��ٸ�
        }
    }

    // ���콺 ���� �Լ�
    void SpawnMouse()
    {
        Vector3 spawnPosition = pos[Random.Range(0, 5)];
        Instantiate(mousePrefab, spawnPosition, Quaternion.identity);
    }
}
