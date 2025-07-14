using UnityEngine;
using System;

namespace MaxyGames.UNode.Nodes {
    [NodeMenu("ByFit Custom nodes/Flow", "Multi Chance Flow", icon = typeof(TypeIcons.BranchIcon), hasFlowOutput = true)]
    [Description("MultiChanceFlow: triggers outputs based on assigned probabilities.")]
    public class MultiChanceFlow : BaseFlowNode {
        [Range(1, 10)]
        public int outputCount = 2;

        // Массив шансов для каждого выхода
        public float[] chances = new float[2] { 50f, 50f };

        [NonSerialized]
        public FlowOutput[] outputs;

        protected override void OnRegister() {
            base.OnRegister();
            // Обновляем массив шансов, если нужно
            EnsureChanceArrayLength();

            outputs = new FlowOutput[outputCount];
            for(int i = 0; i < outputCount; i++) {
                outputs[i] = FlowOutput(i.ToString());
            }
        }

#if UNITY_EDITOR
        // Обновляет длину массива chances в редакторе
        private void OnValidate() {
            EnsureChanceArrayLength();
        }
#endif

        private void EnsureChanceArrayLength() {
            if(chances == null || chances.Length != outputCount) {
                float[] newChances = new float[outputCount];
                for(int i = 0; i < outputCount && chances != null && i < chances.Length; i++) {
                    newChances[i] = chances[i];
                }
                for(int i = chances != null ? chances.Length : 0; i < outputCount; i++) {
                    newChances[i] = 50f;
                }
                chances = newChances;
            }
        }

        protected override void OnExecuted(Flow flow) {
            for(int i = 0; i < outputs.Length; i++) {
                float rnd = UnityEngine.Random.Range(0f, 100f);
                if(rnd <= chances[i]) {
                    flow.Next(outputs[i]);
                }
            }
        }

        protected override string GenerateFlowCode() {
            string data = "";
            for(int i = 0; i < outputCount; i++) {
                data += $"if(UnityEngine.Random.Range(0f, 100f) <= {chances[i].ToString(System.Globalization.CultureInfo.InvariantCulture)}){{\n";
                if(outputs[i].isAssigned)
                    data += CG.Flow(outputs[i]).AddLineInFirst();
                data += "}\n";
            }
            return data;
        }

        protected override bool IsCoroutine() {
            return HasCoroutineInFlows(outputs);
        }
    }
}
