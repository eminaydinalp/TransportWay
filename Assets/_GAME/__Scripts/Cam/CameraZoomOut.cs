using Cinemachine;
using Rentire.Core;
using UnityEngine;

namespace _GAME.__Scripts.Cam
{
    public class CameraZoomOut : Singleton<CameraZoomOut>
    {
        public CinemachineVirtualCamera cinemachineVirtualCamera;

        public float cameraSize;

        public float camSpeed;

        public string cameraSizePref;
        private void Start()
        {
            cameraSize = LocalPrefs.GetFloat(cameraSizePref, cameraSize);
            cinemachineVirtualCamera.m_Lens.OrthographicSize = cameraSize;
        }

        private void FixedUpdate()
        {
            cinemachineVirtualCamera.m_Lens.OrthographicSize =
                cinemachineVirtualCamera.m_Lens.OrthographicSize.RLerp(cameraSize, Time.deltaTime * camSpeed);
        }
    }
}