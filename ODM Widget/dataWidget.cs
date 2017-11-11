using HutongGames.PlayMaker;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Script.ODM_Widget
{
    public class dataWidget
    {
        public static string getTranslaton(string key)
        {
            return FsmVariables.GlobalVariables.GetFsmGameObject("Language Translator").Value.GetComponent<UiInfomationHolder>().getText(key);
        }
        public static string getTranslaton(string key, string _subjectName)
        {
            return FsmVariables.GlobalVariables.GetFsmGameObject("Language Translator").Value.
                GetComponent<UiInfomationHolder>().getText(key).Replace("[INPUT]", _subjectName);
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
            string difficulty_text = "";
            switch (_difficulty)
            {
                case 1:
                    difficulty_text = "Easy";
                    break;
                case 2:
                    difficulty_text = "Normal";
                    break;
                case 3:
                    difficulty_text = "Hard";
                    break;
                case 4:
                    difficulty_text = "Hell";
                    break;
                default:
                    ODM.errorLog("Static", "No difficulty data!", "");
                    break;
            }
            return difficulty_text;
        }
    }
}
