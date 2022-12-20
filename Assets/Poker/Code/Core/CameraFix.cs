using UnityEngine;

namespace Poker.Code.Core
{
    public class CameraFix : MonoBehaviour
    {
        [SerializeField] private Camera _mainCamera;
        [Header("FOV Settings")]
        [SerializeField] private float _iPadFov = 70;
        [SerializeField] private float _iPhoneFov = 115;
        [Header("Height Settings")]
        [SerializeField] private float _iPadY = 10f;
        [SerializeField] private float _iPhoneY = 9.75f;

        private void Awake()
        {
            if(Screen.height > 1500) SetIPadSettings();
            else SetIPhoneSettings();
        }

        private void SetIPadSettings()
        {
            _mainCamera.fieldOfView = _iPadFov;
            SetY(_iPadY);
        }

        private void SetIPhoneSettings()
        {
            _mainCamera.fieldOfView = _iPhoneFov;
            SetY(_iPhoneY);
        }

        private void SetY(float newY)
        {
            Vector3 cameraPosition = transform.position;
            cameraPosition.y = newY;
            transform.position = cameraPosition;
        }
    }
}