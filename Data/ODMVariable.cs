using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using HutongGames.PlayMaker;

public class ODMVariable
{
    public static float ava_hurt_force
    {
        get
        { return FsmVariables.GlobalVariables.GetFsmFloat("ava_hurt_force").Value; }
        set
        { FsmVariables.GlobalVariables.GetFsmFloat("ava_hurt_force").Value = value; }
    }
    public static float ava_current_health
    {
        get
        { return FsmVariables.GlobalVariables.GetFsmFloat("ava_current_health").Value; }
        set
        { FsmVariables.GlobalVariables.GetFsmFloat("ava_current_health").Value = value; }
    }
    public static float ava_current_sp
    {
        get
        { return FsmVariables.GlobalVariables.GetFsmFloat("ava_current_sp").Value; }
        set
        { FsmVariables.GlobalVariables.GetFsmFloat("ava_current_sp").Value = value; }
    }
    public static float ava_move_speed
    {
        get
        { return FsmVariables.GlobalVariables.GetFsmFloat("moveSpeed").Value; }
        set
        { FsmVariables.GlobalVariables.GetFsmFloat("moveSpeed").Value = value; }
    }
    public static float ava_direction
    {
        get
        { return FsmVariables.GlobalVariables.GetFsmFloat("ava_direction").Value; }
        set
        { FsmVariables.GlobalVariables.GetFsmFloat("ava_direction").Value = value; }
    }
    public static float ava_current_poison
    {
        get
        { return FsmVariables.GlobalVariables.GetFsmFloat("ava_current_poison").Value; }
        set
        { FsmVariables.GlobalVariables.GetFsmFloat("ava_current_poison").Value = value; }
    }
    public static bool is_fighting
    {
        get
        { return FsmVariables.GlobalVariables.GetFsmBool("isFighting").Value; }
        set
        { FsmVariables.GlobalVariables.GetFsmBool("isFighting").Value = value; }
    }

    // Configuration
    public static bool create_new_save
    {
        get
        { return FsmVariables.GlobalVariables.GetFsmBool("create_new_save").Value; }
        set
        { FsmVariables.GlobalVariables.GetFsmBool("create_new_save").Value = value; }
    }
    public static int game_difficulty
    {
        get
        { return FsmVariables.GlobalVariables.GetFsmInt("game_difficulty").Value; }
        set
        { FsmVariables.GlobalVariables.GetFsmInt("game_difficulty").Value = value; }
    }
    public static string level_to_load
    {
        get
        {
            return PlayerPrefs.GetString("level_to_load");
        }
        set
        {
            PlayerPrefs.SetString("level_to_load", value);
        }
    }
    public static int ava_current_weapon
    {
        get
        { return FsmVariables.GlobalVariables.GetFsmInt("ava_current_weapon").Value; }
        set
        { FsmVariables.GlobalVariables.GetFsmInt("ava_current_weapon").Value = value; }
    }
    //World
    public static bool is_system_locked
    {
        get
        { return FsmVariables.GlobalVariables.GetFsmBool("isSystemLock").Value; }
        set
        { FsmVariables.GlobalVariables.GetFsmBool("isSystemLock").Value = value; }
    }
    public static bool is_menu_open
    {
        get
        { return FsmVariables.GlobalVariables.GetFsmBool("isMenuOpen").Value; }
        set
        { FsmVariables.GlobalVariables.GetFsmBool("isMenuOpen").Value = value; }
    }
    public static bool is_on_event
    {
        get
        { return FsmVariables.GlobalVariables.GetFsmBool("isOnEvent").Value; }
        set
        { FsmVariables.GlobalVariables.GetFsmBool("isOnEvent").Value = value; }
    }
    public static bool is_lock_event
    {
        get
        { return FsmVariables.GlobalVariables.GetFsmBool("LockEvent").Value; }
        set
        { FsmVariables.GlobalVariables.GetFsmBool("LockEvent").Value = value; }
    }

    //Player status
    public static bool status_armor
    {
        get
        { return FsmVariables.GlobalVariables.GetFsmBool("status_armor").Value; }
        set
        { FsmVariables.GlobalVariables.GetFsmBool("status_armor").Value = value; }
    }
    public static bool status_serum
    {
        get
        { return FsmVariables.GlobalVariables.GetFsmBool("status_serum").Value; }
        set
        { FsmVariables.GlobalVariables.GetFsmBool("status_serum").Value = value; }
    }
    public static bool status_cum
    {
        get
        { return FsmVariables.GlobalVariables.GetFsmBool("status_cum").Value; }
        set
        { FsmVariables.GlobalVariables.GetFsmBool("status_cum").Value = value; }
    }
    public static bool status_protected
    {
        get
        { return FsmVariables.GlobalVariables.GetFsmBool("status_protected").Value; }
        set
        { FsmVariables.GlobalVariables.GetFsmBool("status_protected").Value = value; }
    }
    public static bool status_horney
    {
        get
        { return FsmVariables.GlobalVariables.GetFsmBool("status_horney").Value; }
        set
        { FsmVariables.GlobalVariables.GetFsmBool("status_horney").Value = value; }
    }

    //Player resource
    public static int serum_count
    {
        get
        { return FsmVariables.GlobalVariables.GetFsmInt("serum_count").Value; }
        set
        { FsmVariables.GlobalVariables.GetFsmInt("serum_count").Value = value; }
    }
    public static int serum_max
    {
        get
        { return FsmVariables.GlobalVariables.GetFsmInt("serum_max").Value; }
        set
        { FsmVariables.GlobalVariables.GetFsmInt("serum_max").Value = value; }
    }
    public static int current_bullet
    {
        get
        { return FsmVariables.GlobalVariables.GetFsmInt("currentBullet").Value; }
        set
        { FsmVariables.GlobalVariables.GetFsmInt("currentBullet").Value = value; }
    }
    public static int bullet_max
    {
        get
        { return FsmVariables.GlobalVariables.GetFsmInt("bullet_max").Value; }
        set
        { FsmVariables.GlobalVariables.GetFsmInt("bullet_max").Value = value; }
    }
    public static int bullet_stock
    {
        get
        { return FsmVariables.GlobalVariables.GetFsmInt("bulletStock").Value; }
        set
        { FsmVariables.GlobalVariables.GetFsmInt("bulletStock").Value = value; }
    }
    public static int bullet_stock_max
    {
        get
        { return FsmVariables.GlobalVariables.GetFsmInt("bullet_stock_max").Value; }
        set
        { FsmVariables.GlobalVariables.GetFsmInt("bullet_stock_max").Value = value; }
    }

    //Scene
    public static string current_event_fsm_name
    {
        get
        { return FsmVariables.GlobalVariables.GetFsmString("currentEventFsmName").Value; }
        set
        { FsmVariables.GlobalVariables.GetFsmString("currentEventFsmName").Value = value; }
    }
    public static class world
    {
        public static float move_speed_max
        {
            get
            { return FsmVariables.GlobalVariables.GetFsmFloat("moveSpeed_max").Value; }
            set
            { FsmVariables.GlobalVariables.GetFsmFloat("moveSpeed_max").Value = value; }
        }

        public static string from_door
        {
            get
            { return FsmVariables.GlobalVariables.GetFsmString("fromDoor").Value; }
            set
            { FsmVariables.GlobalVariables.GetFsmString("fromDoor").Value = value; }
        }
    }
    public static class convert
    {
        public static string getAreaFlag(string _level_name)
        {
            return "Area " + _level_name;
        }

        public static string getWarmbugFlag(string _level_name)
        {
            return _level_name + " Warmbug";
        }

        public static string getWarmbugResetFlag(string _level_name)
        {
            return _level_name + " Warmbug Reset";
        }

        public static string getDownDoorFlag(string _level_name)
        {
            return _level_name + " Down Door";
        }

        public static string getRightDoorFlag(string _level_name)
        {
            return _level_name + " Right Door";
        }

        public static string getSceneIdentifier(string _level_name,string _identifier)
        {
            return _level_name + " " + _identifier;
        }

    }

    public static class fsm
    {
        //renew code
        public static PlayMakerFSM event_controller
        {
            get
            {
                return fsmHelper.getFsm(ODMObject.event_manager, "Event Action");
            }
        }
        public static PlayMakerFSM bgm_manager
        {
            get
            {
                return fsmHelper.getFsm(ODMObject.bgm_manager, ODMVariable.common.default_fsm);
            }
        }

        //Event Manager
        public static PlayMakerFSM scene_controller
        {
            get
            {
                return fsmHelper.getFsm(ODMObject.event_manager, "Scene Controller");
            }
        }
        public static PlayMakerFSM fade
        {
            get
            {
                return fsmHelper.getFsm(ODMObject.event_manager, "Fade");
            }
        }

        //Player
        public static PlayMakerFSM player_control
        {
            get
            {
                return fsmHelper.getFsm(ODMObject.character_ava, "Player Control");
            }
        }
        public static PlayMakerFSM player_control_disabled
        {
            get
            {
                return fsmHelper.getFsm(ODMObject.character_ava, "Player Control Disabled");
            }
        }
        public static PlayMakerFSM serum_controller
        {
            get
            {
                return fsmHelper.getFsm(ODMObject.character_ava, "Serum");
            }
        }
    }

    public static class path
    {
        //Base
        public static string application_datapath
        {
            get
            {
                return Application.dataPath;
            }

            set
            {
                application_datapath = value;
            }
        }

        //Directory
        public static string save_folder_directory = path.application_datapath + @"\Save";
        public static string character_image_directory = @"Character Image\";

        //Resources
        public static string item_database_resource = @"Data Collection\" + system.lang + @"\ItemDatabase";
        public static string map_database_resource = @"Data Collection\" + system.lang + @"\MapDatabase";
        public static string document_resource = @"Data Collection\" + system.lang + @"\Document";
        public static string intelligence_resource = @"Data Collection\" + system.lang + @"\Intelligence";
        public static string storyboard_file_resource = @"Data Collection\" + system.lang + @"\Storyboard";
        public static string credit_file_resource = @"Data Collection\" + system.lang + @"\Credit";
        public static string conversation_file_resource = @"Data Collection\" + system.lang + @"\Conversation";

        //Files
        public static string setting_file = path.application_datapath + @"\PPODM_SETTING.txt";
        public static string auto_save_file = save_folder_directory + @"\" + common.auto_save_file_name + ".json";
        public static string game_log_file = path.application_datapath + @"\odm_game_log.txt";
    }
    public static class resource
    {
        public static string item_image = @"Item/";
        public static string base_flag_collection = @"Data Collection\FLAG_COLLECTION";

        //Door Sprite
        public static string left_door_locked = "Background/Left Door Locked";
        public static string left_door_unlocked = "Background/Left Door Unlocked";

        public static string right_door_locked = "Background/Right Door Locked";
        public static string right_door_unlocked = "Background/Right Door Unlocked";

        public static string door_locked = "Background/Door Locked";
        public static string door_unlocked = "Background/Door Unlocked";


        public static string wall_special_file_name = @"Background\Wallspecial";
        public static string wall_file_name = @"Background\Wallbase";

        public static string ui_translation_text = @"Data Collection\UI Text";
    }

    public static class local
    {
        //Ava
        public static string ava_hurt_force = "ava_hurt_force";
        public static string is_hurt_start = "is_hurt_start";
        public static string bullets_needed = "bullets_needed";

        //Confirm panel
        public static string response = "response";

        //Common
        public static string flag_name = "flag_name";
        public static string fsm_name = "fsm_name";
        public static string self = "self";
        public static string msg_flag_name = "msg_flag_name";

        //guiHelper - showMessageText
        //XXX
        //public static string msg = "msg";


        //Scene controller
        public static string is_first_load = "is_first_load";

        //BGM Manager
        public static string is_default = "isDefault";

        //For bullet
        public static string bullet_speed = "bullet_speed";
        public static string bullet_virtical_speed = "bullet_virtical_speed";

        //Warmbug Health System
        public static string bullet_power = "bullet_power";

        //For special items
        public static string is_special_item = "is_special_item";

    }




    public static class common
    {

        public static string default_fsm = "FSM";
        public static string health_fsm = "Health System";

        public static string initilization_level_name = "initialization";

        public static string previous_state = "previous_state";
        public static string auto_save_file_name = "Auto";
        public static string default_save_code = "default";



    }

    public static class animation
    {
        public static string is_selected = "isSelected";
        public static string is_open = "isOpen";
        public static string is_selectable = "isSelectable";
        public static string is_current = "isCurrent";
        public static string is_explored = "isExplored";
        public static string is_activate = "isActivate";

        public static string is_conversation = "isConversation";
        public static string is_focus = "isFocus";

        //Box
        public static string is_crack = "isCrack";
        public static string is_open_final = "isOpenFinal";
        public static string is_crack_final = "isCrackFinal";



    }

    public static class text
    {
        public static string none = "-";
        public static string lang_en = "EN";
        public static string lang_jp = "JP";
        public static string lang_zh = "ZH";
        public static string no_data = "NO DATA";

        public static string map_up = "Map Up";
        public static string map_down = "Map Down";
        public static string map_right = "Map Right";
        public static string map_left = "Map Left";
    }

    public static class tag
    {
        public static string end = "[END]";
        public static string input = "[INPUT]";
        public static string pause = "[PAUSE]";
    }

    public static class level
    {
        public static float activate_range_x
        {
            get
            {
                return PlayerPrefs.GetFloat("activate_range_x");
            }
            set
            {
                PlayerPrefs.SetFloat("activate_range_x", value);
            }
        }
    }

    public static class system
    {
        public static string save_code
        {
            get
            {
                return PlayerPrefs.GetString("save_code");
            }
            set
            {
                PlayerPrefs.SetString("save_code", value);
            }
        }

        public static string lang
        {
            get
            {
                return PlayerPrefs.GetString("lang");
            }
            set
            {
                PlayerPrefs.SetString("lang", value);
            }
        }

        public static string service_base
        {
            get
            {
                return PlayerPrefs.GetString("service_base");
            }
            set
            {
                PlayerPrefs.SetString("service_base", value);
            }
        }
        public static string user_guid
        {
            get
            {
                return PlayerPrefs.GetString("user_guid");
            }
            set
            {
                PlayerPrefs.SetString("user_guid", value);
            }
        }
    }

    public static class save
    {
        public static string save_id = "save_id";
        public static string save_created_time = "save_created_time";

        public static string saved_scene = "saved_scene";
        public static string game_difficulty = "game_difficulty";
        public static string move_speed = "move_speed";

        public static string status_armor = "status_armor";
        public static string status_cum = "status_cum";
        public static string status_protected = "status_protected";
        public static string status_serum = "status_serum";

        public static string ava_current_health = "ava_current_health";
        public static string ava_current_sp = "ava_current_sp";
        public static string ava_current_poison = "ava_current_poison";

        public static string ava_current_position = "ava_current_position";
        public static string current_bullets = "current_bullets";
        public static string inventory_collection = "inventory_collection";
    }

    public static class translation
    {
        //Common Panel
        public static string cancel = "cancel";
        public static string confirm = "confirm";

        //Message Panel
        public static string recover_hp = "recover hp";
        public static string recover_toxic = "recover toxic";
        public static string trans_use_item_confirm = "confirm use item sure";
        public static string eat_warmbug_item = "eat item";
        public static string item_connot_use = "Item connot use";
        public static string use_serum = "use serum";

        //Map
        public static string map_info_currentlocation = "Map Info currentLocation";
        public static string map_info_explored = "Map Info explored";
        public static string map_info_unexplored = "Map Info unexplored";
        public static string map_info_explored_rate = "Map Info explored rate";

        //Difficulty

        public static string easy = "Easy";
        public static string normal = "Normal";
        public static string hard = "Hard";
        public static string hell = "Hell";

        //Save
        public static string auto_save = "auto save";
        public static string save_successed = "save successed";

        //Map Square
        public static string map_info_selected_location = "Map Info selectedLocation";
        public static string map_info_right_door = "Map Info rightDoor";
        public static string map_info_left_door = "Map Info leftDoor";
        public static string map_info_up_door = "Map Info upDoor";
        public static string map_info_down_door = "Map Info downDoor";
        public static string map_info_not_explored = "Map Info not explored";
        public static string accessible = "accessible";

        //Open Door Message
        public static string show_open_door = "showOpenDoor";

        public static string sure_drop_item = "sure drop item";
        public static string cannot_drop_item = "cannot drop item";
        public static string cannot_collect = "cannot collect";

        public static string press_to_use = "cannot collect";

    }

    public static class warmbug
    {
        public static string warmbug_hp = "warmbug_hp";
        public static string sp_bonus = "sp_bonus";
        public static string armor_creak = "armor_creak";
        public static string normal_damage = "normal_damage";
        public static string normal_sp_damage = "normal_sp_damage";
        public static string melee_damage = "melee_damage";
        public static string mate_critial_hit = "mate_critial_hit";
        public static string feed_critial_hit = "feed_critial_hit";
        public static string base_warmbug_mate_force = "base_warmbug_mate_force";
        public static string base_warmbug_feed_force = "base_warmbug_feed_force";
    }

    public static class ending
    {
        //The ending that the player is about to finish
        public static string ending_code
        {
            get
            {
                return PlayerPrefs.GetString("ending_code");
            }

            set
            {
                PlayerPrefs.SetString("ending_code", value);
            }
        }
        public static int ending_count
        {
            get
            {
                return PlayerPrefs.GetInt("ending_count");
            }

            set
            {
                PlayerPrefs.SetInt("ending_count", value);
            }
        }
        //Ending list
        public static string ending_code_maze = "ending code maze";
        public static string ending_code_inhuman = "ending code inhuman";
        public static string ending_code_nightmare = "ending code nightmare";
        public static string ending_code_darkness_love = "ending code darkness love";
        public static string ending_code_empire = "ending code empire";
        public static string ending_code_final_call = "ending code final call";
        public static string ending_code_children = "ending code children";
        public static string ending_victoria_talent = "ending victoria talent";
    }

    public static class endingText
    {
        public static string finished_ending
        {
            get
            {
                return PlayerPrefs.GetString("finished_ending_name");
            }

            set
            {
                PlayerPrefs.SetString("finished_ending_name", value);
            }
        }
        public static string gaming_time
        {
            get
            {
                return PlayerPrefs.GetString("gaming_time");
            }

            set
            {
                PlayerPrefs.SetString("gaming_time", value);
            }
        }
        public static string exploration_rate
        {
            get
            {
                return PlayerPrefs.GetString("exploration_rate");
            }

            set
            {
                PlayerPrefs.SetString("exploration_rate", value);
            }
        }
        public static string death_count
        {
            get
            {
                return PlayerPrefs.GetString("death_count");
            }

            set
            {
                PlayerPrefs.SetString("death_count", value);
            }
        }
        public static string best_death
        {
            get
            {
                return PlayerPrefs.GetString("best_death");
            }

            set
            {
                PlayerPrefs.SetString("best_death", value);
            }
        }
        public static string best_death_count
        {
            get
            {
                return PlayerPrefs.GetString("best_death_count");
            }

            set
            {
                PlayerPrefs.SetString("best_death_count", value);
            }
        }
        public static string mate_count
        {
            get
            {
                return PlayerPrefs.GetString("mate_count");
            }

            set
            {
                PlayerPrefs.SetString("mate_count", value);
            }
        }
        public static string best_mate
        {
            get
            {
                return PlayerPrefs.GetString("best_mate");
            }

            set
            {
                PlayerPrefs.SetString("best_mate", value);
            }
        }
        public static string best_mate_count
        {
            get
            {
                return PlayerPrefs.GetString("best_mate_count");
            }

            set
            {
                PlayerPrefs.SetString("best_mate_count", value);
            }
        }
        public static string climax_count
        {
            get
            {
                return PlayerPrefs.GetString("climax_count");
            }

            set
            {
                PlayerPrefs.SetString("climax_count", value);
            }
        }
        public static string best_climax
        {
            get
            {
                return PlayerPrefs.GetString("best_climax");
            }

            set
            {
                PlayerPrefs.SetString("best_climax", value);
            }
        }
        public static string best_climax_count
        {
            get
            {
                return PlayerPrefs.GetString("best_climax_count");
            }

            set
            {
                PlayerPrefs.SetString("best_climax_count", value);
            }
        }
        public static string feed_count
        {
            get
            {
                return PlayerPrefs.GetString("feed_count");
            }

            set
            {
                PlayerPrefs.SetString("feed_count", value);
            }
        }
        public static string best_feed
        {
            get
            {
                return PlayerPrefs.GetString("best_feed");
            }

            set
            {
                PlayerPrefs.SetString("best_feed", value);
            }
        }
        public static string best_feed_success
        {
            get
            {
                return PlayerPrefs.GetString("best_feed_success");
            }

            set
            {
                PlayerPrefs.SetString("best_feed_success", value);
            }
        }
        public static string best_feed_failure
        {
            get
            {
                return PlayerPrefs.GetString("best_feed_failure");
            }

            set
            {
                PlayerPrefs.SetString("best_feed_failure", value);
            }
        }
    }

    public static class color
    {
        public static Color32 green_accessable_door = new Color32(73, 219, 61, 255);
        public static Color32 red_blocked_door = new Color32(212, 8, 8, 255);
        public static Color32 resistant_bar = new Color32(126, 41, 187, 255);
        public static Color32 feed_bar = new Color32(219, 42, 82, 255);

        public static Color32 hp_perfect = new Color32(54, 255, 233, 255);
        public static Color32 hp_save = new Color32(69, 255, 86, 255);
        public static Color32 hp_warning = new Color32(255, 121, 49, 255);
        public static Color32 hp_dangeous = new Color32(255, 49, 49, 255);

        public static Color32 invisible = new Color32(0, 0, 0, 0);

        //Dash diffuse color
        public static Color32 white = new Color32(255, 255, 255, 255);
        public static Color32 gray_transparent = new Color32(45, 45, 45, 150);
        public static Color32 black = new Color32(0, 0, 0, 255);

        //Item red amount
        public static Color32 red_amount = new Color32(255, 54, 54, 255);
        public static Color32 green_amount = new Color32(39, 184, 46, 255);


    }

    public enum itemCatalogue
    {
        none = -1,

        talent_necklace = 0,
        rct_pistol = 1,
        little_bastard = 2,
        ellas_hair_ornaments = 3,
        countdown_handcuffs = 4,
        parasitism_launcher = 5,
        pheromone_of_warmbugs = 6,
        refined_pheromone_of_warmbugs = 7,
        beacon_of_the_shadows = 8,
        rct_pistol_enhanced = 9,
        enhanced_component_a = 10,
        enhanced_component_b = 11,
        enhanced_component = 12,

        red_core_technology_energy = 20,
        beacon_of_the_esf = 21,
        general_battery = 22,
        creature_serum = 23,
        empire_bullet = 24,

        refined_lactation = 30,
        nourished_larvae = 31,
        semen_of_mecb = 32,
        ambusher_semen = 33,
        silencer_semen = 34,

        device_battery = 40,
        metal_door_component = 41,
        advanced_integrated_cable = 42,
        bio_destruction_device = 43,
        component_courage = 44,
        component_hope = 45,
        component_love = 46,


        lockpick = 60,
        staff_id = 61,
        administrator_id = 62,
        toxic_id = 63,
        equipment_room_id = 64,
        hell_ranch_id = 65,
        deck_chip = 66,
        transportation_room_chip = 67,

        electric_authentication_A = 68,
        electric_authentication_B = 69,
        electric_authentication_C = 70,

        super_item = 99


    }
}
