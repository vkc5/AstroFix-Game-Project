using UnityEngine;

namespace NavKeypad
{
    public class FlashlightItemObject : MonoBehaviour
    {
        [Header("Interaction Settings")]
        [SerializeField] private float interactDistance = 2.5f;

        [Header("HUD / Popup")]
        [SerializeField] private GameObject flashHUD;

        [Header("Camera Switcher (Optional Setup)")]
        [SerializeField] private GameObject camera_1_ThirdPerson;
        [SerializeField] private GameObject camera_3_FirstPerson;

        private Transform playerTransform;
        private PlayerMovement playerMovement;
        private MeshRenderer meshRenderer;
        private Collider objectCollider;
        private bool isPickedUp = false;

        void Start()
        {
            meshRenderer = GetComponent<MeshRenderer>();
            objectCollider = GetComponent<Collider>();

            playerMovement = FindObjectOfType<PlayerMovement>();
            if (playerMovement != null)
                playerTransform = playerMovement.transform;

            if (camera_1_ThirdPerson == null)
                camera_1_ThirdPerson = GameObject.Find("Main Camera");

            if (camera_3_FirstPerson == null && playerTransform != null)
            {
                Transform fpvCamTransform = playerTransform.Find("FirstPersonCamera");
                if (fpvCamTransform != null)
                    camera_3_FirstPerson = fpvCamTransform.gameObject;
            }

            if (flashHUD != null)
                flashHUD.SetActive(true);

        }

        void Update()
        {
            if (!isPickedUp && Input.GetKeyDown(KeyCode.E) && playerTransform != null)
            {
                float distance = Vector3.Distance(transform.position, playerTransform.position);

                if (distance <= interactDistance)
                {
                    Vector3 directionToSub = (transform.position - playerTransform.position).normalized;
                    float dotProduct = Vector3.Dot(playerTransform.forward, directionToSub);

                    if (dotProduct > 0.4f)
                        ExecutePickup();
                }
            }
            else if (isPickedUp && Input.GetKeyDown(KeyCode.O))
            {
                ExecuteDrop();
            }
        }

        private void ExecutePickup()
        {
            isPickedUp = true;

            if (flashHUD != null)
                flashHUD.SetActive(false);

            if (camera_1_ThirdPerson != null) camera_1_ThirdPerson.SetActive(false);
            if (camera_3_FirstPerson != null) camera_3_FirstPerson.SetActive(true);

            if (playerMovement != null)
                playerMovement.SetFlashlightMode(true, camera_3_FirstPerson);

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            if (meshRenderer != null) meshRenderer.enabled = false;
            if (objectCollider != null) objectCollider.enabled = false;

            Debug.Log("Flashlight picked up successfully.");
        }

        private void ExecuteDrop()
        {
            isPickedUp = false;
            transform.parent = null;

            if (camera_1_ThirdPerson != null) camera_1_ThirdPerson.SetActive(true);
            if (camera_3_FirstPerson != null) camera_3_FirstPerson.SetActive(false);

            if (playerMovement != null)
                playerMovement.SetFlashlightMode(false, null);

            // POINTER DISAPPEARS AFTER DROP
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            transform.position = playerTransform.position + Vector3.up * 0.05f;

            float randomYRotation = Random.Range(0f, 360f);
            transform.rotation = Quaternion.Euler(90f, randomYRotation, 0f);

            if (meshRenderer != null) meshRenderer.enabled = true;

            if (objectCollider != null)
            {
                objectCollider.enabled = true;
                objectCollider.isTrigger = true;
            }

            Rigidbody rb = GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.useGravity = false;
                rb.isKinematic = true;
            }

            if (flashHUD != null)
                flashHUD.SetActive(true);

            Debug.Log("Flashlight dropped horizontally on the floor via O.");
        }
    }
}