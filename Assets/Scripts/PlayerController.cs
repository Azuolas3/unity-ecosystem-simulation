using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace EcosystemSimulation
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField]
        private CharacterController characterController;
        [SerializeField]
        private GameObject mapGenerationSettings;
        //[SerializeField]
        //private Canvas uiCanvas;

        [SerializeField]
        GameObject sliderPrefab;

        private GameObject currentUIBar;
        private TMP_Text currentText;
        private Animal currentSelectedAnimal;

        //TMP_Text textField;

        private const float mouseSensitivity = 5f;
        private const float movementSpeed = 15f;


        private float xRotation = 0f;
        private float yRotation = 0f;

        private bool isMovementPaused = true;

        // Update is called once per frame
        void Update()
        {
            if(Input.GetKeyDown(KeyCode.Escape))
            {
                isMovementPaused = !isMovementPaused;
                mapGenerationSettings.SetActive(!mapGenerationSettings.activeSelf);
            }

            if(!isMovementPaused)
            {
                CameraRotation();
                CharacterMovement();
                SelectWithMouse();
            }

            if(currentSelectedAnimal != null)
                UpdateAnimalInfoBar(currentSelectedAnimal, currentText);
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
            float currentSpeed = movementSpeed;
            float downwardsMovement = 0;

            if (Input.GetKey(KeyCode.LeftControl))
                downwardsMovement = 1f;

            float moveX = Input.GetAxis("Horizontal");
            float moveZ = Input.GetAxis("Vertical");
            float moveY = Input.GetAxis("Jump") - downwardsMovement;

            if (Input.GetAxis("Fire3") == 1)
                currentSpeed *= 2;

            Vector3 movement = transform.right * moveX + transform.forward * moveZ + Vector3.up * moveY; //using global up instead of local
            characterController.Move(movement * currentSpeed * Time.deltaTime);
        }

        void SelectWithMouse()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;

            if (Input.GetMouseButtonDown(0))
            {
                Debug.Log("spaudziam lol");
                if(Physics.Raycast(ray, out hitInfo, Mathf.Infinity))
                {
                    Debug.Log("kazka paspaudem lol  " + hitInfo.collider + " " + hitInfo.collider.gameObject);
                    if (hitInfo.collider.GetComponent<Animal>() != null)
                    {
                        currentSelectedAnimal = hitInfo.collider.GetComponent<Animal>();
                        if(currentUIBar == null)
                            currentUIBar = Instantiate(sliderPrefab);

                        currentText = currentUIBar.GetComponentInChildren<TMP_Text>();
                        UpdateAnimalInfoBar(currentSelectedAnimal, currentText);
                        Debug.Log($"selected {currentSelectedAnimal.name} {currentSelectedAnimal.Hydration}");
                    }
                    else
                    {
                        if(currentUIBar != null)
                            Destroy(currentUIBar);
                        currentSelectedAnimal = null;
                    }
                }
            
            }
        }

        //void HandleMenu()
        //{
        //    if (Input.GetKeyDown(KeyCode.Escape))
        //    {
                
        //    }
        //}

        void UpdateAnimalInfoBar(Animal animal, TMP_Text textField)
        {
            Debug.Log(textField);
            textField.alignment = TextAlignmentOptions.Center;
            string healthString = $"Health: {(int)animal.Health}";
            string genderString = $"Gender: {animal.genderHandler.Gender}";
            string nourishmentString = $"Nourishment: {(int)animal.Nourishment}";
            string hydrationString = $"Hydration: {(int)animal.Hydration}";
            string actionString = $"Currently {animal.currentAction}";
            textField.text = $"{healthString}   {genderString}\n{nourishmentString}\n{hydrationString}\n{actionString}";
        }
    }
}
