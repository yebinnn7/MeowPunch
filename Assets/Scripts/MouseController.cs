using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseController : MonoBehaviour
{
    public GameObject gameManagerObj; // GameManager ������Ʈ ����
    public GameManager gameManager; // GameManager ��ũ��Ʈ ����

    public float speed = 5f;                    // �̵� �ӵ�
    public float changeDirectionInterval = 2f;  // ���� ���� �ֱ�
    public float fixedYPosition = -1.16f;       // ���� y ��ǥ ��

    private Vector3 moveVec;                    // �̵� ����
    private Animator anim;                      // �ִϸ�����
    private Rigidbody rb;                       // Rigidbody ����

    void Awake()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();  // Rigidbody ������Ʈ ����

        rb.isKinematic = false; // Rigidbody�� ���� �������� �����ϵ��� ����

        StartCoroutine(ChangeDirectionRoutine()); // ���� ���� �ڷ�ƾ ����

        speed = 2f;
    }

    void Update()
    {
        // �̵� ó��
        rb.velocity = moveVec * speed;  // Rigidbody�� �ӵ� ������ �̵�

        // y ��ǥ�� �����Ͽ� ��ġ ������Ʈ
        transform.position = new Vector3(transform.position.x, fixedYPosition, transform.position.z);

        // �̵� ���⿡ ���� ĳ���� ȸ�� �� �ִϸ��̼�
        if (moveVec != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(moveVec);
        }


    }

    IEnumerator ChangeDirectionRoutine()
    {
        while (true)
        {
            // ���� �ð����� ���� ����
            yield return new WaitForSeconds(changeDirectionInterval);
            SetRandomDirection();
        }
    }

    void SetRandomDirection()
    {
        // �������� ���ο� �̵� ���� ����
        float randomX = UnityEngine.Random.Range(-1f, 1f);
        float randomZ = UnityEngine.Random.Range(-1f, 1f);
        moveVec = new Vector3(randomX, 0, randomZ).normalized;


    }

    void OnCollisionEnter(Collision collision)
    {
        // �ٸ� ������Ʈ�� �浹�ϸ� ���ο� �������� ��ȯ
        SetRandomDirection();
    }

    public void MouseDead()
    {
        anim.SetTrigger("Death");
        StartCoroutine(MouseDestroy(0.5f));
        GameManager.Instance.AddMouseCount();
    }

    IEnumerator MouseDestroy(float delay)
    {
        // Wait for the specified delay (1 second in this case)
        yield return new WaitForSeconds(delay);

        // Destroy the GameObject after the delay
        Destroy(gameObject);
    }
}