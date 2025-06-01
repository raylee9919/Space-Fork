using UnityEngine;
using System.Collections;

public enum GameStage
{
    Intro,       // 0�ܰ�
    PowerOn,     // 1�ܰ�
    OxygenFix,   // 2�ܰ�
    DockRelease, // 3�ܰ�
    FlyAway,     // 4+�̴ϰ���
    Escape       // 5�ܰ�
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
                Debug.Log("StepManager.AdvanceStage(): " + currentStage);
                DLM.Load("PowerOn");
                StartCoroutine(AlienTalk()); 
                DLM.Print();
                break;

            case GameStage.OxygenFix:
                Debug.Log("StepManager.AdvanceStage(): " + currentStage);
                DLM.Load("OxygenFix");
                StartCoroutine(AlienTalk());
                DLM.Print();
                break;

            case GameStage.DockRelease:
                Debug.Log("StepManager.AdvanceStage(): " + currentStage);
                DLM.Load("DockRelease");
                StartCoroutine(AlienTalk());
                DLM.Print();
                break;

            case GameStage.FlyAway:
                Debug.Log("StepManager.AdvanceStage(): " + currentStage);
                DLM.Load("FlyAway");
                StartCoroutine(AlienTalk());
                DLM.Print();
                break;

            case GameStage.Escape:
                Debug.Log("StepManager.AdvanceStage(): " + currentStage);
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
}
