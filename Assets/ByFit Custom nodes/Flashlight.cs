using System;
using UnityEngine;
using MaxyGames.UNode;

namespace MaxyGames.UNode.Nodes {
    [NodeMenu("ByFit node", "Flashlight", tooltip = "Простая логика фонарика. Включение-выключение на клавишу.", hasFlowInput = true, hasFlowOutput = true, inputs = new Type[] { typeof(Light), typeof(KeyCode) }, outputs = new Type[] { typeof(bool) })]
    public class Flashlight : IStaticNode {    
        [Input(typeof(Light), description = "Источник света.")]
        public static ValuePortDefinition Light;
        
        [Input(typeof(KeyCode), description = "Клавиша включения и выключения фонарика.")]
        public static ValuePortDefinition Key;
        
        [Output]
        public static FlowPortDefinition Exit;
        
        [Output(description = "Возвращает включённый фонарик.")]
        public static bool Turn_on(Light Light, KeyCode Key) {
            if (Input.GetKeyDown(Key)) {
                Light.enabled = !Light.enabled;
            }
            return Light.enabled;
        }

        [Input]
        public static void Enter(Light Light, KeyCode Key) {
            if (Input.GetKeyDown(Key)) {
                Light.enabled = !Light.enabled;
            }
        }
    }
}