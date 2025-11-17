# Audio Spectrum Sync Plugin (Prototype)

This plugin is a prototype based on work I did for an unreleased game project.  
It is already usable, but still a bit rough around the edges.  
A few updates are planned to make it more modular and expandable.

---

## Features

- Synchronize event calls with the spectrum of the sound currently playing in the game.
- Trigger an action when the spectrum reaches a certain threshold.
- Manually set a BPM value to get more consistent feedback.
- Modular spectrum visualizer that you can drop into the game, with different available shapes.

---

## To-Do

- Test compatibility with **Wwise** and **FMOD**.
- Add a BPM analyzer to make the setup even easier.
- Expand modularity (more sync option)
---

## To Fix / Known Issues

- Investigate performance on high-BPM music.
- Remove the **DOTween** dependency.
