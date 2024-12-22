using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseGenerator : MonoBehaviour
{
    public GameObject mousePrefab;
    private List<Vector3> pos = new List<Vector3>();

    public float timer = 0f;  // Ÿ�̸� ��
    private float spawnInterval = 3f;  // �⺻ ���� ����
    private float reductionFactor = 0.5f; // ���� ���� ���� ����
    private float reductionPeriod = 15f; // ���� �ֱ�

    // Start is called before the first frame update
    void Start()
    {
        pos.Add(new Vector3(10f, -1.16f, -10f));
        pos.Add(new Vector3(-6f, -1.16f, 8f));
        pos.Add(new Vector3(7.9f, -1.16f, 6.7f));
        pos.Add(new Vector3(-5f, -1.16f, -3.3f));
        pos.Add(new Vector3(-4f, -1.16f, -9f));

        GameManager.Instance.GameReStart += TimerReset;

        StartCoroutine(SpawnMouseCoroutine());  // ���콺 ���� �ڷ�ƾ ����
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;  // �� �����Ӹ��� Ÿ�̸� ����

        // ���� ���� ������Ʈ
        SetSpawnInterval(timer);
    }

    // Ÿ�̸� ���� �´� ���� ������ �����ϴ� �Լ�
    void SetSpawnInterval(float currentTimer)
    {
        // ���� �ֱ⸶�� ���� ���� ���� ���
        int reductionCount = Mathf.FloorToInt(currentTimer / reductionPeriod);
        spawnInterval = 3f * Mathf.Pow(reductionFactor, reductionCount);

        // ���� ������ ���Ѽ��� ���� (�ɼ�)
        spawnInterval = Mathf.Max(spawnInterval, 0.13f);
    }

    // ���콺�� �����ϴ� �ڷ�ƾ
    IEnumerator SpawnMouseCoroutine()
    {
        while (true) // ���� �ݺ�
        {
            SpawnMouse(); // ���콺 ����
            yield return new WaitForSeconds(spawnInterval); // spawnInterval��ŭ ��ٸ�
        }
    }

    // ���콺 ���� �Լ�
    void SpawnMouse()
    {
        Vector3 spawnPosition = pos[UnityEngine.Random.Range(0, pos.Count)];
        Instantiate(mousePrefab, spawnPosition, Quaternion.identity);
    }

    void TimerReset()
    {
        timer = 0;
    }
}