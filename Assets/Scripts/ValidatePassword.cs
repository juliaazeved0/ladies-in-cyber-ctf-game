using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; //Importa as bibliotecas para usar o TextMeshPro

public class ValidatePassword : MonoBehaviour
{
    [Header("Configurações de UI")]
    public TMP_InputField passwordField; //Campo onde o jogador digita a senha do computador
    public GameObject errorMessage; //Variável para arrastar o objeto de texto da "senha inválida"
    public string correctPassword = "MUDAR123"; //A string que define a senha correta

    [Header("Painéis do Computador")]
    public GameObject loginPanel; //Painel inicial
    public GameObject taskbarPanel; //Painel do área de trabalho

    [Header("Interação e Avisos")]
    public GameObject pressEKey; //objeto de texto para a tecla E
    public MonoBehaviour interactionScript; //Script que abre o PC

    [Header("Janelas")]
    public GameObject whatsappWindow; //Painel da janela do WhatsApp

    void Start()
    {
        errorMessage.SetActive(false); //Garante que o pop-up de erro comece escondido
        if(taskbarPanel != null) taskbarPanel.SetActive(false); //Painel da área de trabalho comece desativado
        passwordField.ActivateInputField(); //Faz com que o cursos já apareça piscando dentro do Input Field, sem o jogador precisar clicar
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter)) //Verifica se a tecla Enter principal ou do numérico foi apertada
        {
            CheckPassword(); //Se apertou no Enter, chama a função que valida a senha
        }
    }

    public void CheckPassword()
    {
        if (passwordField.text == correctPassword) //Verifica se o texto escrito no campo é igual ao da variável
        {
            errorMessage.SetActive(false); //Se for igual, esconde a mensagem de erro
            loginPanel.SetActive(false); //Desativa a tela inicial
            taskbarPanel.SetActive(true); //Ativa a tela da área de trabalho

            if(whatsappWindow != null) whatsappWindow.SetActive(false); //Garante que comece fechado

            if(pressEKey != null) pressEKey.SetActive(false); //Desliga o texto visual da tecla E

            if (interactionScript != null) interactionScript.enabled = false; //Desativa o script que detecta o botão E
        }
        else
        {
            errorMessage.SetActive(true); //Mostra a mensagem de erro
            passwordField.text = ""; //Apaga o que o jogador digitou para ele tentar novamente
            passwordField.ActivateInputField(); //Devolve o foco do teclado para o campo automaticamente
        }
    }

    public void OpenWhatsAppWindow()
    {
        if(whatsappWindow != null)
        {
            whatsappWindow.SetActive(true); //Ativa a janela
        }
    }
}