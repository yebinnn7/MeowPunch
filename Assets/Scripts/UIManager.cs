using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }  // 싱글톤 인스턴스

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
            DontDestroyOnLoad(gameObject);  // 씬 전환 시에도 삭제되지 않도록
        }
        else
        {
            Destroy(gameObject);  // 이미 인스턴스가 존재하면 중복 객체를 삭제
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
        // GameManager에서 가져온 mouseCatchCount와 nextLevelCondition을 사용하여 UI를 업데이트
        int nextLevelCondition = GameManager.Instance.GetNextLevelCondition();
        mouseCatchCountText.text = GameManager.Instance.mouseCatchCount + " / " + nextLevelCondition;
    }

    public void UpdateLevelText()
    {
        // 레벨과 레벨업 조건을 UI에 표시
        levelText.text = "Lv " + GameManager.Instance.level;
        UpdateMouseCatchCountText();  // 레벨업 조건에 맞춰 mouseCatchCountText를 업데이트
    }

    public void UpdateTotalMouseCount()
    {
        totalMouseCountText.text = GameManager.Instance.totalMouseCount.ToString();
    }
}
