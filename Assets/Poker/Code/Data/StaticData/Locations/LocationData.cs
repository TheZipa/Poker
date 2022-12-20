using UnityEngine;

namespace Poker.Code.Data.StaticData.Locations
{
    [CreateAssetMenu(fileName = "Location Data", menuName = "Static Data/Location Data")]
    public class LocationData : ScriptableObject
    {
        public PlayerLocation UserLocation;
        public PlayerLocation[] AILocations;
        public Location CardViewLocation;
        public Location CardStackLocation;
    }
}