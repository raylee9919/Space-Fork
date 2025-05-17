using UnityEngine;
using System.Collections; // sound cut

[RequireComponent(typeof(AudioSource))]
public class AlienMover : MonoBehaviour
{
    public float floatAmplitude = 0.05f;
    public float floatFrequency = 1.5f;
    public float turnSpeed = 2.0f;
    public AudioClip hitSound; // sound

    private float baseY;
    private AudioSource audioSource; //sound

    void Start()
    {
        baseY = transform.position.y;
        audioSource = GetComponent<AudioSource>(); // sound
    }

    void Update()
    {
        // Y축 진동 (부유감)
        float floatOffset = Mathf.Sin(Time.time * floatFrequency) * floatAmplitude;
        Vector3 pos = transform.position;
        pos.y = baseY + floatOffset;
        transform.position = pos;

        // 플레이어(카메라)를 바라보게 회전
        if (Camera.main != null)
        {
            Vector3 lookTarget = Camera.main.transform.position;
            Vector3 direction = (lookTarget - transform.position).normalized;
            direction.y = 0; // 수평 회전만

            if (direction != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);
            }
        }
    }

    void OnCollisionEnter(Collision collision) // sound
    {
        if (hitSound != null && !audioSource.isPlaying)
        {
            StartCoroutine(PlayClipForSeconds(2f)); // 앞 2초만 재생
            Debug.Log($"[Alien] 충돌 감지: {collision.gameObject.name} → 사운드 재생");
        }
    }

    IEnumerator PlayClipForSeconds(float duration) // sound cut
    {
        audioSource.clip = hitSound;
        audioSource.Play();
        Debug.Log($"[Alien] 오디오 재생됨? isPlaying: {audioSource.isPlaying}");
        yield return new WaitForSeconds(duration);
        audioSource.Stop();
    }


}
