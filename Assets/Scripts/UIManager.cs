using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;


public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }  // �̱��� �ν��Ͻ�

    public Text mouseCatchCountText;
    public Text timerText;
    public Text levelText;
    public Text totalMouseCountText;

    [SerializeField] private TMP_Text RangeItemText;
    [SerializeField] private TMP_Text SpeedItemText;
    [SerializeField] private TMP_Text BombItemText;

    public Image clearImage;
    public Button restartButton;  // ��ư ��ü


    public float fadeDuration = 0.5f; // Fade Out ȿ���� ���� �ð�

    private List<TMP_Text> activeTexts = new List<TMP_Text>(); // ���� Ȱ��ȭ�� �ؽ�Ʈ���� ����

    private float currentYOffset = 0f;  // �ʱ� Y offset �� (0���� ����)

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


        CatController.OnRangeIncrease += UpdateRangeItemText;
        CatController.OnSpeedIncrease += UpdateSpeedItemText;
        CatController.OnKillAllMouse += UpdateBombItemText;

        

        
        
    }

    // ���� �ε�� ������ UI ������Ʈ���� ã�Ƽ� �Ҵ�
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        mouseCatchCountText = GameObject.Find("mouseCatchCount")?.GetComponent<Text>();
        timerText = GameObject.Find("TimerText")?.GetComponent<Text>();
        levelText = GameObject.Find("Level")?.GetComponent<Text>();
        totalMouseCountText = GameObject.Find("TotalMouseText")?.GetComponent<Text>();

        // UI ������Ʈ���� Canvas ������ ã�Ƽ� �Ҵ�
        Canvas canvas = FindObjectOfType<Canvas>();
        if (canvas != null)
        {
            RangeItemText = canvas.transform.Find("RangeItemText")?.GetComponent<TMP_Text>();
            SpeedItemText = canvas.transform.Find("SpeedItemText")?.GetComponent<TMP_Text>();
            BombItemText = canvas.transform.Find("BombItemText")?.GetComponent<TMP_Text>();
            clearImage = canvas.transform.Find("Image")?.GetComponent<Image>();
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
        if (GameManager.Instance != null)
        {
            totalMouseCountText.text = GameManager.Instance.totalMouseCount.ToString();
        }
        else
        {
            Debug.LogWarning("GameManager instance is not set.");
        }

    }

    public void UpdateRangeItemText()
    {
        ShowAndFadeText(RangeItemText, 5f);  // Range �������� 5�� �� �����
    }

    public void UpdateSpeedItemText()
    {
        ShowAndFadeText(SpeedItemText, 5f);  // Speed �������� 5�� �� �����
    }

    public void UpdateBombItemText()
    {
        ShowAndFadeText(BombItemText, 3f);  // Bomb �������� 3�� �� �����
    }

    public void ShowAndFadeText(TMP_Text targetText, float fadeOutTime)
    {
        // �ؽ�Ʈ�� Ȱ��ȭ�ϰ� ���� ���� 1�� ����
        targetText.gameObject.SetActive(true);
        Color textColor = targetText.color;
        textColor.a = 1f; // ���� �� 1 (������)
        targetText.color = textColor;

        // �ؽ�Ʈ�� ��ġ ���� (���� Y ���� 45�� �߰�)
        targetText.transform.localPosition = new Vector3(0, currentYOffset, 0);

        // �ؽ�Ʈ ����Ʈ�� �߰�
        activeTexts.Add(targetText);

        // ���� �ؽ�Ʈ�� Y ������ ����
        currentYOffset += 45f;

        // ������ �ð� �� Fade Out ȿ�� ����
        StartCoroutine(WaitAndFadeOut(targetText, fadeOutTime));
    }

    private IEnumerator WaitAndFadeOut(TMP_Text targetText, float fadeOutTime)
    {
        // ������ �ð���ŭ ���
        yield return new WaitForSeconds(fadeOutTime);

        // Fade Out ȿ�� ����
        yield return FadeOutText(targetText);
    }

    private IEnumerator FadeOutText(TMP_Text targetText)
    {
        float elapsedTime = 0f;
        Color textColor = targetText.color;

        while (elapsedTime < fadeDuration)
        {
            // ��� �ð��� ���� ���� ���� ����
            elapsedTime += Time.deltaTime;
            textColor.a = Mathf.Lerp(1f, 0f, elapsedTime / fadeDuration);
            targetText.color = textColor;

            yield return null; // ���� �����ӱ��� ���
        }

        // Fade Out �Ϸ� �� �ؽ�Ʈ ��Ȱ��ȭ
        targetText.gameObject.SetActive(false);

        // �ؽ�Ʈ�� ������ٸ�, �� �ؽ�Ʈ���� �� ĭ�� �Ʒ��� ������
        AdjustTextPositions();
    }

    private void AdjustTextPositions()
    {
        // �ؽ�Ʈ�� ������� ��, ������� ���� �ؽ�Ʈ���� ��ġ�� �����ϴ� ���
        List<TMP_Text> remainingTexts = new List<TMP_Text>(); // ������� ���� �ؽ�Ʈ�鸸 ����
        foreach (var text in activeTexts)
        {
            if (text.gameObject.activeSelf)
            {
                remainingTexts.Add(text);
            }
        }

        // Y ��ġ ������
        float newYOffset = 0f;
        foreach (var text in remainingTexts)
        {
            text.transform.localPosition = new Vector3(0, newYOffset, 0);
            newYOffset += 45f; // �� �ؽ�Ʈ���� 45�� ������ �д�
        }

        // Ȱ��ȭ�� �ؽ�Ʈ��� ����Ʈ ����
        activeTexts = remainingTexts;
        // Y �������� ���� (���� �ؽ�Ʈ�� ��Ȯ�� �� ��ġ�� ����)
        currentYOffset = newYOffset;
    }

    public void ClickReStartBtn()
    {
        // ���� �簳 ��ư Ŭ�� ��
        Time.timeScale = 1; // Ÿ�ӽ������� 1�� �����Ͽ� ������ �簳��
        GameManager.Instance.totalMouseCount = 0;

        // UI ���� �ʱ�ȭ
        clearImage.gameObject.SetActive(false);
        restartButton.gameObject.SetActive(false);

        // ���� ����
        GameManager.Instance.ResetGame();

        // UI ������Ʈ
        ResetUI();
    }

    public void ResetUI()
    {
        timer = 0;
        UpdateMouseCatchCountText();
        UpdateLevelText();
        UpdateTotalMouseCount();
        RangeItemText.gameObject.SetActive(false);
        SpeedItemText.gameObject.SetActive(false);
        BombItemText.gameObject.SetActive(false);
    }
}