using UnityEngine;

namespace Poker.Code.Data.StaticData
{
    [CreateAssetMenu(fileName = "User Input Text", menuName = "Static Data/User Input Text")]
    public class UserInputText : ScriptableObject
    {
        public string CheckText;
        public string CallText;
    }
}