using HutongGames.PlayMaker;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Script.ODM_Widget
{
    public static class ODMObject
    {
        public static GameObject character_ava = FsmVariables.GlobalVariables.GetFsmGameObject(ODMVariable.obj_ava).Value;
        public static GameObject event_manager = FsmVariables.GlobalVariables.GetFsmGameObject(ODMVariable.obj_event_manager).Value;


    }
}
