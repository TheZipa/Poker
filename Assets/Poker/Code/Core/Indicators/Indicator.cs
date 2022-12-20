using UnityEngine;

namespace Poker.Code.Core.Indicators
{
    public class Indicator : MonoBehaviour
    {
        public void Show(Vector3 position)
        {
            transform.position = position;
            gameObject.SetActive(true);
        }

        public void Hide() => gameObject.SetActive(false);
    }
}