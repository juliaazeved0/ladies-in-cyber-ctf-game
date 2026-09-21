using UnityEngine;

/// <summary>
/// Garante que exista apenas um painel de fade ativo na cena,
/// destruindo duplicatas encontrada por nome ao inicializar.
/// </summary>
public class BossFadeManager : MonoBehaviour
{
    void Awake()
    {
        /*Busca por nome (nao por Singleton) porque pode haver uma instancia "orfa"
         de PanelFade vinda de uma cena anterior. Se encontrar uma diferente deste
        proprio objeto, destroi a antiga para evitar duplicidade*/
        GameObject existingFadePanel = GameObject.Find("PanelFade"); 
        
        if(existingFadePanel != null && existingFadePanel != this.gameObject)
        {
            Destroy(existingFadePanel);
        }
        else if(existingFadePanel == null)
        {
            Debug.LogWarning("BossFadeManager: nenhum GameObject 'PanelFade' encontrado na cena. " + 
                             "Verifique se o nome do objeto não foi alterado ou se ele possui sufixo.");
        }
    }

    /// <summary>
    /// Callback chamado ao final da animacao de fade do Boss.
    /// Desativa o painel e libera a interacao da jogadora.
    /// </summary>
    public void FinishedFadeBoss()
    {
        gameObject.SetActive(false);
        
        Debug.Log("Luzes acesas! O painel foi desativado e os cliques liberados.");
    }
}