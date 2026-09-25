using UnityEngine;

/// <summary>Contorno do objetivo atual, desligado quando a flag correspondente e coletada.</summary>
public class PulseOutline : MonoBehaviour
{
    [SerializeField] private float pulseSpeed = 0.05f;
    [SerializeField] private float maxThickness = 0.05f;
    [SerializeField] private bool startActive;
    [SerializeField] private int completionFlagIndex = -1;
    [SerializeField] private string highlightAfterChallenge;

    private Material myMaterial;
    private bool initialized;
    private bool isPulsing;
    private string completionFlag;
    private int thicknessID;
    private int alphaID;

    public bool IsPulsing => isPulsing;

    private bool IsCompleted => !string.IsNullOrEmpty(completionFlag) &&
        (FlagManager.Instance != null ? FlagManager.Instance.IsFlagCaptured(completionFlag) :
        FlagManager.HasSavedFlag(completionFlag));

    private void Awake() => Initialize();

    private void Initialize()
    {
        if(initialized) return;
        initialized = true;
        thicknessID = Shader.PropertyToID("_OutlineThickness");
        alphaID = Shader.PropertyToID("_OutlineAlphaMultiplier");
        completionFlag = SafeBase.GetFlag(completionFlagIndex);
        var sprite = GetComponent<SpriteRenderer>();
        if(sprite == null || sprite.sharedMaterial == null ||
           !sprite.sharedMaterial.HasProperty(thicknessID) || !sprite.sharedMaterial.HasProperty(alphaID))
        {
            Debug.LogWarning($"[PulseOutline] {name} precisa de SpriteRenderer com material de outline.", this);
            return;
        }
        myMaterial = sprite.material;
        isPulsing = !IsCompleted && (startActive || ChallengeManager.IsCompleted(highlightAfterChallenge));
        ApplyVisibility();
    }

    private void Update()
    {
        if(IsCompleted && isPulsing) StopPulsing();
        if(isPulsing && myMaterial != null)
            myMaterial.SetFloat(thicknessID, Mathf.PingPong(Time.time * Mathf.Max(0f, pulseSpeed),
                Mathf.Clamp(maxThickness, 0f, 0.1f)));
    }

    public void StartPulsing()
    {
        Initialize();
        if(!isActiveAndEnabled || IsCompleted || myMaterial == null) return;
        isPulsing = true;
        ApplyVisibility();
    }

    public void StopPulsing()
    {
        Initialize();
        isPulsing = false;
        ApplyVisibility();
    }

    private void ApplyVisibility()
    {
        if(myMaterial == null) return;
        myMaterial.SetFloat(alphaID, isPulsing ? 1f : 0f);
        if(!isPulsing) myMaterial.SetFloat(thicknessID, 0f);
    }

    private void OnDisable()
    {
        isPulsing = false;
        ApplyVisibility();
    }

    private void OnDestroy()
    {
        if(myMaterial != null) Destroy(myMaterial);
    }
}
