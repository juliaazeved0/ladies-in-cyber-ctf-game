using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; //Importa as bibliotecas para usar o TextMeshPro

public class ValidatePassword : MonoBehaviour
{
    public TMP_InputField passwordField; //Campo onde o jogador digita a senha do computador
    public GameObject errorMessage; //Variável para arrastar o objeto de texto da "senha inválida"
    public string correctPassword = "MUDAR123"; //A string que define a senha correta

    void Start()
    {
        errorMessage.SetActive(false); //Garante que o pop-up de erro comece escondido
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
            // Lógica de sucesso aqui (ex: abrir porta)
        }
        else
        {
            errorMessage.SetActive(true); //Mostra a mensagem de erro
            passwordField.text = ""; //Apaga o que o jogador digitou para ele tentar novamente
            passwordField.ActivateInputField(); //Devolve o foco do teclado para o campo automaticamente
        }
    }
}