using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    private List<Collider> collidersInRange = new List<Collider>(); // Ʈ���� ������ �ִ� ��ü���� ����

    // Ʈ���ſ� �� ��ü�� ����Ʈ�� �߰�
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("hamster") || other.CompareTag("mouse"))
        {
            if (!collidersInRange.Contains(other)) // �̹� ����Ʈ�� ���� ������ �߰�
            {
                collidersInRange.Add(other);
                Debug.Log("Ʈ���ſ� ��: " + other.gameObject.name); // ����� �α�
            }
        }
    }

    // Ʈ���ſ��� ���� ��ü�� ����Ʈ���� ����
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("hamster") || other.CompareTag("mouse"))
        {
            collidersInRange.Remove(other); // Ʈ���ſ��� ������ ����Ʈ���� ����
            Debug.Log("Ʈ���ſ��� ����: " + other.gameObject.name); // ����� �α�
        }
    }

    // ���콺 Ŭ�� ��, Ʈ���� ������ �ִ� ��� ��ü ����
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // Ʈ���� �ȿ� �ִ� ��� ��ü ����
            foreach (Collider collider in collidersInRange)
            {
                Debug.Log("���� ���: " + collider.gameObject.name); // ����� �α�
                Destroy(collider.gameObject); // �ش� ��ü ����
            }

            // ����Ʈ �ʱ�ȭ (������ ��ü���� �ٽ� ������ �ʿ䰡 ����)
            collidersInRange.Clear();
        }
    }


}
