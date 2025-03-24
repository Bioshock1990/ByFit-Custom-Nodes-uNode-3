using System;
using System.Collections.Generic;
using UnityEngine;
using MaxyGames.UNode;

[NodeMenu("ByFit node", "Invert Boolean", tooltip = "Inverts a boolean value.", inputs = new Type[] { typeof(bool) }, outputs = new Type[] { typeof(bool) })]
public class Invert_Boolean : IStaticNode {
    [Input(typeof(bool), description = "Boolean value that will be inverted.")]
    public static ValuePortDefinition Bool;
    
    [Output(description = "Returns the inverted boolean value.")]
    public static bool Inverted(bool Bool) {
        return !Bool;
    }
}