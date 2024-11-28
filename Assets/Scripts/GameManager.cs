using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public int mouseCatchCount;
    public float timer;
    public int level;

    private int nextLevelCondition;  // 레벨업 조건
    private int[] levelUpConditions = new int[] { 10, 30, 60, 100, 150, 210, 280 };  // 레벨업 조건 (10, 30, 60, 100, ...)

    public event Action OnLevelUp;

    public int totalMouseCount;
    private float checkInterval = 0.1f; // 1초 간격
    public int gameOverMouse;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // 씬 전환 시에도 삭제되지 않도록
        }
        else
        {
            Destroy(gameObject);
        }

        StartCoroutine(CountMiceRoutine());

        mouseCatchCount = 0;
        timer = 0f;
        level = 1;
        nextLevelCondition = levelUpConditions[level - 1];  // 처음 레벨의 조건은 10

        
    }

    void Update()
    {
        timer += Time.deltaTime;

        // 레벨업 조건을 초과하면 레벨업
        if (mouseCatchCount >= nextLevelCondition)
        {
            LevelUp();
        }

        if (totalMouseCount > gameOverMouse)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("ClearScene");
            


        }
    }

    public void AddMouseCount()
    {
        mouseCatchCount++;
        UIManager.Instance.UpdateMouseCatchCountText();
    }

    public float GetTimer()
    {
        return timer;
    }

    public void LevelUp()
    {
        if (level < levelUpConditions.Length)
        {
            level++;
            nextLevelCondition = levelUpConditions[level - 1];  // 다음 레벨의 조건으로 업데이트
            OnLevelUp?.Invoke();  // 레벨업 이벤트 호출
            UIManager.Instance.UpdateLevelText();
        }
    }

    public int GetNextLevelCondition()
    {
        return nextLevelCondition;
    }

    IEnumerator CountMiceRoutine()
    {
        while (true)
        {
            CountMice();
            yield return new WaitForSeconds(checkInterval);
        }
    }

    void CountMice()
    {
        totalMouseCount = GameObject.FindGameObjectsWithTag("mouse").Length;
        UIManager.Instance.UpdateTotalMouseCount();
    }

    
}
