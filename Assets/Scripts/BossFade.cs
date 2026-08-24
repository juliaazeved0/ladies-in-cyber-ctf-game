using UnityEngine;

/// <summary>
/// Garante que exista apenas um painel de fade "PanelFade" ativo na cena
/// do boss, destruindo qualquer duplicata remanescente ao carregar. 
/// </summary>
public class BossFadeManager : MonoBehaviour
{
    void Awake()
    {
        //Busca por um nome na hierarquia
        GameObject oldFade = GameObject.Find("PanelFade"); 
        
        if(oldFade != null && oldFade != this.gameObject)
        {
            Destroy(oldFade);
        }
    }

    /// <summary>
    /// Chamado ao final da animacao/efeito de fade do boss, desativando
    /// o painel para liberar a interacao da jogadora com aq cena novamente.
    /// </summary>
    public void FinishedFadeBoss()
    {
        gameObject.SetActive(false);
        
        Debug.Log("Luzes acesas! O painel foi desativado e os cliques liberados.");
    }
}