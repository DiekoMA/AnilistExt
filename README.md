# Anilist Extension for CmdPal

This is an extension for [PowerToys](https://github.com/microsoft/PowerToys) [Command Palette](https://learn.microsoft.com/en-us/windows/powertoys/command-palette/overview) that allows you to search Anilist and Edit your anime and manga lists.

 This is a port of my Anilist plugin for [Flow Launcher](https://github.com/DiekoMA/Flow.Launcher.Plugin.Anilist) and should have the same features

![Main Screen](https://raw.githubusercontent.com/DiekoMA/AnilistExt/refs/heads/master/Assets/Snippet%201.png)

## Features

- Search for Manga and Anime through Anilist 
![Search Page](https://raw.githubusercontent.com/DiekoMA/AnilistExt/refs/heads/master/Assets/Snippet%202.png)
- Add and remove entries from your lists.
![enter image description here](https://raw.githubusercontent.com/DiekoMA/AnilistExt/refs/heads/master/Assets/Snippet%203.png)

## Upcoming Features
- More granular control over adding to and editing user lists
- Searching for users, characters and staff
- Dock integrations for your user profile and currently airing anime.

## Installation

> [!IMPORTANT]
> An Anilist account is required to use this extension

1. Ensure you have the [latest version](https://github.com/microsoft/PowerToys/releases/latest) of PowerToys installed.
2. Download the [newest release](https://github.com/DiekoMA/AnilistExt/releases/latest) zip file.
3. Extract the zip file.
4. Import the `AnilistExt.pfx`certificate (by double clicking on it and going through the import wizard).
	- Use `Local Machine` for the store location.
	- Use the following password when prompted for it: `N44Jgz#MWqfB3v`.
	- Place the certificate in the `Trusted People` store.
5. Execute the `AnilistExt` file to install the extension.
6. Type `anilist` in Command Palette. You should see `Anilist CMD Pal`. Hit `enter` and go through the process to get your token and authenticate with the extension.

## Contributing

Contributions are welcome! If you have any ideas, improvements, or bug fixes, please open an issue or submit a pull request.

To contribute to this project, follow these steps:

1. Fork the repository.
2. Create a new branch for your feature/fix.
3. Make your changes and commit them with descriptive commit messages.
4. Push your changes to your forked repository.
5. Submit a pull request to the main repository.

Please ensure that your code adheres to the existing code style. Also, make sure to update the documentation as needed.



## License

This project is licensed under the [MIT License](LICENSE)