using UnityEngine;
namespace Homework
{
    public class SimpleCameraHandlerHW : MonoBehaviour
    {

        [SerializeField] private Vector3 CameraOffset;
        [SerializeField] private Vector3 CameraRotation;

        private Transform _objectToFollow;

        public void SetCameraOnObject(Transform objectToFollow)
        {
            _objectToFollow = objectToFollow;
            transform.SetParent(objectToFollow);

            transform.localPosition = CameraOffset;
            transform.localRotation = Quaternion.Euler(CameraRotation);
        }
    }
}