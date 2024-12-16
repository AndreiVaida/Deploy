# Deploy

Deploy automatically re-deploys the platform with a single click. You save seconds each time you need to rebuild the project and restart the server and the application to test your new code changes.
<br>
Download the executable: https://drive.google.com/drive/folders/1jRGDC-J8TB_M-BRELUQnkWROjGm3s4oI?usp=sharing
<br><br>
![Screenshot](https://github.com/AndreiVaida/Deploy/blob/master/Resources/Screenshot_202024.11.16.png?raw=true "Screenshot")

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

[projects]
1 = Project 1,C:\Projects\project1,C:\Projects\Platforms\platform1
2 = Project 2,C:\Projects\project2,C:\Projects\Platforms\platform2
3 = Project 3,C:\Projects\project3,C:\Projects\Platforms\platform3
```
Configuration description:
- `application-process-name` (mandatory): the name of your app from Task Manager
- `application-location` (mandatory): the path to the executable of your app
- `application-arguments`: args to start your app
- `server-name`: the name of your server which will be displayed on UI (not yet implemented)
- `server-window-name` (mandatory): the name of the CMD window with your server
- `server-start-file-relative-location` (mandatory): the relative path to the `.bat` executable of your server, starting from the platform location
- projects: enumerate, on each line, your `ProjectName,ProjectLocation,PlatformLocation`
  - `ProjectName`: to easily identify your project UI
  - `ProjectLocation`: path to the project's folder
  - `PlatformLocation`: path to the server's folder

2. Start Deploy.exe. You will see your configured projects. Click on the `🛠️ Build 🛠️` button to rebuild one.

## Deploy steps
When you click the `🛠️ Build 🛠️` button, Deploy deploys your project in sequential and parallel steps.
1. Build your project: run `gradlew assemble --parallel` in `ProjectLocation`.
2. Stop the application and the service immediately, without waiting for project building.
3. Copy the generated jar from `ProjectLocation\build\distributions\` to `PlatformLocation\lib\extensions\`. Delete the old jar (send to Recycle Bin).
4. Start the server, read its logs in real-time and wait for the starting log.
5. Start the application.