using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Controla a navegacao entre paineis de tutorial e, ao finalizar a
/// sequencia, realiza um fade-in visual antes de carregar a proxima cena.
/// </summary>
public class TutorialController : MonoBehaviour
{
    [Header("Panels Configuration")]
    [Tooltip("Lista de paineis tutoriais ordenados que serao mostrados em sequencia.")]
    [SerializeField] private GameObject[] tutorialPanels;

    [Header("Navigation")]
    [SerializeField] private string nextSceneName = "PlayerMap";

    [Header("Visual Fade")]
    [SerializeField] private GameObject fadePanel;
    [SerializeField] private float fadeDuration = 1f;

    private Image fadeImage;
    private int currentPanelIndex = 0;

    void Start()
    {
        InitializeUI();
    }

    /// <summary>
    /// Garante que apenas o primeiro painel do tutorial esteja visivel no inicio,
    /// e prepara o painel de fade (desativado por padrao).
    /// </summary>
    private void InitializeUI()
    {
        //Se o array nao for preenchido no Inspector, evita erros e avisa caso esteja vazio
        if(tutorialPanels == null || tutorialPanels.Length == 0)
        {
            Debug.LogError($"{gameObject.name} não possui painéis de tutorial configurados!");
            return;
        }

        for(int i = 0; i < tutorialPanels.Length; i++)
        {
            tutorialPanels[i].SetActive(i == 0);
        }

        if(fadePanel != null)
        {
            fadeImage = fadePanel.GetComponent<Image>();

            if(fadeImage == null)
            {
                Debug.LogWarning($"{fadePanel.name} não possui um componente Image! O fade visual será substituído por uma espera fixa.");
            }

            fadePanel.SetActive(false);
        }
    }

    /// <summary>
    /// Avanca para o proximo painel do tutorial. Se ja estiver no ultimo,
    /// inicia o fade e o carregamento da proxima cena.
    /// </summary>
    public void OnNextClicked()
    {
        if(currentPanelIndex < tutorialPanels.Length - 1)
        {
            tutorialPanels[currentPanelIndex].SetActive(false);
            currentPanelIndex++;
            tutorialPanels[currentPanelIndex].SetActive(true);
        }
        else
        {
            StartCoroutine(FadeAndLoadRoutine());
        }
    }

    //Volta para o painel anterior do tutorial, se houver um
    public void OnBackClicked()
    {
        if(currentPanelIndex > 0)
        {
            tutorialPanels[currentPanelIndex].SetActive(false);
            currentPanelIndex--;
            tutorialPanels[currentPanelIndex].SetActive(true);
        }
    }

    /// <summary>
    /// Realiza um fade visual antes de carregar a proxima cena. Se nao houver um painel
    /// de fade ou imagem configurados, apenas aguarda um tempo fixo como fallback.
    /// </summary>
    private IEnumerator FadeAndLoadRoutine()
    {
        if(fadePanel != null && fadeImage != null)
        {
            //Desativa qualquer Animator no painel de fade para evitar que ele sobrescreva
            Animator anim = fadePanel.GetComponent<Animator>();

            if(anim != null) anim.enabled = false;

            fadePanel.SetActive(true);
            Color color = fadeImage.color;

            float elapsed = 0f;

            while(elapsed < fadeDuration)
            {
                elapsed += Time.deltaTime;
                color.a = Mathf.Clamp01(elapsed / fadeDuration);
                fadeImage.color = color;
                yield return null;
            }
        }
        else
        {
            //Sem painel de fade configurado, apenas espera um tempo fixo antes de trocar de cena
            yield return new WaitForSeconds(0.5f);
        }

        //Verifica se a cena existe e esta registrada no Build Settings
        if (!Application.CanStreamedLevelBeLoaded(nextSceneName))
        {
            Debug.LogError($"A cena '{nextSceneName}' não existe ou não está no Build Settings!");
            yield break;
        }

        SceneManager.LoadScene(nextSceneName);
    }
}