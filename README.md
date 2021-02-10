**SWBFLauncher** is an application with the original purpose of giving some extra support features to the game when launching it. Because the game is almost over 15 years old, I thought it would be a good idea to make a project like this one. Also this is my first project on GitHub as a non-professional purpose, I have some programming knowledge derived from what I'm currently studying. If you play the game, specially on Windows, I hope you consider trying it!

## Main features:
SWBFLauncher has the interface like any other launchers seen from different videogames. I put a lot of effort to make the design very intuitive for users so you do not get lost about what function does each button. Below I give a detailed description:

- **Play:** As indicates its label, it runs the game. At the time of execution the launcher will collect preset information from the settings.ini file attached with the main executable.
- **AddOns:** This button will open a new window called AddOns. As SWBF1 players know there is a folder called AddOn which you can install new maps to the game and play them without modyfing or replacing the original files. Its main purpose is to deal with AddOn folders as you want and give more information to the files, such as the in-game name, autor and description. Because the original AddOns does not include any additional information of its kind these two features will be optional and stored in a text file called `addinfo.txt`. There is also a Map Preview picture box where you can load an image to see how the map looks in-game by putting an `addinfo.png` file. Both files should be put into the appropiate `MapID` folder to make them work. And not for less, I included an ability to move the maps between AddOn and Mods folders if anytime some of you do not want to show specific maps in-game.

- **Mods:** As the previous button does, it brings you to a new window but supported for mods. When you click on it you will see a Message Box saying *This feature is not available yet*, because I decided not to implement this part due to its complexity. The main idea is to manage with modified files created by modders and replace them with any original files anytime you want and as you desire (i.e. install a list of modified files and then make a back-up of the stock files). In a similar way, you can also view additional information about the mod in question as is done with the AddOns.

- **Settings:** The most useful window, it displays several options which you can modify anytime and will affect to the game. Some of them are based on the command line options which originally can be used in the `battlefront.exe`. Others, such as the Language and the Resolution, are just new and adapted parameters. Each time you open this window it will collect information from `settings.ini` file, and if there are any changes it will save again to this file (except the profiles).

- **About:** A Message Box which shows copyright information about the application designer.

- **Exit:** Closes the application.

## Additional file/directory information:
| File/Directory | Description |
|--|--|
| `addinfo.txt` | `MapID` text information. First line is for the author, the rest is for the description. |
| `addinfo.png` | `MapID` map preview image. Proportions of the image must be 160x120. |
| `SWBFLauncher.exe` | Main executable. Must be opened along `winini.dll` file.  |
| `settings.ini` | Windows INI settings file. If it doesn't exist, `SWBFLauncher.exe` creates it. |
| `winini.dll` | Dynamic library which allows to use an INI class to load/modify/save `.ini` files. |
| `Data/_BACKUP` | Directory which saves original SWBF files in case of mod installation (`core.lvl` is saved here after language modification). If it doesn't exist, `SWBFLauncher.exe` creates it. |
| `Mods` | Directory which stores `MapID` folders unused in-game. If it doesn't exist, `SWBFLauncher.exe` creates it. |
## How to install:

 1. Open the `.zip` folder an extract the files in the folder you installed Star Wars Battlefront (2004) (`LucasArts\Star Wars Battlefront\GameData`).
2. Run the application. If you have problems while executing, open *Properties* and set *Run as administrator*.

**NOTE: Make sure you must extract the app with the `winini.dll` file to avoid problems.**
## Screenshots:
![Main menu](https://i.ibb.co/JczQhXp/Main-Menu.png)
![AddOns](https://i.ibb.co/9pjbBCB/AddOns.png)
![enter image description here](https://i.ibb.co/QKVHj1g/Settings.png)
## Download:
[Download v0.1](https://github.com/Red04akaCC4398/SWBFLauncher/releases/tag/v0.1)

**NOTE: It is probably the app will receive upcoming updates temporarily, so I recommend to check this page often if you are interested in upgrading to the most recent version.**

To sum up, don't be afraid to let me know about any problems or bugs. ;)

All icons used on this interface are free-licensed.

All credits goes to LucasArts/Pandemic for the Star Wars Battlefront (2004) logo.

Special thanks to Battlebelk and Dark Phanthom who discovered how to change resolution directly from process memory and language localization.
