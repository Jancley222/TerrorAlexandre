using UnityEngine;

public class SomButton : MonoBehaviour
{
    public AudioSource audioSource;

    public void PlayClick()
    {
        audioSource.Play();
    }
}