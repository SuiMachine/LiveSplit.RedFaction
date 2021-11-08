LiveSplit.RedFaction
=====================

LiveSplit.RedFaction is a [LiveSplit](http://livesplit.org/) component for Red Faction (Pure Faction).

Features
--------
  * Keeps track of Game Time to get rid of loading times.
  * Auto start/stop the timer.
  * Splits when you finish each split in Any%. (configurable)
  
Requirements
-------
* LiveSplit 1.6+
* Red Faction 1.20 (can be DashFaction) or Pure Faction 3.0d

Install
-------
Starting with LiveSplit 1.4, you can download and install LiveSplit.RedFaction automatically from within the Splits Editor with just one click. Just type in "Red Faction" and click Activate. This downloads LiveSplit.RedFaction to the Components folder.

If the plugin is not working with this process, download the plugin from the [releases page](https://github.com/SuiMachine/LiveSplit.RedFaction/releases) and place the LiveSplit.RedFaction.dll in your Components directory of LiveSplit.

Configure
---------
Open your Splits Editor and active the autosplitter. If this is not working, leave it deactivated and manually add it in the Layout Editor. You can configure the settings in whichever editor it has been enabled in.

After configuring everything you'll most likely want to turn on game time as primary timing, so that your splits will run off game time. You can do this by right-clicking LiveSplit and going to Compare Against -> Game Time.

#### Auto Split
The default settings are to automatically reset, start, and end the splits (the first and last splits). You can enable individual splits here.

#### Alternate Timing Method
If you wish to show Real Time on your layout, download AlternateTimingMethod from the [LiveSplit Components page](http://livesplit.org/components/) or its own [Github page](https://github.com/Dalet/LiveSplit.AlternateTimingMethod/releases).

Change Log
----------
https://github.com/SuiMachine/LiveSplit.RedFaction/releases

Adding support for other mods
----------
To add other mods, see [https://github.com/SuiMachine/LiveSplit.RedFaction/wiki](https://github.com/SuiMachine/LiveSplit.RedFaction/wiki).

Credits
-------
  * [SuicideMachine](http://twitch.tv/suicidemachine)
  * Plugin is based off [LiveSplit.Dishonored](https://github.com/fatalis/LiveSplit.Dishonored) by [Fatalis](http://twitch.tv/fatalis_).
