using UnityEngine;

/// <summary>
/// Controla um efeito de contorno pulsante (outline) em um SpriteRenderer,
/// variando a espessura do contorno ao longo do tempo via shader properties.
/// </summary>
public class PulseOutline : MonoBehaviour
{
    [Header("Pulse Settings")]
    [Tooltip("Velocidade da pulsacao do contorno. Quanto maior, mais rapida a oscilacao.")]
    [SerializeField] private float pulseSpeed = 0.05f;

    [Tooltip("Espessura maxima que o contorno atinge durante a pulsacao.")]
    [SerializeField] private float maxThickness = 0.05f;

    [Tooltip("Se marcado, a pulsacao ja comeca ativa assim que o objeto eh carregado.")]
    [SerializeField] private bool startActive = false;

    private Material myMaterial;

    private bool isPulsing = false;

    private int thicknessID;

    void Start()
    {
        var renderer = GetComponent<SpriteRenderer>();

        if(renderer != null)
        {
            myMaterial = renderer.material;
        }
        else
        {
            Debug.LogWarning($"{gameObject.name} não possui um SpriteRenderer. O contorno não poderá ser exibido.");
        }

        thicknessID = Shader.PropertyToID("_OutlineThickness");

        if(startActive)
        {
            StartPulsing();
        }
        else
        {
            StopPulsing();
        }
    }

    void Update()
    {
        if(isPulsing && myMaterial != null)
        {
            float currentThickness = Mathf.PingPong(Time.time * pulseSpeed, maxThickness);
            myMaterial.SetFloat(thicknessID, currentThickness);
        }
    }

    /// <summary>
    /// Ativa a pulsacao do contorno e torna o efeito visivel
    /// </summary>
    public void StartPulsing()
    {
        isPulsing = true;

        if(myMaterial != null)
        {
            myMaterial.SetFloat("_OutlineAlphaMultiplier", 1.0f);
        }
    }

    /// <summary>
    /// Interrompe a pulsacao e zera a espessura/visibilidade do contorno
    /// </summary>
    public void StopPulsing()
    {
        isPulsing = false;

        if(myMaterial != null)
        {
            myMaterial.SetFloat(thicknessID, 0.0f);
            myMaterial.SetFloat("_OutlineAlphaMultiplier", 0.0f);
        }
    }
}