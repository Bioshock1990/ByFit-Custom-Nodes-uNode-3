using UnityEngine;
using System;
using System.Linq;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;

namespace MaxyGames.UNode.Nodes {
	[NodeMenu("ByFit Custom nodes/Statement", "Foreach + Index", inputs = new[] { typeof(IEnumerable) })]
	[Description("Foreach with output index. Loops through a collection and outputs current index.")]
	public class ForeachLoopWithIndex : FlowNode {
		public bool deconstructValue = true;

		public class DeconstructData {
			public string id = uNodeUtility.GenerateUID();
			public string name;
			[NonSerialized]
			public string originalName;
			public ValueOutput port { get; set; }
		}
		public List<DeconstructData> deconstructDatas;

		public FlowOutput body { get; private set; }
		public ValueInput collection { get; set; }
		public ValueOutput output { get; set; }
		public ValueOutput index { get; private set; }

		private MethodInfo deconstructMethod;
		private static readonly FilterAttribute filter;

		static ForeachLoopWithIndex() {
			filter = new FilterAttribute(typeof(IEnumerable)) {
				ValidateType = type => {
					if (type.IsCastableTo(typeof(IEnumerable))) return true;
					var member = type.GetMemberCached(nameof(IEnumerable.GetEnumerator));
					return member is MethodInfo;
				},
			};
		}

		protected override void OnRegister() {
			body = FlowOutput(nameof(body));
			collection = ValueInput<IEnumerable>(nameof(collection));
			collection.filter = filter;

			index = ValueOutput(nameof(index), typeof(int));

			if (deconstructValue && collection.hasValidConnections && CanDeconstruct(collection.ValueType.ElementType())) {
				output = null;
				var type = collection.ValueType.ElementType();
				if (deconstructDatas == null)
					deconstructDatas = new List<DeconstructData>();
				deconstructMethod = type.GetMemberCached(nameof(KeyValuePair<int, int>.Deconstruct)) as MethodInfo;

				if (deconstructMethod != null) {
					var parameters = deconstructMethod.GetParameters();
					uNodeUtility.ResizeList(deconstructDatas, parameters.Length);
					for (int i = 0; i < parameters.Length; i++) {
						var index = i;
						deconstructDatas[index].originalName = parameters[index].Name;
						deconstructDatas[index].port = ValueOutput(deconstructDatas[index].id,
							parameters[index].ParameterType.ElementType() ?? parameters[index].ParameterType,
							PortAccessibility.ReadOnly);
						deconstructDatas[index].port.SetName(string.IsNullOrEmpty(deconstructDatas[index].name) ? deconstructDatas[index].originalName : deconstructDatas[index].name);
						deconstructDatas[index].port.isVariable = true;
					}
				} else if (type.IsGenericType && type.HasImplementInterface(typeof(System.Runtime.CompilerServices.ITuple))) {
					var arguments = type.GetGenericArguments();
					uNodeUtility.ResizeList(deconstructDatas, arguments.Length);
					for (int i = 0; i < arguments.Length; i++) {
						var index = i;
						deconstructDatas[index].port = ValueOutput(deconstructDatas[index].id, arguments[index], PortAccessibility.ReadWrite);
						deconstructDatas[index].originalName = "Item" + (i + 1);
						deconstructDatas[index].port.SetName(string.IsNullOrEmpty(deconstructDatas[index].name) ? deconstructDatas[index].originalName : deconstructDatas[index].name);
						deconstructDatas[index].port.isVariable = true;
					}
				}
			} else {
				output = ValueOutput(nameof(output), ReturnType);
			}

			base.OnRegister();
			exit.SetName("Exit");
		}

		private bool CanDeconstruct(Type type) {
			if (type == null) return false;
			if (type.GetMemberCached(nameof(KeyValuePair<int, int>.Deconstruct)) is MethodInfo) return true;
			if (type.IsGenericType && type.HasImplementInterface(typeof(System.Runtime.CompilerServices.ITuple)))
				return true;
			return false;
		}

		public override Type ReturnType() {
			if (collection != null) {
				var type = collection.ValueType;
				return type.ElementType() ?? typeof(object);
			}
			return base.ReturnType();
		}

		protected override bool IsCoroutine() {
			return HasCoroutineInFlows(body, exit);
		}

		private void UpdateLoopValue(Flow flow, object obj, int i) {
			flow.SetPortData(index, i);
			if (output == null) {
				if (deconstructMethod != null) {
					var parameters = new object[deconstructDatas.Count];
					deconstructMethod.InvokeOptimized(obj, parameters);
					for (int j = 0; j < parameters.Length; j++) {
						flow.SetPortData(deconstructDatas[j].port, parameters[j]);
					}
				} else if (obj is System.Runtime.CompilerServices.ITuple tuple) {
					for (int j = 0; j < tuple.Length; j++) {
						flow.SetPortData(deconstructDatas[j].port, tuple[j]);
					}
				}
			} else {
				flow.SetPortData(output, obj);
			}
		}

		protected override void OnExecuted(Flow flow) {
			IEnumerable lObj = collection.GetValue<IEnumerable>(flow);
			if (lObj != null) {
				int i = 0;
				foreach (object obj in lObj) {
					if (!body.isConnected) continue;
					UpdateLoopValue(flow, obj, i++);
					flow.Trigger(body, out var js);
					if (js != null) {
						if (js.jumpType == JumpStatementType.Continue) continue;
						if (js.jumpType == JumpStatementType.Return) {
							flow.jumpStatement = js;
							return;
						}
						break;
					}
				}
			} else {
				Debug.LogError("The collection is null or not IEnumerable");
			}
		}

		protected override IEnumerator OnExecutedCoroutine(Flow flow) {
			IEnumerable lObj = collection.GetValue<IEnumerable>(flow);
			if (lObj != null) {
				int i = 0;
				foreach (object obj in lObj) {
					if (!body.isConnected) continue;
					UpdateLoopValue(flow, obj, i++);
					flow.TriggerCoroutine(body, out var wait, out var jump);
					if (wait != null) yield return wait;
					var js = jump();
					if (js != null) {
						if (js.jumpType == JumpStatementType.Continue) continue;
						if (js.jumpType == JumpStatementType.Return) {
							flow.jumpStatement = js;
							yield break;
						}
						break;
					}
				}
			} else {
				Debug.LogError("The collection is null or not IEnumerable");
			}
		}

		public override void OnGeneratorInitialize() {
			base.OnGeneratorInitialize();

			if (output == null) {
				foreach (var data in deconstructDatas) {
					if (data.port != null) {
						var vName = CG.RegisterVariable(data.port);
						CG.RegisterPort(data.port, () => vName);
					}
				}
			} else {
				var vName = CG.RegisterVariable(output, "loopValue");
				CG.RegisterPort(output, () => vName);
			}

			var indexVar = CG.RegisterVariable(index, "loopIndex");
			CG.RegisterPort(index, () => indexVar);
		}

		protected override string GenerateFlowCode() {
			string targetCollection = CG.Value(collection);
			if (!string.IsNullOrEmpty(targetCollection)) {
				string contents = CG.Flow(body).AddLineInFirst();
				string additionalContents = CG.Set(CG.GetVariableName(index), "loopIndex++") + "\n";
				if (output == null) {
					string[] paramDatas = new string[deconstructDatas.Count];
					for (int i = 0; i < deconstructDatas.Count; i++) {
						paramDatas[i] = "_" ;
					}
					return CG.Flow(
						"int loopIndex = 0;",
						CG.Condition("foreach", "var (" + string.Join(", ", paramDatas) + ") in " + targetCollection, additionalContents + contents),
						CG.FlowFinish(enter, exit)
					);
				} else {
					string vName = CG.GetVariableName(output);
					string elementType = CG.Type(collection.ValueType.ElementType() ?? typeof(object));
					return CG.Flow(
						"int loopIndex = 0;",
						CG.Condition("foreach", elementType + " " + vName + " in " + targetCollection, additionalContents + contents),
						CG.FlowFinish(enter, exit)
					);
				}
			}
			return null;
		}

		public override string GetRichName() {
			return uNodeUtility.WrapTextWithKeywordColor("foreach:") + collection.GetRichName() + " with index";
		}
	}
}
