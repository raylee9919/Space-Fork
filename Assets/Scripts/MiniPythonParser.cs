using System;
using System.Collections.Generic;

public class MiniPythonParser
{
    public bool HasSyntaxError(string code)
    {
        if (string.IsNullOrWhiteSpace(code))
            return true;

        code = code.Trim();

        // 괄호 개수 체크
        if (!BracketsAreBalanced(code))
            return true;

        // 기본 구문 형태 체크
        if (!(code.Contains("print") || code.Contains("if") || code.Contains("for") || code.Contains("decode")))
            return true;

        // 간단한 패턴 예외: 콜론이 필요한데 없는 경우
        if ((code.StartsWith("if") || code.StartsWith("for")) && !code.Contains(":"))
            return true;

        return false;
    }

    private bool BracketsAreBalanced(string code)
    {
        int open = 0;
        foreach (char c in code)
        {
            if (c == '(') open++;
            if (c == ')') open--;
            if (open < 0) return false;
        }
        return open == 0;
    }
}
