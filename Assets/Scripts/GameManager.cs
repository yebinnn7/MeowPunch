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

    // ������ �̺�Ʈ
    public event Action OnLevelUp;

    // Start is called before the first frame update
    void Awake()
    {
        // �̱��� �ν��Ͻ� �ʱ�ȭ
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // �� �̵� �� �ı����� �ʰ� ����
        }
        else
        {
            Destroy(gameObject); // �ߺ��� �ν��Ͻ� �ı�
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

    // Ÿ�̸� ���� ��ȯ�ϴ� �Լ�
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
