; Script generated by the Inno Setup Script Wizard.
; SEE THE DOCUMENTATION FOR DETAILS ON CREATING INNO SETUP SCRIPT FILES!

#define MyAppName "PC Timer Off"
#define MyAppVersion "1.0.0"
#define MyAppPublisher "Jefferson Dantas"
#define MyAppURL "https://github.com/josejefferson/pc-timer-off"

[Setup]
AppId={{07398FFC-DCC6-4532-A3B6-FCD1CF20B10B}
AppName={#MyAppName}
AppVersion={#MyAppVersion}
AppVerName={#MyAppName}
AppPublisher={#MyAppPublisher}
AppPublisherURL={#MyAppURL}
AppSupportURL={#MyAppURL}
AppUpdatesURL={#MyAppURL}
DefaultDirName={autopf}\{#MyAppName}
DefaultGroupName={#MyAppName}
DisableProgramGroupPage=yes
OutputBaseFilename=PC Timer Off
SetupIconFile=Shutdown Timer\shutdown.ico
Compression=lzma
SolidCompression=yes
WizardStyle=modern
UninstallDisplayIcon={app}\Shutdown Timer.exe
CloseApplications=force
AlwaysRestart=yes
VersionInfoVersion={#MyAppVersion}

[Languages]
Name: "brazilianportuguese"; MessagesFile: "compiler:Languages\BrazilianPortuguese.isl"

[Files]
Source: "Shutdown Timer\bin\Release\net8.0-windows\publish\win-x86\Shutdown Timer.exe"; DestDir: "{app}"; Flags: ignoreversion
Source: "Logoff Timer\bin\Release\net8.0-windows\publish\win-x86\Logoff Timer.exe"; DestDir: "{app}"; Flags: ignoreversion

[Registry]
; Inicia o software para logoff do usuário quando cada usuário fizer login
Root: HKLM; Subkey: "SOFTWARE\Microsoft\Windows\CurrentVersion\Run"; ValueName: "Logoff Timer"; ValueType: string; ValueData: "{app}\Logoff Timer.exe"; Flags: uninsdeletevalue

[Code]
// Retorna o XML para criação da tarefa no Agendador de Tarefas do Windows para iniciar o "Shutdown Timer.exe" junto ao sistema
function CreateTaskXML: String;
var
  TaskXML: String;
begin
  TaskXML := '<?xml version="1.0" encoding="UTF-16"?>' + #13#10 +
             '<Task version="1.2" xmlns="http://schemas.microsoft.com/windows/2004/02/mit/task">' + #13#10 +
             '  <RegistrationInfo>' + #13#10 +
             '    <Author>NT AUTHORITY\SYSTEM</Author>' + #13#10 +
             '    <Description>Desliga o computador quando nenhum usuário estiver logado</Description>' + #13#10 +
             '  </RegistrationInfo>' + #13#10 +
             '  <Triggers>' + #13#10 +
             '    <BootTrigger>' + #13#10 +
             '      <Enabled>true</Enabled>' + #13#10 +
             '    </BootTrigger>' + #13#10 +
             '  </Triggers>' + #13#10 +
             '  <Principals>' + #13#10 +
             '    <Principal id="Author">' + #13#10 +
             '      <UserId>S-1-5-18</UserId>' + #13#10 +
             '      <RunLevel>HighestAvailable</RunLevel>' + #13#10 +
             '    </Principal>' + #13#10 +
             '  </Principals>' + #13#10 +
             '  <Actions Context="Author">' + #13#10 +
             '    <Exec>' + #13#10 +
             '      <Command>"' + ExpandConstant('{app}\Shutdown Timer.exe') + '"</Command>' + #13#10 +
             '    </Exec>' + #13#10 +
             '  </Actions>' + #13#10 +
             '  <Settings>' + #13#10 +
             '    <DisallowStartIfOnBatteries>false</DisallowStartIfOnBatteries>' + #13#10 +
             '    <StopIfGoingOnBatteries>false</StopIfGoingOnBatteries>' + #13#10 +
             '  </Settings>' + #13#10 +
             '</Task>';

  Result := TaskXML;
end;

// Prepara a instalação do programa
function PrepareToInstall(var NeedsRestart: Boolean): String;
var
  TaskXML: String;
  XMLFileName: String;
  ResultCode: Integer;
begin
  // Gera o XML da tarefa
  TaskXML := CreateTaskXML;

  // Salva o XML em um arquivo temporário
  XMLFileName := ExpandConstant('{tmp}\task.xml');
  SaveStringToFile(XMLFileName, TaskXML, False);

  Exec('taskkill', '/f /im "Shutdown Timer.exe"', '', SW_HIDE, ewWaitUntilTerminated, ResultCode); // Termina o processo "Shutdown Timer.exe"
  Exec('taskkill', '/f /im "Logoff Timer.exe"', '', SW_HIDE, ewWaitUntilTerminated, ResultCode); // Termina o processo "Logoff Timer.exe"
  Sleep(2000);
  Exec('schtasks', '/delete /tn "Shutdown Timer" /f', '', SW_HIDE, ewWaitUntilTerminated, ResultCode); // Remove uma tarefa existente
  Exec('schtasks', '/create /tn "Shutdown Timer" /xml "' + XMLFileName + '"', '', SW_HIDE, ewWaitUntilTerminated, ResultCode); // Criar a tarefa agendada usando o XML
end;

[UninstallRun]
; Encerra os processos
Filename: "taskkill"; Parameters: "/f /im ""Shutdown Timer.exe"""; Flags: runhidden; RunOnceId: "KillShutdownTimer"
Filename: "taskkill"; Parameters: "/f /im ""Logoff Timer.exe"""; Flags: runhidden; RunOnceId: "KillLogoffTimer"
Filename: "timeout"; Parameters: "2"; Flags: runhidden; RunOnceId: "Wait"

; Remove a tarefa do Agendador de Tarefas do Windows
Filename: "schtasks"; Parameters: "/delete /tn ""Shutdown Timer"" /f"; Flags: runhidden; RunOnceId: "RemoveTask"

; Remove as configurações do programa do registro do Windows
Filename: "reg"; Parameters: "delete ""HKEY_LOCAL_MACHINE\SOFTWARE\PCTimerOff"" /f"; Flags: runhidden; RunOnceId: "RemoveRegistryKey"
Filename: "reg"; Parameters: "delete ""HKEY_LOCAL_MACHINE\SOFTWARE\WOW6432Node\PCTimerOff"" /f"; Flags: runhidden; RunOnceId: "RemoveRegistryKey"
