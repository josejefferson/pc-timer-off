# PC Timer Off

**PC Timer Off** é um software projetado para desligar automaticamente computadores que permanecem inativos por um período configurável. Ideal para economizar energia, otimizar recursos e reduzir custos operacionais.

[➡ Download da versão mais recente](https://github.com/josejefferson/pc-timer-off/releases/latest)

## Instruções de uso

### Instalação

Acesse a aba "Releases" e baixe a versão mais recente

[➡ Download da versão mais recente](https://github.com/josejefferson/pc-timer-off/releases/latest)

Abra o instalador e siga as instruções

### Requisitos

- Windows 7 ou superior
- 32 ou 64 bits

> [!IMPORTANT]
> Se você utilizar o **Windows 7**, é necessário instalar o [Microsoft Visual C++ 2015-2019 Redistributable](https://learn.microsoft.com/en-us/dotnet/core/install/windows?tabs=net60#additional-deps) e a atualização [KB3063858](https://learn.microsoft.com/en-us/dotnet/core/install/windows?tabs=net60#additional-deps)\
> Caso utilize o **Windows 8.1**, instale o [Microsoft Visual C++ 2015-2019 Redistributable](https://learn.microsoft.com/en-us/dotnet/core/install/windows?tabs=net60#additional-deps)

### Personalizar tempos de inatividade

Por enquanto não há uma interface gráfica para personalizar os tempos de inatividade. Para configurá-los é necessário editar o registro do Windows.

Abra o Prompt de Comando (`cmd.exe`) como administrador e execute os comandos abaixo de acordo com a arquitetura do seu sistema operacional, modificando os números no final do comando de acordo com a sua preferência.

```bat
:: --- WINDOWS 64 BITS ---

:: Tempo de inatividade para exibir o aviso de desconexão (em minutos)
reg add "HKLM\SOFTWARE\WOW6432Node\PCTimerOff" /v InactivityTime /t REG_DWORD /d 60 /f
:: Tempo após o aviso de desconexão para deslogar o usuário (em minutos)
reg add "HKLM\SOFTWARE\WOW6432Node\PCTimerOff" /v WarningTime /t REG_DWORD /d 5 /f
:: Tempo após o último usuário desconectar para desligar o computador (em minutos)
reg add "HKLM\SOFTWARE\WOW6432Node\PCTimerOff" /v ShutdownTime /t REG_DWORD /d 10 /f


:: --- WINDOWS 32 BITS ---

:: Tempo de inatividade para exibir o aviso de desconexão (em minutos)
reg add "HKLM\SOFTWARE\PCTimerOff" /v InactivityTime /t REG_DWORD /d 60 /f
:: Tempo após o aviso de desconexão para deslogar o usuário (em minutos)
reg add "HKLM\SOFTWARE\PCTimerOff" /v WarningTime /t REG_DWORD /d 5 /f
:: Tempo após o último usuário desconectar para desligar o computador (em minutos)
reg add "HKLM\SOFTWARE\PCTimerOff" /v ShutdownTime /t REG_DWORD /d 10 /f
```

> [!NOTE]
> Para aplicar as alterações, é necessário reiniciar o computador.

### Instalação silenciosa

```bat
PCTimerOff.exe /verysilent /norestart
```

## Como funciona

O projeto é composto por dois executáveis: `Logoff Timer.exe` e `Shutdown Timer.exe`, cada um com funções específicas no ambiente em que são executados.

### `Logoff Timer.exe` (Executado no ambiente de cada usuário)

Este executável é iniciado individualmente para cada usuário logado no computador. Ele monitora o tempo de inatividade do usuário e, quando o tempo configurado é ultrapassado:

1. Exibe um aviso na tela informando que o usuário será desconectado.
2. Após um período adicional configurável, o usuário é automaticamente deslogado.

![Logoff Timer](https://github.com/user-attachments/assets/99597a6e-3510-4843-80d6-375fd45f4a4f)

### `Shutdown Timer.exe` (Executado no ambiente do sistema)

Este executável opera em nível de sistema, monitorando a quantidade de usuários logados no computador. Quando não há mais usuários ativos:

1. Inicia um temporizador para desligar o computador após o tempo configurado.
2. Caso um novo login seja realizado antes do término do temporizador, ele é automaticamente cancelado.

![Shutdown Timer](https://github.com/user-attachments/assets/7866a1ae-7500-4473-a82e-0d21c825b47a)

## Tecnologias utilizadas

![Windows](https://img.shields.io/badge/Windows-0078D6?style=for-the-badge&logo=windows&logoColor=white)
![C#](https://img.shields.io/badge/c%23-%23239120.svg?style=for-the-badge&logo=csharp&logoColor=white)
![.Net](https://img.shields.io/badge/.NET-5C2D91?style=for-the-badge&logo=.net&logoColor=white)
![Visual Studio](https://img.shields.io/badge/Visual%20Studio-5C2D91.svg?style=for-the-badge&logo=visual-studio&logoColor=white)
![Inno Setup](https://img.shields.io/badge/Inno%20Setup-005799.svg?style=for-the-badge&logo=inno-setup&logoColor=white)
