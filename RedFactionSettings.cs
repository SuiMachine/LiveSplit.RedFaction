using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;

namespace LiveSplit.RedFaction
{
	public partial class RedFactionSettings : UserControl
	{
		public bool AutoReset { get; set; }
		public bool AutoStart { get; set; }

		private const bool DEFAULT_AUTORESET = false;
		private const bool DEFAULT_AUTOSTART = true;

		[XmlArrayItem]
		public List<Mod> Mods = new List<Mod>()
		{
			new Mod("Base Game", new List<SplitStructOverall>()
			{
				new SplitLevelChange("Chapter 1 (Mines)", "l1s3.rfl", "l2s1.rfl"),
				new SplitLevelChange("Chapter 2 (Barracks)", "l2s3.rfl", "l3s1.rfl"),
				new SplitLevelChange("Chapter 3 (Reception && Docks)", "l3s4.rfl", "l4s1a.rfl"),

				//Fix that
				new SplitLevelChange("Chapter 4 (Ventilation)", "l4s1b.rfl", "l4s4.rfl"),
				new SplitLevelChange("Chapter 5 (Geothermal Plant)", "l5s1.rfl", ),
				new SplitLevelChange("Chapter 6 (Administration)", "l5s4.rfl", "l6s1.rfl"),
				new SplitLevelChange("Chapter 7 (Backstage)", "l6s3.rfl", "l7s1.rfl"),
				new SplitLevelChange("Chapter 8 (Medical Labs)", "l7s4.rfl", "l8s1.rfl"),
				new SplitLevelChange("Chapter 9 (Caves)", "l8s4.rfl", "l9s1.rfl"),
				new SplitLevelChange("Chapter 10 (The \"Zoo\")", "l9s4.rfl", "l10s1.rfl"),
				new SplitLevelChange("Chapter 11 (Capek\'s Secret Facility)", "l10s4.rfl", "l11s1.rfl"),
				new SplitLevelChange("Chapter 12 (Canion)", "l11s3.rfl", "l12s1.rfl"),
				new SplitLevelChange("Chapter 13 (Satelite Control)", "l12s1.rfl", "l13s1.rfl"),
				new SplitLevelChange("Chapter 14 (Missile Command Center)", "l13s3.rfl", "l14s1.rfl"),
				new SplitLevelChange("Chapter 15 (Catch a Shuttle)", "l14s3.rfl", "l15s1.rfl"),
				new SplitLevelChange("Chapter 16 (Space Station)", "l15s4.rfl", "l17s1.rfl"), //Chapter 16 missing
				new SplitLevelChange("Chapter 17 (Back on Mars)", "l17s4.rfl", "l18s1.rfl"),
				new SplitLevelChange("Chapter 18 (Merc\'s Base)", "l18s3.rfl", "l19s1.rfl"),
				new SplitLevelChange("Chapter 19 (Finale)", "l19s3.rfl", "l20s1.rfl"),
				//            public const string Chapter19End = "l20s2.rfl";

				new SplitVideoPlays("A Bomb!", "l20s3.rfl")
			})
		};


		public RedFactionSettings()
		{
			InitializeComponent();

			this.chkAutoReset.DataBindings.Add("Checked", this, "AutoReset", false, DataSourceUpdateMode.OnPropertyChanged);
			this.chkAutoStart.DataBindings.Add("Checked", this, "AutoStart", false, DataSourceUpdateMode.OnPropertyChanged);

			// defaults
			this.AutoReset = DEFAULT_AUTORESET;
			this.AutoStart = DEFAULT_AUTOSTART;
		}

		public XmlNode GetSettings(XmlDocument doc)
		{
			XmlElement settingsNode = doc.CreateElement("Settings");

			settingsNode.AppendChild(ToElement(doc, "Version", Assembly.GetExecutingAssembly().GetName().Version.ToString(3)));

			settingsNode.AppendChild(ToElement(doc, "AutoReset", this.AutoReset));
			settingsNode.AppendChild(ToElement(doc, "AutoStart", this.AutoStart));

			return settingsNode;
		}

		public void SetSettings(XmlNode settings)
		{
			this.AutoReset = ParseBool(settings, "AutoReset", DEFAULT_AUTORESET);
			this.AutoStart = ParseBool(settings, "AutoStart", DEFAULT_AUTOSTART);
		}

		static bool ParseBool(XmlNode settings, string setting, bool default_ = false)
		{
			bool val;
			return settings[setting] != null ?
				(Boolean.TryParse(settings[setting].InnerText, out val) ? val : default_)
				: default_;
		}

		static XmlElement ToElement<T>(XmlDocument document, string name, T value)
		{
			XmlElement str = document.CreateElement(name);
			str.InnerText = value.ToString();
			return str;
		}
	}
}
