A game that runs in the Unity editor, without using Unity's play mode at all!

## How does it work?
This is accomplished thanks to a custom engine created with C# using Unity's types, such as Texture and the old GUI system; no custom internal/native functions were created as part of my main goal.

## Features:
- Oversimplified physics system to detect AABB collisions and ray casting.
- Uses the audio library "NAudio" to play SFX sounds and background music.
- Sprite Rendering uses the old GUI and calls Graphics.DrawTexture(), and it uses simple math (Not optimal, but for this project, the goal was not to use internal graphics functions, but 100% Unity's ones)
- Simple component system that resembles Unity3D's (GameObject, Components, Start(), Update() functions).
- Level editor to create, edit, and paint worlds in a tilemap system.
- AStar algorithm for the enemies' AI.
- Sprite animator.

## How to play?
Open the **Game** scene and select the **Game** GameObject in the **Hierarchy**. Ensure the inspector window is visible!

![Gameplay Demo](gamepreview.gif)

## Future?
This was a fun side project that allowed me to explore the limits of creating a game in the Inspector using only Unity’s built-in functionality.

My verdict? You 100% can, and you could even plug in a more robust physics library like Box2D if you wanted to.

There are still tons of things to fix and refactor, but in reality... making games with a full, “real” game engine, whether custom or commercial, is always a better idea than trying to build it all in the Inspector.

**Tested on Unity3D ver **2021.3.11f1**. Please use this version for initial testing; later ones might show mixed results.**
