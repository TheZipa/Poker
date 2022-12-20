using Poker.Code.Data.StaticData.Locations;
using UnityEngine;

namespace Poker.Code.Data.Extensions
{
    public static class DataExtensions
    {
        public static T ToDeserialized<T>(this string json) =>
            JsonUtility.FromJson<T>(json);

        public static string ToJson(this object obj) =>
            JsonUtility.ToJson(obj);

        public static Location ToLocation(this Transform transform) =>
            new Location(transform.position, transform.rotation);

        public static PlayerLocation ToPlayerLocation(this PlayerLocationMarker marker) =>
            new PlayerLocation()
            {
                LeftCardLocation = marker.LeftCardTransform.ToLocation(),
                RightCardLocation = marker.RightCardTransform.ToLocation(),
                TurnMarkerLocation = marker.TurnMarkerTransform.ToLocation(),
                PlayerTextLocation = marker.playerTextTransform.ToLocation()
            };
    }
}