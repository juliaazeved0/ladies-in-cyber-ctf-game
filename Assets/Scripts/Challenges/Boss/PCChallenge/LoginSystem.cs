using System.Collections;
using UnityEngine;
using TMPro;

/// <summary>
/// Gerencia a tela de login do desktop do Boss: valida a senha digitada,
/// alterna entre a tela inicial e o desktop, e controla a funcionalidade
/// de copiar o conteudo do post-it. A validacao so eh permitida apos o
/// dialogo do Boss estar finalizado.
/// </summary>
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

    void Start()
    {
        if(errorPopup != null) errorPopup.SetActive(false);

        if(postItInput != null)
        {
            //Impede edicao do texto
            postItInput.readOnly = true;

            //Mantem o campo clicavel/selecionavel para copiar
            postItInput.interactable = true;
        }
    }

    void Update()
    {
        if(inputPassword != null)
            inputPassword.interactable = DialogueManagerBoss.dialogueBossFinished;
    }

    //Valida a senha digitada e, se correta, avanca para a tela do desktop
    public void ValidatePasswordBoss()
    {
        if(!DialogueManagerBoss.dialogueBossFinished) return;

        if(inputPassword == null)
        {
            Debug.LogError("Input Password não foi configurado no Inspector.", this);
            return;
        }

        if(inputPassword.text.Trim() == passwordCorrect)
        {
            ChangeScreen();
        }
        else
        {
            inputPassword.text = "";

            inputPassword.ActivateInputField();

            StopCoroutine("ShowErrorTemporary");
            StartCoroutine(ShowErrorTemporary());
        }
    }

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

    //Copia o texto do post-it para a area de transferencia do sistema
    public void CopyPostIt()
    {
        if(postItInput != null && !string.IsNullOrEmpty(postItInput.text))
        {
            GUIUtility.systemCopyBuffer = postItInput.text;

            StopAllCoroutines();
            StartCoroutine(HighlightEffect());
        }
    }

    //Fecha o desafio do Boss, retornando a tela inicial e ao estado normal do CanvasManager
   public void ExitChallengeBoss()
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

    IEnumerator HighlightEffect()
    {
        if(highlightBackground != null) highlightBackground.SetActive(true);

        yield return new WaitForSeconds(1.5f);

        if(highlightBackground != null) highlightBackground.SetActive(false);
    }

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