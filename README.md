# Prototype_Unity

### Attention: It compiles and you can play it, but, you cannot login because currently I do not have a server to deploy the strapi project (CMS - Backend).

Most of the code is in the **"Assets/_App/Packages/"** folder, there you will find several folders such as **"MessageSystem"**, **"Mvc"**, **"Http"**, etc.
The goal I have is to transform each one of those folders into Packages to be able to add them to new projects using the Package Manager together with Verdaccio (npm).

Here is a brief description of each package:
**Core:** All the packages depend on Core, here I put the scripts that could be useful for any module.
**Authentification:** It is used to log in with a backend made with Strapi.
**FileLoader:** It is still in progress, the goal is to load files from cache if they exist or download them if necessary.
**GroupNodes:** Still in progress, the goal is to create a generic level selector that allows to unlock items, levels, play different games without changing the previously selected mode, etc.
**Http:** To facilitate the use of http request and to allow to modify in the future the way in which they are made without requiring changes in the ones that reference this module, for example, at the moment I use UniTask to make the request since it works in WebGL and it makes a smaller use of resources compared with the coroutines.
**MessageSystem:** This package implements the Observer pattern.
**Mvc:** This package implements the MVC pattern and its variants (adapted for Unity).
**ScriptTemplate:** It facilitates the creation of new scripts, it is similar to the **"Create/C# Script"** of Unity but adapted to my needs based on the packages that Mvc and Http (is useful to create new services). If in the future I want to make it work for another set of scripts, I can do it in a few minutes inheriting from the base scripts and adding more code without needing to modify the existing code.
**Topics:** It is a topic selector that serves to know what content I should show to the user.
**TweenSystem:** It is similar to LeanTween and DoTween but simplified and made by me. I made it a long time ago and I must modify it since it breaks several programming principles and design patterns.
**UIKit:** Simplifies the animation of screens, buttons, toggles, etc.

In the **"Assets/_App/Runtime"** folder you will find some scripts that are in progress and I am looking for the best way to package them.
