using UnityEngine;
using TMPro;
using Microsoft.MixedReality.Toolkit.Experimental.UI;

public class KeyboardNewlineHandler : MonoBehaviour
{
    public void InsertNewLine()
    {
        TMP_InputField inputField = NonNativeKeyboard.Instance.InputField;
        if (inputField != null)
        {
            // 현재 커서 위치 가져오기
            int caretIndex = inputField.caretPosition;

            // 텍스트 삽입: 커서 위치 기준 앞/뒤로 나누기
            string before = inputField.text.Substring(0, caretIndex);
            string after = inputField.text.Substring(caretIndex);
            inputField.text = before + "\n" + after;

            // 텍스트 재삽입 이후 커서 위치 다시 지정
            inputField.caretPosition = caretIndex + 1;
            inputField.selectionAnchorPosition = inputField.caretPosition;
            inputField.selectionFocusPosition = inputField.caretPosition;

            // ※ ActivateInputField()는 호출하지 않음 ← 커서 초기화 방지
        }
    }
}
