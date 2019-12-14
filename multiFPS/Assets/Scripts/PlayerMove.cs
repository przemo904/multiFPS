using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour {


    [SerializeField] private string horizontalInputName, verticalInputName;
    private float movementSpeed;

    [SerializeField] private float walkSpeed, runSpeed;
    [SerializeField] private float runBuildUpSpeed;
    [SerializeField] private KeyCode runKey;

    [SerializeField] private float slopeForce;
    [SerializeField] private float slopeForceRayLenght;

    [SerializeField] private AnimationCurve jumpFallOff;
    [SerializeField] private float jumpMultiplier;
    [SerializeField] private KeyCode jumpKey;

    private CharacterController charController;
    private bool isJumping;
    


    private void Awake()
    {
        charController = GetComponent<CharacterController>();
    }

    void Update () {
        PlayerMovement();

	}

    private void PlayerMovement()
    {
        float vertInput = Input.GetAxis(verticalInputName);
        float horiInput = Input.GetAxis(horizontalInputName);

        Vector3 forwardMovement = transform.forward * vertInput;
        Vector3 rightMovement = transform.right * horiInput;

        charController.SimpleMove(Vector3.ClampMagnitude(forwardMovement + rightMovement,1.0f)*movementSpeed);

        if ((vertInput != 0 || horiInput != 0) && onSlope()) {
            charController.Move(Vector3.down * charController.height / 2 * slopeForce * Time.deltaTime);

        }
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, 100f)&& isJumping == false) { if (hit.distance > charController.height) {
                Debug.Log("air");
                charController.Move(Vector3.ClampMagnitude(forwardMovement + rightMovement, 1.0f) * Time.deltaTime * walkSpeed / 2);
            } }


        SetMovementSpeed();
        JumpInput();
    }

    private bool onSlope() {
        if (isJumping) return false;

        RaycastHit hit;

        if (Physics.Raycast(transform.position, Vector3.down, out hit, charController.height / 2 * slopeForceRayLenght)) {
            if (hit.normal != Vector3.up) return true;
        }
        return false;
    }
    private void JumpInput()
    {
        if (Input.GetKeyDown(jumpKey) && !isJumping)
        {
            isJumping = true;
            StartCoroutine(JumpEvent());
        }

    }

    private void SetMovementSpeed() {
        if (Input.GetKey(runKey))
        {
            movementSpeed = Mathf.Lerp(movementSpeed, runSpeed, Time.deltaTime * runBuildUpSpeed);
        }
        else {
            movementSpeed = Mathf.Lerp(movementSpeed, walkSpeed, Time.deltaTime * runBuildUpSpeed);
        }
    }

    private IEnumerator JumpEvent() 
    {
        charController.slopeLimit = 90.0f;
        float timeInAir = 0.0f;


        do
        {

            float vertInput = Input.GetAxis(verticalInputName);
            float horiInput = Input.GetAxis(horizontalInputName);

            Vector3 forwardMovement = transform.forward * vertInput;
            Vector3 rightMovement = transform.right * horiInput;



            float jumpForce = jumpFallOff.Evaluate(timeInAir);
            charController.Move(Vector3.up * jumpForce * jumpMultiplier * Time.deltaTime);

            charController.Move(Vector3.ClampMagnitude(forwardMovement + rightMovement, 1.0f) * Time.deltaTime * walkSpeed/2);

            timeInAir += Time.deltaTime;

            yield return null;
        } while (!charController.isGrounded && charController.collisionFlags != CollisionFlags.Above);

        charController.slopeLimit = 45.0f;
        isJumping = false;
    }
}
