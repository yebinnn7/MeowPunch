using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    public AudioClip[] audio_clips;

    AudioSource bgmPlayer;
    AudioSource sfxPlayer;

    // Start is called before the first frame update
    void Start()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // 씬 전환 시에도 삭제되지 않도록
        }
        else
        {
            Destroy(gameObject);
        }

        bgmPlayer = GameObject.Find("BGM Player").GetComponent<AudioSource>();
        sfxPlayer = GameObject.Find("Sfx Player").GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlaySound(string type)
    {
        int index = 0;

        switch (type)
        {
            case "attack": index = 0; break;
            case "bomb": index = 1; break;
            case "button2": index = 2; break;
            case "cat": index = 3; break;
            case "cat2": index = 4; break;
            case "dead": index = 5; break;
            case "item": index = 6; break;
            case "levelup": index = 7; break;
        }

        sfxPlayer.clip = audio_clips[index];
        sfxPlayer.Play();
    }
}
