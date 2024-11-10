using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    private List<Collider> collidersInRange = new List<Collider>(); // 트리거 영역에 있는 객체들을 저장

    void Awake()
    {

    }

    // 트리거에 들어간 객체를 리스트에 추가
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("hamster") || other.CompareTag("mouse"))
        {
            if (!collidersInRange.Contains(other)) // 이미 리스트에 있지 않으면 추가
            {
                collidersInRange.Add(other);
                // Debug.Log("트리거에 들어감: " + other.gameObject.name); // 디버그 로그
            }
        }
    }

    // 트리거에서 나간 객체를 리스트에서 제거
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("hamster") || other.CompareTag("mouse"))
        {
            collidersInRange.Remove(other); // 트리거에서 나가면 리스트에서 제거
            // Debug.Log("트리거에서 나감: " + other.gameObject.name); // 디버그 로그
        }
    }

    // 마우스 클릭 시, 트리거 영역에 있는 모든 객체 삭제
    void Update()
    {
        // 트리거 영역 내에서 삭제된 객체는 null 참조가 될 수 있으므로, 그 객체가 유효한지 체크
        for (int i = collidersInRange.Count - 1; i >= 0; i--)
        {
            Collider collider = collidersInRange[i];

            // 객체가 삭제되었으면 리스트에서 제거
            if (collider == null)
            {
                collidersInRange.RemoveAt(i);
                continue;
            }
        }

            if (Input.GetMouseButtonDown(0))
        {
            // 트리거 안에 있는 모든 객체 삭제
            foreach (Collider collider in collidersInRange)
            {
                // Debug.Log("삭제 대상: " + collider.gameObject.name); // 디버그 로그

                if (collider.tag == "mouse")
                {
                    MouseController mouseController = collider.GetComponent<MouseController>(); // MouseController 컴포넌트 가져오기
                    if (mouseController != null)
                    {
                        mouseController.MouseDead(); // MouseDead 함수 호출
                    }
                }

                if (collider.tag == "hamster")
                {
                    HamsterController hamsterController = collider.GetComponent<HamsterController>(); // MouseController 컴포넌트 가져오기
                    if (hamsterController != null)
                    {
                        hamsterController.HamsterDead(); // MouseDead 함수 호출
                    }
                }
            }

            // 리스트 초기화 (삭제된 객체들을 다시 관리할 필요가 없음)
            collidersInRange.Clear();
        }
    }


}
