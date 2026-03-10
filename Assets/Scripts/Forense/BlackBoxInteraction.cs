using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackBoxInteraction : ObjectInteraction //Herda de ObjectInteraction, pois já possui todos os comportamentos puxados do script
{
    [Header("Configuração do Desbloqueio")] //Cria um título no Inspector para organização
    public ServerVisuals visuals;

    //"protected override" indica que este método está sobrescrevendo a função de interação padrão que foi definida na classe pai (ObjectInteraction)
    protected override void Interact()
    {
        CanvasManager.Instance.ToggleMiniMap(false); //Acessa o CanvasManager através do "Instance" e chama a função para desligar o mapa da tela
        CanvasManager.Instance.OpenPanel("InitialBackground"); //Acessa novamente o CanvasManager para abrir um painel específico, passando o nome do painel identificador

        UnlockServer(); //Chamada da função
    }

    private void UnlockServer()
    {
        if(visuals != null) //Verifica se a variável não é nula
        {
            visuals.Unlock(); //Mostra o cadeado aberto
        }
        else //Se a variável for nula
        {
            Debug.LogWarning("Esqueceu de arrastar o cadeado para o script!"); //Envia um alerta amarelo no console
        }
    }
}