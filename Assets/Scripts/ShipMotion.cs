using UnityEngine;

public class ShipMotion : MonoBehaviour
{
    public float moveAmplitude = 0.5f;     // 如甸覆 气
    public float moveFrequency = 0.2f;     // 框流捞绰 加档

    private Vector3 initialPosition;

    void Start()
    {
        initialPosition = transform.position;
    }

    void Update()
    {
        float offset = Mathf.Sin(Time.time * moveFrequency) * moveAmplitude;
        transform.position = initialPosition + transform.forward * offset;
    }
}
