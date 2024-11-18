using System.Collections;
using System.Collections.Generic;
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

    void Start()
    {
        anim = GetComponent<Animator>();
        GameManager.Instance.OnLevelUp += speedUp;
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
    }
}
