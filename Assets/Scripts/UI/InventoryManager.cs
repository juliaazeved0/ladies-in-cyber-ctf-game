using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
[Header("Configurações")]
    public float velocidade = 10f; // Quão rápido a borda desliza

    // O objeto que a borda está perseguindo no momento
    private RectTransform alvoAtual; 

    void Update()
    {
        // Se temos um alvo, a borda desliza suavemente até ele a cada frame
        if (alvoAtual != null)
        {
            transform.position = Vector3.Lerp(transform.position, alvoAtual.position, Time.deltaTime * velocidade);
        }
    }

    // Função que os botões vão chamar quando forem clicados
    public void MoverParaBotao(RectTransform novoAlvo)
    {
        // Ativa a borda (caso ela comece invisível)
        gameObject.SetActive(true); 
        
        // Define o novo alvo para a borda perseguir
        alvoAtual = novoAlvo;
    }
}
