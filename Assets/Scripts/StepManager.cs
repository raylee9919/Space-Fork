using UnityEngine;
using TMPro;

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

    void Start()
    {
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
                DLM.Print();
                break;

            case GameStage.OxygenFix:
                Debug.Log("StepManager.AdvanceStage(): " + currentStage);
                DLM.Load("OxygenFix");
                DLM.Print();
                break;

            case GameStage.DockRelease:
                Debug.Log("StepManager.AdvanceStage(): " + currentStage);
                DLM.Load("DockRelease");
                DLM.Print();
                break;

            case GameStage.FlyAway:
                Debug.Log("StepManager.AdvanceStage(): " + currentStage);
                DLM.Load("FlyAway");
                DLM.Print();
                break;

            case GameStage.Escape:
                Debug.Log("StepManager.AdvanceStage(): " + currentStage);
                DLM.Load("Escape");
                DLM.Print();
                break;
        }
    }

}
