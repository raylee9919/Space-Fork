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

    // 현재 게임 단계 (1 ~ 5)
    [Range(1, 5)]
    public int stage = 1;

    public void Evaluate()
    {
        string code = inputField.text.Trim();
        Debug.Log($"[CodeEvaluator] 입력된 코드: '{code}'");

        // ────────────────────────────────
        //  공통 1단계: 문법 검사
        // ────────────────────────────────
        if (parser.HasSyntaxError(code))
        {
            monitorText.text += "\nSyntax Error";
            Debug.Log($"문법검사 시 오류발생");

            return;
        }

        // ────────────────────────────────
        //  단계별 정답 조건 분기
        // ────────────────────────────────
        switch (stage)
        {
            //  1단계: print("Hello World!")
            case 1:
                if (!code.Contains("print") || !code.Contains("\"Hello World!\""))
                {
                    monitorText.text += "\n Missing print or string";

                    return;
                }

                monitorText.text += "\nHello World!";

                lightController.ActivateLights();
                break;

            //  2단계: decode('3%2&')
            case 2:
                if (!code.Contains("decode") || !code.Contains("'3%2&'"))
                {
                    monitorText.text = "Missing decode or argument";
                    return;
                }
                monitorText.text = "Decoded!";
                lightController.ActivateLights();
                break;

            //  3단계: 조건문 포함
            case 3:
                if (!code.Contains("if"))
                {
                    monitorText.text = "조건문(if)이 필요합니다";
                    return;
                }
                monitorText.text = "조건문 확인!";
                lightController.ActivateLights();
                break;

            //  4단계: 반복문 포함
            case 4:
                if (!(code.Contains("for") || code.Contains("while")))
                {
                    monitorText.text = "반복문(for/while)이 필요합니다";
                    return;
                }
                monitorText.text = "반복문 확인!";
                lightController.ActivateLights();
                break;

            //  5단계: 브루트포스 키워드 검사
            case 5:
                var bruteKeywords = new List<string> { "for", "range", "if" };
                foreach (var kw in bruteKeywords)
                {
                    if (!code.Contains(kw))
                    {
                        monitorText.text = $"Missing keyword: {kw}";
                        return;
                    }
                }
                monitorText.text = "브루트포스 통과!";
                lightController.ActivateLights();
                break;

            default:
                monitorText.text = "정의되지 않은 단계입니다";
                break;
        }

        // 입력창 초기화 + 포커스 복구 //////////
        inputField.text = "";
        inputField.Select();
        inputField.ActivateInputField();
    }
}
