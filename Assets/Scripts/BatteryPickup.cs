using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class BatteryPickup : MonoBehaviour
{
    public GameObject objectToDestroy; // Object to be destroyed (e.g., air wall)
    public GameObject objectToDestroy2; // Additional object to be destroyed
    public GameObject batteryObject; // Object to be displayed (tagged as "Battery")
    public GameObject uiCanvas; // UI Canvas to be shown
    public Text uiText; // UI text
    public Image uiImage; // UI image to be shown on second interaction
    public AudioClip pickupSound; // Sound effect to play
    public InputActionReference hideUIAction; // Input action to hide UI

    private AudioSource audioSource;
    private bool isPickedUp = false; // Whether the battery has been picked up
    private ToggleUIWithButton uiToggleScript; // UI toggle script

    private void Start()
    {
        // Hide UI by default
        if (uiCanvas != null)
        {
            uiCanvas.SetActive(false);
        }

        if (uiImage != null)
        {
            uiImage.gameObject.SetActive(false);
        }

        // Get or add an audio source
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // Ensure the object tagged "Battery" is hidden initially
        if (batteryObject != null && batteryObject.CompareTag("Battery"))
        {
            batteryObject.SetActive(false);
        }

        // Listen to the input action
        hideUIAction.action.performed += ctx => HideUI();

        // Get UI toggle script
        uiToggleScript = FindObjectOfType<ToggleUIWithButton>();
    }

    private void OnTriggerEnter(Collider other)
    {
        // Player (hand) touches Battery1
        //if ((other.CompareTag("Left Hand") || other.CompareTag("Right Hand")) && !isPickedUp)
        //{
            isPickedUp = true;
            //Debug.Log("Player picked up Battery1");

            // Show UI and update text
            if (uiCanvas != null && uiText != null)
            {
                uiCanvas.SetActive(true);
                uiText.text = "This is a radio battery! I need to insert it into my left-hand receiver.";
                //Debug.Log("UI displayed and text updated: " + uiText.text);
            }

            // Destroy specified objects
            if (objectToDestroy != null)
            {
                //Debug.Log("Destroying: " + objectToDestroy.name);
                Destroy(objectToDestroy);
            }

            if (objectToDestroy2 != null)
            {
                //Debug.Log("Destroying additional object: " + objectToDestroy2.name);
                Destroy(objectToDestroy2);
            }
        //}

        // Battery1 touches Detection
        if (other.CompareTag("Detection") && isPickedUp)
        {
            Debug.Log("Battery1 touched Detection");

            // Show the object tagged as "Battery"
            if (batteryObject != null && batteryObject.CompareTag("Battery"))
            {
                batteryObject.SetActive(true);
                //Debug.Log("Battery object is now visible: " + batteryObject.name);
            }

            // Play sound effect
            if (pickupSound != null)
            {
                audioSource.PlayOneShot(pickupSound);
                Debug.Log("Playing sound: " + pickupSound.name);
            }

            // Destroy Battery1 immediately
            Destroy(gameObject);
            //Debug.Log("Battery1 destroyed");

            // Update UI text and show image
            if (uiCanvas != null && uiText != null && uiImage != null)
            {
                uiText.text = "Now I need to tune the frequency... Tactical radio communication frequency is around 150 to 174MHz. (Press right-hand Trigger)";
                uiImage.gameObject.SetActive(true);
                Debug.Log("UI text updated and image displayed");
            }

            // Enable UI toggle interaction
            if (uiToggleScript != null)
            {
                uiToggleScript.EnableUIToggle();
            }
        }
    }

    private void HideUI()
    {
        if (uiCanvas != null)
        {
            uiCanvas.SetActive(false);
            Debug.Log("UI hidden");
        }
    }

    private void OnDestroy()
    {
        // Unsubscribe from input action to prevent errors
        hideUIAction.action.performed -= ctx => HideUI();
    }
}
