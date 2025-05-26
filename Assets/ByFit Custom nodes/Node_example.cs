using System;
using System.Collections.Generic;
using UnityEngine;
using MaxyGames.UNode;

namespace MaxyGames.UNode.Nodes {
	[NodeMenu("Custom Nodes", "Node example", hasFlowInput = true, hasFlowOutput = true, inputs = new Type[] { typeof(float), typeof(int) })]
	public class Node_example : IStaticNode {	
		[Input(typeof(float))]
		public static ValuePortDefinition Float;
		[Input(typeof(int))]
		public static ValuePortDefinition Int;
		[Output]
		public static FlowPortDefinition Exit0;
		
		[Input]
		public static void Execute0(float Float, int Int) {
			/*Tips: remove any parameter if not used.*/
			/*Insert code here*/
			throw new NotImplementedException();
		}
	}

}
