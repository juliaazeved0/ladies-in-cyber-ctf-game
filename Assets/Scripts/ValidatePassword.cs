using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; //Importa as ferramentas do TextMeshPro

public class ValidatePassword : MonoBehaviour
{
    public TMP_InputField passwordField; //Campo onde o jogador digita a senha do computador
    public GameObject errorMessage; //Variável para arrastar o objeto de texto da "senha inválida"
    public string correctPassword = "1234"; //A senha correta

    public void CheckPassword()
    {
        if (passwordField.text == correctPassword) //Se o texto digitado for exatamente igual  senha
        {
            Debug.Log("Senha Certa!"); //Teste para mostrar a senha correta
            errorMessage.SetActive(false); //Esconde a mensagem de erro
        }
        else //Se a senha estiver errada
        {
            errorMessage.SetActive(true); //Ativa a mensagem de mensagem
        }
    }
}
