using Poker.Code.Data.StaticData.Locations;
using UnityEditor;
using UnityEngine;

namespace Poker.Code.Editor
{
    [CustomEditor(typeof(PlayerLocationMarker))]
    public class PlayerLocationMarkerEditor : UnityEditor.Editor
    {
        [DrawGizmo(GizmoType.Active | GizmoType.Pickable | GizmoType.NonSelected)]
        public static void RenderCustomGizmo(PlayerLocationMarker marker, GizmoType gizmo)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(marker.LeftCardTransform.position, 0.25f);
            Gizmos.DrawSphere(marker.RightCardTransform.position, 0.25f);
            Gizmos.color = Color.white;
            Gizmos.DrawSphere(marker.TurnMarkerTransform.position, 0.35f);
        }
    }
}