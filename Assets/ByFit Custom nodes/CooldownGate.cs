using UnityEngine;
using System;

namespace MaxyGames.UNode.Nodes {
    [NodeMenu("ByFit Custom nodes/Flow", "Cooldown Gate", icon = typeof(TypeIcons.BranchIcon), hasFlowOutput = true)]
    [Description("CooldownGate: blocks flow for cooldown duration before allowing next.")]
    public class CooldownGate : BaseFlowNode {
        public float cooldown = 1f;

        private float lastTriggerTime = -9999f;
        private bool isCooldownActive = false;

        public FlowOutput onStarted;
        public FlowOutput finished;

        protected override void OnRegister() {
            base.OnRegister();
            onStarted = FlowOutput("Started");
            finished = FlowOutput("Finished");
        }

        protected override void OnExecuted(Flow flow) {
            if (!isCooldownActive) {
                isCooldownActive = true;
                lastTriggerTime = Time.time;
                flow.Next(onStarted);
            } else {
                if (Time.time >= lastTriggerTime + cooldown) {
                    isCooldownActive = false;
                    flow.Next(finished);
                }
            }
        }

        protected override string GenerateFlowCode() {
            string varTime = CG.RegisterPrivateVariable("lastTriggerTime", typeof(float), "-9999f");
            string varActive = CG.RegisterPrivateVariable("isCooldownActive", typeof(bool), "false");
            string cdStr = cooldown.ToString(System.Globalization.CultureInfo.InvariantCulture);

            string code =
                $"if(!{varActive}) {{\n" +
                $"  {varActive} = true;\n" +
                $"  {varTime} = UnityEngine.Time.time;\n";

            if (onStarted.isAssigned)
                code += CG.Flow(onStarted).AddLineInFirst();

            code +=
                "} else {\n" +
                $"  if(UnityEngine.Time.time >= {varTime} + {cdStr}) {{\n" +
                $"    {varActive} = false;\n";

            if (finished.isAssigned)
                code += CG.Flow(finished).AddLineInFirst();

            code +=
                "  }\n" +
                "}\n";

            return code;
        }

        protected override bool IsCoroutine() {
            return HasCoroutineInFlows(onStarted, finished);
        }
    }
}
