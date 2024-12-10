using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Security.Cryptography;
using UnityEngine;

public class Attack : MonoBehaviour
{
    private List<Collider> collidersInRange = new List<Collider>(); // 트리거 영역에 있는 객체들을 저장
    private Vector3 originalScale;

    // 공격 범위가 증가했는지 확인하는 변수
    private bool isRangeIncreased = false;
    private float rangeIncreaseEndTime = 0f; // 범위 증가 종료 시간


    void Awake()
    {
        // GameManager의 OnLevelUp 이벤트 구독
        GameManager.Instance.OnLevelUp += RangeIncrease;
        CatController.OnRangeIncrease += RangeIncreaseItem;
        CatController.OnKillAllMouse += KillAllMouseItem;

        GameManager.Instance.GameReStart += ResetRange;

        originalScale = transform.localScale;


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
            if (!UIManager.Instance.clearImage.gameObject.activeSelf)
            {
                SoundManager.Instance.PlaySound("attack");
            }


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
                        GameManager.Instance.GameOver();
                    }
                }
            }

            // 리스트 초기화 (삭제된 객체들을 다시 관리할 필요가 없음)
            collidersInRange.Clear();
        }
    }

    void RangeIncrease()
    {
        // 현재 오브젝트의 스케일 값을 가져옴
        Vector3 currentScale = transform.localScale;

        // X와 Z 축의 스케일 값을 증가
        currentScale.x += 0.25f; 
        currentScale.z += 0.25f;

        // 스케일 값을 적용
        transform.localScale = currentScale;

        originalScale = currentScale;
    }

    // 아이템 사용 시 공격 범위 증가 (5초 동안만)
    void RangeIncreaseItem()
    {
        if (!isRangeIncreased)
        {
            // 범위 증가 시작
            StartCoroutine(IncreaseRangeTemporarily());
        }
        else
        {
            // 범위 증가가 이미 진행 중이면 5초를 추가
            rangeIncreaseEndTime += 5f;
        }
    }

    // 5초 동안 공격 범위 증가
    IEnumerator IncreaseRangeTemporarily()
    {
        isRangeIncreased = true;
        rangeIncreaseEndTime = Time.time + 5f; // 5초 후 종료 시간을 설정

        // 현재 범위 저장
        Vector3 increasedScale = transform.localScale;
        increasedScale.x += 2f;
        increasedScale.z += 2f;
        transform.localScale = increasedScale;

        // 5초 기다린 후, 원래 크기로 돌아감
        while (Time.time < rangeIncreaseEndTime)
        {
            yield return null; // 범위 증가가 끝날 때까지 기다림
        }

        // 원래 크기로 복원
        transform.localScale = originalScale;
        isRangeIncreased = false; // 범위 증가 상태 해제
    }

    void KillAllMouseItem()
    {
        // 태그가 "mouse"인 모든 오브젝트를 배열로 가져옴
        GameObject[] mice = GameObject.FindGameObjectsWithTag("mouse");

        // 각 마우스 오브젝트 삭제
        foreach (GameObject mouse in mice)
        {
            MouseController mouseController = mouse.GetComponent<MouseController>(); // MouseController 컴포넌트를 가져옴
                  
            mouseController.MouseDead(); // MouseDead 함수 호출
            
        }
    }

    void ResetRange()
    {
        transform.localScale = new Vector3(1.5f, 0.001f, 1.5f);
    }
}
