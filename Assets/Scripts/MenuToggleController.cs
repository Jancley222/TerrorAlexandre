using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Toggle))]
public class MenuToggleController : MonoBehaviour
{
    private Toggle toggle;

    void Awake()
    {
        toggle = GetComponent<Toggle>();
    }

    void OnEnable()
    {
        if (toggle != null)
        {
            toggle.onValueChanged.AddListener(OnToggleValueChanged);

            // Sincroniza o estado global com o valor que o Toggle já tem ao iniciar
            MenuRotator.GlobalRotationActive = toggle.isOn;
        }
    }

    void OnDisable()
    {
        if (toggle != null)
        {
            toggle.onValueChanged.RemoveListener(OnToggleValueChanged);
        }
    }

    private void OnToggleValueChanged(bool value)
    {
        // Altera o estado de TODOS os MenuRotators do jogo de uma só vez
        MenuRotator.GlobalRotationActive = value;
    }
}