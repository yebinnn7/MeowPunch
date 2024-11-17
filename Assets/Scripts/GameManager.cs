using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public int mouseCatchCount;
    public float timer;
    public int level;

    private int nextLevelCondition;

    // 레벨업 이벤트
    public event Action OnLevelUp;

    // Start is called before the first frame update
    void Awake()
    {
        // 싱글톤 인스턴스 초기화
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // 씬 이동 시 파괴되지 않게 설정
        }
        else
        {
            Destroy(gameObject); // 중복된 인스턴스 파괴
        }

        mouseCatchCount = 0;
        timer = 0f;
        level = 1;
        nextLevelCondition = 20;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if (mouseCatchCount >= nextLevelCondition)
        {
            LevelUp();
        }

    }

    public void AddMouseCount()
    {
        mouseCatchCount++;

        UIManager.Instance.UpdateMouseCatchCountText();
    }

    // 타이머 값을 반환하는 함수
    public float GetTimer()
    {
        return timer;
    }

    public void LevelUp()
    {
        level += 1;
        nextLevelCondition += 20;

        OnLevelUp?.Invoke();
        UIManager.Instance.UpdateLevelText();
    }
}
