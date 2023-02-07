# Balls (I Guess?)
Balls (I Guess?) is a 2D puzzle platformer developed in Unity.

It uses Unity's Input System and Localization packages. Any code
written specifically for this resides in Assets/Scripts.

Right now, this is a WIP. The following needs sorting:
 - Creation of levels (5 are implemented, up to 90 are planned)
 - The level select screen has some bugs in it as I tried to use numbers and that was a mistake

## Scenes
There are 2 scenes - the main menu, and the level.

The main menu switches between objects in a canvas, turning the level
select or options menus on when required.

The level loads in a prefab containing a room, then continues through
the prefabs until reaching the end.