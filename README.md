A game that runs in the Unity editor, without using Unity's play mode at all!

## How does this work?
This is accomplished thanks to a custom engine created with C# using Unity's types, such as Texture and the old GUI system; no custom internal/native functions were created as part of my main goal.

## Features:
- A simple physics system to detect AABB collisions and ray casting.
- Uses the audio library "NAudio" to play SFX and music.
- Sprite Rendering uses the old GUI, just calling drawTexture, and simple math (Not optimal, but for this project, the goal was to not use internal graphics functions but 100% Unity's ones)
- Simple component system that resembles Unity3D's (GameObject, Components, Start(), Update() functions).
- Level editor to create, edit, and paint worlds in a tilemap system.
- AStar algorithm for the enemies.
- Sprite animator.

## How to play?
Open the **Game** scene and select the **Game** GameObject in the **Hierarchy**. Ensure the inspector window is visible!

![Gameplay Demo](gamepreview.gif)

## Future?
This was a fun side project to explore how far I can go with creating a game in the Inspector using just Unity’s built-in functionality.

My verdict? You 100% can, and you could even plug in a more robust physics library like Box2D if you wanted to.

Still tons of things to fix, but in reality… making games with a full, “real” game engine, custom or commercial, is 100% of the time a better idea than trying to build it all in the Inspector :)

**Tested on Unity3D ver **2021.3.11f1**. Please use this version for initial testing; later ones might show mixed results.**
