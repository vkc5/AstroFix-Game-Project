using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    public float interactDistance = 3f;
    public PlayerMovement playerMovement;

    private MonoBehaviour currentControl;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (currentControl != null)
            {
                ExitControl();
                return;
            }

            Collider[] hits = Physics.OverlapSphere(transform.position, interactDistance);

            foreach (Collider hit in hits)
            {
                LaserController laser = hit.GetComponent<LaserController>();
                if (laser != null)
                {
                    currentControl = laser;
                    laser.ToggleControl();
                    playerMovement.canMove = false;
                    Debug.Log("Controlling laser");
                    return;
                }

                MirrorController mirror = hit.GetComponent<MirrorController>();
                if (mirror != null)
                {
                    currentControl = mirror;
                    mirror.ToggleControl();
                    playerMovement.canMove = false;
                    Debug.Log("Controlling mirror");
                    return;
                }
                if (hit.CompareTag("Generator"))
                {
                    hit.GetComponent<LaserReceiver>()?.Activate();
                    return;
                }
            }

            Debug.Log("Nothing to interact with");
        }
    }

    void ExitControl()
    {
        if (currentControl is LaserController laser)
            laser.ToggleControl();

        if (currentControl is MirrorController mirror)
            mirror.ToggleControl();

        currentControl = null;
        playerMovement.canMove = true;
        Debug.Log("Stopped controlling");
    }
}