using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace MaxyGames.UNode.Nodes {
    [NodeMenu("ByFit Custom nodes/Collections.List", "Get Random Item", icon = typeof(IList))]
    [Description("GetListItemRandom: returns a random item from a given list.")]
    public class GetListItemRandom : ValueNode {
        public ValueInput target { get; set; }

        protected override void OnRegister() {
            base.OnRegister();
            target = ValueInput(nameof(target), typeof(IList)).SetName("List");
        }

        public override System.Type ReturnType() {
            if (target != null && target.isAssigned) {
                return target.ValueType.ElementType();
            }
            return typeof(object);
        }

        public override object GetValue(Flow flow) {
            var list = target.GetValue<IList>(flow);
            if (list == null || list.Count == 0)
                return null;
            int randIndex = UnityEngine.Random.Range(0, list.Count);
            return list[randIndex];
        }

        protected override string GenerateValueCode() {
            string listName = CG.Value(target);
            return $"{listName} != null && {listName}.Count > 0 ? {listName}[UnityEngine.Random.Range(0, {listName}.Count)] : null";
        }

        public override string GetTitle() {
            return "Get Random Item";
        }

        public override string GetRichName() {
            return $"<b>RandomItem</b> of {target.GetRichName()}";
        }
    }
}