using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Controla a navegacao sequencial entre paineis de tutorial (proximo/anterior)
/// e, ao final, aplica um fade visual antes de carregar a proxima cena.
/// </summary>
public class TutorialController : MonoBehaviour
{
    [Header("Panels Configuration")]
    [Tooltip("Lista de paineis tutoriais ordenados que serao mostrados em sequencia.")]
    [SerializeField] private GameObject[] tutorialPanels;

    [Header("Navigation")]
    [Tooltip("Nome da cena carregada apos o ultimo painel do tutorial.")]
    [SerializeField] private string nextSceneName = "PlayerMap";

    [Header("Visual Fade")]
    [Tooltip("Painel preto usado para o efeito de fade antes de trocar de cena.")]
    [SerializeField] private GameObject fadePanel;

    [Tooltip("Duracao em segundos da transicao de fade antes de trocar de cena.")]
    [SerializeField] private float fadeDuration = 1f;

    private Image fadeImage;
    private int currentPanelIndex = 0;

    void Start()
    {
        InitializeUI();
    }

    private void InitializeUI()
    {
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

    //Avanca para o proximo painel do tutorial, ou inicia o fade da proxima cena se ja estiver no ultimo
    public void OnNextClicked()
    {
        if(tutorialPanels == null || tutorialPanels.Length == 0) return;

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

    //Retorna ao painel anterior do tutorial, se houver
    public void OnBackClicked()
    {
        if(tutorialPanels == null || tutorialPanels.Length == 0) return;

        if(currentPanelIndex > 0)
        {
            tutorialPanels[currentPanelIndex].SetActive(false);
            currentPanelIndex--;
            tutorialPanels[currentPanelIndex].SetActive(true);
        }
    }

    private IEnumerator FadeAndLoadRoutine()
    {
        if(fadePanel != null && fadeImage != null)
        {
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
            yield return new WaitForSeconds(0.5f);
        }

        if (!Application.CanStreamedLevelBeLoaded(nextSceneName))
        {
            Debug.LogError($"A cena '{nextSceneName}' não existe ou não está no Build Settings!");
            yield break;
        }

        SceneManager.LoadScene(nextSceneName);
    }
}