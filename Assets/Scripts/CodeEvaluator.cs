using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System.Linq;

public class CodeEvaluator : MonoBehaviour
{
    public TMP_InputField inputField;
    public TextMeshProUGUI monitorText;
    public LightController lightController;

    private MiniPythonParser parser = new MiniPythonParser();

    public AudioClip successSound;
    private AudioSource audioSource;
    public HapticTrigger hapticTrigger;

    public StepManager stepManager;

    private bool awaitingNextInput = false;

    void Start()
    {
        if (inputField != null)
        {
            inputField.onValueChanged.AddListener(HandleInputStarted);
        }

        audioSource = gameObject.AddComponent<AudioSource>();
    }

    private void HandleInputStarted(string _)
    {
        if (awaitingNextInput)
        {
            inputField.text = "";
            monitorText.text = "";
            awaitingNextInput = false;
        }
    }

    public void Evaluate()
    {
        if (awaitingNextInput)
        {
            inputField.text = "";
            monitorText.text = "";
            awaitingNextInput = false;
            return;
        }

        string code = inputField.text.Trim();

        if (string.IsNullOrWhiteSpace(code))
            return;

        if (parser.HasSyntaxError(code))
        {
            monitorText.text += "\nSyntax Error";
            awaitingNextInput = true;
            return;
        }

        GameStage stage = stepManager.currentStage;
        switch (stage)
        {
            case GameStage.PowerOn:
                if (!code.Contains("print") || !code.Contains("\"Hello World!\""))
                {
                    monitorText.text += "\nWrong answer";
                    break;
                }
                monitorText.text += "\nLogin Success: Hello World!";
                Success();
                lightController.ActivateLights();
                break;

            case GameStage.OxygenFix:
                if (!code.Contains("if oxygen") || !code.Contains("<") || !code.Contains("18") || !code.Contains("=") || !code.Contains("21"))
                {
                    monitorText.text += "\nWrong answer";
                    break;
                }
                monitorText.text += "\nCongratulations!! Oxygen system has been normalized.";
                Success();
                break;

            case GameStage.DockRelease:
                if ((!code.Contains("for") || !code.Contains("in range(5):") || !code.Contains("release_bolt()")) && (!code.Contains("while") || !(code.Contains("+= 1") || code.Contains("-= 1")) || !code.Contains("release_bolt()")))
                {
                    monitorText.text += "\nWrong answer";
                    break;
                }
                monitorText.text += "\nThe docking module is disconnected. You can go anywhere you want.";
                Success();
                break;

            case GameStage.FlyAway:
                if (!code.Contains("navigate") || !(code.Contains("Earth") || code.Contains("Mars") || code.Contains("Jupiter")))
                {
                    monitorText.text += "\nWrong answer";
                    break;
                }
                if (code.Contains("Earth"))
                {
                    monitorText.text += "\nDestination set: Earth";
                }
                else if (code.Contains("Mars"))
                {
                    monitorText.text += "\nDestination set: Mars";
                }
                else if (code.Contains("Jupiter"))
                {
                    monitorText.text += "\nDestination set: Jupiter";
                }
                else
                {
                    monitorText.text += "\nDestination set: ?";
                }
                Success();
                break;

            case GameStage.Escape:
                bool ans = true;
                var bruteKeywords = new List<string> { "for", "in range", "(10000)", "unlock(" };
                foreach (var kw in bruteKeywords)
                {
                    if (!code.Contains(kw))
                    {
                        ans = false;                       
                    }
                }

                if (ans)
                {
                    monitorText.text += "\nThe door opens. Congratulations on your escape!!";
                    Success();
                    break;
                }
                else
                {
                    monitorText.text += "\nWrong answer";
                    break;
                }

            default:
                monitorText.text += "Error";
                break;
        }

        awaitingNextInput = true;
    }

    void Success()
    {
        DLM.Print(); // EOD 호출
        hapticTrigger.TriggerHaptic();
        audioSource.clip = successSound;
        audioSource.Play();
        StartCoroutine(DelayNextStage());
    }

    private System.Collections.IEnumerator DelayNextStage()
    {
        yield return new WaitForSeconds(4f);
        stepManager.AdvanceStage();
    }
    
}