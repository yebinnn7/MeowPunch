using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public enum Type { Range, Speed, Bomb }
    public Type type;
    public int value;

    private Vector3 startPosition; // 시작 위치 저장
    public float floatAmplitude = 0.2f; // 떠다니는 높이
    public float floatSpeed = 2f; // 떠다니는 속도

    // Start is called before the first frame update
    void Start()
    {
        startPosition = transform.position; // 시작 위치 저장
    }

    // Update is called once per frame
    void Update()
    {
        // 위아래로 떠다니는 효과
        float newY = startPosition.y + Mathf.Sin(Time.time * floatSpeed) * floatAmplitude;
        transform.position = new Vector3(startPosition.x, newY, startPosition.z);

        // 회전 효과
        transform.Rotate(Vector3.up * 100 * Time.deltaTime);
    }

    
}
