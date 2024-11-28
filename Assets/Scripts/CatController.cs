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
                        OnRangeIncrease?.Invoke();
                        break;
                    case Item.Type.Speed:
                        UnityEngine.Debug.Log("���ǵ� ������ ȹ��");
                        SpeedUpItem();
                        OnSpeedIncrease?.Invoke();
                        break;
                    case Item.Type.Bomb:
                        UnityEngine.Debug.Log("��ź ������ ȹ��");
                        OnKillAllMouse?.Invoke();
                        break;
                }

                // ������ ����
                Destroy(other.gameObject);
            }
        }
    }

    void SpeedUpItem()
    {
        StartCoroutine(IncreaseSpeedTemporarily());
    }

    IEnumerator IncreaseSpeedTemporarily()
    {
        float increasedSpeed = speed;

        increasedSpeed += 2f;
        speed = increasedSpeed;

        // 5�� ��ٸ� ��, ���� ũ��� ���ư�
        yield return new WaitForSeconds(5f);

        speed = originalSpeed;
    }

    void ResetSpeed()
    {
        speed = 3f;
        transform.position = new Vector3(-3f, -1.16f, 0.66f);
        transform.rotation = Quaternion.Euler(0, 0, 0); // Quaternion���� ȸ�� ����
    }
}




