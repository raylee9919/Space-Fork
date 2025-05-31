using UnityEngine;
using LitJson;
using TMPro;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit.UI;

public class DLM : MonoBehaviour
{
    static DLM instance;

    [SerializeField] public TextMeshProUGUI textDisplay;
    [SerializeField] public GameObject[] buttons;

    private JsonData dialogue;
    private int index;
    private string speaker;
    private JsonData currentLayer;
    private bool inDialogue;

    static public bool
    Load(string path)
    {
        bool result = false;
        
        if (!instance.inDialogue)
        {
            instance.index = 0;
            var jsonTextFile = Resources.Load<TextAsset>("Dialogues/JSON/" + path);
            instance.dialogue = JsonMapper.ToObject(jsonTextFile.text);
            instance.currentLayer = instance.dialogue;
            instance.inDialogue = true;
            result = true;
        }
        return result;
    }

    static public bool
    Print()
    {
        bool result;
        if (instance.inDialogue)
        {
            JsonData line = instance.currentLayer[instance.index];

            foreach (JsonData key in line.Keys)
                instance.speaker = key.ToString();

            if (instance.speaker == "EOD")
            {
                instance.inDialogue = false;
                instance.textDisplay.text = "";
                result = false;
            }
            else if (instance.speaker == "?")
            {
                JsonData options = line[0];
                for (int optionsNumber = 0; optionsNumber < options.Count; ++optionsNumber)
                {
                    ActivateButton(instance.buttons[optionsNumber], options[optionsNumber]);
                }
                result = true;
            }
            else
            {
                instance.textDisplay.text = instance.speaker + ": " + line[0].ToString();
                ++instance.index;
                result = true;
            }
        }
        else
        {
            result = false;
        }

        return result;
    }

    static public void
    Attach(TextMeshProUGUI textMesh)
    {
        instance.textDisplay = textMesh;
    }

    private static void
    ActivateButton(GameObject button, JsonData choice)
    {
        button.SetActive(true);
        button.GetComponentInChildren<TextMeshProUGUI>().text = choice[0][0].ToString();
        button.GetComponent<Button>().onClick.AddListener(delegate { OnClick(choice); });
    }

    private static void
    DeactivateButtons()
    {
        foreach (GameObject button in instance.buttons)
        {
            button.SetActive(false);
            button.GetComponent<Button>().onClick.RemoveAllListeners();
        }
    }

    private static void
    OnClick(JsonData choice)
    {
        instance.currentLayer = choice[0];
        instance.index = 1;
        Print();
        DeactivateButtons();
    }

    private void Awake()
    {
        instance = this;
        instance.textDisplay.text = "";
        DeactivateButtons();
    }
}