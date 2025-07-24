#pragma warning disable
using UnityEngine;
using MaxyGames.UNode;

[NodeMenu("ByFit Custom nodes/Math", "Angle Between Objects 3D", 
    tooltip = "Returns the angle in degrees between two objects’ forward directions.", 
    icon = typeof(Mathf), outputs = new System.Type[] { typeof(float) })]
[TypeIcons.IconGuid("bf1b02ea3c7d4c2488a932ec68d0018a")]
public class AngleBetweenObjects3D : IStaticNode {
    [Input]
    public static GameObject fromObject;
    [Input]
    public static GameObject toObject;

    [Output]
    public static float Angle(GameObject fromObject, GameObject toObject) {
        if (fromObject == null || toObject == null) return 0f;
        return Vector3.Angle(
            fromObject.transform.forward, 
            toObject.transform.forward
        );
    }
}
