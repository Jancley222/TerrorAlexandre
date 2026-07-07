using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Footsteps : MonoBehaviour
{
    public CharacterController controller;

    public AudioClip[] footstepSounds;

    public float stepDelay = 0.45f;

         private AudioSource audioSource;
         private float timer;

            void Start()
            {
                 audioSource = GetComponent<AudioSource>();
            }

    void Update()
                {
                     if (controller.isGrounded && controller.velocity.magnitude > 0.2f)
                        {
                              timer += Time.deltaTime;

                             if (timer >= stepDelay)
                                {
                                    PlayFootstep();
                                         timer = 0;
                                 }
        }
                     else
                        {
                             timer = 0;
                        }
                }

                        void PlayFootstep()
                        {
                         int random = Random.Range(0, footstepSounds.Length);

                         audioSource.PlayOneShot(footstepSounds[random]);
                         }
}