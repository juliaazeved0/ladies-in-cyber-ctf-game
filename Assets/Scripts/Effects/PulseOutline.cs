using UnityEngine;

/// <summary>
/// Faz o contorno de um aprite pulsar suavemente, variando a espessura
/// via shader property. Pode ser ativado/desativado sob demanda, para
/// destacar objetos interativos ou selecionaveis.
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

    private Material myMaterial; //Usado para nao afetar outros objetos que compartilhem o mesmo material

    private bool isPulsing = false;

    private int thicknessID; //Cache do ID da propriedade do shader

    void Start()
    {
        var renderer = GetComponent<SpriteRenderer>();

        if(renderer != null)
        {
            //Acessar ".material" ja cria uma copia unica do material para esse objeto especificamente
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
            //PingPong faz o valor oscilar entre 0 e maxThickness de forma suave, criando o efeito de pulsacao continua
            float currentThickness = Mathf.PingPong(Time.time * pulseSpeed, maxThickness);
            myMaterial.SetFloat(thicknessID, currentThickness);
        }
    }

    //Ativa a pulsacao do contorno e garante que ele esteja visivel
    public void StartPulsing()
    {
        isPulsing = true;

        if(myMaterial != null)
        {
            myMaterial.SetFloat("_OutlineAlphaMultiplier", 1.0f);
        }
    }

    //Interrompe a pulsacao e zera tanto a espessura quanto o alpha do contorno
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