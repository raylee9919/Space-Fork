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

    public int stage = 1;

    private bool awaitingNextInput = false;

    void Start()
    {
        if (inputField != null)
        {
            inputField.onValueChanged.AddListener(HandleInputStarted);
        }

        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = successSound;
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

        switch (stage)
        {
            case 1:
                if (!code.Contains("print") || !code.Contains("\"Hello World!\""))
                {
                    monitorText.text += "\nWrong answer";
                    break;
                }
                monitorText.text += "\nLogin Success: Hello World!";
                Success();
                lightController.ActivateLights();
                break;

            case 2:
                if (!code.Contains("decode") || !code.Contains("3%2&"))
                {
                    monitorText.text += "\nWrong answer";
                    break;
                }
                monitorText.text += "\nAlien: Oxygen levels are decreasing. It is dangerous if oxygen is below 18%. 21% is safe.";
                Success();
                break;

            case 3:
                if (!code.Contains("if oxygen") || !code.Contains("<") || !code.Contains("18") || !code.Contains("=") || !code.Contains("21"))
                {
                    monitorText.text += "\nWrong answer";
                    break;
                }
                monitorText.text += "\nCongratulations!! Oxygen system has been normalized.";
                Success();
                break;

            case 4:
                if (!code.Contains("decode") || !code.Contains("*7^5"))
                {
                    monitorText.text += "\nWrong answer";
                    break;
                }
                monitorText.text += "\nAlien: The five bolts of the docking module must be released.";
                Success();
                break;

            case 5:
                if ((!code.Contains("for") || !code.Contains("in range(5):") || !code.Contains("release_bolt()")) && (!code.Contains("while") || !(code.Contains("+= 1") || code.Contains("-= 1")) || !code.Contains("release_bolt()")))
                {
                    monitorText.text += "\nWrong answer";
                    break;
                }
                monitorText.text += "\nAll bolts released complete. The docking module is disconnected. You can go anywhere you want.";
                Success();
                break;

            case 6:
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

            case 7:
                var bruteKeywords = new List<string> { "for", "in range(9999)", "if", "== password", "print(" };
                foreach (var kw in bruteKeywords)
                {
                    if (!code.Contains(kw))
                    {
                        monitorText.text += "\nWrong answer";
                        break;
                    }
                }
                monitorText.text += "Password is 1004. Congratulations on your escape!!";
                Success();
                break;

            default:
                monitorText.text += "Error";
                break;
        }

        awaitingNextInput = true;
    }

    void Success()
    {
        stage++;
        hapticTrigger.TriggerHaptic();
        audioSource.Play();
        // Sound Effect by <a href="https://pixabay.com/users/freesound_community-46691455/?utm_source=link-attribution&utm_medium=referral&utm_campaign=music&utm_content=6185">freesound_community</a> from <a href="https://pixabay.com/sound-effects//?utm_source=link-attribution&utm_medium=referral&utm_campaign=music&utm_content=6185">Pixabay</a>
    }
}