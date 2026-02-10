using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//script criado para o efeito de pulsar nos outlines
//deve ser adicionado ao objeto com material do shader
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
            float currentThickness = Mathf.PingPong(Time.time * pulseSpeed, maxThickness);
            myMaterial.SetFloat(thicknessID, currentThickness);
        }
    }

// metodo para chamar no botao de ajudar
// arrastar o objeto q contem esse script no botao e selecionar a funcao abaixo
// com o objeto q contem esse script selecionado, deve arrastar o panel do dialogo q deve ser fechado
    public void StartPulsing()
    {
        isPulsing = true;
        if(myMaterial != null)
        {
            myMaterial.SetFloat("_OutlineAlphaMultiplier", 1.0f);
        }
    }


// para chamar quando a player concluir a task 
    public void StopPulsing()
    {
        isPulsing = false;

        if(myMaterial != null)
        {
            myMaterial.SetFloat(thicknessID, 0.0f);
        }
    }
}
