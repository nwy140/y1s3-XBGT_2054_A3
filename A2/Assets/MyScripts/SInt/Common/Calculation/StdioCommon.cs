using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// My Own Custom Standard Library // named after stdio.h in the C language
public static class StdioCommon
{
    // Ref: https://answers.unity.com/questions/803672/capitalize-first-letter-in-textfield-only.html
    public static string StringUppercaseFirstLetter(string str)
    {
        if (string.IsNullOrEmpty(str) == false)
        {
            str = str.Substring(0, 1).ToUpper() + str.Substring(1);
        }
        return str;
    }
}
