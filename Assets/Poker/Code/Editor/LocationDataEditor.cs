using System.Collections.Generic;
using System.Linq;
using Poker.Code.Data.Extensions;
using Poker.Code.Data.StaticData.Locations;
using UnityEditor;
using UnityEngine;

namespace Poker.Code.Editor
{
    [CustomEditor(typeof(LocationData))]
    public class LocationDataEditor : UnityEditor.Editor
    {
        private const string UserLocationTag = "UserLocation";
        private const string CardViewLocationTag = "CardViewLocation";
        private const string CardStackLocationTag = "CardStackLocation";

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (GUILayout.Button("Collect") == false) return;

            LocationData locationData = (LocationData)target;
            
            locationData.CardViewLocation = FindLocationWithTag(CardViewLocationTag);
            locationData.CardStackLocation = FindLocationWithTag(CardStackLocationTag);
            locationData.AILocations = FindPlayerLocations(out PlayerLocation userLocation);
            locationData.UserLocation = userLocation;

            EditorUtility.SetDirty(locationData);
        }

        private Location FindLocationWithTag(string tag) =>
            GameObject.FindWithTag(tag).transform.ToLocation();

        private PlayerLocation[] FindPlayerLocations(out PlayerLocation userLocation)
        {
            PlayerLocationMarker[] playerLocationMarkers = FindObjectsOfType<PlayerLocationMarker>();
            playerLocationMarkers = playerLocationMarkers.OrderBy(player => player.name).ToArray();
            List<PlayerLocation> aiLocations = new List<PlayerLocation>(playerLocationMarkers.Length - 1);
            userLocation = null;

            foreach (PlayerLocationMarker marker in playerLocationMarkers)
            {
                if (marker.CompareTag(UserLocationTag))
                    userLocation = marker.ToPlayerLocation();
                else
                    aiLocations.Add(marker.ToPlayerLocation());
            }
            
            return aiLocations.ToArray();
        }
    }
}