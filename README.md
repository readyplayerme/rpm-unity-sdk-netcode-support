# Ready Player Me Unity SDK - Netcode Support

This provides support for loading RPM avatar in a multiplayer application using [Netcode](https://unity.com/products/netcode) and [Ready Player Me Unity SDK](https://github.com/readyplayerme/rpm-unity-sdk-core). It also provides a working sample of a multiplayer.
This can be used as a reference for anyone wanting to use Ready Player Me Unity SDK to create a multiplayer using Unity Netcode.
The sample showcases a rudimentary PvP fighting game (e.g., Street Fighter, Tekken).

> Note: This sample doesn't contain a completed game but rather a working example of a very basic multiplayer fighting game.

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
- Copy git URL by clicking on green CODE button and then by clicking on copy button as shown.

  <img width="500" alt="image" src="https://github.com/readyplayerme/rpm-unity-sdk-netcode-support/assets/1121080/f16e66c4-f484-4ef4-9ce3-ad315e4aeb8f">

- Open `Windox > Package Manager` menu, click on Plus (+) button and select `Add package from git URL`.

  <img width="500" alt="image" src="https://github.com/readyplayerme/rpm-unity-sdk-photon-support/assets/3163281/54874f4e-8ff9-4011-aca1-0826ac0538a7">

- Paste the URL in and then click on `Add` button.

  <img width="500" alt="image" src="https://github.com/readyplayerme/rpm-unity-sdk-netcode-support/assets/1121080/c6309852-be91-47ba-8620-308f37e6c147">

- After package installed you should see it under Ready Player Me block.

  <img width="500" alt="image" src="https://github.com/readyplayerme/rpm-unity-sdk-netcode-support/assets/1121080/8de12393-233c-49e5-bb6d-6d099940740d">

## Testing the Sample Project
- Select Ready Player Me Netcode Support in Package manager and import the Basic Fighting Game sample.
- Open the menu scene.<br>
  <img width="500" alt="image" src="https://github.com/readyplayerme/rpm-unity-sdk-netcode-support/assets/1121080/a3ad9210-1e4d-4207-8281-dc503228a0b0">
- Add menu and game scene to build settings.
- Run the scene, paste your avatar URL, add name and click on start button.
- Build the scene and run it on another device to observe multiple avatars in the same scene.<br>
  <img width="500" alt="image" src="https://github.com/readyplayerme/rpm-unity-netcode-example/assets/1121080/b29dcb1d-5240-4249-b296-1abd6ee96caa">

## Netcode API Used 
#### [Network Mananger](https://docs-multiplayer.unity3d.com/netcode/current/components/networkmanager/)
- Used for starting as host, server, or client. 
- Provides API for checking if isHost, isOwner, and isClient. 
- For spawning player and fireball.

#### [Client Network Transform](https://docs-multiplayer.unity3d.com/netcode/current/components/networktransform/#owner-authoritative-mode)
- A component which synchronizes the position of the owner client to the server and all other client allowing for client authoritative gameplay.

#### [Network Variable](https://docs-multiplayer.unity3d.com/netcode/current/basics/networkvariable/)
- Synchronizes a property ("variable") between a server and client(s).
- Used for syncing player avatar url, name and animator parameters for movement.

#### [Server RPC](https://docs-multiplayer.unity3d.com/netcode/current/advanced-topics/message-system/serverrpc/)
- A remote procedure call (RPC) that can be only invoked by a client and will always be received and executed on the server/host.
- Used for spawning fireball on server when a client player fires it.

## TODO

- Use [Network Animator](https://docs-multiplayer.unity3d.com/netcode/current/learn/dilmer/networkanimator/) instead of syncing animator parameters using network variable.
- Fix player rotation sync.
- Animation for fireball.
