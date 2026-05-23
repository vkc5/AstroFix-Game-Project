using UnityEngine;

public class AlienEggSpore : MonoBehaviour
{
    private Animation anim;

    void Start()
    {
       
        anim = GetComponent<Animation>();

        if (anim != null)
        {
            anim.wrapMode = WrapMode.Loop;
            anim.Play("spore");
        }
    }
}