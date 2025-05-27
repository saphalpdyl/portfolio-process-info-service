### What is this and how is it related to my portfolio ?
Below the about me section, my portfolio website has an awesome component that displays the selected currently opened applications in my local machine. For that, I needed to write a Windows Service that actively pinged the information to the web server, which is why `portfolio-process-info-service` was created.

### Tech Stack
[![Tech Stack](https://skillicons.dev/icons?i=cs,dotnet,windows)]()

### Installation
Open the solution in `Visual Studio` and run it in `Debug` mode, which will generate the necessary folder and configuration. The default configuration location is `C:\ProgramData\saphalpdyl_portfolio\config\service.config.json`. 

Add the `authorization_secret` that matches with the hosted NextJS application. 

Build the solution in `Release` mode. Open `Developer Console for Visual Studio 2022` which will have the path to `installutil`. Run this command:
```shell
installutil '<path_to_release_build.exe>'
```
