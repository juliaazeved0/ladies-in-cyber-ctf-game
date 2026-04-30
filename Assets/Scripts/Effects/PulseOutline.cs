using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Cria um efeito de pulsacao no contorno (outline) de um Sprite atraves do Shader.
/// Serve ara destacar objetos interativos ou guiar a jogadora.
/// </summary>
public class PulseOutline : MonoBehaviour
{
    [Header("Settings pulse effect")]
    public float pulseSpeed = 0.05f;
    public float maxThickness = 0.05f;
    public bool startActive = false;

    private Material myMaterial;
    private bool isPulsing = false;

    private int thicknessID;

    void Start()
    {
        var renderer = GetComponent<SpriteRenderer>();

        if(renderer != null)
        {
            //.material cria uma instancia unica para este objeto,
            //evitando que todos os objetos com o mesmo material pulsem juntos
            myMaterial = renderer.material;
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
            //Mathf.PingPong cria o efeito de "ida e volta" suave
            float currentThickness = Mathf.PingPong(Time.time * pulseSpeed, maxThickness);
            myMaterial.SetFloat(thicknessID, currentThickness);
        }
    }

    /// <summary>
    /// Ativa o efeito de pulsacao e torna o contorno visivel.
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
    /// Desativa o efeito e esconde o contorno.
    /// Metodo chamado quando a jogadora se afasta do objeto.
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