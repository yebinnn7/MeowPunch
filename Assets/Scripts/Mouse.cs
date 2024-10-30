using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mouse : MonoBehaviour
{
    public float speed = 1f;                    // �̵� �ӵ�
    public float changeDirectionInterval = 2f;  // ���� ���� �ֱ�
    public float fixedYPosition = -1.16f;         // ���� y ��ǥ ��

    private Vector3 moveVec;                    // �̵� ����
    private Animator anim;                      // �ִϸ�����

    void Start()
    {
        anim = GetComponent<Animator>();
        StartCoroutine(ChangeDirectionRoutine()); // ���� ���� �ڷ�ƾ ����
    }

    void Update()
    {
        // �̵� ó��
        transform.position += moveVec * speed * Time.deltaTime;

        // y ��ǥ�� �����Ͽ� ��ġ ������Ʈ
        transform.position = new Vector3(transform.position.x, fixedYPosition, transform.position.z);

        // �̵� ���⿡ ���� ĳ���� ȸ�� �� �ִϸ��̼�
        if (moveVec != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(moveVec);
            // anim.SetBool("isWalk", true);
        }
        else
        {
            // anim.SetBool("isWalk", false);
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
}