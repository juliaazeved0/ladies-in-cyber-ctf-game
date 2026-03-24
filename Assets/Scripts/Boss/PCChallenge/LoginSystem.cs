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
    public GameObject errorPopup; 

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

        if(errorPopup != null) errorPopup.SetActive(false); //Garante que o popup de erro inicie escondido
    }

    void Update()
    {
        //Se o diálogo não acabou, o campo de senha fica desativado
        if(inputPassword != null)
        {
            inputPassword.interactable = DialogueManagerBoss.dialogueBossFinished;
        }
    }

    public void ValidatePasswordBoss() //Validação da senha
    {
        if (!DialogueManagerBoss.dialogueBossFinished)
        {
            return;
        }

        if(inputPassword.text.Trim() == passwordCorrect) //Verifica se a senha escrita no input é igual a senha correta
        {
            ChangeScreen();
        }
        else
        {
            inputPassword.text = ""; //Limpa o campo se errar
            inputPassword.ActivateInputField(); //Foca novamente no campo

            //Chama o feedback de erro
            StopCoroutine("ShowErrorTemporary"); //Se já estiver rodando
            StartCoroutine(ShowErrorTemporary());
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

    IEnumerator ShowErrorTemporary() //Corrotina para mostrar o erro por apenas 2 segundos
    {
        if(errorPopup != null)
        {
            errorPopup.SetActive(true);
            yield return new WaitForSeconds(2f); //Tempo que ele fica aparecendo na tela
            errorPopup.SetActive(false);
        }
    }
}
