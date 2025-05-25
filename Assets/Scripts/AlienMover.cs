using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(Rigidbody))]
public class AlienMover : MonoBehaviour
{
    public float turnSpeed = 2.0f;
    public float moveSpeed = 1.5f;
    public float followDistance = 1.5f;

    private bool star = true;
    public AudioClip hitSound1;
    public AudioClip hitSound2;
    private AudioSource audioSource;
    public HapticTrigger hapticTrigger;

    private Rigidbody rb;
    private Transform playerTransform;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        // 플레이어 위치 추적
        GameObject player = GameObject.FindWithTag("MainCamera");
        if (player != null)
            playerTransform = player.transform;
        else
            playerTransform = Camera.main?.transform;
    }

    void Update()
    {
        // 회전 - 플레이어 바라보기
        if (playerTransform != null)
        {
            Vector3 lookTarget = playerTransform.position;
            Vector3 direction = (lookTarget - transform.position).normalized;
            direction.y = 0;

            if (direction != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);
            }
        }
    }

    void FixedUpdate()
    {
        if (playerTransform == null) return;

        Vector3 direction = (playerTransform.position - rb.position);
        direction.y = 0;

        if (direction.magnitude > followDistance)
        {
            Vector3 moveDir = direction.normalized * moveSpeed * Time.fixedDeltaTime;
            Vector3 moveTarget = rb.position + moveDir;
            moveTarget.y = rb.position.y; // Y축은 건드리지 않음

            rb.MovePosition(moveTarget);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "Left Controller" || collision.gameObject.name == "Right Controller")
        {
            if (!audioSource.isPlaying)
            {
                hapticTrigger.TriggerHaptic();
                StartCoroutine(PlayClipForSeconds(2f));
                Debug.Log($"[Alien] 충돌 감지: {collision.gameObject.name} → 사운드 재생");
            }
        }
    }

    IEnumerator PlayClipForSeconds(float duration)
    {
        if (star)
        {
            audioSource.clip = hitSound1;
            star = false;
        }
        else
        {
            audioSource.clip = hitSound2;
            star = true;
        }
        audioSource.Play();
        yield return new WaitForSeconds(duration);
        audioSource.Stop();
    }
}
