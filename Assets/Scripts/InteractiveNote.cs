using UnityEngine;
using TMPro;

public class InteractiveNote : MonoBehaviour
{
    [Header("UI References")]
    public GameObject interactionNotice;
    public GameObject notePanel; 

    private bool isPlayerClose = false;
    private bool isNoteOpen = false;

    void Start()
    {
        if (interactionNotice != null) interactionNotice.SetActive(false);
        if (notePanel != null) notePanel.SetActive(false);
    }

    void Update()
    {
        if (isPlayerClose && Input.GetKeyDown(KeyCode.E))
        {
            if (isNoteOpen)
            {
                CloseNote();
            }
            else
            {
                OpenNote();
            }
        }
    }

    void OpenNote()
    {
        isNoteOpen = true;
        notePanel.SetActive(true);
        interactionNotice.SetActive(false);
    }

    public void CloseNote()
    {
        isNoteOpen = false;
        notePanel.SetActive(false);
        if (isPlayerClose) interactionNotice.SetActive(true);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerClose = true;
            if (!isNoteOpen) interactionNotice.SetActive(true);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerClose = false;
            interactionNotice.SetActive(false);
            CloseNote();
        }
    }
}