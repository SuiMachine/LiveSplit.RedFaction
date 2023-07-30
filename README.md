LiveSplit.RedFaction
=====================

LiveSplit.RedFaction is a [LiveSplit](http://livesplit.org/) component for Red Faction (Pure Faction).

Features
--------
  * Keeps track of Game Time to get rid of loading times.
  * Auto start/stop the timer.
  * Splits when you finish certain splits for Inbounds runs. (configurable - can be set up to use main splits like the original)
  
Requirements
-------
* LiveSplit 1.6+
* Red Faction 1.20 (can be DashFaction) or Pure Faction 3.0d

Install
-------
Simply build the solution on your machine, and copy the created DLL to your LiveSplit/Components directory.
When you open Red Faction in LiveSplit you'll be prompted to update to latest package. Click "No" to use this custom DLL instead.

Normally I'd say take a backup before you do so, but given that LiveSplit recognises that this build does not match the latest, if this doesn't work for you for whatever reason you can simply press "Yes" when it asks you to update to get back to the original SuiMachine version

Configure
---------
Open your Splits Editor and active the autosplitter. If this is not working, leave it deactivated and manually add it in the Layout Editor. You can configure the settings in whichever editor it has been enabled in.
I've included a layout file which includes the new sub-splits

After configuring everything you'll most likely want to turn on game time as primary timing, so that your splits will run off game time. You can do this by right-clicking LiveSplit and going to Compare Against -> Game Time.

#### Auto Split
The default settings are to automatically reset, start, and end the splits (the first and last splits). You can enable individual splits here.

#### Alternate Timing Method
If you wish to show Real Time on your layout, download AlternateTimingMethod from the [LiveSplit Components page](http://livesplit.org/components/) or its own [Github page](https://github.com/Dalet/LiveSplit.AlternateTimingMethod/releases).

Change Log
----------
https://github.com/eckozero/LiveSplit.RedFaction/releases

Adding support for other mods
----------
To add other mods, see [https://github.com/SuiMachine/LiveSplit.RedFaction/wiki](https://github.com/SuiMachine/LiveSplit.RedFaction/wiki).

Credits
-------
  * [EckoZero](http://twitch.tv/eckozero1987)
  * Plugin is based off [LiveSplit.RedFaction](https://github.com/SuiMachine/LiveSplit.RedFaction) by [SuicideMachine](http://twitch.tv/suicidemachine)
