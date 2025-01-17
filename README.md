# Automatic Mouse Mover

[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://raw.githubusercontent.com/ebellocchia/auto_mouse_mover/master/LICENSE)

## Introduction

This small GUI application simulates a periodical user input by automatically moving the mouse cursor, written in C#.\
It may seem useless at a first glance, but I use it quite often on company laptops where the power options are locked and cannot be modified. My typical use case is for running demos in fairs or in front of customers, when you cannot move the mouse and you don't want the laptop to automatically lock the screen. In this way, you don't have to remember to move the mouse every now and them and you can concentrate on the demo.

## How it works

Once started, the application is minimized to the tray bar and runs in background. The user can choose if showing the tray bar icon to make it completely invisible.\
The application periodically checks the mouse position and simulates a mouse moving. In order to avoid disturbing the user if he's using the PC, the mouse is moved only if its position remained the same since the last time.\
The cursor is moved back and forth, so it always remains in the same spot and does not go outside the screen.\
In order to move the mouse automatically while in background, the application directly imports and calls the Windows APIs for sending input to the operating system and getting the cursor position, since there is no native function in C# for doing this (at least when I coded it).

The app supports multi-language via localization (a different form is created for each language, which is automatically selected by Windows depending on the system language). The current supported languages are English and Italian, but it can be easily extended to support other languages.

## Building

Just open the Visual Studio solution and build the project in Debug or Release.\
The output folder is *AutoMouseMover\bin*.

## Usage

For a basic usage, just open the application and click the *Start* button leaving the default settings.\
If you want (but it's not necessary, I only added it for completeness), you can select the time period in seconds and the number of pixels to move the cursor. In addition to this, you can choose if minimizing the application to tray bar (suggested) and if showing the tray bar icon. In case the tray bar icon is not shown, the only way to close the application after started is using the *Task Manager*.\
When minimized to the tray bar, you can open the application again by double-clicking the icon. You can also close it directly by right-clicking the icon and select *Close*.\
If you don't want to see the cursor moving by itself, just choose a small amount of moving pixels. The default value of 5 pixels should be enough (unless you stare at the cursor), at least on modern high-resolution screens.\
All the settings are saved when the application is closed and loaded when started, so you don't have to set them every time.

## License

This software is available under the MIT license.
