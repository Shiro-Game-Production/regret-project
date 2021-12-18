using UnityEngine;

namespace Puzzle{
    public class CapsuleController : MonoBehaviour
    {
        private Camera mainCamera;

        public CameraManager cameraManager;

        

        [SerializeField]
        private float movementSpeed;

        // Start is called before the first frame update
        void Start()
        {
            cameraManager = FindObjectOfType<CameraManager>();
        
        }

        // Update is called once per frame
        void Update()
        {
            HandleMovementInput();
        }

        void HandleMovementInput()
        {
            float _horizontal = Input.GetAxis("Horizontal");
            float _vertical = Input.GetAxis("Vertical");

            Vector3 _movement = new Vector3(_horizontal, 0, _vertical);
            transform.Translate(_movement * movementSpeed * Time.deltaTime, Space.World);
        }

        private void OnTriggerEnter(Collider other) 
        {
            if(other.gameObject.CompareTag("Pintu"))
            {
                cameraManager.MoveCameraToMainRoom();
            }

            if(other.gameObject.CompareTag("Pintu1"))
            {
                Debug.Log("Test 1");
                //cameraManager.MoveCamera();
                cameraManager.MoveCameraToRoom1();
                Debug.Log("Test");
                
                //Destroy(gameObject);
            }
            
            if(other.gameObject.CompareTag("Pintu2"))
            {
                cameraManager.MoveCameraToRoom2();
            }

            if(other.gameObject.CompareTag("Pintu3"))
            {
                cameraManager.MoveCameraToRoom3();
            }

            //cameraManager.MoveCameraToRoom1();
        }
    }
}