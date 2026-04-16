using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using TMPro;

public class UnlockBossRoom : MonoBehaviour
{
    [Header("Panels UI")]
    public GameObject passwordPanel;
    public GameObject devicePanel;
    public GameObject panelDialogueJoana;

    [Header("Input")]
    public TMP_InputField input;

    [Header("Puzzle Visual Lock (Panel que cobre a sala)")]
    public GameObject lockObject;

    [Header("Pulse do Device")]
    public PulseOutline pulse;
    private bool unlocked = false;

    [Header("Fade Global (Animator no Panel preto da tela inteira)")]
    public Animator fadeAnimator;
    public GameObject panelBlack;

    [Header("Referências Extras")]
    public LockObjectInteraction lockInteraction;
    public DialogueManager dialogueManager;

    [Header("Cena Boss")]
    public string bossSceneName = "BossRoom";
    public AudioClip bossMusic;

    [Header("Posição de spawn inicial na BossRoom")]
    [Tooltip("Coordenadas onde a player vai aparecer na primeira entrada na BossRoom.")]
    public Vector2 bossRoomSpawnPosition = new Vector2(-2.8f, -2.5f);

    private string correctPassword = "1541";
    private bool isTransitioning = false;

    void Start()
    {
        // Restaura o estado de desbloqueio após reload de página.
        if (PlayerPrefs.GetInt("BossRoomUnlocked", 0) == 1)
        {
            unlocked = true;
            if (lockObject != null) lockObject.SetActive(false);
        }
    }

    public void OpenPasswordPanel()
    {
        dialogueManager.OnClickExit();
        CanvasManager.Instance.OpenPanel(passwordPanel.name);
        if (pulse != null) pulse.StartPulsing();
    }

    public void ClosePasswordPanel()
    {
        CanvasManager.Instance.ClosedPanel(passwordPanel.name);
        if (pulse != null) pulse.StopPulsing();
        if (lockInteraction != null) lockInteraction.isUnlocked = true;
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
        if (panelDialogueJoana != null)
            CanvasManager.Instance.ClosedPanel(panelDialogueJoana.name);
    }

    public void PressKey(string value)
    {
        input.text += value;
    }

    public void ClearInput()
    {
        input.text = "";
    }

    public void PressEnter()
    {
        if (input.text == correctPassword)
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
        lockObject.SetActive(false);
        yield return new WaitForSeconds(3f);
        unlocked = true;

        // Persiste o desbloqueio e define a posição de spawn na BossRoom
        // (apenas se ainda não foi definida por uma sessão anterior).
        PlayerPrefs.SetInt("BossRoomUnlocked", 1);
        if (!PlayerPrefs.HasKey("BossRoom_PlayerX"))
        {
            PlayerPrefs.SetFloat("BossRoom_PlayerX", bossRoomSpawnPosition.x);
            PlayerPrefs.SetFloat("BossRoom_PlayerY", bossRoomSpawnPosition.y);
        }
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
        if (other.CompareTag("Player") && unlocked && !isTransitioning)
        {
            IniciarTransiçãoBoss(other);
        }
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
        if (bossMusic != null)
            BackgroundMusic.ChangeMusic(bossMusic);

        Debug.Log("Carregando cena: " + bossSceneName);
        SceneManager.LoadSceneAsync(bossSceneName);
    }

    public void FinalizarFade()
    {
        CanvasManager.Instance.ClosedPanel(panelBlack.name);
        isTransitioning = false;
    }
}
