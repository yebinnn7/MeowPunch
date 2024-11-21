using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public enum Type { Range, Speed, Bomb }
    public Type type;
    public int value;

    private Vector3 startPosition; // ���� ��ġ ����
    public float floatAmplitude = 0.2f; // ���ٴϴ� ����
    public float floatSpeed = 2f; // ���ٴϴ� �ӵ�

    // Start is called before the first frame update
    void Start()
    {
        startPosition = transform.position; // ���� ��ġ ����
    }

    // Update is called once per frame
    void Update()
    {
        // ���Ʒ��� ���ٴϴ� ȿ��
        float newY = startPosition.y + Mathf.Sin(Time.time * floatSpeed) * floatAmplitude;
        transform.position = new Vector3(startPosition.x, newY, startPosition.z);

        // ȸ�� ȿ��
        transform.Rotate(Vector3.up * 100 * Time.deltaTime);
    }
}
