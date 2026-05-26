using UnityEngine;
using System.Collections;

public class KeyItem : MonoBehaviour
{
    public string letter = "T";
    public GameObject interactText;

    [Header("Player Animation")]
    public Animator playerAnimator;
    public string pickupTriggerName = "PickUp";
    public float pickupAnimationDelay = 1.2f;

    [Header("Collect Animation")]
    public Transform collectTarget;
    public float moveDuration = 0.45f;

    [Header("Final Hand Position")]
    public Vector3 handLocalPosition = Vector3.zero;
    public Vector3 handLocalRotation = new Vector3(90f, 0f, 90f);
    public Vector3 handLocalScale = new Vector3(0.0001f, 0.0001f, 0.0001f);

    private bool playerNear = false;
    private bool collected = false;
    private PlayerKeyInventory playerInventory;
    private Collider keyCollider;

    private void Start()
    {
        keyCollider = GetComponent<Collider>();

        if (interactText != null)
            interactText.SetActive(false);
    }

    private void Update()
    {
        if (playerNear && !collected && Input.GetKeyDown(KeyCode.E))
        {
            if (playerInventory == null)
            {
                Debug.LogWarning("Player inventory missing.");
                return;
            }

            StartCoroutine(CollectKeyRoutine());
        }
    }

    private IEnumerator CollectKeyRoutine()
    {
        collected = true;

        if (interactText != null)
            interactText.SetActive(false);

        if (keyCollider != null)
            keyCollider.enabled = false;

        if (playerAnimator != null)
            playerAnimator.SetTrigger(pickupTriggerName);

        yield return new WaitForSeconds(pickupAnimationDelay);

        playerInventory.AddKey(letter);

        yield return StartCoroutine(AnimateToHand());

        gameObject.SetActive(false);
    }

    private IEnumerator AnimateToHand()
    {
        if (collectTarget == null)
        {
            Debug.LogWarning("Collect Target is missing.");
            yield break;
        }

        Vector3 startPosition = transform.position;
        Quaternion startRotation = transform.rotation;

        Vector3 endPosition = collectTarget.position;
        Quaternion endRotation = collectTarget.rotation;

        float timer = 0f;

        while (timer < moveDuration)
        {
            timer += Time.deltaTime;
            float t = timer / moveDuration;

            transform.position = Vector3.Lerp(startPosition, endPosition, t);
            transform.rotation = Quaternion.Slerp(startRotation, endRotation, t);

            yield return null;
        }

        transform.SetParent(collectTarget, false);
        transform.localPosition = handLocalPosition;
        transform.localEulerAngles = handLocalRotation;
        transform.localScale = handLocalScale;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") || other.transform.root.CompareTag("Player"))
        {
            playerNear = true;

            playerInventory = other.GetComponent<PlayerKeyInventory>();

            if (playerInventory == null)
                playerInventory = other.transform.root.GetComponent<PlayerKeyInventory>();

            if (playerAnimator == null && playerInventory != null)
                playerAnimator = playerInventory.GetComponent<Animator>();

            if (interactText != null)
                interactText.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") || other.transform.root.CompareTag("Player"))
        {
            playerNear = false;

            if (interactText != null)
                interactText.SetActive(false);
        }
    }
}