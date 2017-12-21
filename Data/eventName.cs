using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class eventName
{
    public static string default_fsm = "FSM";

    //Common
    public static string next = "next";
    public static string back = "back";

    
    //System
    public static string script_ready = "script ready";
    public static string warmbug_ready = "WB Ready";

    //Player control
    public static string ava_jump_Idle = "jump_Idle";

    //Event status
    public static string ava_get_hit = "ava get hit";
    public static string ava_pick_item = "ava pick item";
    public static string ava_down_check = "ava down check";
    public static string ava_event_hurt = "ava event hurt";
    public static string ava_event_faint = "ava event faint";
    public static string ava_event_awake = "ava event awake";
    public static string ava_event_dead = "ava event dead";
    public static string ava_wait = "ava wait";
    public static string ava_event_aim = "ava event aim";
    public static string ava_event_walk = "ava event walk";

    //Player status control
    public static string resume_fsm = "resume FSM";
    public static string hold_fsm = "hold FSM";

    //Scene control
    public static string start_hold = "start hold";
    public static string end_hold = "end hold";
    public static string start_menu_hold = "start menu hold";
    public static string end_menu_hold = "end menu hold";
    public static string end_system_hold = "end system hold";//no start system hold required, variable only


    //Dialog / Inventory Dash
    public static string start_function = "start function";
    public static string end_function = "end function";

    //UI
    public static string set_selectable = "set selectable";
    public static string set_unselectable = "set unselectable";
    
    //Data
    public static string show_data = "show data";
    public static string no_data = "no data";

    //Common event
    public static string show_menu = "show menu";
    public static string hide_menu = "hide menu";

    //Decision
    //public static string selection_true = "selection true";
    //public static string selection_false = "selection true";
    public static string proceed_event = "proceed event";


    //Message display panel
    public static string show_message = "show message";
    public static string hide_message = "hide message";

    //Inventory
    public static string use_inventory_item = "use inventory item";
    public static string activate_serum = "activate serum";

    //Flag check
    public static string true_result = "true result";
    public static string false_result = "false result";
    public static string precondition_not_done = "precondition not done";
    public static string event_retain = "event retain";

    //Story Borad
    public static string load_level = "load level";

    //Fade
    public static string fade_in = "fade in";

    //BGM
    public static string set_default_bgm = "set default bgm";



    //BS
    //public static string no_key = "no key";
    //public static string has_key = "has key";
    //public static string no_internet = "no internet";
    //public static string has_internet = "has internet";
    //public static string mac_success = "mac success";
    //public static string mac_failed = "mac failed";
    public static class system
    {
        public static string hide_gui = "hideGUI";
        public static string show_gui = "showGUI";

        public static string self_register = "selfRegister";

        
    }

}
