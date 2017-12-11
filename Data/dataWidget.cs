using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class dataWidget
{
    public static string getTranslaton(string key)
    {
        return ODMObject.language_translator.GetComponent<UiInfomationHolder>().getText(key);
    }
    public static string getTranslaton(string key, string _subjectName)
    {
        return ODMObject.language_translator.GetComponent<UiInfomationHolder>().getText(key).Replace("[INPUT]", _subjectName);
    }
    public static string[] getMapName()
    {
        int idx = 0;
        string[] mapNames = new string[60];
        string[] s = { "A", "B", "C", "D", "E", "F" };
        for (int i = 0; i < 10; i++)
        {
            for (int k = 0; k < 6; k++)
            {
                mapNames[idx] = s[k] + i;
                idx++;
            }
        }
        return mapNames;
    }
    public static string getDifficultyText(int _difficulty)
    {
        string difficulty_translation_key = "";
        switch (_difficulty)
        {
            case 1:
                difficulty_translation_key = ODMVariable.translation.easy;
                break;
            case 2:
                difficulty_translation_key = ODMVariable.translation.normal;
                break;
            case 3:
                difficulty_translation_key = ODMVariable.translation.hard;
                break;
            case 4:
                difficulty_translation_key = ODMVariable.translation.hell;
                break;
            default:
                ODM.errorLog("Stript - dataWidget", "Cannot find difficulty translation! Missing difficulty int.");
                break;
        }
        return difficulty_translation_key;
    }
}
