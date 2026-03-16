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
            [[ "$args" == *"-a"* ]] && SHOW_ALL=true
            files=""
            if [ "$DIR" == "/home/Luiz" ]; then files="documentos/ .backup/ sistema_2021.bak"
            elif [ "$DIR" == "/home/Luiz/documentos" ]; then files="listaCompras.txt notas.txt"
            elif [ "$DIR" == "/home/Luiz/.backup" ]; then files=".old/"
            elif [ "$DIR" == "/home/Luiz/.backup/.old" ]; then files=".cache/"
            elif [ "$DIR" == "/home/Luiz/.backup/.old/.cache" ]; then files="notas.txt"; fi
            
            [ "$SHOW_ALL" = true ] && echo -e ".\n.."
            echo -e "$files" ;;

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
                        echo -e "\033[1;32mFLAG: L1C{FiqueAtentaAInformacoesEscondidas}\033[0m"
                        # CRUCIAL: Cria o arquivo de vitória para a Unity ler no Linux
                        touch vitoria.txt 
                        echo "Fechando em 5 segundos..."
                        sleep 5
                        exit 99
                    else echo "Pista: verifique o cache oculto."; fi ;;
                *) echo "Arquivo nao encontrado." ;;
            esac ;;

        "help") echo "Comandos: ls, ls -a, cd, cat, clear, exit" ;;
        "clear") clear ;;
        "exit") exit 0 ;;
        *) [ ! -z "$cmd" ] && echo "Comando nao encontrado." ;;
    esac
done
