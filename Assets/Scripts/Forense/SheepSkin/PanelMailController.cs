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
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
