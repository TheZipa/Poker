using UnityEngine;

namespace Poker.Code.Core.Cards.View
{
    [RequireComponent(typeof(MeshRenderer))]
    [RequireComponent(typeof(Outline))]
    public class CardView : MonoBehaviour
    {
        private MeshRenderer _cardRenderer;
        private Outline _outline;

        private void Awake()
        {
            _cardRenderer = GetComponent<MeshRenderer>();
            _outline = GetComponent<Outline>();
            _outline.enabled = false;
        }
        
        public void SetValueMaterial(Material cardValueMaterial) =>
            _cardRenderer.sharedMaterial = cardValueMaterial;

        public void EnableOutline() => _outline.enabled = true;

        private void OnDisable() => _outline.enabled = false;
        
    }
}