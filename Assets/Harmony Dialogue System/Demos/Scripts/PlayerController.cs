using UnityEngine;

namespace HarmonyDialogueSystem.Demo
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] float moveSpeed = 50;
        [SerializeField] float smoothMoveTime = .1f;
        [SerializeField] float turnSpeed = 8;

        private Rigidbody playerRb;

        float angle;
        Vector3 velocity;
        float smoothInputMagnitude;
        float smoothMoveVelocity;

        public bool stopMovement { get; private set; }

        // Start is called before the first frame update
        void Start()
        {
            playerRb = GetComponent<Rigidbody>();
            stopMovement = false;
        }

        // Update is called once per frame
        void Update()
        {
            //Gets the Horizontal and Vertical Axis of the Player
            float horizontalInput = Input.GetAxisRaw("Horizontal");
            float verticalInput = Input.GetAxisRaw("Vertical");
            Vector3 moveDir = Vector3.zero;

            if (!stopMovement && !DialogueManager.instance.dialogueIsPlaying)
            {

                moveDir = new Vector3(horizontalInput, 0, verticalInput);

                //Gets the magnitude of the direction the player is moving
                float inputMagnitude = moveDir.magnitude;
                //Smothens the player's movement
                smoothInputMagnitude = Mathf.SmoothDamp(smoothInputMagnitude, inputMagnitude, ref smoothMoveVelocity, smoothMoveTime);

                float inputAngle = Mathf.Atan2(moveDir.x, moveDir.z) * Mathf.Rad2Deg;

                angle = Mathf.LerpAngle(angle, inputAngle, turnSpeed * Time.deltaTime * inputMagnitude);
                velocity = moveSpeed * smoothInputMagnitude * transform.forward;
            }
            //transform.Translate(transform.forward * moveSpeed * Time.deltaTime * smoothInputMagnitude, Space.World);
        }

        private void FixedUpdate()
        {
            if (!stopMovement && !DialogueManager.instance.dialogueIsPlaying)
            {
                //Moves the rigidbody of the player
                playerRb.MoveRotation(Quaternion.Euler(Vector3.up * angle));
                playerRb.MovePosition(playerRb.position + velocity * Time.deltaTime);
            }
        }

        private void LockMovement()
        {
            stopMovement = true;
        }

        private void UnlockMovement()
        {
            stopMovement = false;
        }
    }
}
