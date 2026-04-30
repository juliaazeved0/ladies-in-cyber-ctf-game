using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Gerencia a navegacao entre as telas do tutorial inicia.
/// Responsavel por alternar a visibilidade dos paineis e carregar a cena do jogo.
/// </summary>
public class TutorialController : MonoBehaviour
{
    [Header("Panels")]
    [Tooltip("Lista de paineis tutoriais em ordem.")]
    [SerializeField] private GameObject[] panels;

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

    private void InitializeUI()
    {
        //Garante que apenas o primeiro painel esteja ativo
        for(int i = 0; i < panels.Length; i++)
        {
            panels[i].SetActive(i == 0);
        }

        if(fadePanel != null)
        {
            fadeImage = fadePanel.GetComponent<Image>();
            fadePanel.SetActive(false); //Esconde o fade no início
        }
    }

    public void OnNextClicked()
    {
        if(currentPanelIndex < panels.Length - 1)
        {
            //Avanca para o proximo painel
            panels[currentPanelIndex].SetActive(false);
            currentPanelIndex++;
            panels[currentPanelIndex].SetActive(true);
        }
        else
        {
            //Se for o ultimo, inicia a transicao de cena
            StartCoroutine(FadeAndLoadRoutine());
        }
    }

    public void OnBackClicked()
    {
        if(currentPanelIndex > 0)
        {
            //Volta para o painel anterior
            panels[currentPanelIndex].SetActive(false);
            currentPanelIndex--;
            panels[currentPanelIndex].SetActive(true);
        }
    }

    private IEnumerator FadeAndLoadRoutine()
    {
        if(fadePanel != null && fadeImage != null)
        {
            //Desativa Animator se existir para nao conflitar com o fade manual
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
            //Delay de seguranca caso nao haja painel de fade
            yield return new WaitForSeconds(0.5f);
        }

        SceneManager.LoadScene(nextSceneName);
    }
}