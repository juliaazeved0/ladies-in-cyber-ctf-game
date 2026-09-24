#!/bin/bash

# Identidade do Luiz
USER="Luiz"
HOSTNAME="hidrogenio-pc"
HOME_DIR="/home/Luiz"
DIR="/home/Luiz"

update_prompt() {
    RELATIVE_PATH=$(echo $DIR | sed "s|$HOME_DIR|~|")
    PROMPT="\033[1;32m${USER}@${HOSTNAME}\033[0m:\033[1;34m${RELATIVE_PATH}\033[0m$ "
}

format_ls_la() {
    local file=$1
    local is_dir=$2
    local owner="Luiz"
    local group="users"
    
    if [ "$is_dir" = true ]; then
        echo -e "drwxr-xr-x  2 $owner $group  4096 Oct 12 09:15 $file"
    else
        echo -e "-rw-r--r--  1 $owner $group   234 Nov 05 14:30 $file"
    fi
}

update_prompt
clear
echo "ESTACAO DE TRABALHO - ANALISTA LUIZ"
echo "Digite 'help' para comandos."
echo ""

while true; do
    echo -ne "$PROMPT"
    read cmd args
    cmd=$(echo $cmd | xargs)
    args=$(echo $args | xargs | sed 's|/$||')

    case $cmd in
        "ls")
            SHOW_ALL=false
            SHOW_LONG=false
            
            # Ampliação das variações do comando ls
            case "$args" in
                *"-a "*|*"-a"|*"-la"*|*"-al"*|*"-l -a"*|*"-a -l"*) SHOW_ALL=true ;;
            esac
            case "$args" in
                *"-l "*|*"-l"|*"-la"*|*"-al"*|*"-a -l"*|*"-l -a"*) SHOW_LONG=true ;;
            esac

            files=""
            if [ "$DIR" == "/home/Luiz" ]; then 
                files="documentos/ sistema_2021.bak"
                [ "$SHOW_ALL" = true ] && files="$files .backup/"
            
            elif [ "$DIR" == "/home/Luiz/documentos" ]; then 
                files="listaCompras.txt notas.txt"
            
            elif [ "$DIR" == "/home/Luiz/.backup" ]; then 
                files=""
                [ "$SHOW_ALL" = true ] && files=".old/"
            
            elif [ "$DIR" == "/home/Luiz/.backup/.old" ]; then 
                files=""
                [ "$SHOW_ALL" = true ] && files=".cache/"
            
            elif [ "$DIR" == "/home/Luiz/.backup/.old/.cache" ]; then 
                files="notas.txt"
            fi
            
            # Imprime os diretórios raiz se for -a
            [ "$SHOW_ALL" = true ] && echo -e ".\n.."
            
            # Renderiza a lista dependendo de ter -l ou não
            for f in $files; do
                if [ "$SHOW_LONG" = true ]; then
                    is_d=false; [[ "$f" == */ ]] && is_d=true
                    format_ls_la "$f" "$is_d"
                else
                    echo -n "$f  "
                fi
            done
            # Quebra a linha final do ls, caso existam arquivos
            [ ! -z "$files" ] && echo "" 
            ;;

        "cd")
            if [[ "$args" == "documentos" ]]; then DIR="/home/Luiz/documentos"
            elif [[ "$args" == ".backup" ]]; then DIR="/home/Luiz/.backup"
            elif [[ "$args" == ".old" && "$DIR" == "/home/Luiz/.backup" ]]; then DIR="/home/Luiz/.backup/.old"
            elif [[ "$args" == ".cache" && "$DIR" == "/home/Luiz/.backup/.old" ]]; then DIR="/home/Luiz/.backup/.old/.cache"
            elif [[ "$args" == ".." ]]; then
                if [ "$DIR" == "/home/Luiz/documentos" ] || [ "$DIR" == "/home/Luiz/.backup" ]; then DIR="/home/Luiz"
                elif [ "$DIR" == "/home/Luiz/.backup/.old" ]; then DIR="/home/Luiz/.backup"
                elif [ "$DIR" == "/home/Luiz/.backup/.old/.cache" ]; then DIR="/home/Luiz/.backup/.old"
                else DIR="/home/Luiz"; fi
            else echo "-bash: cd: $args: Diretorio inexistente"; fi
            update_prompt ;;

        "cat")
            case $args in
                "listaCompras.txt") echo -e "Racao cachorro\nRacao gato\nWhey sabor cafe" ;;
                "notas.txt")
                    if [ "$DIR" == "/home/Luiz/.backup/.old/.cache" ]; then
                        # FLAG REMOVIDA. Substituída por mensagem narrativa.
                        echo -e "\033[1;32m> SUCESSO: Registro oculto encontrado.\033[0m"
                        echo -e "Nota de Luiz: 'Nunca confie no que esta na superficie.'"
                        
                        # Cria o arquivo de vitória para a Unity ler no Linux
                        touch vitoria.txt 
                        
                        echo "Fechando terminal em 5 segundos..."
                        sleep 5
                        exit 99
                    else echo "Pista: verifique o cache oculto."; fi ;;
                *) echo "Arquivo nao encontrado." ;;
            esac ;;

        "help") 
            echo -e "\033[1;33mComandos disponiveis:\033[0m"
            echo -e "  ls         - Lista os arquivos e pastas no diretorio atual."
            echo -e "  ls -a      - Lista todos os arquivos, incluindo os \033[1;31mocultos\033[0m."
            echo -e "  ls -l      - Exibe os arquivos em formato de lista detalhada."
            echo -e "  ls -la     - Combina parametros de arquivos ocultos e detalhes."
            echo -e "  cd [pasta] - Entra em uma pasta especifica. Use 'cd ..' para voltar."
            echo -e "  cat [arq]  - Le e exibe o conteudo de um arquivo de texto."
            echo -e "  clear      - Limpa a tela do terminal."
            echo -e "  exit       - Fecha o terminal."
            ;;
            
        "clear") clear ;;
        "exit") exit 0 ;;
        *) [ ! -z "$cmd" ] && echo "Comando nao encontrado." ;;
    esac
done