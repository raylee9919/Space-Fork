using UnityEngine;

public class PlayerCollisionLogger : MonoBehaviour
{
    void OnCollisionEnter(Collision collision)
    {
        Debug.Log($"[Player] 충돌 감지: {collision.gameObject.name}");
    }

    void OnCollisionStay(Collision collision)
    {
        Debug.Log($"[Player] 충돌 중... 대상: {collision.gameObject.name}");
    }
}
