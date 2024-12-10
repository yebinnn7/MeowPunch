using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManager : MonoBehaviour
{
    public void ClickStartBtn()
    {
        
        UnityEngine.SceneManagement.SceneManager.LoadScene("SampleScene");
        SoundManager.Instance.PlaySound("button2");
    }

   
}
