using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }  // �̱��� �ν��Ͻ�

    public Text mouseCatchCountText;
    public Text timerText;

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
        mouseCatchCountText.text = "���� ��: " + GameManager.Instance.mouseCatchCount;
    }
}
