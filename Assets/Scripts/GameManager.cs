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


    public event Action GameReStart;

    public int bestscore;


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
        bestscore = 0;
        
        


    }

    void Update()
    {
        timer += Time.deltaTime;

        // 레벨업 조건을 초과하면 레벨업
        if (mouseCatchCount >= nextLevelCondition)
        {
            LevelUp();
        }

        if (totalMouseCount >= gameOverMouse)
        {
            GameOver();
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
            SoundManager.Instance.PlaySound("levelup");
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
        if (UIManager.Instance != null)
        {
            UIManager.Instance.UpdateTotalMouseCount();
        }
        else
        {
            Debug.LogWarning("UIManager instance is not set.");
        }
    }

    public void GameOver()
    {
        SoundManager.Instance.PlaySound("dead");

        if (mouseCatchCount > bestscore)
        {
            bestscore = mouseCatchCount;
        }

        // 게임 오버 상태에서 UI 갱신
        UIManager.Instance.clearImage.gameObject.SetActive(true);
        UIManager.Instance.restartButton.gameObject.SetActive(true);

        UIManager.Instance.bestScoreText.text = "최고점수: " + bestscore;
        UIManager.Instance.currentScoreText.text = "현재점수: " + mouseCatchCount;

        // "Mouse" 태그를 가진 모든 오브젝트를 찾음
        GameObject[] mouseObjects = GameObject.FindGameObjectsWithTag("mouse");
        foreach (GameObject mouse in mouseObjects)
        {
            Destroy(mouse); // 해당 오브젝트 제거
        }

        // "Hamster" 태그를 가진 모든 오브젝트를 찾음
        GameObject[] hamsters = GameObject.FindGameObjectsWithTag("hamster");
        foreach (GameObject hamster in hamsters)
        {
            Destroy(hamster); // 해당 오브젝트 제거
        }

        // "Item" 태그를 가진 모든 오브젝트를 찾음
        GameObject[] items = GameObject.FindGameObjectsWithTag("Item");
        foreach (GameObject item in items)
        {
            Destroy(item); // 해당 오브젝트 제거
        }

        // 게임이 종료되면 타임스케일을 0으로 설정하여 시간 멈춤
        Time.timeScale = 0;
    }

    public void ResetGame()
    {
        Time.timeScale = 1; // 게임 재개 시 타임스케일을 1로 설정하여 시간 시작

        mouseCatchCount = 0;
        timer = 0f;
        level = 1;
        nextLevelCondition = levelUpConditions[level - 1];
        totalMouseCount = 0;

        GameReStart?.Invoke();
        
    }
}


