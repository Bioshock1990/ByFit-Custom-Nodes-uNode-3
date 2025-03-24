using System;
using UnityEngine;
using MaxyGames.UNode;

namespace MaxyGames.UNode.Nodes {
    [NodeMenu("Custom Nodes", "Standard node", tooltip = "Basic node demonstrating the default values of each type.", hasFlowInput = true, hasFlowOutput = true, inputs = new Type[] { typeof(float), typeof(int), typeof(Color), typeof(Vector2), typeof(Vector3), typeof(string), typeof(Quaternion) })]
    public class Standard_node : Node {
        [NonSerialized]
        public static ValuePort Float;
        [NonSerialized]
        public static ValuePort Integer;
        [NonSerialized]
        public static ValuePort Color;
        [NonSerialized]
        public static ValuePort Vector2;
        [NonSerialized]
        public static ValuePort Vector3;
        [NonSerialized]
        public static ValuePort String;
        [NonSerialized]
        public static ValuePort Quaternion;
        [Output]
        public static FlowPort Exit;

        protected override void OnRegister() {
            Float = ValueInput("Float", 5f);
            Integer = ValueInput("Integer", 10);
            Color = ValueInput("Color", new Color(1f, 0f, 0f));
            Vector2 = ValueInput("Vector2", new Vector2(0f, 0f));
            Vector3 = ValueInput("Vector3", new Vector3(0f, 0f, 0f));
            String = ValueInput("String", "Default String");
            Quaternion = ValueInput("Quaternion", UnityEngine.Quaternion.identity);
        }

        [Input]
        public static void Enter(float Float, int Integer, Color Color, Vector2 Vector2, Vector3 Vector3, string String, Quaternion Quaternion) {
            /*Insert code here*/
            throw new NotImplementedException();
        }
    }
}