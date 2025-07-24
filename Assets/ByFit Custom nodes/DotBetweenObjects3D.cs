#pragma warning disable
using UnityEngine;
using MaxyGames.UNode;

[NodeMenu("ByFit Custom nodes/Math", "Dot Between Objects 3D",
    tooltip = "Calculates the dot product between objects A and B, returning:\n- dot (float): a value from -1 to 1 indicating how aligned object B is with the specified local axis of object A;\n- isInDirection (bool): true if object B is located in the positive direction of the chosen axis of object A, and within the specified max distance if set;\n- distance (float): the distance between object A and B.\nIf maxDistance = 0, the distance check is ignored.",
    icon = typeof(Mathf), outputs = new System.Type[] { typeof(float), typeof(bool), typeof(float) })]
[TypeIcons.IconGuid("a1b2c3d4e5f67890123456789abcdef1")] // заменяй на свой ID
public class DotBetweenObjects3D : IStaticNode {
    [Input] public static GameObject objectA;
    [Input] public static GameObject objectB;
    [Input] public static Vector3 localAxis = Vector3.forward;
    [Input] public static float maxDistance = 0f; // <=0 игнорирует проверку дистанции

    [Output]
    public static float Dot(GameObject objectA, GameObject objectB, Vector3 localAxis, float maxDistance) {
        if (objectA == null || objectB == null) return 0f;
        Vector3 worldDir = objectA.transform.TransformDirection(localAxis.normalized);
        Vector3 toB = objectB.transform.position - objectA.transform.position;
        float distance = toB.magnitude;
        if (maxDistance > 0f && distance > maxDistance) return 0f;
        return Vector3.Dot(worldDir, toB.normalized);
    }

    [Output]
    public static bool IsInDirection(GameObject objectA, GameObject objectB, Vector3 localAxis, float maxDistance) {
        if (objectA == null || objectB == null) return false;
        Vector3 worldDir = objectA.transform.TransformDirection(localAxis.normalized);
        Vector3 toB = objectB.transform.position - objectA.transform.position;
        float distance = toB.magnitude;
        if (maxDistance > 0f && distance > maxDistance) return false;
        return Vector3.Dot(worldDir, toB.normalized) > 0f;
    }

    [Output]
    public static float Distance(GameObject objectA, GameObject objectB) {
        if (objectA == null || objectB == null) return float.MaxValue;
        return Vector3.Distance(objectA.transform.position, objectB.transform.position);
    }
}
