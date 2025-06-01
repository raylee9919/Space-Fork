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
    private AudioSource audioSource;
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
                StartCoroutine(AlienTalk(4));
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
            yield return new WaitForSeconds(3f);
            audioSource.Stop();
            yield return new WaitForSeconds(3f);
        }
    }

}