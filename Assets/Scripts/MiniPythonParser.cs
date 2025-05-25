using System;
using System.Collections.Generic;

public class MiniPythonParser
{
    public bool HasSyntaxError(string code)
    {
        if (string.IsNullOrWhiteSpace(code))
            return false;

        code = code.Trim();

        // ��ȣ ���� üũ
        if (!BracketsAreBalanced(code))
            return true;

        // �⺻ ���� ���� üũ
        if (!(code.Contains("print") || code.Contains("if") || code.Contains("for") || code.Contains("decode") || code.Contains("navigate"))) 
            return true;

        // ������ ���� ����: �ݷ��� �ʿ��ѵ� ���� ���
        if ((code.StartsWith("if") || code.StartsWith("for")) && !code.Contains(":"))
            return true;

        // ������ ���� ����: ��ȣ�� �ʿ��ѵ� ���� ���
        if ((code.StartsWith("print") || code.StartsWith("decode") || code.StartsWith("navigate")) && !code.Contains("("))
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
