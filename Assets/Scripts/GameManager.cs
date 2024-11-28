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
        UIManager.Instance.UpdateTotalMouseCount();
    }

    
}
