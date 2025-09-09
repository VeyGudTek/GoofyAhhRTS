using Unity.VisualScripting;
using UnityEngine;

namespace Source.Shared
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField]
        private Rigidbody Rigidbody;

        private readonly float LinearDamping = 2f;

        void Awake()
        {
            Rigidbody.linearDamping = LinearDamping;
        }

        public void OnMove(float x, float y)
        {
            Rigidbody.AddForce(new Vector3(x, 0f, y));
        }
    }
}

