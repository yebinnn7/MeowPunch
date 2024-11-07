using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    private List<Collider> collidersInRange = new List<Collider>(); // Ʈ���� ������ �ִ� ��ü���� ����

    void Awake()
    {

    }

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
        // Ʈ���� ���� ������ ������ ��ü�� null ������ �� �� �����Ƿ�, �� ��ü�� ��ȿ���� üũ
        for (int i = collidersInRange.Count - 1; i >= 0; i--)
        {
            Collider collider = collidersInRange[i];

            // ��ü�� �����Ǿ����� ����Ʈ���� ����
            if (collider == null)
            {
                collidersInRange.RemoveAt(i);
                continue;
            }
        }

            if (Input.GetMouseButtonDown(0))
        {
            // Ʈ���� �ȿ� �ִ� ��� ��ü ����
            foreach (Collider collider in collidersInRange)
            {
                Debug.Log("���� ���: " + collider.gameObject.name); // ����� �α�

                if (collider.tag == "mouse")
                {
                    MouseController mouseController = collider.GetComponent<MouseController>(); // MouseController ������Ʈ ��������
                    if (mouseController != null)
                    {
                        mouseController.MouseDead(); // MouseDead �Լ� ȣ��
                    }
                }

                if (collider.tag == "hamster")
                {
                    HamsterController hamsterController = collider.GetComponent<HamsterController>(); // MouseController ������Ʈ ��������
                    if (hamsterController != null)
                    {
                        hamsterController.HamsterDead(); // MouseDead �Լ� ȣ��
                    }
                }
            }

            // ����Ʈ �ʱ�ȭ (������ ��ü���� �ٽ� ������ �ʿ䰡 ����)
            collidersInRange.Clear();
        }
    }


}
