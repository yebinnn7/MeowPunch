using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }  // �̱��� �ν��Ͻ�

    public Text mouseCatchCountText;
    public Text timerText;
    public Text levelText;
    public Text totalMouseCountText;

    public float timer;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);  // �� ��ȯ �ÿ��� �������� �ʵ���
        }
        else
        {
            Destroy(gameObject);  // �̹� �ν��Ͻ��� �����ϸ� �ߺ� ��ü�� ����
        }
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        timerText.text = timer.ToString("F2");
    }

    public void UpdateMouseCatchCountText()
    {
        // GameManager���� ������ mouseCatchCount�� nextLevelCondition�� ����Ͽ� UI�� ������Ʈ
        int nextLevelCondition = GameManager.Instance.GetNextLevelCondition();
        mouseCatchCountText.text = GameManager.Instance.mouseCatchCount + " / " + nextLevelCondition;
    }

    public void UpdateLevelText()
    {
        // ������ ������ ������ UI�� ǥ��
        levelText.text = "Lv " + GameManager.Instance.level;
        UpdateMouseCatchCountText();  // ������ ���ǿ� ���� mouseCatchCountText�� ������Ʈ
    }

    public void UpdateTotalMouseCount()
    {
        totalMouseCountText.text = GameManager.Instance.totalMouseCount.ToString();
    }
}
