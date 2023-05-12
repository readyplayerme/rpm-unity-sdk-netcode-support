# Ready Player Me Unity Netcode Integration Example

![image](https://github.com/readyplayerme/rpm-unity-netcode-example/assets/1121080/b29dcb1d-5240-4249-b296-1abd6ee96caa)

This repository contains a Unity project that uses [Netcode](https://unity.com/products/netcode) and Ready [Player Me Unity SDK](https://github.com/readyplayerme/rpm-unity-sdk-core) inside to demonstrate working example of a multiplayer. This project can be used as a reference for anybody wanting to use Ready Player Me Unity sdk to create a multiplayer using unity Netcode. 
It showcases a rudimentary PvP Fighting game(eg, Street Fighter, Tekken).

> Note: This repo doesn't contain a completed game but rather a working example of a very basic multiplayer fighting game.

## Dependencies
- Unity Version 2020.3 or higher
- [Ready Player Me Core](https://github.com/readyplayerme/rpm-unity-sdk-core) - v1.2.0
- [Ready Player Me Avatar Loader](https://github.com/readyplayerme/rpm-unity-sdk-avatar-loader) - v1.2.0
- [Ready Player Me WebView](https://github.com/readyplayerme/rpm-unity-sdk-webview) - v1.1.1
- [glTFast](https://github.com/atteneder/glTFast) - v5.0.0
- [Netcode for Gameobjects](https://docs-multiplayer.unity3d.com/netcode/current/installation/) - 1.2.0
- [Multiplayer Tools](https://docs-multiplayer.unity3d.com/tools/current/install-tools/) - 1.1.0
- [Multiplayer Samples Coop](https://docs-multiplayer.unity3d.com/netcode/current/components/networktransform/#owner-authoritative-mode) - main

## Quick Start
- Run from editor or build.
- Enter player name and a RPM avatar url. 
- Host a server or join as a client.
- Play the game.

## Netcode API Used 
#### [Network Mananger](https://docs-multiplayer.unity3d.com/netcode/current/components/networkmanager/)
- To start host, server, or client. 
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

### TODO

- Use [Network Animator](https://docs-multiplayer.unity3d.com/netcode/current/learn/dilmer/networkanimator/) instead of syncing animator parameters using network variable.
- Fix player rotation sync.
- Animation for fireball.
