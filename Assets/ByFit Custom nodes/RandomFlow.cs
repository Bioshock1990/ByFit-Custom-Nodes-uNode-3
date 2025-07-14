using UnityEngine;
using System;

namespace MaxyGames.UNode.Nodes {
    [NodeMenu("ByFit Custom nodes/Flow", "Random Flow", icon = typeof(TypeIcons.BranchIcon), hasFlowOutput = true)]
    [Description("RandomFlow: randomly selects and triggers one output flow.")]
    public class RandomFlow : BaseFlowNode {
        [Range(1, 10)]
        public int outputCount = 2;

        [NonSerialized]
        public FlowOutput[] outputs;

        protected override void OnRegister() {
            base.OnRegister();
            outputs = new FlowOutput[outputCount];
            for(int i = 0; i < outputCount; i++) {
                outputs[i] = FlowOutput(i.ToString());
            }
        }

        protected override void OnExecuted(Flow flow) {
            int index = UnityEngine.Random.Range(0, outputs.Length);
            flow.Next(outputs[index]);
        }

        protected override string GenerateFlowCode() {
            string data = "switch(UnityEngine.Random.Range(0, " + outputCount + ")) {\n";
            for(int i = 0; i < outputCount; i++) {
                data += "case " + i + ": {\n";
                if(outputs[i].isAssigned) data += CG.Flow(outputs[i]).AddLineInFirst();
                data += "break; }\n";
            }
            data += "}";
            return data;
        }

        protected override bool IsCoroutine() {
            return HasCoroutineInFlows(outputs);
        }
    }
}
