using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelMailController : MonoBehaviour
{
    [Header("Painéis Principais")]
    public GameObject panelMailInput;
    public GameObject panelEmailRh;
    public GameObject panelEmailKassime;
    public GameObject panelEmailFran;
    public GameObject panelEmailAmin;
    public GameObject panelEmailAcessoTI;
    public GameObject panelGlitch;
    public GameObject panelInspect;
    public GameObject panelPhishing;

    [Header("Feedback Visual")]
    [SerializeField] private GameObject imagemSelecaoLink;

    public void AbrirPanelMailInput() //Abrir o painel principais de e-mails
    {
        panelMailInput.SetActive(true);
    }

    public void FecharPanelMailInput() //Fechar o painel principal de e-mails
    {
        panelMailInput.SetActive(false);
    }

    //Funções para intercalar entre e-mails
    public void AbrirEmailRh()
    {
        panelEmailRh.SetActive(true);
    }

    public void AbrirEmailKassime()
    {
        panelEmailKassime.SetActive(true);
    }

    public void AbrirEmailFran()
    {
        panelEmailFran.SetActive(true);
    }

    public void AbrirEmailAmin()
    {
        panelEmailAmin.SetActive(true);
    }

    public void AbrirEmailAcessoTI()
    {
        panelEmailAcessoTI.SetActive(true);
    }

    public void AbrirPanelGlitch()
    {
        panelGlitch.SetActive(true);
    }

    public void AbrirPanelInspect()
    {
        if(panelInspect != null)
        {
            panelInspect.SetActive(true);
        }
    }

    public void AbrirPanelPhishing()
    {
        panelPhishing.SetActive(true);
    }
    
    //Função para retornar a tela inicial de e-mails
    public void RetornarEmailPrincipal()
    {
        if (panelEmailRh != null) panelEmailRh.SetActive(false);
        if(panelMailInput != null) panelMailInput.SetActive(true);

        if (panelEmailKassime != null) panelEmailKassime.SetActive(false);
        if(panelMailInput != null) panelMailInput.SetActive(true);

        if(panelEmailFran != null) panelEmailFran.SetActive(false);
        if(panelMailInput != null) panelMailInput.SetActive(true);

        if(panelEmailAmin != null) panelEmailAmin.SetActive(false);
        if(panelMailInput != null) panelMailInput.SetActive(true);

        if (panelEmailAcessoTI != null) panelEmailAcessoTI.SetActive(false);
        if(panelMailInput != null ) panelMailInput.SetActive(true);

        if(panelInspect != null) panelInspect.SetActive(false);

        if (panelPhishing != null) panelPhishing.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Update()
    {
        if (panelEmailAcessoTI != null && panelEmailAcessoTI.activeSelf)
        {
            // Agora o grupo dos Controls é avaliado primeiro, e o resultado DEVE ter o U junto
            if ((Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)) && Input.GetKeyDown(KeyCode.Y))
            {
                AbrirPanelInspect();
            }
        }
    }

    public void CopiarLink(string linkParaCopiar)
    {
        GUIUtility.systemCopyBuffer = linkParaCopiar;

        if(imagemSelecaoLink != null)
        {
            StopAllCoroutines();
            StartCoroutine(EfeitoMarcaTexto());
        }
    }

    IEnumerator EfeitoMarcaTexto()
    {
        imagemSelecaoLink.SetActive(true);

        yield return new WaitForSeconds(2f);

        imagemSelecaoLink.SetActive(false);
    }
}
