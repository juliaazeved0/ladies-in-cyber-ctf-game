using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; //Biblioteca para usar o TextMeshPro
using UnityEngine.EventSystems; //Detectar cliques

public class LoginSystem : MonoBehaviour, IPointerClickHandler
{
    [Header("Configuracoes de Login")]
    public TMP_InputField inputPassword; //Campo onde a jogadora digita a senha
    public string passwordCorrect = "Ch3f1nh0"; //Senha correta guardada em uma string

    [Header("Telas")] //Paineis utilizados no Inspector
    public GameObject initialBackground;
    public GameObject desktopBackground;
    public GameObject errorPopup; 

    [Header("Post-it")]
    [SerializeField] private TMP_Text postItText; //Texto do post-it
    [SerializeField] private GameObject highlightBackground; //Fundo de destaque ao clicar

    private Color originalColor; //Guarda a cor original do texto

    void Start()
    {
        if(postItText != null)
        {
            originalColor = postItText.color; //Salva a cor original
        }

        if(errorPopup != null) errorPopup.SetActive(false); //Garante que o popup de erro inicie escondido

        if(inputPassword != null)
        {
            inputPassword.onEndEdit.AddListener(delegate { OnEndEditValidate(); });
        }
    }

    void Update()
    {
        //Se o dialogo nao acabou, o campo de senha fica desativado
        if(inputPassword != null)
        {
            inputPassword.interactable = DialogueManagerBoss.dialogueBossFinished;
        }
    }

    public void ValidatePasswordBoss() //Validacao da senha
    {
        if (!DialogueManagerBoss.dialogueBossFinished) //Se o dialogo ainda nao terminou, nem tenta validar
        {
            return;
        }

        string senhaDigitada = inputPassword.text.Trim();

        Debug.Log("Tentativa de Login com: " + senhaDigitada);

        if(senhaDigitada == passwordCorrect)
        {
            Debug.Log("SENHA CORRETA!");
            ChangeScreen();
        }
        else
        {
            Debug.Log("SENHA INCORRETA!");
            inputPassword.text = "";
            inputPassword.ActivateInputField();
            StopCoroutine("ShowErrorTemporary");
            StartCoroutine(ShowErrorTemporary());
        }
        
        //if (inputPassword.text.Trim() == passwordCorrect) //Verifica se a senha escrita no input eh igual a senha correta
        //{
        //    ChangeScreen(); //Se colocar a senha correta, troca de painel
        //}
        //else
        //{
         //   inputPassword.text = ""; //Limpa o campo
         //   inputPassword.ActivateInputField(); //Foca novamente no campo de input
         //
         //   //Chama o feedback de erro
         //   StopCoroutine("ShowErrorTemporary"); //Evita duplicar coroutine
        //    StartCoroutine(ShowErrorTemporary()); //Mostra o erro tempor�ria
        //}
    }

    void ChangeScreen() //Mudanca de telas
    {
        initialBackground.SetActive(false);
        desktopBackground.SetActive(true);
    }

    public void CopyPassword(string password)
    {
        GUIUtility.systemCopyBuffer = password; //Copia o texto para a area de transferencia
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(eventData.pointerCurrentRaycast.gameObject == postItText.gameObject) //Verifica se clicou no post-it
        {
            CopyPassword(postItText.text); //Copia a senha

            StopAllCoroutines(); //Para efeitos anteriores
            StartCoroutine(HighlightEffect()); //Inicia efeito visual
        }
    }

    public void ExitChallengBoss()
    {
        desktopBackground.SetActive(false);
    }

    IEnumerator HighlightEffect()
    {
        highlightBackground.SetActive(true); //Mostra o destaque

        yield return new WaitForSeconds(1.5f); //Espera 1.5 segundos

        highlightBackground.SetActive(false); //Esconde o destaque depois que os segundos acabaram
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

    private void OnEndEditValidate()
    {
        // No WebGL, é mais seguro checar apenas se o texto não está vazio 
        // ou confiar no clique do botão físico "ENTRAR" que você criou.
        if (!string.IsNullOrEmpty(inputPassword.text))
        {
            ValidatePasswordBoss();
        }
    }
}