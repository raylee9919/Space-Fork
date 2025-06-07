using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;


public class OpenDoor : MonoBehaviour
{
    public Animator animator;
    public AudioClip doorSound;
    private AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
    public void TriggerOpen() 
    {
        StartCoroutine(Open());
    }

    IEnumerator Open()
    {
        yield return new WaitForSeconds(3f);
        audioSource.clip = doorSound;
        audioSource.Play();
        yield return new WaitForSeconds(1f);
        animator.SetBool("Open", true);
    }
}