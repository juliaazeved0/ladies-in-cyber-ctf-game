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
    public GameObject passwordPanel;
    public GameObject devicePanel;
    public GameObject panelDialogueJoana;

    [Header("Input Settings")]
    public TMP_InputField input;
    [SerializeField] private string correctPassword = "1541";

    [Header("Visual Feedback")]
    [Tooltip("Objeto que obstrui a entrada da sala ate ser desbloqueado.")]
    public GameObject lockObject;
    public PulseOutline pulse;

    [Header("Transition Settings")]
    public Animator fadeAnimator;
    public GameObject panelBlack;
    public string bossSceneName = "BossRoom";
    public AudioClip bossMusic;

    [Header("Extra References")]
    public LockObjectInteraction lockInteraction;
    public DialogueManager dialogueManager;

    [Header("Spawn Configuration")]
    [Tooltip("Coordenadas de spawn na primeira entrada no BossRoom.")]
    public Vector2 bossRoomSpawnPosition = new Vector2(-2.8f, -2.5f);

    private bool unlocked = false;
    private bool isTransitioning = false;

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

    private IEnumerator FazerFadeIn()
{
    Image fadeImage = panelBlack.GetComponent<Image>();
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

    //Logica de UI e Input
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

    public void PressKey(string value) { input.text += value; }

    public void ClearInput() { input.text = ""; }

    public void PressEnter()
    {
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
        
        lockObject.SetActive(false);
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