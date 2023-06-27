# Ready Player Me Unity SDK - Netcode Support

This provides support for loading RPM avatar in a multiplayer application using [Netcode](https://unity.com/products/netcode) and [Ready Player Me Unity SDK](https://github.com/readyplayerme/rpm-unity-sdk-core). This can be used as a reference for anyone wanting to use Ready Player Me Unity SDK to create a multiplayer using Unity Netcode. It also provides a working samples:
- Avatar Control - It showcases how to have a simple controls for avatar in a multiplayer game.
- Basic Fighting Game - It showcases a rudimentary PvP fighting game (e.g., Street Fighter, Tekken).

> Note: These sample doesn't contain a completed games but rather a working example of a very basic multiplayer.

## Requirements
- Unity Version 2021.3 or higher
- [Ready Player Me Core](https://github.com/readyplayerme/rpm-unity-sdk-core) - v1.2.0
- [Ready Player Me Avatar Loader](https://github.com/readyplayerme/rpm-unity-sdk-avatar-loader) - v1.2.0
- [Ready Player Me WebView](https://github.com/readyplayerme/rpm-unity-sdk-webview) - v1.1.1
- [glTFast](https://github.com/atteneder/glTFast) - v5.0.0
- [Multiplayer Samples Coop](https://docs-multiplayer.unity3d.com/netcode/current/components/networktransform/#owner-authoritative-mode) - main

### Dependencies included in package
- Netcode for Gameobjects - v1.4.0
- Multiplayer Tools - v1.1.0

## Installation
- Copy git URL by clicking on green Code button and then by clicking on copy button as shown.

  <img width="500" alt="image" src="https://github.com/readyplayerme/rpm-unity-sdk-netcode-support/assets/1121080/f2cfab7b-ac70-4120-ba13-4254647ad363">

- Open `Windox > Package Manager` menu, click on Plus (+) button and select `Add package from git URL`.

  <img width="500" alt="image" src="https://github.com/readyplayerme/rpm-unity-sdk-photon-support/assets/3163281/54874f4e-8ff9-4011-aca1-0826ac0538a7">

- Paste the URL in and then click on `Add` button.

  <img width="500" alt="image" src="https://github.com/readyplayerme/rpm-unity-sdk-netcode-support/assets/1121080/6b69f1b1-0453-4591-9c6a-017de113d40c">

- After package installed you should see it under Ready Player Me block.

  <img width="500" alt="image" src="https://github.com/readyplayerme/rpm-unity-sdk-netcode-support/assets/1121080/7f9620cd-40aa-4d9a-92ab-090607782558">

- To get support for [Client Network Transform](https://docs-multiplayer.unity3d.com/netcode/current/components/networktransform/#owner-authoritative-mode) a package has to be installed manually. 
  Copy the following url and past in `Add package from git URL` window in package manager.
  ```
  https://github.com/Unity-Technologies/com.unity.multiplayer.samples.coop.git?path=/Packages/com.unity.multiplayer.samples.coop#main
  ```

## Testing the Sample Project
### Avatar Control
- Select Ready Player Me Netcode Support in Package manager and import the Avatar Control sample.
  <img width="500" alt="image" src="https://github.com/readyplayerme/rpm-unity-sdk-netcode-support/assets/1121080/79dd9449-76d7-41df-ac43-bff53000281d">
- Open the Avatar Control scene.<br>
  <img width="500" alt="image" src="https://github.com/readyplayerme/rpm-unity-sdk-netcode-support/assets/1121080/c4cebd6e-2e22-4316-bc0a-09fe1f106e57">
- Add the scene to build settings.
- Run the scene, paste your avatar URL and click on start button.
- Build the scene and run it on another device to observe multiple avatars in the same scene.<br>
  <img width="500" alt="image" src="https://github.com/readyplayerme/rpm-unity-sdk-netcode-support/assets/1121080/5bcbf2a0-e113-403c-9327-18d8f5e14288">
### Simple Figthing Game
- Select Ready Player Me Netcode Support in Package manager and import the Basic Fighting Game sample.
  <img width="500" alt="image" src="https://github.com/readyplayerme/rpm-unity-sdk-netcode-support/assets/1121080/79401c2a-40e6-44d3-ac71-ceefda3252cd">
- Open the menu scene.<br>
  <img width="500" alt="image" src="https://github.com/readyplayerme/rpm-unity-sdk-netcode-support/assets/1121080/c53ee18b-3e64-4534-82e1-ae05aabf1736">
- Add menu and game scene to build settings.
- Run the scene, paste your avatar URL, add name and click on start button.
- Build the scene and run it on another device to observe multiple avatars in the same scene.<br>
  <img width="500" alt="image" src="https://github.com/readyplayerme/rpm-unity-sdk-netcode-support/assets/1121080/f00706c4-51af-4626-8b05-e4b56cd0ea5c">

## Netcode API Used 
#### [Network Mananger](https://docs-multiplayer.unity3d.com/netcode/current/components/networkmanager/)
- Used for starting as host, server, or client. 
- Provides API for checking if isHost, isOwner, and isClient. 
- For spawning player and fireball.

#### [Client Network Transform](https://docs-multiplayer.unity3d.com/netcode/current/components/networktransform/#owner-authoritative-mode)
- A component which synchronizes the position of the owner client to the server and all other client allowing for client authoritative gameplay.

#### [Network Variable](https://docs-multiplayer.unity3d.com/netcode/current/basics/networkvariable/)
- Synchronizes a property ("variable") between a server and client(s).
- Used for syncing player avatar url, name.

#### [Server RPC](https://docs-multiplayer.unity3d.com/netcode/current/advanced-topics/message-system/serverrpc/)
- A remote procedure call (RPC) that can be only invoked by a client and will always be received and executed on the server/host.
- Used for spawning fireball on server when a client player fires it.

#### [Network Animator](https://docs-multiplayer.unity3d.com/netcode/current/components/networkanimator/)
- A component that synchronizes the state of an Animator between a server and client(s).

## TODO

- Fix player rotation sync.
- Animation for fireball.

## Known Issues
- **Avatar Partially Loading:** If you did not use a Texture Atlas size selected in yout Avatar Config, you will receive avatar in multiple meshes and you might observe only eyes of the avatar being loaded and that you are getting an `IndexOutOfRangeException: Index was outside the bounds of the array.` error message. Multiple mesh avatars are not yet supported in the package so please use a config with atlas size selected.
- **Shared Access Violation Error:** When you test your build locally, all the game instances will try to write avatar file at the same time due to first time download and cause a file read error. Even though remote players won't experience this issue it is troubling while testing your app. You might test with same avatars and after they are cached this issue should not occur.
