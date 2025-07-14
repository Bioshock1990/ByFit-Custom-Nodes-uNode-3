using UnityEngine;
using System;

namespace MaxyGames.UNode.Nodes {
    [NodeMenu("ByFit Custom nodes/Flow", "Chance Flow", icon = typeof(TypeIcons.BranchIcon), hasFlowOutput = true)]
    [Description("ChanceFlow: randomly selects one output based on assigned probabilities.")]
    public class ChanceFlow : BaseFlowNode {
        [Range(0f, 100f)]
        public float chance = 20f;

        public FlowOutput onSuccess;
        public FlowOutput onFail;

        protected override void OnRegister() {
            base.OnRegister();
            onSuccess = FlowOutput("Success");
            onFail = FlowOutput("Fail");
        }

        protected override void OnExecuted(Flow flow) {
            float rnd = UnityEngine.Random.Range(0f, 100f);
            if (rnd <= chance) {
                flow.Next(onSuccess);
            } else {
                flow.Next(onFail);
            }
        }

        protected override string GenerateFlowCode() {
            string data = $"if(UnityEngine.Random.Range(0f, 100f) <= {chance.ToString(System.Globalization.CultureInfo.InvariantCulture)}){{\n";
            if (onSuccess.isAssigned)
                data += CG.Flow(onSuccess).AddLineInFirst();
            data += "\n}else{\n";
            if (onFail.isAssigned)
                data += CG.Flow(onFail).AddLineInFirst();
            data += "\n}";
            return data;
        }

        protected override bool IsCoroutine() {
            return HasCoroutineInFlows(onSuccess, onFail);
        }
    }
}
