#!/bin/bash

# Configuração de Identidade
USER="carlos"
HOSTNAME="itaipu-parquetec"
HOME_DIR="/var/www/html/h2_scada"
DIR="/var/www/html/h2_scada"
HAS_PERMISSION=false

update_prompt() {
    RELATIVE_PATH=$(echo $DIR | sed "s|$HOME_DIR|~|")
    PROMPT="\033[1;32m${USER}@${HOSTNAME}\033[0m:\033[1;34m${RELATIVE_PATH}\033[0m$ "
}

format_ls_la() {
    local file=$1
    local is_dir=$2
    local owner="carlos"
    local group="users"
    
    if [ "$is_dir" = true ]; then
        echo -e "drwxr-xr-x  2 $owner $group  4096 Feb 23 20:45 $file"
    else
        local perm="-rw-r--r--"
        if [[ "$file" == ".H2_reset.sh" && "$HAS_PERMISSION" = true ]]; then
            perm="-rwxr-xr-x"
        fi
        [[ "$file" == "log_erro.log" ]] && owner="root" && group="root"
        echo -e "$perm  1 $owner $group   802 Dec 14 22:10 $file"
    fi
}

update_prompt
clear
echo -e "\033[1;33mACESSO RESTRITO - ITAIPU PARQUETEC - MONITORAMENTO H2\033[0m"
echo "Sessao iniciada para: Engenheiro Carlos"
echo "Digite 'help' para comandos."
echo ""

while true; do
    echo -ne "$PROMPT"
    read cmd args
    cmd=$(echo $cmd | xargs)
    args=$(echo $args | xargs | sed 's|/$||')

    case $cmd in
        "whoami") echo "carlos" ;;
        "pwd") echo "$DIR" ;;
        "ls")
            SHOW_ALL=false
            SHOW_LONG=false
            [[ "$args" == *"-a"* ]] && SHOW_ALL=true
            [[ "$args" == *"-l"* ]] && SHOW_LONG=true

            files=""
            if [ "$DIR" == "/var/www/html/h2_scada" ]; then
                files="manual_eletrolise.txt log_erro.log config_vazamento.cfg sensores_temp.db logs/ backups/"
                # Adiciona o arquivo oculto apenas se usar -a
                [ "$SHOW_ALL" = true ] && files="$files .H2_reset.sh"
            
            elif [ "$DIR" == "/var/www/html/h2_scada/logs" ]; then
                files="system_uptime.log access_denied.log temp_history.csv"
            
            elif [ "$DIR" == "/var/www/html/h2_scada/backups" ]; then
                files="old_config.bak emergency_stop_v1.sh"
            fi

            [ "$SHOW_ALL" = true ] && echo -e ".\n.."
            
            for f in $files; do
                if [ "$SHOW_LONG" = true ]; then
                    is_d=false; [[ "$f" == */ ]] && is_d=true
                    format_ls_la "$f" "$is_d"
                else
                    echo -n "$f  "
                fi
            done
            echo "" ;;

        "cd")
            if [[ "$args" == "logs" ]]; then DIR="/var/www/html/h2_scada/logs"
            elif [[ "$args" == "backups" ]]; then DIR="/var/www/html/h2_scada/backups"
            elif [[ "$args" == ".." || "$args" == "~" ]]; then DIR="/var/www/html/h2_scada"
            else echo "-bash: cd: $args: No such directory"; fi
            update_prompt ;;

        "cat")
            case $args in
                "manual_eletrolise.txt")
                    echo -e "\033[1;36mMANUAL DE OPERACAO\033[0m\nNota: 'Aquilo que esta parado precisa receber o poder (+x) para agir.'" ;;
                "log_erro.log") echo -e "\033[0;31m[CRITICO] Monitoramento parado.\033[0m" ;;
                *) echo "Arquivo inexistente." ;;
            esac ;;

        "chmod")
            if [[ "$args" == "+x .H2_reset.sh" || "$args" == "+x ./.H2_reset.sh" ]]; then
                HAS_PERMISSION=true
                echo "Permissoes atualizadas."
            else echo "Uso: chmod +x [arquivo]"; fi ;;

        "./.H2_reset.sh" | ".H2_reset.sh")
            if [ "$HAS_PERMISSION" = true ]; then
                echo "> INICIANDO PROTOCOLO DE SEGURANCA...."
                sleep 1
                echo -e "\033[1;32m> FLAG: L1C{Pr3ss40_Est4b1l1z4d4}\033[0m"
                
                # CRUCIAL: Cria o arquivo de vitoria para a Unity no Linux
                touch vitoria_h2.txt 
                
                echo "Fechando em 5 segundos..."
                sleep 5
                exit 99
            else echo "BASH: Permission denied. (Dica: verifique as permissoes de execucao)"; fi ;;

        "help") 
            echo -e "\033[1;33mComandos disponiveis:\033[0m"
            echo -e "  ls           - Lista arquivos visiveis." 
            echo -e "  ls -a        - Lista arquivos visiveis e ocultos."
            echo -e "  ls -l        - Lista arquivos visiveis e detalhes."
            echo -e "  cd [pasta]   - Entra em um diretorio. Use 'cd ..' para voltar."
            echo -e "  cat [arq]    - Exibe o conteudo de um arquivo de texto."
            echo -e "  chmod +x     - Da permissao de execucao a um script."
            echo -e "  ./[arquivo]  - Executa um script na pasta atual."
            echo -e "  whoami       - Exibe o usuario logado."
            echo -e "  pwd          - Mostra o caminho da pasta atual."
            echo -e "  clear        - Limpa a tela do terminal."
            echo -e "  exit         - Encerra a sessao."
            ;;
            
        "clear") clear ;;
        "exit") exit 0 ;;
        *) [ ! -z "$cmd" ] && echo "BASH: $cmd: command not found" ;;
    esac
done