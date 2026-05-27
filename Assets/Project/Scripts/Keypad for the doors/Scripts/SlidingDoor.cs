using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NavKeypad
{
    public class SlidingDoor : MonoBehaviour
    {
        [SerializeField] private Animator anim;

        // The door starts locked. It won't open until the keypad unlocks it.
        [SerializeField] private bool isLocked = true;

        public bool IsOpen => isOpen;
        private bool isOpen = false;

        // Proximity trigger (Optional: if you want it to also check proximity AFTER unlocking)
        private void OnTriggerEnter(Collider other)
        {
            if (isLocked) return;

            if (other.CompareTag("Player"))
            {
                OpenDoor();
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                CloseDoor();
            }
        }

        public void ToggleDoor()
        {
            isOpen = !isOpen;
            anim.SetBool("isOpen", isOpen);
        }

        public void OpenDoor()
        {
            isOpen = true;
            // Triggers the Animator parameter to play the opening animation
            anim.SetBool("isOpen", isOpen);
        }

        public void CloseDoor()
        {
            isOpen = false;
            anim.SetBool("isOpen", isOpen);
        }

        /// <summary>
        /// This is the function called by the Keypad when the password is CORRECT.
        /// It unlocks the door and triggers the animation instantly.
        /// </summary>
        public void UnlockAndOpen()
        {
            isLocked = false; // 1. Remove the lock
            OpenDoor();       // 2. Play the animation immediately via the OpenDoor function
        }
    }
}