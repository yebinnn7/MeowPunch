using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemGenerator : MonoBehaviour
{
    public GameObject rangeItemPrefab;
    public GameObject speedItemPrefab;
    public GameObject bombItemPrefab;

    private List<Vector3> pos = new List<Vector3>();

    // ������ ���� �ֱ� ����
    private float rangeMinTime = 30f;
    private float rangeMaxTime = 50f;
    private float bombMinTime = 90f; // 1�� 30��
    private float bombMaxTime = 150f; // 2�� 30��
    private float speedMinTime = 30f;
    private float speedMaxTime = 50f;

    // Start is called before the first frame update
    void Start()
    {
        // ���� ��ġ ����Ʈ �ʱ�ȭ
        pos.Add(new Vector3(UnityEngine.Random.Range(-7, -4), -0.76f, 7.5f));
        pos.Add(new Vector3(-6.8f, -0.76f, UnityEngine.Random.Range(-5, 7.7f)));
        pos.Add(new Vector3(UnityEngine.Random.Range(-1.8f, 0f), -0.76f, UnityEngine.Random.Range(0.5f, 6f)));
        pos.Add(new Vector3(4f, -0.76f, UnityEngine.Random.Range(2.7f, 6.5f)));
        pos.Add(new Vector3(8f, -0.76f, UnityEngine.Random.Range(2.7f, 6.5f)));
        pos.Add(new Vector3(UnityEngine.Random.Range(1.5f, 9f), -0.76f, -3.2f));
        pos.Add(new Vector3(UnityEngine.Random.Range(2f, 5f), -0.76f, UnityEngine.Random.Range(-10f, -4f)));
        pos.Add(new Vector3(UnityEngine.Random.Range(-6f, -3.5f), -0.76f, UnityEngine.Random.Range(-9.5f, -8f)));
        pos.Add(new Vector3(UnityEngine.Random.Range(-9f, -2.5f), -0.76f, UnityEngine.Random.Range(-4.8f, -1.7f)));

        // �� �������� ���� �ڷ�ƾ�� ����
        StartCoroutine(SpawnRangeItemCoroutine());
        StartCoroutine(SpawnSpeedItemCoroutine());
        StartCoroutine(SpawnBombItemCoroutine());
    }

    // Update�� ���� ������� ����
    void Update()
    {

    }

    // Range ������ ���� �ֱ�
    IEnumerator SpawnRangeItemCoroutine()
    {
        while (true) // ���� �ݺ�
        {
            float spawnTime = UnityEngine.Random.Range(rangeMinTime, rangeMaxTime); // 30�� ~ 50�� ������ ���� �ð�
            SpawnItem(rangeItemPrefab); // Range ������ ����
            yield return new WaitForSeconds(spawnTime); // ���� ��� �ð�
        }
    }

    // Speed ������ ���� �ֱ�
    IEnumerator SpawnSpeedItemCoroutine()
    {
        while (true) // ���� �ݺ�
        {
            float spawnTime = UnityEngine.Random.Range(speedMinTime, speedMaxTime); // 30�� ~ 50�� ������ ���� �ð�
            SpawnItem(speedItemPrefab); // Speed ������ ����
            yield return new WaitForSeconds(spawnTime); // ���� ��� �ð�
        }
    }

    // Bomb ������ ���� �ֱ�
    IEnumerator SpawnBombItemCoroutine()
    {
        while (true) // ���� �ݺ�
        {
            float spawnTime = UnityEngine.Random.Range(bombMinTime, bombMaxTime); // 1�� 30�� ~ 2�� 30�� ������ ���� �ð�
            SpawnItem(bombItemPrefab); // Bomb ������ ����
            yield return new WaitForSeconds(spawnTime); // ���� ��� �ð�
        }
    }

    // ������ ���� �Լ� (�������� ���)
    void SpawnItem(GameObject itemPrefab)
    {
        Vector3 spawnPosition = pos[UnityEngine.Random.Range(0, pos.Count)];
        Instantiate(itemPrefab, spawnPosition, Quaternion.identity);
    }
}