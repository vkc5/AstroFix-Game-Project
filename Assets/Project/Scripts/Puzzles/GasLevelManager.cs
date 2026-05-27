using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GasLevelManager : MonoBehaviour
{
    [Header("Player / Monster")]
    public PlayerMovement playerMovement;
    public MonoBehaviour monsterMovement;

    [Header("Gas Boxes")]
    public GameObject[] gasBoxes;
    public GameObject[] gasBoxHUDs;

    [Header("Gas Tanks")]
    public Transform[] gasTanks;
    public GameObject[] tankHUDs;
    public Text[] tankPercentTexts;

    [Header("Interaction")]
    public Transform player;
    public float interactRange = 3f;
    public float fillTime = 10f;

    [Header("Animation")]
    public Animator playerAnimator;
    public string pickupTriggerName = "PickUp";
    public float pickupDelay = 1.2f;

    [Header("Win")]
    public AudioSource victorySound;

    [Header("Tank Complete Sound")]
    public AudioSource tankCompleteSound;

    [Header("Win Popup")]
    public GameObject winPopup;

    private int currentIndex = 0;
    private bool carryingGasBox = false;
    private bool isPickingUp = false;
    private bool levelComplete = false;

    private float[] tankProgress;

    void Start()
    {
        tankProgress = new float[gasTanks.Length];

        for (int i = 0; i < gasBoxes.Length; i++)
        {
            gasBoxes[i].SetActive(true);

            if (gasBoxHUDs[i] != null)
                gasBoxHUDs[i].SetActive(i == 0);

            if (tankHUDs[i] != null)
                tankHUDs[i].SetActive(false);

            UpdateTankText(i);
        }
    }

    void Update()
    {
        if (levelComplete || isPickingUp)
            return;

        HandleGasBoxPickup();
        HandleTankFilling();
    }

    void HandleGasBoxPickup()
    {
        if (carryingGasBox)
            return;

        if (!Input.GetKeyDown(KeyCode.E))
            return;

        if (currentIndex >= gasBoxes.Length)
            return;

        float distance = Vector3.Distance(player.position, gasBoxes[currentIndex].transform.position);

        if (distance <= interactRange)
        {
            StartCoroutine(PickupGasBoxRoutine());
        }
    }

    IEnumerator PickupGasBoxRoutine()
    {
        isPickingUp = true;

        if (playerAnimator != null)
            playerAnimator.SetTrigger(pickupTriggerName);

        yield return new WaitForSeconds(pickupDelay);

        gasBoxes[currentIndex].SetActive(false);

        if (gasBoxHUDs[currentIndex] != null)
            gasBoxHUDs[currentIndex].SetActive(false);

        if (tankHUDs[currentIndex] != null)
            tankHUDs[currentIndex].SetActive(true);

        carryingGasBox = true;
        isPickingUp = false;
    }

    void HandleTankFilling()
    {
        if (!carryingGasBox)
            return;

        if (currentIndex >= gasTanks.Length)
            return;

        float distance = Vector3.Distance(player.position, gasTanks[currentIndex].position);

        if (distance > interactRange)
            return;

        if (Input.GetKey(KeyCode.E))
        {
            tankProgress[currentIndex] += Time.deltaTime / fillTime;
            tankProgress[currentIndex] = Mathf.Clamp01(tankProgress[currentIndex]);

            UpdateTankText(currentIndex);

            if (tankProgress[currentIndex] >= 1f)
            {
                CompleteCurrentTank();
            }
        }
    }

    void CompleteCurrentTank()
    {
        if (tankCompleteSound != null)
            tankCompleteSound.Play();

        if (tankHUDs[currentIndex] != null)
            tankHUDs[currentIndex].SetActive(false);

        carryingGasBox = false;
        currentIndex++;

        if (currentIndex >= gasTanks.Length)
        {
            StartCoroutine(LevelCompleteRoutine());
            return;
        }

        if (gasBoxHUDs[currentIndex] != null)
            gasBoxHUDs[currentIndex].SetActive(true);
    }

    void UpdateTankText(int index)
    {
        if (tankPercentTexts[index] != null)
        {
            int percent = Mathf.RoundToInt(tankProgress[index] * 100f);
            tankPercentTexts[index].text = percent + "%";
        }
    }

    IEnumerator LevelCompleteRoutine()
    {
        levelComplete = true;

        if (playerMovement != null)
            playerMovement.canMove = false;

        if (monsterMovement != null)
            monsterMovement.enabled = false;

        if (victorySound != null)
            victorySound.Play();

        if (winPopup != null)
        {
            winPopup.SetActive(true);

            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }

        yield return new WaitForSeconds(3f);

        GameProgressManager.Instance.CompleteStep(4);
    }
}