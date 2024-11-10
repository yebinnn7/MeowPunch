using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Security.Cryptography;
using System.Threading;
using UnityEngine;

public class CatController : MonoBehaviour
{
    public float speed = 5f; // �̵� �ӵ�
    public Transform cameraTransform; // ī�޶� Transform

    float hAxis;
    float vAxis;

    Vector3 moveVec;
    Animator anim;


    void Start()
    {
        anim = GetComponent<Animator>();

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
        moveVec = (camForward * vAxis + camRight * hAxis).normalized;

        // �̵� ó��
        transform.position += moveVec * speed * Time.deltaTime;

        // �̵� ���⿡ ���� ĳ���� ȸ��
        if (moveVec != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(moveVec);
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
}