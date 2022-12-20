using System;
using UnityEngine;

namespace Poker.Code.Data.StaticData.Locations
{
    [Serializable]
    public class Location
    {
        public Vector3 Position;
        public Quaternion Rotation;

        public Location(Vector3 position, Quaternion rotation)
        {
            Position = position;
            Rotation = rotation;
        }
    }
}