using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using UnityEngine;


public class CatController : MonoBehaviour
{
    public float speed = 5f; // �̵� �ӵ�
    public Transform cameraTransform; // ī�޶� Transform
    public float smoothTime = 0.1f; // Lerp�� ���� �ε巴�� ����

    private float hAxis;
    private float vAxis;

    private Vector3 moveVec;
    private Vector3 currentVelocity; // Lerp�� ���� �ӵ�
    private Animator anim;

    public static event Action OnRangeIncrease;
    public static event Action OnSpeedIncrease;
    public static event Action OnKillAllMouse;

    private float originalSpeed;
    // �ӵ� ������ ���� ������ Ȯ���ϴ� ����
    private bool isSpeedIncreased = false;
    private float speedIncreaseEndTime = 0f; // �ӵ� ���� ���� �ð�


    void Start()
    {
        anim = GetComponent<Animator>();
        GameManager.Instance.OnLevelUp += speedUp;
        GameManager.Instance.GameReStart += ResetSpeed;

        originalSpeed = speed;
    }

    void Update()
    {
        // �Է°� �ޱ�
        hAxis = Input.GetAxisRaw("Horizontal"); // A, D Ű
        vAxis = Input.GetAxisRaw("Vertical");   // W, S Ű

        // ī�޶��� ������ �������� �̵� ���� ���
        Vector3 camForward = cameraTransform.forward;
        Vector3 camRight = cameraTransform.right;

        // ����, ���� ������ y �� ������ ��� �̵� ����
        camForward.y = 0;
        camRight.y = 0;

        // �Է� �������� �̵� ���� ����
        Vector3 targetMoveVec = (camForward * vAxis + camRight * hAxis).normalized;

        // Lerp�� �ε巴�� �̵� ���� ���
        moveVec = Vector3.SmoothDamp(moveVec, targetMoveVec, ref currentVelocity, smoothTime);

        // �̵� ó��
        transform.position += moveVec * speed * Time.deltaTime;

        // �̵� ���⿡ ���� ĳ���� ȸ��
        if (moveVec != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveVec);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * 10f);
            anim.SetBool("isWalk", true);
        }
        else
        {
            anim.SetBool("isWalk", false);
        }

        // ���� ���
        if (Input.GetMouseButtonDown(0))
        {
            anim.SetTrigger("isAttack");
        }
    }

    void speedUp()
    {
        speed += 0.5f;

        originalSpeed = speed;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Item")) // �±� Ȯ��
        {
            Item item = other.GetComponent<Item>();

            if (item != null)
            {
                // ����� �޽����� ȿ�� ����
                switch (item.type)
                {
                    case Item.Type.Range:
                        UnityEngine.Debug.Log("���� ������ ȹ��");
                        SoundManager.Instance.PlaySound("item");
                        OnRangeIncrease?.Invoke();
                        break;
                    case Item.Type.Speed:
                        UnityEngine.Debug.Log("���ǵ� ������ ȹ��");
                        SoundManager.Instance.PlaySound("item");
                        SpeedUpItem();
                        OnSpeedIncrease?.Invoke();
                        break;
                    case Item.Type.Bomb:
                        UnityEngine.Debug.Log("��ź ������ ȹ��");
                        SoundManager.Instance.PlaySound("bomb");
                        OnKillAllMouse?.Invoke();
                        break;
                }

                // ������ ����
                Destroy(other.gameObject);
            }
        }
    }

    // ������ ��� �� �ӵ� ���� (5�� ���ȸ�)
    void SpeedUpItem()
    {
        if (!isSpeedIncreased)
        {
            // �ӵ� ���� ����
            StartCoroutine(IncreaseSpeedTemporarily());
        }
        else
        {
            // �ӵ��� �̹� ���� ���̸� 5�ʸ� �߰�
            speedIncreaseEndTime += 5f;
        }
    }

    // 5�� ���� �ӵ� ����
    IEnumerator IncreaseSpeedTemporarily()
    {
        isSpeedIncreased = true;
        speedIncreaseEndTime = Time.time + 5f; // 5�� �� ���� �ð��� ����

        float increasedSpeed = speed;
        increasedSpeed += 2f;
        speed = increasedSpeed;

        // 5�� ��ٸ� ��, ���� �ӵ��� ���ư�
        while (Time.time < speedIncreaseEndTime)
        {
            yield return null; // �ӵ� ������ ���� ������ ��ٸ�
        }

        speed = originalSpeed;
        isSpeedIncreased = false; // �ӵ� ���� ���� ����
    }

    void ResetSpeed()
    {
        speed = 3f;
        transform.position = new Vector3(-3f, -1.16f, 0.66f);
        transform.rotation = Quaternion.Euler(0, 0, 0); // Quaternion���� ȸ�� ����
    }
}




