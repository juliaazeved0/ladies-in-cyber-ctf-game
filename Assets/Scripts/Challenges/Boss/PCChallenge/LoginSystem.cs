using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class LoginSystem : MonoBehaviour
{
    [Header("Login Settings")]
    [Tooltip("Campo onde a jogadora devera inserir a senha.")]
    public TMP_InputField inputPassword;

    [Tooltip("Senha correta necessaria para acessar o desktop.")]
    public string passwordCorrect = "Ch3f1nh0";

    [Header("Screens")]
    [Tooltip("Tela inicial exibida antes do login ser realizado.")]
    public GameObject initialBackground;

    [Tooltip("Tela do desktop exibida apos o login bem-sucedido.")]
    public GameObject desktopBackground;

    [Tooltip("Popup exibido quando a senha informada estiver incorreta.")]
    public GameObject errorPopup;

    [Header("Post-it Note")]
    [Tooltip("Campo que contem o texto do post-it. A jogadora pode copiar o conteudo, mas nao edita-lo.")]
    [SerializeField] private TMP_InputField postItInput;

    [Tooltip("Objeto visual utilizado para destacar o post-it apos o conteudo ser copiado.")]
    [SerializeField] private GameObject highlightBackground;

    //Inicializa o estado da tela de login e configura o campo do post-it
    void Start()
    {
        //O popup de erro comeca oculta e so sera exibido quando a jogadora inserir uma senha incorreta
        if(errorPopup != null) errorPopup.SetActive(false);

        if(postItInput != null)
        {
            //A jogadora nao pode alterar o conteudo do post-it, apenas permite selecao/copia
            postItInput.readOnly = true;
            postItInput.interactable = true;
        }
    }

    //Mantem o campo de senha habiliado somente apos a jogadora finalizar o dialogo com o Boss
    void Update()
    {
        if(inputPassword != null)
            inputPassword.interactable = DialogueManagerBoss.dialogueBossFinished;
    }

    //Verifica a senha informada pela jogadora
    public void ValidatePasswordBoss()
    {
        //Impede a tentativa de login antes que o dialogo seja finalizado
        if(!DialogueManagerBoss.dialogueBossFinished) return;

        if(inputPassword == null)
        {
            Debug.LogError("Input Password não foi configurado no Inspector.", this);
            return;
        }

        //Remove espacos no inicio e no final da senha antes da comparacao
        if(inputPassword.text.Trim() == passwordCorrect)
        {
            ChangeScreen(); //Senha correta: acessa o desktop
        }
        else
        {
            //Senha incorreta: limpa o campo para uma nova tentativa
            inputPassword.text = "";

            inputPassword.ActivateInputField();

            //Reinicia o popup de erro, mostrando a cada tentativa incorreta
            StopCoroutine("ShowErrorTemporary");
            StartCoroutine(ShowErrorTemporary());
        }
    }

    //Alterna da tela inicial para a tela do desktop
    void ChangeScreen()
    {
        if(initialBackground == null || desktopBackground == null)
        {
            Debug.LogError("Initial Background ou Desktop Background não foi configurado no Inspector.", this);
            return;
        }

        initialBackground.SetActive(false);
        desktopBackground.SetActive(true);
    }

    /// <summary>
    /// Copia o conteudo do post-it para a area de transferencia do sistema
    /// e ativa temporariamente o destaque visual.
    /// </summary>
    public void CopyPostIt()
    {
        //So realiza a copia se o campo existir e possuir algum conteudo
        if(postItInput != null && !string.IsNullOrEmpty(postItInput.text))
        {
            GUIUtility.systemCopyBuffer = postItInput.text;

            //Garante que apenas um efeito de destaque esteja ativo
            StopAllCoroutines();
            StartCoroutine(HighlightEffect());
        }
    }

    //Fecha o desafio do Boss, retornando para a tela inicial e mostrando o minimapa
   public void ExitChallengBoss()
    {
        if(initialBackground == null || desktopBackground == null)
        {
            Debug.LogError("Initial Background ou Desktop Background não foi configurado no Inspector.", this);
            return;
        }

        initialBackground.SetActive(true);
        desktopBackground.SetActive(false);

        if(CanvasManager.Instance != null)
        {
            CanvasManager.Instance.ClosedAllPanels();
            
            CanvasManager.Instance.ToggleMiniMap(true);
        }
    }

    //Ativa o destaque visual do post-it por um periodo determinado
    IEnumerator HighlightEffect()
    {
        if(highlightBackground != null) highlightBackground.SetActive(true);

        yield return new WaitForSeconds(1.5f); //Mantem o destaque por 1,5 segundos

        if(highlightBackground != null) highlightBackground.SetActive(false);
    }

    //Mostra temporariamente o popup de senha incorreta
    IEnumerator ShowErrorTemporary()
    {
        if(errorPopup != null)
        {
            errorPopup.SetActive(true);

            yield return new WaitForSeconds(2f);

            errorPopup.SetActive(false);
        }
    }
}