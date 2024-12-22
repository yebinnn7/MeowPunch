using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class HamsterController : MonoBehaviour
{
    public float speed = 1f;                    // �̵� �ӵ�
    public float changeDirectionInterval = 2f;  // ���� ���� �ֱ�
    public float fixedYPosition = -1.16f;       // ���� y ��ǥ ��
    public float maxLifeTime = 10f;
    public float currentLifeTime;

    private Vector3 moveVec;                    // �̵� ����
    private Animator anim;                      // �ִϸ�����
    private Rigidbody rb;                       // Rigidbody ����

    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();          // Rigidbody ������Ʈ ����
        currentLifeTime = 0f;                    // Ÿ�̸� �ʱ�ȭ
        StartCoroutine(ChangeDirectionRoutine()); // ���� ���� �ڷ�ƾ ����

        rb.isKinematic = false; // Rigidbody�� ���� ������ ���� ����ǵ��� ����
    }

    void Update()
    {
        if (currentLifeTime > maxLifeTime)
        {
            Destroy(gameObject);
            return;
        }

        // �̵� ó�� (Rigidbody�� �ӵ��� ����Ͽ� �̵�)
        rb.velocity = moveVec * speed;  // Rigidbody�� �ӵ� ������ �̵�, deltaTime�� �̿��� ������ ���������� ó��

        // y ��ǥ�� �����Ͽ� ��ġ ������Ʈ
        transform.position = new Vector3(transform.position.x, fixedYPosition, transform.position.z);

        // �̵� ���⿡ ���� ĳ���� ȸ��
        if (moveVec != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(moveVec);
        }

        currentLifeTime += Time.deltaTime;
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
        float randomX = Random.Range(-1f, 1f);
        float randomZ = Random.Range(-1f, 1f);
        moveVec = new Vector3(randomX, 0, randomZ).normalized; // ���͸� ����ȭ�Ͽ� �̵� ���� ����
    }

    void OnCollisionEnter(Collision collision)
    {
        // �ٸ� ������Ʈ�� �浹�ϸ� ���ο� �������� ��ȯ
        SetRandomDirection();
    }

    public void HamsterDead()
    {
        anim.SetTrigger("Death");
        StartCoroutine(HamsterDestroy(0.5f));
    }

    IEnumerator HamsterDestroy(float delay)
    {
        // ������ �ð� �Ŀ� ��ü ����
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
    }
}