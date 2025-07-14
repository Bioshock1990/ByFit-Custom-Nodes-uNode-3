using UnityEngine;
using System;

namespace MaxyGames.UNode.Nodes {
    [NodeMenu("ByFit Custom nodes/Flow", "Flow Control With Delay", icon = typeof(TypeIcons.BranchIcon), hasFlowOutput = true)]
    [Description("FlowControlWithDelay: triggers multiple outputs with individual delays.")]
    public class FlowControlWithDelay : BaseFlowNode {
        [Range(1, 10)]
        public int flowCount = 2;

        public float[] delays = new float[2];

        [NonSerialized]
        public FlowOutput[] flows;

        private int currentIndex = 0;
        private float lastTriggerTime = 0f;
        private bool isRunning = false;

        protected override void OnRegister() {
            base.OnRegister();

            // Синхронизируем delays по размеру flowCount
            if (delays == null || delays.Length != flowCount) {
                float[] newDelays = new float[flowCount];
                if (delays != null) {
                    for (int i = 0; i < Mathf.Min(delays.Length, flowCount); i++) {
                        newDelays[i] = delays[i];
                    }
                }
                for (int i = (delays != null ? delays.Length : 0); i < flowCount; i++) {
                    newDelays[i] = 0f; // Значение по умолчанию
                }
                delays = newDelays;
            }

            flows = new FlowOutput[flowCount];
            for (int i = 0; i < flowCount; i++) {
                flows[i] = FlowOutput("Flow-" + i).SetName(i.ToString());
            }
        }

        protected override void OnExecuted(Flow flow) {
            if (!isRunning) {
                // Начинаем последовательность
                currentIndex = 0;
                lastTriggerTime = Time.time;
                isRunning = true;
            }

            if (currentIndex < flowCount) {
                float delay = (delays != null && currentIndex < delays.Length) ? delays[currentIndex] : 0f;
                
                if (Time.time - lastTriggerTime >= delay) {
                    // Выполняем текущий поток
                    flow.Next(flows[currentIndex]);
                    currentIndex++;
                    lastTriggerTime = Time.time;
                }
            } else {
                // Все потоки выполнены, сбрасываем состояние
                isRunning = false;
            }
        }

        protected override string GenerateFlowCode() {
            string varCurrentIndex = CG.RegisterPrivateVariable("currentIndex", typeof(int), "0");
            string varLastTriggerTime = CG.RegisterPrivateVariable("lastTriggerTime", typeof(float), "0f");
            string varIsRunning = CG.RegisterPrivateVariable("isRunning", typeof(bool), "false");

            string code = $"if(!{varIsRunning}) {{\n" +
                         $"  {varCurrentIndex} = 0;\n" +
                         $"  {varLastTriggerTime} = UnityEngine.Time.time;\n" +
                         $"  {varIsRunning} = true;\n" +
                         "}\n\n";

            code += $"if({varCurrentIndex} < {flowCount}) {{\n";

            // Генерируем if-else цепочку для каждого потока
            for (int i = 0; i < flowCount; i++) {
                string delayStr = (delays != null && i < delays.Length ? delays[i] : 0f).ToString(System.Globalization.CultureInfo.InvariantCulture);
                
                if (i > 0) code += "  else ";
                else code += "  ";
                
                code += $"if({varCurrentIndex} == {i}) {{\n" +
                       $"    if(UnityEngine.Time.time - {varLastTriggerTime} >= {delayStr}) {{\n";

                if (flows[i].isAssigned)
                    code += CG.Flow(flows[i]).AddLineInFirst();

                code += $"      {varCurrentIndex}++;\n" +
                       $"      {varLastTriggerTime} = UnityEngine.Time.time;\n" +
                       "    }\n" +
                       "  }\n";
            }

            code += "} else {\n" +
                   $"  {varIsRunning} = false;\n" +
                   "}\n";

            return code;
        }

        protected override bool IsCoroutine() {
            return HasCoroutineInFlows(flows);
        }
    }
}