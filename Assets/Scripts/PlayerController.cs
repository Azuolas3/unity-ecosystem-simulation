using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private CharacterController characterController;

    private const float mouseSensitivity = 5f;
    private const float movementSpeed = 15f;


    private float xRotation = 0f;
    private float yRotation = 0f;

    private bool isMovementPaused;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            isMovementPaused = !isMovementPaused;
        }

        if(!isMovementPaused)
        {
            CameraRotation();
            CharacterMovement();
        }
    }

    void CameraRotation()
    {
        yRotation += Input.GetAxis("Mouse X") * mouseSensitivity; //* Time.deltaTime;
        xRotation += Input.GetAxis("Mouse Y") * -1 * mouseSensitivity; // * Time.deltaTime;

        xRotation = Mathf.Clamp(xRotation, -90, 90);

        transform.localEulerAngles = new Vector3(xRotation, yRotation, 0);
    }

    void CharacterMovement()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");
        float moveY = Input.GetAxis("Jump") - Input.GetAxis("Fire3");

        Vector3 movement = transform.right * moveX + transform.forward * moveZ + Vector3.up * moveY; //using global up instead of local
        characterController.Move(movement * movementSpeed * Time.deltaTime);
    }
}
