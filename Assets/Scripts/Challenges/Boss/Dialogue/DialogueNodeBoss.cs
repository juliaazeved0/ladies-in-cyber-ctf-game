using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewDialogue", menuName = "Dialogue/NodeBoss")] //Permite criar direto pelo menu da Unity
public class DialogueNodeBoss : ScriptableObject
{
    public string question; //Armazena a pergunta principal do dialogo

    [TextArea(3, 10)] public string[] options; //Array de opções de resposta. TextArea é uma caixa de texto maior no Inspector

    public DialogueNodeBoss[] nextDialogue; //Array de próximos dialogos (cada opção pode levar a um outro node)

    public ButtonType buttonType; //Tipo de botão: jogar novamente, finalizado

    public bool HasOptions() //Verifica se o node possui opções ou não
    {
        return options != null && options.Length > 0; //Retorna true se o array options existe e se tem pelo menos 1 opção
    }
}