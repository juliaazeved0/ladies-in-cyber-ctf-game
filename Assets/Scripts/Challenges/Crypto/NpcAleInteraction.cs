using UnityEngine;

public class NpcAleInteraction : NPCInteraction
{
    [Header("Nodes Alexandra")]
    public NPCDialogueNode sucessNode;
    public NPCDialogueNode finalNode;
    public PulseOutline nextChallengePulse;

    public string challenge1ID = "CryptoPassword";
    public string challenge2ID = "CryptoCapivara";

    protected override void Update()
    {
        if(CanvasManager.Instance != null && CanvasManager.Instance.IsAnyPanelOpen()) return;
        if (!playerIsHere || SimpleDialogue.isSimpleDialogueActive) return;

        if (Input.GetKeyDown(KeyCode.E))
        {
            SelectDialogue();
        }
    }

    public void SelectDialogue()
    {
    if (SimpleDialogue.isSimpleDialogueActive) return;


    CanvasManager.Instance.ToggleMiniMap(false);

    bool challenge1Done = ChallengeManager.Instance.IsChallengeCompleted(challenge1ID);
    bool challenge2Done = ChallengeManager.Instance.IsChallengeCompleted(challenge2ID);

    simpleDialogue.pulsingObject = !challenge1Done ? pulseObjectInitial : !challenge2Done ? nextChallengePulse : null;

    if (challenge1Done && challenge2Done)
    {
        simpleDialogue.StartDialogue(finalNode);
    }
    else if (challenge1Done)
    {
        simpleDialogue.StartDialogue(sucessNode);
    }
    else
    {
        simpleDialogue.StartDialogue(firstNode);
    }

    if (interactionNotice != null)
        interactionNotice.SetActive(false);
    }
}