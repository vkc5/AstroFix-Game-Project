using UnityEngine;

public class Swithecamera : MonoBehaviour
{
    public GameObject Camera_1; // Assign the Main Camera in the Inspector
    public GameObject Camera_2; // Assign the Keypad Camera in the Inspector

    public int Manager;


    public void MangerCamera()
    {         if (Manager == 0)
        {
            Cam_2();
            Manager = 1;
        }
        else 
        {
            Cam_1();
            Manager = 0;
        }
    }
    void Cam_1()
    {
        Camera_1.SetActive(true);
        Camera_2.SetActive(false);
    }

    void Cam_2()
    {
        Camera_1.SetActive(false);
        Camera_2.SetActive(true);
    }
}
