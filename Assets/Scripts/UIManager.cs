using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }  // 싱글톤 인스턴스

    public Text mouseCatchCountText;
    public Text timerText;
    public Text levelText;
    public Text totalMouseCountText;

    [SerializeField] private TMP_Text RangeItemText;
    [SerializeField] private TMP_Text SpeedItemText;
    [SerializeField] private TMP_Text BombItemText;
    public float fadeDuration = 0.5f; // Fade Out 효과의 지속 시간

    private List<TMP_Text> activeTexts = new List<TMP_Text>(); // 현재 활성화된 텍스트들을 추적

    private float currentYOffset = 0f;  // 초기 Y offset 값 (0에서 시작)

    public float timer;
    public bool isTimer;

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


        CatController.OnRangeIncrease += UpdateRangeItemText;
        CatController.OnSpeedIncrease += UpdateSpeedItemText;
        CatController.OnKillAllMouse += UpdateBombItemText;

        isTimer = true;
        
        
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

    public void UpdateRangeItemText()
    {
        ShowAndFadeText(RangeItemText, 5f);  // Range 아이템은 5초 후 사라짐
    }

    public void UpdateSpeedItemText()
    {
        ShowAndFadeText(SpeedItemText, 5f);  // Speed 아이템은 5초 후 사라짐
    }

    public void UpdateBombItemText()
    {
        ShowAndFadeText(BombItemText, 3f);  // Bomb 아이템은 3초 후 사라짐
    }

    public void ShowAndFadeText(TMP_Text targetText, float fadeOutTime)
    {
        // 텍스트를 활성화하고 알파 값을 1로 설정
        targetText.gameObject.SetActive(true);
        Color textColor = targetText.color;
        textColor.a = 1f; // 알파 값 1 (불투명)
        targetText.color = textColor;

        // 텍스트의 위치 설정 (현재 Y 값에 45씩 추가)
        targetText.transform.localPosition = new Vector3(0, currentYOffset, 0);

        // 텍스트 리스트에 추가
        activeTexts.Add(targetText);

        // 다음 텍스트의 Y 오프셋 증가
        currentYOffset += 45f;

        // 지정된 시간 후 Fade Out 효과 시작
        StartCoroutine(WaitAndFadeOut(targetText, fadeOutTime));
    }

    private IEnumerator WaitAndFadeOut(TMP_Text targetText, float fadeOutTime)
    {
        // 지정된 시간만큼 대기
        yield return new WaitForSeconds(fadeOutTime);

        // Fade Out 효과 시작
        yield return FadeOutText(targetText);
    }

    private IEnumerator FadeOutText(TMP_Text targetText)
    {
        float elapsedTime = 0f;
        Color textColor = targetText.color;

        while (elapsedTime < fadeDuration)
        {
            // 경과 시간에 따라 알파 값을 감소
            elapsedTime += Time.deltaTime;
            textColor.a = Mathf.Lerp(1f, 0f, elapsedTime / fadeDuration);
            targetText.color = textColor;

            yield return null; // 다음 프레임까지 대기
        }

        // Fade Out 완료 후 텍스트 비활성화
        targetText.gameObject.SetActive(false);

        // 텍스트가 사라졌다면, 위 텍스트들을 한 칸씩 아래로 내리기
        AdjustTextPositions();
    }

    private void AdjustTextPositions()
    {
        // 텍스트가 사라졌을 때, 사라지지 않은 텍스트들의 위치를 갱신하는 방식
        List<TMP_Text> remainingTexts = new List<TMP_Text>(); // 사라지지 않은 텍스트들만 관리
        foreach (var text in activeTexts)
        {
            if (text.gameObject.activeSelf)
            {
                remainingTexts.Add(text);
            }
        }

        // Y 위치 재조정
        float newYOffset = 0f;
        foreach (var text in remainingTexts)
        {
            text.transform.localPosition = new Vector3(0, newYOffset, 0);
            newYOffset += 45f; // 각 텍스트마다 45씩 간격을 둔다
        }

        // 활성화된 텍스트들로 리스트 갱신
        activeTexts = remainingTexts;
        // Y 오프셋을 갱신 (다음 텍스트가 정확히 올 위치로 설정)
        currentYOffset = newYOffset;
    }
}