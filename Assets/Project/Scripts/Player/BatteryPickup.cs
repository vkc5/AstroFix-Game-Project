using UnityEngine;
using TMPro;
using System.Collections;

public class BatteryPickup : MonoBehaviour
{
    public Transform holdPoint;
    public float interactRange = 4f;
    public TMP_Text batteryText;
    public int totalBatteries = 3;

    public PowerLightingManager lightingManager;
    public PowerGlowManager glowManager;

    [Header("Animation")]
    public Animator playerAnimator;
    public float pickupAnimationDelay = 1.2f;

    [Header("HUD")]
    public GameObject carryingBatteryHUD;

    [Header("Audio")]
    public AudioSource powerRestoredSound;

    [Header("Puzzle Unlock")]
    public GameObject puzzleHUD;
    public MemoryGridPuzzle memoryPuzzle;

    private GameObject heldBattery;
    private int placedBatteries = 0;
    private bool powerMessageShown = false;
    private bool isPickingUp = false;

    public bool nearMonitor = false;

    public bool HasAllBatteries()
    {
        return placedBatteries >= totalBatteries;
    }

    void Start()
    {
        UpdateBatteryText();

        if (carryingBatteryHUD != null)
            carryingBatteryHUD.SetActive(false);

        if (puzzleHUD != null)
            puzzleHUD.SetActive(false);

        if (memoryPuzzle != null)
            memoryPuzzle.SetPuzzleUnlocked(false);
    }

    void Update()
    {
        if (nearMonitor) return;
        if (isPickingUp) return;

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (heldBattery == null)
                PickUpNearestBattery();
            else
                PlaceInNearestSlot();
        }
    }

    void PickUpNearestBattery()
    {
        GameObject[] batteries = GameObject.FindGameObjectsWithTag("Battery");

        foreach (GameObject battery in batteries)
        {
            float distance = Vector3.Distance(transform.position, battery.transform.position);

            if (distance <= interactRange)
            {
                StartCoroutine(PickupBatteryRoutine(battery));
                return;
            }
        }
    }

    IEnumerator PickupBatteryRoutine(GameObject battery)
    {
        isPickingUp = true;

        if (playerAnimator != null)
            playerAnimator.SetTrigger("PickUp");

        yield return new WaitForSeconds(pickupAnimationDelay);

        heldBattery = battery;

        heldBattery.transform.SetParent(holdPoint);
        heldBattery.transform.localPosition = Vector3.zero;
        heldBattery.transform.localRotation = Quaternion.identity;

        Rigidbody rb = heldBattery.GetComponent<Rigidbody>();
        if (rb != null)
            rb.isKinematic = true;

        Renderer[] renderers = heldBattery.GetComponentsInChildren<Renderer>();
        foreach (Renderer r in renderers)
            r.enabled = false;

        Collider[] colliders = heldBattery.GetComponentsInChildren<Collider>();
        foreach (Collider c in colliders)
            c.enabled = false;

        if (carryingBatteryHUD != null)
            carryingBatteryHUD.SetActive(true);

        isPickingUp = false;
    }

    void PlaceInNearestSlot()
    {
        GameObject[] slots = GameObject.FindGameObjectsWithTag("BatterySlot");

        foreach (GameObject slot in slots)
        {
            float distance = Vector3.Distance(transform.position, slot.transform.position);

            if (distance <= interactRange)
            {
                heldBattery.transform.SetParent(null);
                heldBattery.transform.position = slot.transform.position;
                heldBattery.transform.rotation = slot.transform.rotation;
                heldBattery.tag = "Untagged";

                Renderer[] renderers = heldBattery.GetComponentsInChildren<Renderer>();
                foreach (Renderer r in renderers)
                    r.enabled = true;

                heldBattery = null;

                if (carryingBatteryHUD != null)
                    carryingBatteryHUD.SetActive(false);

                if (placedBatteries < totalBatteries)
                    placedBatteries++;

                UpdateBatteryText();

                if (placedBatteries >= totalBatteries && !powerMessageShown)
                {
                    powerMessageShown = true;

                    if (carryingBatteryHUD != null)
                        carryingBatteryHUD.SetActive(false);

                    if (powerRestoredSound != null)
                        powerRestoredSound.Play();

                    if (lightingManager != null)
                        lightingManager.TurnOnPowerLights();

                    if (glowManager != null)
                        glowManager.TurnOnGlow();

                    if (puzzleHUD != null)
                        puzzleHUD.SetActive(true);

                    if (memoryPuzzle != null)
                        memoryPuzzle.SetPuzzleUnlocked(true);

                    StartCoroutine(ShowPowerRestoredMessage());
                }

                return;
            }
        }
    }

    void UpdateBatteryText()
    {
        if (batteryText != null)
            batteryText.text = "Batteries: " + placedBatteries + " / " + totalBatteries;
    }

    IEnumerator ShowPowerRestoredMessage()
    {
        if (batteryText != null)
        {
            batteryText.gameObject.SetActive(true);
            batteryText.text = "Power restored!";

            yield return new WaitForSeconds(5f);

            batteryText.gameObject.SetActive(false);
        }
    }
}