using System.Collections;
using UnityEngine;
using TMPro;

public class FuelSystemManager : MonoBehaviour
{
    public FuelPuzzleLevel levelController;
    public TMP_Text fuelHUDText;

    public PowerGlowManager glowManager;
    public PowerLightingManager lightingManager;

    IEnumerator Start()
    {
        // wait one frame so FuelInventory.Awake on any GameObject (or the
        // auto-created one from FuelTankPickup.Start) has run before we read it
        yield return null;
        UpdateHUD();
    }

    public void OnTankPlaced()
    {
        UpdateHUD();

        if (glowManager != null)
            glowManager.TurnOnGlow();

        if (FuelInventory.Instance != null && FuelInventory.Instance.HasAllTanks())
        {
            if (lightingManager != null)
                lightingManager.TurnOnPowerLights();

            if (levelController != null)
                levelController.PuzzleSolved();
        }
    }

    void UpdateHUD()
    {
        if (fuelHUDText == null || FuelInventory.Instance == null) return;
        fuelHUDText.text = "Fuel Generators: " + FuelInventory.Instance.placedTanks + " / " + FuelInventory.totalTanks;
    }
}
