using Poker.Code.Data.Enums;
using UnityEngine;

namespace Poker.Code.Core.Cards.View
{
    public class CardMaterialProvider
    {
        private Material[][] _materials;
    
        private const string MaterialPath = "Materials/Cards/";
        private const string Of = "of";

        public void LoadCardMaterials()
        {
            _materials = new Material[4][];
            for (int i = 0; i < _materials.Length; i++)
                _materials[i] = new Material[13];

            for (int i = 2; i <= 14; i++)
            {
                var suit = CardSuit.Clubs;
                for (int j = 0; j < 4; j++)
                {
                    var materialName = i + Of + suit;
                    _materials[j][i - 2] = Resources.Load<Material>(MaterialPath + materialName);
                    suit++;
                }
            }
        }

        public Material GetMaterial(CardValue value, CardSuit suit) =>
            _materials[(int)suit - 1][(int)value - 2];
    }
}