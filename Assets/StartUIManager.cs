using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartUIManager : MonoBehaviour
{

    public Image ExplainPanel;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ClickExplainBtn()
    {
        SoundManager.Instance.PlaySound("button2");
        ExplainPanel.gameObject.SetActive(true);

    }

    public void ClickExplainExitBtn()
    {
        SoundManager.Instance.PlaySound("button2");
        ExplainPanel.gameObject.SetActive(false);
    }
}
