using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Playerlogic : MonoBehaviour
{
    // CharacterController component attached to the Player, responsible for Player's Movement
    CharacterController m_characterController = null;

    // Constant parameters for movement, jumping and gravity
    const float MOVEMENT_SPEED = 5.0f;
    const float JUMP_HEIGHT = 0.4f;
    const float GRAVITY = 0.04f;
    const float GRAVITY_MULTIPLIER = 1.5f;

    // Player input
    float m_horizontalInput = 0.0f;
    float m_verticalInput = 0.0f;

    // Movement vectors to store Player Input
    Vector3 m_movementInput = Vector3.zero;
    Vector3 m_movement = Vector3.zero;
    Vector3 m_heightMovement = Vector3.zero;

    // Trigger a jump action in next FixedUpdate
    bool m_jump = false;




    [SerializeField] GameObject WeaponEquipmentPosition;
    GameObject interactiveObject = null;
    GameObject EquipedObject = null;

    [SerializeField] Text HealthUIText;
    [SerializeField] Text AmmoUIText;
    
    int MaxHealth = 100;
    int CurrentHealth;
    

    // Start is called before the first frame update
    void Start()
    {
        // Access the Character Controller component attached to the GameObject that this script is attached to
        m_characterController = GetComponent<CharacterController>();
        CurrentHealth = MaxHealth;
        UpdateHealthUI();
    }
    void RotateCharacterTowardsMouseCursor()
    {
        Vector3 mousePos = Input.mousePosition;
        Vector3 playerPos = Camera.main.WorldToScreenPoint(transform.position);
        Vector3 direction = mousePos - playerPos;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(-angle + 90, Vector3.up);
    }
    void UpdatePlayerInput()
    {
        // Store Horizontal and Vertical raw input values
        m_horizontalInput = Input.GetAxisRaw("Horizontal");
        m_verticalInput = Input.GetAxisRaw("Vertical");

        // Store Input values in MovementInput vector
        m_movementInput = new Vector3(m_horizontalInput, 0, m_verticalInput);

        // If the player is not jumping, is standing on the ground
        if (!m_jump && m_heightMovement.y == -GRAVITY && Input.GetButtonDown("Jump"))
        {
            m_jump = true;
        }
    }

    void UpdatePlayerMovement()
    {
        // Multiply the Movement Input vector with the Movement Speed constant and with the deltaTime (time that passed since last frame)
        m_movement = m_movementInput.normalized * MOVEMENT_SPEED * Time.deltaTime;

        // If the character is not standing on the ground apply gravity
        if (m_characterController && !m_characterController.isGrounded)
        {
            // If the character is jumping and going upwards we apply the default Gravity value
            if (m_heightMovement.y > 0)
            {
                m_heightMovement.y -= GRAVITY;
            }
            // If the character is jumping and going downwards (falling) we apply an increased Gravity value (similar to how Super Mario games do it)
            else
            {
                m_heightMovement.y -= GRAVITY * GRAVITY_MULTIPLIER;
            }
        }
        // If the character is standing on the ground set the heightMovement to -Gravity, this to ensure that the CharacterController can detect when the player is grounded
        else
        {
            m_heightMovement.y = -GRAVITY;
        }

        // Set the HeightMovement vector to the JumpHeight constant and mark it that the jump action has been completed
        if (m_jump)
        {
            m_heightMovement.y = JUMP_HEIGHT;
            m_jump = false;
        }

        // Apply the Movement and HeightMovement to the CharacterController
        if (m_characterController)
        {
            m_characterController.Move(m_movement + m_heightMovement);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Calculate the Player's Input during the Update function (for highest accuracy)
        UpdatePlayerInput();
        UpdateHealthUI();
        
            if (interactiveObject && Input.GetButtonDown("Fire2")) //if interactable object exists and player presses button
            {
                if (!EquipedObject)
                { 
                    GunLogic gun = interactiveObject.GetComponent<GunLogic>();
                   
                    if (gun)
                    {

                        //Sets the Position Of the Gun
                        interactiveObject.transform.position = WeaponEquipmentPosition.transform.position;
                        interactiveObject.transform.rotation = WeaponEquipmentPosition.transform.rotation;
                        
                        interactiveObject.transform.parent = WeaponEquipmentPosition.transform.parent;

                        // Deactivates Gravity and Collider
                        gun.EquipGun();


                        EquipedObject = interactiveObject;
                        gun.SetAmmoText();

                    }
                }
                else if (EquipedObject) // if the player has the weapon equiped
                {
                    

                   GunLogic gun = EquipedObject.GetComponent<GunLogic>(); // get the gunlogic
                   if (gun)                                              // if gun logic exists
                   {
                   
                    //sets the position of the equiped weqpon
                    EquipedObject.transform.parent = null;


                    // Activates Gravity and Collider
                    gun.UnequipGun();
                    gun.ClearAmmoText();
                    EquipedObject = null;
                   }

                }
            }
       

    }

    void FixedUpdate()
    {
        // Calculate the Player's Movement during the FixedUpdate function (so the physics perform constantly using a constant fixed Physics framerate)
        UpdatePlayerMovement();
        RotateCharacterTowardsMouseCursor();
    }



   public void TakeDamage(int Damage)
    {
        CurrentHealth -= Damage;

        if(CurrentHealth < 0)
        {
            SceneManager.LoadScene(0);
        }
    }

    void UpdateHealthUI()
    {
        HealthUIText.text = $"Health: {CurrentHealth}";
    }


   

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Weapon")
        {
            interactiveObject = other.gameObject;
            Debug.Log("You can pick this up!!");
        }
        if(other.tag == "Damage Zone")
        {
            TakeDamage(20);
            UpdateHealthUI();
        }
    }
    void OnTriggerExit(Collider other)
    {

        if (other.tag == "Weapon" && interactiveObject.gameObject == other.gameObject)
        {
            interactiveObject = null;

            if(interactiveObject == null)
            {
                Debug.Log("Interactable object is null");
            }
        }

    }
}

    

