using UnityEngine;

public class FuelInventory : MonoBehaviour
{
    public static FuelInventory Instance;

    public bool carryingTank = false;
    public int placedTanks = 0;
    public const int totalTanks = 4;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void PickUp()
    {
        carryingTank = true;
    }

    public void Drop()
    {
        carryingTank = false;
    }

    public void PlaceInGenerator()
    {
        carryingTank = false;
        placedTanks++;
    }

    public bool HasAllTanks()
    {
        return placedTanks >= totalTanks;
    }

    public void ResetInventory()
    {
        carryingTank = false;
        placedTanks = 0;
    }
}
