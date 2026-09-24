#!/bin/bash

# Configuração de Ambiente do BOSS
HOME_DIR="/home/BOSS"
DIR="/home/BOSS"

# Função para atualizar o prompt visual
update_prompt() { 
    PROMPT="\033[1;32mBOSS@MAIN_SERVER\033[0m:\033[1;34m$(echo $DIR | sed "s|$HOME_DIR|~|")\033[0m$ "
}

clear
echo -e "\033[1;31mSISTEMA OPERACIONAL CORE - ACESSO NIVEL 5\033[0m"
echo "Bem-vinda, Administradora. Acesso ao Terminal Liberado."
echo ""

while true; do
    update_prompt
    echo -ne "$PROMPT"
    read cmd args file_target
    
    # Limpeza de strings para evitar erros com espaços extras
    cmd=$(echo $cmd | xargs)
    args=$(echo $args | xargs | sed 's|/$||')
    file_target=$(echo $file_target | xargs | sed 's|/$||')

    case $cmd in
        "ls")
            if [ "$DIR" == "/home/BOSS" ]; then 
                echo -e "Documentos/  Imagens/  Projetos/"
            elif [ "$DIR" == "/home/BOSS/Documentos" ]; then 
                if [ -f "boss_resolvido.txt" ]; then 
                    echo "pasta vazia"
                else 
                    echo "praia.jpg"
                fi
            elif [ "$DIR" == "/home/BOSS/Imagens" ]; then
                if [ -f "boss_resolvido.txt" ]; then 
                    echo "praia.jpg"
                else 
                    echo "pasta vazia"
                fi
            else
                echo "system_core.bin  logs_setor_7.db"
            fi ;;

        "cd")
            if [[ "$args" == "Documentos" ]]; then DIR="/home/BOSS/Documentos"
            elif [[ "$args" == "Imagens" ]]; then DIR="/home/BOSS/Imagens"
            elif [[ "$args" == "Projetos" ]]; then DIR="/home/BOSS/Projetos"
            elif [[ "$args" == ".." ]]; then DIR="/home/BOSS"
            elif [[ "$args" == "~" ]]; then DIR="/home/BOSS"
            else echo "-bash: cd: $args: Diretorio nao encontrado"; fi ;;

        "mv")
            # Lógica de vitória: deve estar em Documentos e mover a foto para Imagens 
            if [[ "$DIR" == "/home/BOSS/Documentos" && "$args" == "praia.jpg" ]]; then
                if [[ "$file_target" == "../Imagens" || "$file_target" == "/home/BOSS/Imagens" ]]; then
                    echo "Iniciando transferencia de 'praia.jpg'..."
                    sleep 1
                    echo "Movendo para o diretorio de processamento esteganografico..."
                    
                    # Cria o sinalizador para a Unity liberar o aplicativo Steghide 
                    touch boss_resolvido.txt 
                    
                    sleep 2
                    echo -e "\033[1;32mSucesso! Arquivo movido. O Steghide ja pode processar a imagem.\033[0m"
                    sleep 1
                    exit 0 
                else
                    echo "Erro: Destino invalido. O processador exige o caminho ~/Imagens."
                fi
            else
                echo "Erro: Arquivo nao encontrado ou sintaxe de comando incorreta."
            fi ;;

        "help") echo "Comandos: ls, cd, mv, cat, clear, exit" ;;
        "cat") echo "cat: $args: Arquivo nao encontrado ou sem permissao de leitura." ;;
        "clear") clear ;;
        "exit") exit 0 ;;
        *) [ ! -z "$cmd" ] && echo "BASH: $cmd: comando nao encontrado" ;;
    esac
done
