using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class AlienMover : MonoBehaviour
{
    public float moveSpeed = 1.0f;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        // 전방 방향으로만 이동
        Vector3 move = transform.forward * moveSpeed * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + move);
    }

    //디버그용
    
    void OnCollisionEnter(Collision collision)
    {
        Debug.Log($"[Alien] 충돌 감지: {collision.gameObject.name}");
    }
    void OnCollisionStay(Collision collision)
    {
        Debug.Log($"[Alien] 충돌 중... 대상: {collision.gameObject.name}");
    }

    void OnCollisionExit(Collision collision)
    {
        Debug.Log($"[Alien] 충돌 종료: {collision.gameObject.name}");
    }
    void OnTriggerEnter(Collider other)
    {
        Debug.Log($"[Alien] 트리거 진입: {other.gameObject.name}");
    }

}
