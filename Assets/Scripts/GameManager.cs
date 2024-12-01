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

    private int nextLevelCondition;  // ������ ����
    private int[] levelUpConditions = new int[] { 10, 30, 60, 100, 150, 210, 280 };  // ������ ���� (10, 30, 60, 100, ...)

    public event Action OnLevelUp;

    public int totalMouseCount;
    private float checkInterval = 0.1f; // 1�� ����
    public int gameOverMouse;


    public event Action GameReStart;

    public int bestscore;


    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // �� ��ȯ �ÿ��� �������� �ʵ���
        }
        else
        {
            Destroy(gameObject);
        }

        StartCoroutine(CountMiceRoutine());

        mouseCatchCount = 0;
        timer = 0f;
        level = 1;
        nextLevelCondition = levelUpConditions[level - 1];  // ó�� ������ ������ 10
        bestscore = 0;
        
        


    }

    void Update()
    {
        timer += Time.deltaTime;

        // ������ ������ �ʰ��ϸ� ������
        if (mouseCatchCount >= nextLevelCondition)
        {
            LevelUp();
        }

        if (totalMouseCount > gameOverMouse)
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
            nextLevelCondition = levelUpConditions[level - 1];  // ���� ������ �������� ������Ʈ
            OnLevelUp?.Invoke();  // ������ �̺�Ʈ ȣ��
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
        if (mouseCatchCount > bestscore)
        {
            bestscore = mouseCatchCount;
        }

        // ���� ���� ���¿��� UI ����
        UIManager.Instance.clearImage.gameObject.SetActive(true);
        UIManager.Instance.restartButton.gameObject.SetActive(true);

        UIManager.Instance.bestScoreText.text = "�ְ�����: " + bestscore;
        UIManager.Instance.currentScoreText.text = "��������: " + mouseCatchCount;

        // "Mouse" �±׸� ���� ��� ������Ʈ�� ã��
        GameObject[] mouseObjects = GameObject.FindGameObjectsWithTag("mouse");
        foreach (GameObject mouse in mouseObjects)
        {
            Destroy(mouse); // �ش� ������Ʈ ����
        }

        // "Hamster" �±׸� ���� ��� ������Ʈ�� ã��
        GameObject[] hamsters = GameObject.FindGameObjectsWithTag("hamster");
        foreach (GameObject hamster in hamsters)
        {
            Destroy(hamster); // �ش� ������Ʈ ����
        }

        // "Item" �±׸� ���� ��� ������Ʈ�� ã��
        GameObject[] items = GameObject.FindGameObjectsWithTag("Item");
        foreach (GameObject item in items)
        {
            Destroy(item); // �ش� ������Ʈ ����
        }

        // ������ ����Ǹ� Ÿ�ӽ������� 0���� �����Ͽ� �ð� ����
        Time.timeScale = 0;
    }

    public void ResetGame()
    {
        Time.timeScale = 1; // ���� �簳 �� Ÿ�ӽ������� 1�� �����Ͽ� �ð� ����

        mouseCatchCount = 0;
        timer = 0f;
        level = 1;
        nextLevelCondition = levelUpConditions[level - 1];
        totalMouseCount = 0;

        GameReStart?.Invoke();
        
    }
}


