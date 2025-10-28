# Deploy

Deploy automatically re-deploys the platform with a single click. You save seconds each time you need to rebuild the project and restart the server and the application to test your new code changes.
<br>
Download the executable: https://drive.google.com/drive/folders/1jRGDC-J8TB_M-BRELUQnkWROjGm3s4oI?usp=sharing
<br><br>
![Screenshot](https://github.com/AndreiVaida/Deploy/blob/master/Resources/Screenshot_202025.01.08.png?raw=true "Screenshot")

## Instructions
1. Configure your projects and platform in `Configuration.ini`. This file must be available in the root folder of _Deploy.exe_. Configuration example:

```
[application]
application-process-name = MyApp
application-location = C:\Program Files\MyApp\MyApp.exe
application-arguments = username=andrei password=andrei
server-name = MyServer
server-window-name = My Server
server-start-file-relative-location = bin\start_server.bat
server-start-log = Server successfully started
project-cache-folder-to-delete = server\build\

[projects]
1 = Project 1,C:\Projects\project1,C:\Projects\Platforms\platform1
2 = Project 2,C:\Projects\project2,C:\Projects\Platforms\platform2
3 = Project 3,C:\Projects\project3,C:\Projects\Platforms\platform3
```
Configuration description:
- `application-process-name` (mandatory): the name of your app from Task Manager
- `application-location` (mandatory): the path to the executable of your app
- `application-arguments`: args to start your app
- `server-name`: (mandatory) the name of your server which will be displayed on UI (not yet implemented)
- `server-window-name` (mandatory): the name of the CMD window with your server
- `server-start-file-relative-location` (mandatory): the relative path to the `.bat` executable of your server, starting from the platform location
- `server-start-log` (mandatory): the line, logged in server logs, which tells that the server is started and ready for login
- `project-cache-folder-to-delete`: relative path, starting from `ProjectLocation`, of a folder to be deleted before building the project
- projects: enumerate, on each line, your `ProjectName,ProjectLocation,PlatformLocation`
  - `ProjectName`: to easily identify your project UI
  - `ProjectLocation`: path to the project's folder
  - `PlatformLocation`: path to the server's folder

2. Start Deploy.exe. You will see your configured projects. Click on the `🛠️ Build 🛠️` button to rebuild one.

## Features
### Start/Restart the platform
When you click the `🔁 Restart ▶️` button, Deploy (re)starts the platform.
1. Stop the application and the server (if they are open).
2. Start the server, read its logs in real-time and wait for the starting log.
3. Start the application.

## Re-deploy the platform
When you click the `🛠️ Build 🛠️` button, Deploy deploys your project in sequential and parallel steps.
1. Delete the `<ProjectLocation>\<project-cache-folder-to-delete>\ ` folder (move to Recycle Bin) if it's available in Configuration.ini.
2. Build your project with `gradlew assemble --parallel` in `ProjectLocation`.
3. Stop the application and the server immediately, without waiting for project building.
4. Copy the generated jar from `ProjectLocation\build\distributions\` to `PlatformLocation\lib\extensions\`. Delete the old jar (send to Recycle Bin).
5. Start the server, read its logs in real-time and wait for the starting log.
6. Start the application.