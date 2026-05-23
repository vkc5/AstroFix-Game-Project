using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NavKeypad
{
    public class KeypadInteractionFPV : MonoBehaviour
    {
        [Header("Interaction Settings")]
        [SerializeField] private float interactDistance = 2.5f;
        [SerializeField] private LayerMask keypadLayer; // Select 'Keypad' layer here     

        [Header("Camera Switcher (Your Idea!)")]
        [SerializeField] private GameObject camera_1; // Drag 'Main Camera' here
        [SerializeField] private GameObject camera_2; // Drag 'Keypad Camera' here

        private bool isInteracting = false;
        private PlayerMovement playerMovement;
        private MonoBehaviour tpCameraScript; // Reference to your ThirdPersonCamera script
        private Keypad activeKeypad;

        private void Start()
        {
            playerMovement = GetComponent<PlayerMovement>();

            // Find your specific camera script on Camera 1 to freeze it
            if (camera_1 != null)
            {
                tpCameraScript = camera_1.GetComponent<MonoBehaviour>();
            }

            // Ensure Camera 2 (Keypad Cam) is turned off at start
            if (camera_2 != null) camera_2.SetActive(false);
        }

        private void Update()
        {
            // Press Q to Switch Cameras and states
            if (Input.GetKeyDown(KeyCode.Q))
            {
                if (isInteracting)
                {
                    ExitInteraction(); // Switches back to Cam 1
                }
                else
                {
                    TryEnterInteraction(); // Switches to Cam 2
                }
            }

            if (isInteracting)
            {
                // FORCE the mouse cursor to stay free and visible for clicking
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;

                HandleKeyboardInput();

                if (Input.GetMouseButtonDown(0))
                {
                    HandleMouseClick();
                }
            }
        }

        private void TryEnterInteraction()
        {
            Vector3 rayOrigin = transform.position + Vector3.up * 1.2f;
            Ray ray = new Ray(rayOrigin, transform.forward);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, interactDistance, keypadLayer))
            {
                activeKeypad = hit.collider.transform.root.GetComponentInChildren<Keypad>();
                if (activeKeypad != null)
                {
                    EnterInteraction();
                }
            }
        }

        private void EnterInteraction()
        {
            isInteracting = true;

            // 1. Apply Your Idea: Turn off Cam 1, Turn on Cam 2
            if (camera_1 != null) camera_1.SetActive(false);
            if (camera_2 != null) camera_2.SetActive(true);

            // 2. Freeze your ThirdPersonCamera script completely so it doesn't break
            if (tpCameraScript != null) tpCameraScript.enabled = false;

            // 3. Freeze player movement inputs
            if (playerMovement != null) playerMovement.SetMovementState(false);
        }

        public void ExitInteraction()
        {
            isInteracting = false;
            activeKeypad = null;

            // 1. Apply Your Idea: Turn on Cam 1, Turn off Cam 2
            if (camera_1 != null) camera_1.SetActive(true);
            if (camera_2 != null) camera_2.SetActive(false);

            // 2. Unfreeze your camera lookup script for gameplay
            if (tpCameraScript != null) tpCameraScript.enabled = true;

            // 3. Lock mouse cursor back to gameplay
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            // 4. Enable player movement again
            if (playerMovement != null) playerMovement.SetMovementState(true);
        }

        private void HandleMouseClick()
        {
            if (camera_2 == null) return;

            // Get the camera component from Camera_2 to cast mouse rays
            Camera keypadCam = camera_2.GetComponent<Camera>();
            if (keypadCam == null) return;

            Ray mouseRay = keypadCam.ScreenPointToRay(Input.mousePosition);
            RaycastHit mouseHit;

            if (Physics.Raycast(mouseRay, out mouseHit, 10f, keypadLayer))
            {
                if (mouseHit.collider.TryGetComponent(out KeypadButton keypadButton))
                {
                    keypadButton.PressButton();
                }
            }
        }

        private void HandleKeyboardInput()
        {
            if (activeKeypad == null) return;

            for (int i = 0; i <= 9; i++)
            {
                if (Input.GetKeyDown(i.ToString()) || Input.GetKeyDown(KeyCode.Keypad0 + i))
                {
                    activeKeypad.AddInput(i.ToString());
                }
            }

            if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
            {
                activeKeypad.AddInput("enter");
            }
        }
    }
}