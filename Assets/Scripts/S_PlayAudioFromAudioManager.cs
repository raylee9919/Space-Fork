using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_PlayAudioFromAudioManager : MonoBehaviour
{
    public string target;

    public void Play()
    {
        S_AudioManager.instance.Play(target);
    }

    public void Play(string audioName)
    {
        S_AudioManager.instance.Play(audioName);
    }
}
