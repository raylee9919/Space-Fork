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

    [Range(1, 5)]
    public int stage = 1;

    private bool awaitingNextInput = false;

    void Start()
    {
        if (inputField != null)
        {
            inputField.onValueChanged.AddListener(HandleInputStarted);
        }

        audioSource = gameObject.AddComponent<AudioSource>(); //
        audioSource.clip = successSound; //
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
                monitorText.text += "\nHello World!";
                Success();
                lightController.ActivateLights();
                break;

            case 2:
                if (!code.Contains("decode") || !code.Contains("'3%2&'"))
                {
                    monitorText.text += "\nWrong answer";
                    break;
                }
                monitorText.text += "\nDecoded!";
                Success();
                break;

            case 3:
                if (!code.Contains("if"))
                {
                    monitorText.text += "\nWrong answer";
                    break;
                }
                monitorText.text += "\nlearned if!";
                Success();
                break;

            case 4:
                if (!(code.Contains("for") || code.Contains("while")))
                {
                    monitorText.text += "\nWrong answer";
                    break;
                }
                monitorText.text += "\nlearned for/while";
                Success();
                break;

            case 5:
                var bruteKeywords = new List<string> { "for", "range", "if" };
                foreach (var kw in bruteKeywords)
                {
                    if (!code.Contains(kw))
                    {
                        monitorText.text += "\nWrong answer";
                        break;
                    }
                }
                monitorText.text += "Password is 1004";
                Success();
                break;

            default:
                monitorText.text += "What is this?";
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