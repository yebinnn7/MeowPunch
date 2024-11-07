using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class HamsterController : MonoBehaviour
{
    public float speed = 1f;                    // �̵� �ӵ�
    public float changeDirectionInterval = 2f;  // ���� ���� �ֱ�
    public float fixedYPosition = -1.16f;         // ���� y ��ǥ ��
    public float maxLifeTime = 10f;
    public float currentLifeTime;

    private Vector3 moveVec;                    // �̵� ����
    private Animator anim;                      // �ִϸ�����


    void Start()
    {
        anim = GetComponent<Animator>();
        currentLifeTime = 0f; // Ÿ�̸� �ʱ�ȭ
        StartCoroutine(ChangeDirectionRoutine()); // ���� ���� �ڷ�ƾ ����
    }

    void Update()
    {
        if (currentLifeTime > maxLifeTime)
        {
            Destroy(gameObject);

            return;
        }

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
        moveVec = new Vector3(randomX, 0, randomZ).normalized;
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
        // Wait for the specified delay (1 second in this case)
        yield return new WaitForSeconds(delay);

        // Destroy the GameObject after the delay
        Destroy(gameObject);
    }
}