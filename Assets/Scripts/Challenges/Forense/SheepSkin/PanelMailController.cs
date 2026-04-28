using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelMailController : MonoBehaviour
{
    [Header("Pain�is Principais")]
    public GameObject panelMailInput;
    public GameObject panelEmailRh;
    public GameObject panelEmailKassime;
    public GameObject panelEmailFran;
    public GameObject panelEmailAmin;
    public GameObject panelEmailAcessoTI;
    public GameObject panelGlitch;
    public GameObject panelInspect;
    public GameObject panelPhishing;
    public GameObject panelRansomware;

    [Header("Feedback Visual")]
    [SerializeField] private GameObject imagemSelecaoLink;

    public static bool pcInfectado = false; //Variavel para ver se a jogadora acessou o link infectado

    public void AbrirPanelMailInput() //Abrir o painel principais de e-mails
    {
        panelMailInput.SetActive(true);
    }

    public void FecharPanelMailInput() //Fechar o painel principal de e-mails
    {
        panelMailInput.SetActive(false);
    }

    //Funcoes para intercalar entre e-mails
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
        //Inicia a contagem automatica
        StartCoroutine(SequenciaGlitchParaRansomware());
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
    
    //Funcao para retornar a tela inicial de e-mails
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

    void Update()
    {
        //Booleano para verificar se a jogadora esta segurando a tecla Control, seja a da direita ou esquerda
        bool teclaPressionada = Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl);

        if(teclaPressionada && Input.GetKeyDown(KeyCode.U)) //Verifica se o Control est� sendo pressionado e a tecla U tamb�m, ao mesmo tempo
        {
            AbrirPanelInspect(); //Abre o painel com o link
        }
    }

    public void CopiarLink(string linkParaCopiar)
    {
        GUIUtility.systemCopyBuffer = linkParaCopiar; //Quando a jogadora clica no link, ele eh copiado

        if(imagemSelecaoLink != null)
        {
            StopAllCoroutines(); //Garante que o cronometro seja resetado antes de come�ar um novo
            StartCoroutine(EfeitoMarcaTexto());
        }
    }

    IEnumerator EfeitoMarcaTexto()
    {
        imagemSelecaoLink.SetActive(true); //Liga o marca-texto, ou seja, o destaque aparece no painel

        yield return new WaitForSeconds(2f); //O efeito dura por 2 segundos

        imagemSelecaoLink.SetActive(false); //Apos os 2 segundos, desliga o objeto, ou seja, o efeito de "selecionado" some automaticamente
    }

    IEnumerator SequenciaGlitchParaRansomware()
    {
        //Ativa o painel de anima��o Glitch
        panelGlitch.SetActive(true);

        //Espera exatamente 4 segundos para realizar a troca de paineis
        yield return new WaitForSeconds(4f);

        //Desativa o painel Glitch e ativa o painel de Ransomware
        panelGlitch.SetActive(false);
        panelRansomware.SetActive(true);

        //Marca que o PC agora esta no estado pos-ataque
        pcInfectado = true;
    }
}