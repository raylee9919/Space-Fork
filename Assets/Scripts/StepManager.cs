using UnityEngine;
using System.Collections;

public enum GameStage
{
    Intro,       
    PowerOn,    
    OxygenFix,   
    DockRelease, 
    FlyAway,     
    Escape,
    Finish
}

public class StepManager : MonoBehaviour
{
    public GameStage currentStage = GameStage.Intro;
    public AudioClip alienTalkSound;
    public AudioClip spaceShipSound;
    private AudioSource audioSource;
    public HapticTrigger hapticTrigger;
    private int[] stageTalkCount = { 3, 2, 2, 1, 3, 2 };

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        AdvanceStage();
    }
    public void AdvanceStage()
    {
        currentStage++;

        switch (currentStage)
        {
            case GameStage.PowerOn:
                DLM.Load("PowerOn");
                StartCoroutine(AlienTalk(0));
                break;

            case GameStage.OxygenFix:
                DLM.Load("OxygenFix");
                StartCoroutine(AlienTalk(1));
                break;

            case GameStage.DockRelease:
                DLM.Load("DockRelease");
                StartCoroutine(AlienTalk(2));
                break;

            case GameStage.FlyAway:
                DLM.Load("FlyAway");
                StartCoroutine(AlienTalk(3));
                break;

            case GameStage.Escape:
                DLM.Load("Escape");
                // 우주 비행하는 소리 및 진동
                StartCoroutine(FlyAndAlienTalk(4));
                break;

            case GameStage.Finish:
                DLM.Load("Finish");
                StartCoroutine(AlienTalk(5));
                break;

        }
    }

    IEnumerator AlienTalk(int i)
    {
        for (int j = 0; j < stageTalkCount[i]; j++)
        {
            audioSource.clip = alienTalkSound;
            audioSource.Play();
            DLM.Print();
            yield return new WaitForSeconds(2f);
            audioSource.Stop();
            yield return new WaitForSeconds(4f);
        }
    }

    IEnumerator FlyAndAlienTalk(int i)
    {
        audioSource.clip = spaceShipSound;
        audioSource.Play();
        hapticTrigger.TriggerHaptic();

        yield return new WaitForSeconds(6f);
        audioSource.Stop();
        yield return new WaitForSeconds(1f);

        for (int j = 0; j < stageTalkCount[i]; j++)
        {
            audioSource.clip = alienTalkSound;
            audioSource.Play();
            DLM.Print();
            yield return new WaitForSeconds(2f);
            audioSource.Stop();
            yield return new WaitForSeconds(4f);
        }
    }
}