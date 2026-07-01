using UnityEngine;

public class MenuRotator : MonoBehaviour
{
    [SerializeField] private float angle = 3f;
    [SerializeField] private float speed = 0.7f;

    private RectTransform rect;

    // 1. ESTADO ESTÁTICO: Fica na memória do jogo, independente de cena ou GameObject
    private static bool globalRotationActive = true;

    public static bool GlobalRotationActive
    {
        get => globalRotationActive;
        set
        {
            if (globalRotationActive != value)
            {
                globalRotationActive = value;
                // Notifica todas as instâncias que estão a ouvir este evento
                OnGlobalRotationChanged?.Invoke(value);
            }
        }
    }

    // 2. EVENTO ESTÁTICO: Permite o padrão Observer entre a classe e as suas instâncias
    private static System.Action<bool> OnGlobalRotationChanged;

    void Awake()
    {
        rect = GetComponent<RectTransform>();
    }

    void OnEnable()
    {
        // Inscreve esta instância específica no evento global
        OnGlobalRotationChanged += HandleGlobalRotationChanged;

        // Se o jogo já desativou a rotação global anteriormente, garante que este objeto já nasce parado
        if (!globalRotationActive)
        {
            rect.localRotation = Quaternion.identity;
        }
    }

    void OnDisable()
    {
        // Desinscreve-se ao ser desativado/destruído para evitar vazamentos de memória (Memory Leaks)
        OnGlobalRotationChanged -= HandleGlobalRotationChanged;
    }

    void Update()
    {
        // Se a rotação global estiver desativada, interrompe o cálculo imediatamente
        if (!globalRotationActive) return;

        float rot = Mathf.Sin(Time.time * speed) * angle +
                    Mathf.Sin(Time.time * speed * 0.37f) * (angle * 0.35f);

        rect.localRotation = Quaternion.Euler(0, 0, rot);
    }

    // Método que responde ao evento global
    private void HandleGlobalRotationChanged(bool active)
    {
        if (!active)
        {
            // Se foi desativado, reseta a rotação para o padrão original
            rect.localRotation = Quaternion.identity;
        }
    }
}