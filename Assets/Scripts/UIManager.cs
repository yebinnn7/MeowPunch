using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }  // 싱글톤 인스턴스

    public Text mouseCatchCountText;
    public Text timerText;

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
        mouseCatchCountText.text = "잡은 쥐: " + GameManager.Instance.mouseCatchCount;
    }
}
