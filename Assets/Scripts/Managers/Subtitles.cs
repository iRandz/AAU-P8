using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Subtitles : MonoBehaviour
{
    public Text textBox;
    private const string KeyWordWaitStart = "wait(";
    private const string EndWaitSymbol = ")";
    private readonly string[] _lineSeparator = {";\n", ";"};
    
    //Array holding the strings after they have been split
    private string[] _splitOutput;
    
    public IEnumerator ConvertAndDisplaySubtitles(string subtitles)
    {
        _splitOutput = subtitles.Split(_lineSeparator, StringSplitOptions.None);
        foreach (string s in _splitOutput)
        {
            string waitTime = GetBetween(s, KeyWordWaitStart, EndWaitSymbol);
            if (waitTime != null)
            {
                float floatWaitTime = float.Parse(waitTime);
                yield return new WaitForSeconds(floatWaitTime);
            }
            if(waitTime != null && s.Contains(waitTime)) continue;
            textBox.text = s;
        }
        textBox.text = "";
    }
    
    //Method from the internet, this is basically used to find the wait time between the symbols
    private static string GetBetween(string strSource, string strStart, string strEnd)
    {
        if (strSource.Contains(strStart) && strSource.Contains(strEnd))
        {
            int Start, End;
            Start = strSource.IndexOf(strStart, 0) + strStart.Length;
            End = strSource.IndexOf(strEnd, Start);
            return strSource.Substring(Start, End - Start);
        }

        return null;
    }
}
