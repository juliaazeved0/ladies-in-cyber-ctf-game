using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Gerencia o desbloqueio do BossRoom atraves de senha e controla a transicao de cena.
/// </summary>
public class UnlockBossRoom : MonoBehaviour
{
    [Header("UI Panels")]
    [Tooltip("Painel que contem o teclado e campo de senha.")]
    public GameObject passwordPanel;

    [Tooltip("Painel do dispositivo de interacao visual/mecanismo.")]
    public GameObject devicePanel;

    [Tooltip("Painel do dialogo especifico da Joana.")]
    public GameObject panelDialogueJoana;

    [Header("Input Settings")]
    [Tooltip("Campo de texto onde a senha inserida eh exibida.")]
    public TMP_InputField input;

    [Tooltip("A senha correta necessaria para desbloquear o BossRoom.")]
    [SerializeField] private string correctPassword = "1541";

    [Header("Visual Feedback")]
    [Tooltip("Objeto que obstrui a entrada da sala ate ser desbloqueado.")]
    public GameObject lockObject;

    [Tooltip("Efeito de pulso visual para guiar a jogadora ao ponto de interacao.")]
    public PulseOutline pulse;

    [Header("Transition Settings")]
    [Tooltip("Animator responsavel pelas animacoes de fade in e fade out.")]
    public Animator fadeAnimator;

    [Tooltip("Painel preto usado na sobreposicao da tela durante a transicao de cena.")]
    public GameObject panelBlack;

    [Tooltip("Nome da cena do BossRoom na lista de Build Settings.")]
    public string bossSceneName = "BossRoom";

    [Tooltip("Clipe de audio que tocara ao entrar na area do Boss.")]
    public AudioClip bossMusic;

    [Header("Extra References")]
    [Tooltip("Referencia ao script de interacao de bloqueio.")]
    public LockObjectInteraction lockInteraction;

    [Tooltip("Referencia ao gerenciador de dialogos.")]
    public DialogueManager dialogueManager;

    [Header("Spawn Configuration")]
    [Tooltip("Coordenadas de spawn na primeira entrada no BossRoom.")]
    public Vector2 bossRoomSpawnPosition = new Vector2(-2.8f, -2.5f);

    private bool unlocked = false;
    private bool isTransitioning = false;

    //Verifica se a sala ja foi desbloqueada ou se a jogadora esta retornando do BossRoom ao iniciar
    void Start()
    {
        if(PlayerPrefs.GetInt("BossRoomUnlocked", 0) == 1)
        {
            unlocked = true;

            if(lockObject != null) lockObject.SetActive(false);
        }

        if(PlayerPrefs.GetInt("ReturningFromBoss", 0) == 1)
        {
            PlayerPrefs.DeleteKey("ReturningFromBoss");
            PlayerPrefs.SetFloat("BossRoom_PlayerX", bossRoomSpawnPosition.x);
            PlayerPrefs.SetFloat("BossRoom_PlayerY", bossRoomSpawnPosition.y);
            PlayerPrefs.SetFloat("BossRoom_PlayerZ", 0f);
            PlayerPrefs.Save();
            StartCoroutine(FazerFadeIn());
        }
    }

    //Executa uma transicao suave de Fade In via corrotina e ajusta os componentes do painel preto
    private IEnumerator FazerFadeIn()
    {
        if(panelBlack == null)
        {
            Debug.LogWarning("PanelBlack não está atribuído no Inspector.");
            yield break;
        }

        Image fadeImage = panelBlack.GetComponent<Image>();

        if(fadeImage == null)
        {
            Debug.LogWarning("Nenhum componente Image foi encontrado em PanelBlack.");
            yield break;
        }

        Animator anim = panelBlack.GetComponent<Animator>();

        if(anim != null) anim.enabled = false;

        panelBlack.SetActive(true);

        Color cor = fadeImage.color;
        cor.a = 1f;
        fadeImage.color = cor;

        float tempo = 0f;
        float duracaoFade = 1f;

        while(tempo < duracaoFade)
        {
            tempo += Time.deltaTime;
            cor.a = Mathf.Clamp01(1f - (tempo / duracaoFade));
            fadeImage.color = cor;
            yield return null;
        }

        panelBlack.SetActive(false);

        if(anim != null) anim.enabled = true;
    }

    public void OpenPasswordPanel()
    {
        dialogueManager.OnClickExit();
        CanvasManager.Instance.OpenPanel(passwordPanel.name);

        if(pulse != null) pulse.StartPulsing();
    }

    public void ClosePasswordPanel()
    {
        CanvasManager.Instance.ClosedPanel(passwordPanel.name);

        if(pulse != null) pulse.StopPulsing();
        if(lockInteraction != null) lockInteraction.isUnlocked = true;

        CanvasManager.Instance.ToggleMiniMap(true);
    }

    public void OpenDevicePanel()
    {
        CanvasManager.Instance.OpenPanel(devicePanel.name);
    }

    public void CloseDevicePanel()
    {
        CanvasManager.Instance.ClosedPanel(devicePanel.name);
        CanvasManager.Instance.ToggleMiniMap(true);

        if(panelDialogueJoana != null)
            CanvasManager.Instance.ClosedPanel(panelDialogueJoana.name);
    }

    public void PressKey(string value) 
    {
        if(input != null)
        {
            input.text += value;
        }
    }

    public void ClearInput() 
    { 
        if(input != null)
        {
            input.text = "";
        } 
    }

    public void PressEnter()
    {
        if(input == null) return;

        if(input.text == correctPassword)
            StartCoroutine(SuccessRoutine());
        else
        {
            input.text = "ACESSO NEGADO";
            StartCoroutine(ClearAfterDelay());
        }
    }

    IEnumerator SuccessRoutine()
    {
        input.text = "ACESSO CONCEDIDO";
        yield return new WaitForSeconds(0.5f);

        if (lockObject != null) lockObject.SetActive(false);
        if (dialogueManager != null) dialogueManager.HideLock();

        unlocked = true;
        PlayerPrefs.SetInt("BossRoomUnlocked", 1);
        PlayerPrefs.Save();

        CloseDevicePanel();
    }

    IEnumerator ClearAfterDelay()
    {
        yield return new WaitForSeconds(3f);
        ClearInput();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player") && unlocked && !isTransitioning)
            IniciarTransiçãoBoss(other);
    }

    private void IniciarTransiçãoBoss(Collider2D other)
    {
        Debug.Log("Iniciando transição para o Boss");
        isTransitioning = true;
        CanvasManager.Instance.OpenPanel(panelBlack.name);
        fadeAnimator.SetTrigger("FadeOut");
    }

    public void CarregarCenaBoss()
    {
        if(bossMusic != null)
            BackgroundMusic.ChangeMusic(bossMusic);

        PlayerPrefs.SetFloat("BossRoom_PlayerX", bossRoomSpawnPosition.x);
        PlayerPrefs.SetFloat("BossRoom_PlayerY", bossRoomSpawnPosition.y);
        PlayerPrefs.SetFloat("BossRoom_PlayerZ", 0f);
        PlayerPrefs.Save();

        Debug.Log("Carregando cena: " + bossSceneName);
        SceneManager.LoadSceneAsync(bossSceneName);
    }

    public void FinalizarFade()
    {
        CanvasManager.Instance.ClosedPanel(panelBlack.name);
        isTransitioning = false;
    }
}