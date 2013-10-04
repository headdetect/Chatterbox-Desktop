Chatterbox-Desktop
==================

Chatterbox is an all around chat client that is designed for multiple protocols. 

## Supported Protocols
- Hipchat
- Skype
- Irc

# Plugins
Everything in chatterbox is designed around plugins. Each protocol will have its own plugin. Plugins are just a library where it contains a type that extends the base `ChatPlugin`. 

## Included Plugins
The plugins that are included in the source cannot all be placed in the plugin folder at the same time. (Yet)
As of now all plugins, after build, are placed in the `Plugins` folder in the `bin/Build/` folder.
If you do not want a plugin to be built. Unload the project before building.
