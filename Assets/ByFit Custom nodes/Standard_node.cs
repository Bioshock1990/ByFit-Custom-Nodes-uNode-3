using System;
using UnityEngine;
using MaxyGames.UNode;

namespace MaxyGames.UNode.Nodes
{
    [NodeMenu("Custom Nodes", "Standard node", tooltip = "Basic node demonstrating the default values of each type.", icon = typeof(FlashlightIcon)/* icon = typeof(FlashlightIcon) Там, где написано в скобочках Flashlight Icon, сюда вставлять название, точнее имя иконки.Это отвечает за то, чтобы отображалась в нод-меню иконка.*/)]
    public class Standard_node : Node
    {
        [NonSerialized]
        public ValueInput Float;
        [NonSerialized]
        public ValueInput Integer;
        [NonSerialized]
        public ValueInput Color;
        [NonSerialized]
        public ValueInput Vector2;
        [NonSerialized]
        public ValueInput Vector3;
        [NonSerialized]
        public ValueInput String;
        [NonSerialized]
        public ValueInput Quaternion;
        [NonSerialized]
        public FlowOutput Exit;
        [NonSerialized]
        public FlowInput Enter;

        protected override void OnRegister()
        {
            // Initialize the value ports with default values
            Float = ValueInput("Float", 5f);
            Integer = ValueInput("Integer", 10);
            Color = ValueInput("Color", new Color(1f, 0f, 0f));
            Vector2 = ValueInput("Vector2", new Vector2(0f, 0f));
            Vector3 = ValueInput("Vector3", new Vector3(0f, 0f, 0f));
            String = ValueInput("String", "Default String");
            Quaternion = ValueInput("Quaternion", UnityEngine.Quaternion.identity);
            Exit = FlowOutput("Exit");
            Enter = FlowInput("Enter", EnterLogic);
        }

        public void EnterLogic(Flow flow)
        {
            // Log the current values of all inputs
            Debug.Log($"Float: {Float.GetValue<float>(flow)}");
            Debug.Log($"Integer: {Integer.GetValue<int>(flow)}");
            Debug.Log($"Color: {Color.GetValue<Color>(flow)}");
            Debug.Log($"Vector2: {Vector2.GetValue<Vector2>(flow)}");
            Debug.Log($"Vector3: {Vector3.GetValue<Vector3>(flow)}");
            Debug.Log($"String: {String.GetValue<string>(flow)}");
            Debug.Log($"Quaternion: {Quaternion.GetValue<Quaternion>(flow)}");

            // Proceed to the next flow state
            flow.Next(Exit);
        }

        public override Type GetNodeIcon()
        {
            return typeof(FlashlightIcon);//Здесь указать имя иконки, это уже отображается на самом узле.
        }
    }
    [TypeIcons.IconPath("FlashlightIcon")]//Также здесь нужно указать имя иконки.
    public class FlashlightIcon { }
}
