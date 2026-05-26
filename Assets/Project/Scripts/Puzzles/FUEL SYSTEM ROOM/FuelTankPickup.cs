using UnityEngine;

public class FuelTankPickup : MonoBehaviour
{
    public Transform holdPoint;
    public float interactRange = 4f;
    public GameObject fuelTankPrefab;

    public FuelSystemManager systemManager;

    private GameObject heldTank;

    void Start()
    {
        if (FuelInventory.Instance == null)
        {
            GameObject inv = new GameObject("FuelInventory");
            inv.AddComponent<FuelInventory>();
        }

        if (FuelInventory.Instance.carryingTank && fuelTankPrefab != null && holdPoint != null)
        {
            heldTank = Instantiate(fuelTankPrefab, holdPoint);
            heldTank.transform.localPosition = Vector3.zero;
            heldTank.transform.localRotation = Quaternion.identity;
            ConfigureHeld(heldTank);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (heldTank == null)
                PickUpNearestTank();
            else
                PlaceInNearestSlot();
        }
        else if (Input.GetKeyDown(KeyCode.O) && heldTank != null)
        {
            DropHeldTank();
        }
    }

    void PickUpNearestTank()
    {
        GameObject[] tanks = GameObject.FindGameObjectsWithTag("FuelTank");

        foreach (GameObject tank in tanks)
        {
            float distance = Vector3.Distance(transform.position, tank.transform.position);

            if (distance <= interactRange)
            {
                heldTank = tank;
                heldTank.transform.SetParent(holdPoint);
                heldTank.transform.localPosition = Vector3.zero;
                heldTank.transform.localRotation = Quaternion.identity;
                ConfigureHeld(heldTank);

                if (FuelInventory.Instance != null)
                    FuelInventory.Instance.PickUp();

                return;
            }
        }
    }

    void PlaceInNearestSlot()
    {
        GameObject[] slots = GameObject.FindGameObjectsWithTag("FuelSlot");

        foreach (GameObject slot in slots)
        {
            float distance = Vector3.Distance(transform.position, slot.transform.position);

            if (distance <= interactRange)
            {
                heldTank.transform.SetParent(null);
                heldTank.transform.position = slot.transform.position;
                heldTank.transform.rotation = slot.transform.rotation;

                // re-enable collider so the placed tank is solid in the world,
                // but keep kinematic so it doesn't fall through floor
                Collider c = heldTank.GetComponent<Collider>();
                if (c != null) c.enabled = true;

                heldTank = null;
                slot.tag = "Untagged";

                if (FuelInventory.Instance != null)
                    FuelInventory.Instance.PlaceInGenerator();

                if (systemManager != null)
                    systemManager.OnTankPlaced();

                return;
            }
        }
    }

    void DropHeldTank()
    {
        heldTank.transform.SetParent(null);

        Rigidbody rb = heldTank.GetComponent<Rigidbody>();
        if (rb != null) rb.isKinematic = false;

        Collider c = heldTank.GetComponent<Collider>();
        if (c != null) c.enabled = true;

        heldTank.tag = "FuelTank";
        heldTank = null;

        if (FuelInventory.Instance != null)
            FuelInventory.Instance.Drop();
    }

    // Disable Collider while carrying so the tank doesn't push against the
    // player's CharacterController and block forward movement.
    void ConfigureHeld(GameObject tank)
    {
        Rigidbody rb = tank.GetComponent<Rigidbody>();
        if (rb != null) rb.isKinematic = true;

        Collider c = tank.GetComponent<Collider>();
        if (c != null) c.enabled = false;

        tank.tag = "Untagged";
    }
}
