using System;
using UnityEngine;
using MaxyGames.UNode;

namespace MaxyGames.UNode.Nodes
{
    [NodeMenu("Custom Nodes", "Enemy Vision", tooltip = "Simulates enemy vision using raycasts.", icon = typeof(EyeIcon))]
    public class EnemyVision : Node
    {
        [NonSerialized] public ValueInput SourceObject;
        [NonSerialized] public ValueInput RayLength;
        [NonSerialized] public ValueInput RayAngleHorizontal;
        [NonSerialized] public ValueInput RayAngleVertical;
        [NonSerialized] public ValueInput RayCountHorizontal;
        [NonSerialized] public ValueInput RayCountVertical;
        [NonSerialized] public ValueInput ShowGizmos;
        [NonSerialized] public ValueInput TagToDetect; // Input for tag value
        [NonSerialized] public ValueOutput NearestObjectDistance; // Output port for nearest distance
        [NonSerialized] public FlowOutput Exit;
        [NonSerialized] public FlowInput Enter;
        float nearestDistance = Mathf.Infinity;

        protected override void OnRegister()
        {
            SourceObject = ValueInput("Source Object", (GameObject)null);
            RayLength = ValueInput("Ray Length", 10f);
            RayAngleHorizontal = ValueInput("Ray Angle Horizontal", 90f);
            RayAngleVertical = ValueInput("Ray Angle Vertical", 45f);
            RayCountHorizontal = ValueInput("Ray Count Horizontal", 10);
            RayCountVertical = ValueInput("Ray Count Vertical", 5);
            ShowGizmos = ValueInput("Show Gizmos", true);
            TagToDetect = ValueInput("Tag To Detect", "Enemy"); // Default tag value set to "Enemy"

            // Initialize the output for nearest object distance
            NearestObjectDistance = ValueOutput<float>("Nearest Object Distance");
            NearestObjectDistance.AssignGetCallback((flow) => nearestDistance);

            Exit = FlowOutput("Exit");
            Enter = FlowInput("Enter", EnterLogic);
        }

        public void EnterLogic(Flow flow)
        {
            GameObject source = SourceObject.GetValue<GameObject>(flow);
            float length = RayLength.GetValue<float>(flow);
            float angleH = RayAngleHorizontal.GetValue<float>(flow);
            float angleV = RayAngleVertical.GetValue<float>(flow);
            int countH = RayCountHorizontal.GetValue<int>(flow);
            int countV = RayCountVertical.GetValue<int>(flow);
            bool showGizmos = ShowGizmos.GetValue<bool>(flow);
            string tagToDetect = TagToDetect.GetValue<string>(flow); // Get the tag to detect from input

            if (source == null)
            {
                Debug.LogWarning("Source Object is null!");
                flow.Next(Exit);
                return;
            }

            Vector3 position = source.transform.position;
            float startAngleH = -angleH / 2f;
            float startAngleV = -angleV / 2f;
            float angleStepH = angleH / (countH - 1);
            float angleStepV = angleV / (countV - 1);

            for (int i = 0; i < countH; i++)
            {
                for (int j = 0; j < countV; j++)
                {
                    float currentAngleH = startAngleH + (angleStepH * i);
                    float currentAngleV = startAngleV + (angleStepV * j);
                    Vector3 direction = Quaternion.Euler(currentAngleV, currentAngleH, 0) * source.transform.forward;
                    RaycastHit hit;
                    if (Physics.Raycast(position, direction, out hit, length))
                    {
                        if (hit.collider.CompareTag(tagToDetect)) // Check if hit object has the specified tag
                        {
                            if (showGizmos)
                                Debug.DrawLine(position, hit.point, Color.red);

                            // Calculate distance to nearest object
                            nearestDistance = Mathf.Min(nearestDistance, hit.distance);
                        }
                    }
                    else
                    {
                        if (showGizmos)
                            Debug.DrawLine(position, position + direction * length, Color.green);
                    }
                }
            }

            flow.Next(Exit);
        }

        public override Type GetNodeIcon()
        {
            return typeof(EyeIcon);
        }
    }

    [TypeIcons.IconPath("EyeIcon")]
    public class EyeIcon { }
}
