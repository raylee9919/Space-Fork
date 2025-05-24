using UnityEngine;
using TMPro;

public class InputMirror : MonoBehaviour
{
    [Header("입력창 (TMP_InputField)")]
    public TMP_InputField inputField;

    [Header("출력창 (TextMeshProUGUI)")]
    public TextMeshProUGUI monitorText;

    void Start()
    {
        if (inputField != null && monitorText != null)
        {
            // 입력 내용이 바뀔 때마다 monitorText로 동기화
            inputField.onValueChanged.AddListener(OnInputChanged);
        }
    }

    void OnDestroy()
    {
        if (inputField != null)
            inputField.onValueChanged.RemoveListener(OnInputChanged);
    }

    private void OnInputChanged(string value)
    {
        monitorText.text = value;
    }
}
