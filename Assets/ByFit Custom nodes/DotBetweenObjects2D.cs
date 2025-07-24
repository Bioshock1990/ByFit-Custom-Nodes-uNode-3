#pragma warning disable
using UnityEngine;
using MaxyGames.UNode;

[NodeMenu("ByFit Custom nodes/Math", "Dot Between Objects 2D",
    tooltip = "Calculates the 2D dot product between objects A and B, returning:\n- dot (float): a value from -1 to 1 indicating how aligned object B is with the specified local axis of object A (in 2D);\n- isInDirection (bool): true if object B is located in the positive direction of the chosen axis of object A, and within the specified max distance if set;\n- distance (float): the 2D distance between object A and B.\nIf maxDistance = 0, the distance check is ignored.",
    icon = typeof(Mathf), outputs = new System.Type[] { typeof(float), typeof(bool), typeof(float) })]
[TypeIcons.IconGuid("abcdefabcdefabcdefabcdefabcdefab")] // замени на свой GUID
public class DotBetweenObjects2D : IStaticNode {
    [Input] public static GameObject objectA;
    [Input] public static GameObject objectB;
    [Input] public static Vector2 localAxis = Vector2.right;
    [Input] public static float maxDistance = 0f;

    [Output]
    public static float Dot(GameObject objectA, GameObject objectB, Vector2 localAxis, float maxDistance) {
        if (objectA == null || objectB == null) return 0f;
        Vector2 worldDir = objectA.transform.TransformDirection(localAxis.normalized);
        Vector2 toB = (Vector2)objectB.transform.position - (Vector2)objectA.transform.position;
        float distance = toB.magnitude;
        if (maxDistance > 0f && distance > maxDistance) return 0f;
        return Vector2.Dot(worldDir, toB.normalized);
    }

    [Output]
    public static bool IsInDirection(GameObject objectA, GameObject objectB, Vector2 localAxis, float maxDistance) {
        if (objectA == null || objectB == null) return false;
        Vector2 worldDir = objectA.transform.TransformDirection(localAxis.normalized);
        Vector2 toB = (Vector2)objectB.transform.position - (Vector2)objectA.transform.position;
        float distance = toB.magnitude;
        if (maxDistance > 0f && distance > maxDistance) return false;
        return Vector2.Dot(worldDir, toB.normalized) > 0f;
    }

    [Output]
    public static float Distance(GameObject objectA, GameObject objectB) {
        if (objectA == null || objectB == null) return float.MaxValue;
        return Vector2.Distance(objectA.transform.position, objectB.transform.position);
    }
}
