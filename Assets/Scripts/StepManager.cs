using UnityEngine;
using System.Collections;

public enum GameStage
{
    Intro,       // 0단계
    PowerOn,     // 1단계
    OxygenFix,   // 2단계
    DockRelease, // 3단계
    FlyAway,     // 4+미니게임
    Escape       // 5단계
}

public class StepManager : MonoBehaviour
{
    // public AlienDialogueManager dialogueManager;
    // public GameObject consolePanel, oxygenPanel, boltPanel;
    public GameStage currentStage = GameStage.Intro;
    public AudioClip alienTalkSound;
    private AudioSource audioSource;


    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        AdvanceStage();
    }
    public void AdvanceStage()
    {
        currentStage++;

        // dialogueManager.SetDialogue(currentStage);

        switch (currentStage)
        {
            case GameStage.PowerOn:
                DLM.Load("PowerOn");
                DLM.Print();
                StartCoroutine(AlienTalk());
                break;

            case GameStage.OxygenFix:
                DLM.Load("OxygenFix");
                DLM.Print();
                StartCoroutine(AlienTalk());

                break;

            case GameStage.DockRelease:
                DLM.Load("DockRelease");
                StartCoroutine(AlienTalk());
                DLM.Print();
                break;

            case GameStage.FlyAway:
                DLM.Load("FlyAway");
                StartCoroutine(AlienTalk());
                DLM.Print();
                break;

            case GameStage.Escape:
                DLM.Load("Escape");
                StartCoroutine(AlienTalk());
                DLM.Print();
                break;
        }
    }

    IEnumerator AlienTalk()
    {
        audioSource.clip = alienTalkSound;
        audioSource.Play();
        yield return new WaitForSeconds(3f);
        audioSource.Stop();
    }
    IEnumerator Delay()
    {
        yield return new WaitForSeconds(5f);
    }
}
