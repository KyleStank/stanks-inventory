using UnityEngine;
using System.Collections;

namespace KStank.stanks_inventory.Example {
    /// <summary>
    /// Basic player movment used for the example scene.
    /// </summary>
    public sealed class PlayerMovement : MonoBehaviour {
        //Public movement variables
        public float moveSpeed = 5.0f;

        //Private movement variables
        CharacterController controller;

        void Awake() {
            controller = GetComponent<CharacterController>();

            //If controller is still somehow missing, even though the script requires it, add it
            if(controller == null) {
                gameObject.AddComponent<CharacterController>();

                //Assign character controller
                controller = GetComponent<CharacterController>();
            }
        }

        void Update() {
            //Get input from user
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");
            Vector3 speed = new Vector3(horizontal * moveSpeed, 0.0f, vertical * moveSpeed);

            //Move player
            controller.SimpleMove(speed);
        }
    }
}
