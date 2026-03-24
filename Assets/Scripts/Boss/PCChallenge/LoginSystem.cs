using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; //Biblioteca para usar o TextMeshPro
using UnityEngine.EventSystems; //Interface de clique

public class LoginSystem : MonoBehaviour, IPointerClickHandler
{
    [Header("Configurações de Login")]
    public TMP_InputField inputPassword;
    public string passwordCorrect = "Ch3f1nh0";

    [Header("Telas")]
    public GameObject initialBackground; //Tela bloqueada
    public GameObject desktopBackground; //Tela da área de trabalho

    [Header("Post-it")]
    [SerializeField] private TMP_Text postItText;
    [SerializeField] private GameObject highlightBackground;

    private Color originalColor;

    void Start()
    {
        if(postItText != null)
        {
            originalColor = postItText.color;
        }
    }

    void Update()
    {
        //Verifica se o InputField está focado e se a jogadora apertou Enter
        if (inputPassword.isFocused && (Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.Return)))
        {
            ValidatePasswordBoss();
        }
    }

    public void ValidatePasswordBoss() //Validação da senha
    {
        if(inputPassword.text.Trim() == passwordCorrect) //Verifica se a senha escrita no input é igual a senha correta
        {
            ChangeScreen();
        }
        else
        {
            inputPassword.text = ""; //Limpa o campo se errar
            inputPassword.ActivateInputField(); //Foca novamente no campo
        }
    }

    void ChangeScreen() //Mudança de telas
    {
        initialBackground.SetActive(false);
        desktopBackground.SetActive(true);
    }

    public void CopyPassword(string password)
    {
        GUIUtility.systemCopyBuffer = password; //Copia a senha
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(eventData.pointerCurrentRaycast.gameObject == postItText.gameObject)
        {
            CopyPassword(postItText.text);

            StopAllCoroutines();
            StartCoroutine(HighlightEffect());
        }
    }

    IEnumerator HighlightEffect()
    {
        highlightBackground.SetActive(true);

        yield return new WaitForSeconds(1.5f);

        highlightBackground.SetActive(false);
    }
}
