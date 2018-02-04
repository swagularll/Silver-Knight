using HutongGames.PlayMaker;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public static class ODMObject
{
    //Using get/set can avoid initilization problem

    //Static
    public static GameObject character_ava
    {
        get
        {
            return FsmVariables.GlobalVariables.GetFsmGameObject("Ava").Value;
        }

        set
        {
            FsmVariables.GlobalVariables.GetFsmGameObject("Ava").Value = value;
        }
    }
    public static GameObject event_manager
    {
        get
        {
            return FsmVariables.GlobalVariables.GetFsmGameObject("event manager").Value;
        }

        set
        {
            FsmVariables.GlobalVariables.GetFsmGameObject("event manager").Value = value;
        }
    }
    public static GameObject obj_matebar_canvas
    {
        get
        {
            return FsmVariables.GlobalVariables.GetFsmGameObject("obj_Matebar_Canvas").Value;
        }

        set
        {
            FsmVariables.GlobalVariables.GetFsmGameObject("obj_Matebar_Canvas").Value = value;
        }
    }
    public static GameObject language_translator
    {
        get
        {
            return FsmVariables.GlobalVariables.GetFsmGameObject("Language Translator").Value;
        }

        set
        {
            FsmVariables.GlobalVariables.GetFsmGameObject("Language Translator").Value = value;
        }
    }
    public static GameObject save_builder
    {
        get
        {
            return FsmVariables.GlobalVariables.GetFsmGameObject("saveBuilder").Value;
        }

        set
        {
            FsmVariables.GlobalVariables.GetFsmGameObject("saveBuilder").Value = value;
        }
    }
    public static GameObject conversation_panel
    {
        get
        {
            return FsmVariables.GlobalVariables.GetFsmGameObject("conversationPanel").Value;
        }

        set
        {
            FsmVariables.GlobalVariables.GetFsmGameObject("conversationPanel").Value = value;
        }
    }
    public static GameObject bgm_manager
    {
        get
        {
            return FsmVariables.GlobalVariables.GetFsmGameObject("bgm manager").Value;
        }

        set
        {
            FsmVariables.GlobalVariables.GetFsmGameObject("bgm manager").Value = value;
        }
    }

    //Panel
    public static GameObject confirmation_panel
    {
        get
        {
            return FsmVariables.GlobalVariables.GetFsmGameObject("obj_ConfirmPanel").Value;
        }

        set
        {
            FsmVariables.GlobalVariables.GetFsmGameObject("obj_ConfirmPanel").Value = value;
        }
    }
    public static GameObject message_display_panel
    {
        get
        {
            return FsmVariables.GlobalVariables.GetFsmGameObject("messageDisplayPanel").Value;
        }

        set
        {
            FsmVariables.GlobalVariables.GetFsmGameObject("messageDisplayPanel").Value = value;
        }
    }
    public static GameObject player_condition_panel
    {
        get
        {
            return FsmVariables.GlobalVariables.GetFsmGameObject("obj_player_status_panel").Value;
        }

        set
        {
            FsmVariables.GlobalVariables.GetFsmGameObject("obj_player_status_panel").Value = value;
        }
    }

    //Variables
    public static GameObject current_mate
    {
        get
        {
            return FsmVariables.GlobalVariables.GetFsmGameObject("currentMate").Value;
        }

        set
        {
            FsmVariables.GlobalVariables.GetFsmGameObject("currentMate").Value = value;
        }
    }
    public static GameObject current_activate_door
    {
        get
        {
            return FsmVariables.GlobalVariables.GetFsmGameObject("currentActivateDoor").Value;
        }

        set
        {
            FsmVariables.GlobalVariables.GetFsmGameObject("currentActivateDoor").Value = value;
        }
    }
    public static GameObject current_activate_box
    {
        get
        {
            return FsmVariables.GlobalVariables.GetFsmGameObject("currentActivateBox").Value;
        }

        set
        {
            FsmVariables.GlobalVariables.GetFsmGameObject("currentActivateBox").Value = value;
        }
    }
    public static GameObject current_event_object
    {
        get
        {
            return FsmVariables.GlobalVariables.GetFsmGameObject("currentEventObject").Value;
        }

        set
        {
            FsmVariables.GlobalVariables.GetFsmGameObject("currentEventObject").Value = value;
        }
    }

    public static GameObject current_level_lair
    {
        get
        {
            return FsmVariables.GlobalVariables.GetFsmGameObject("currentLevelLair").Value;
        }

        set
        {
            FsmVariables.GlobalVariables.GetFsmGameObject("currentLevelLair").Value = value;
        }
    }
}
