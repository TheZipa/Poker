using System;

namespace Poker.Code.Data.StaticData.Locations
{
    [Serializable]
    public class PlayerLocation
    {
        public Location LeftCardLocation;
        public Location RightCardLocation;
        public Location TurnMarkerLocation;
        public Location PlayerTextLocation;
    }
}