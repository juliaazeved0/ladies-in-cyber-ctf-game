using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class LoginSystem : MonoBehaviour
{
    [Header("Configuracoes de Login")]
    public TMP_InputField inputPassword;
    public string passwordCorrect = "Ch3f1nh0";

    [Header("Telas")]
    public GameObject initialBackground;
    public GameObject desktopBackground;
    public GameObject errorPopup;

    [Header("Post-it")]
    [SerializeField] private TMP_InputField postItInput;
    [SerializeField] private GameObject highlightBackground;

    void Start()
    {
        if (errorPopup != null) errorPopup.SetActive(false);

        if (postItInput != null)
        {
            postItInput.readOnly = true;
            postItInput.interactable = true;
        }
    }

    void Update()
    {
        if (inputPassword != null)
            inputPassword.interactable = DialogueManagerBoss.dialogueBossFinished;
    }

    public void ValidatePasswordBoss()
    {
        if (!DialogueManagerBoss.dialogueBossFinished) return;

        if (inputPassword.text.Trim() == passwordCorrect)
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
        initialBackground.SetActive(false);
        desktopBackground.SetActive(true);
    }

    public void CopyPostIt()
    {
        if (postItInput != null && !string.IsNullOrEmpty(postItInput.text))
        {
            GUIUtility.systemCopyBuffer = postItInput.text;
            StopAllCoroutines();
            StartCoroutine(HighlightEffect());
        }
    }

   public void ExitChallengBoss()
    {
        // 1. Reseta o PC deixando a tela de login pronta para a próxima vez
        initialBackground.SetActive(true);
        desktopBackground.SetActive(false);

        if (CanvasManager.Instance != null)
        {
            // 2. A MÁGICA: Avisamos o CanvasManager para fechar o painel pai de verdade!
            // É isso que destrava o movimento da player e libera a tecla E de novo.
            CanvasManager.Instance.ClosedAllPanels();
            
            CanvasManager.Instance.ToggleMiniMap(true);
        }
    }

    IEnumerator HighlightEffect()
    {
        if (highlightBackground != null) highlightBackground.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        if (highlightBackground != null) highlightBackground.SetActive(false);
    }

    IEnumerator ShowErrorTemporary()
    {
        if (errorPopup != null)
        {
            errorPopup.SetActive(true);
            yield return new WaitForSeconds(2f);
            errorPopup.SetActive(false);
        }
    }
}
