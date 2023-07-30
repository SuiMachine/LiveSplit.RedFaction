using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using System.Xml;

namespace LiveSplit.RedFaction
{
	public partial class RedFactionSettings : UserControl
	{
		public bool AutoReset { get; set; }
		public bool AutoStart { get; set; }
		public int ModIndex { get; set; }

		private const bool DEFAULT_AUTORESET = false;
		private const bool DEFAULT_AUTOSTART = true;
		private const int DEFAULT_MODINDEX = 0;
		private static readonly Mod[] DEFAULT_MODS = new Mod[]
		{
			new Mod("Base Game - Subsplits", new List<SplitStructOverall>()
			{
				new SplitLevelChange("Mines - Section 1", "l1s1.rfl", "l1s2.rfl"), // Pistol Miner
				new SplitLevelChange("Mines - Section 2", "l1s2.rfl", "l1s3.rfl"), // APC Corridor
				new SplitLevelChange("Mines - Total", "l1s3.rfl", "l2s1.rfl"), // Mine end
                new SplitLevelChange("Barracks - Section 1", "l2s1.rfl", "l2s2a.rfl"),
				new SplitLevelChange("Barracks - Section 2", "l2s2a.rfl", "l2s3.rfl"),
				new SplitLevelChange("Barracks - Total", "l2s3.rfl", "l3s1.rfl"),
				new SplitLevelChange("Reception && Docks - Section 1", "l3s1.rfl", "l3s2.rfl"),
				new SplitLevelChange("Reception && Docks - Section 2", "l3s2.rfl", "l3s3.rfl"),
				new SplitLevelChange("Reception && Docks - Section 3", "l3s3.rfl", "l3s4.rfl"),
				new SplitLevelChange("Reception && Docks - Total", "l3s4.rfl", "l4s1a.rfl"),
				new SplitLevelChange("Ventilation - Section 1a", "l4s1a.rfl", "l4s1b.rfl"),
				new SplitLevelChange("Ventilation - Section 1", "l4s1b.rfl", "l4s2.rfl"),
				// Vent skip causes this level to be super weird
                //new SplitLevelChange("L4S2", "l4s2.rfl", "l4s4.rfl"),
                //new SplitLevelChange("L4S4", "l4s4.rfl", "l4s5.rfl"),
                new SplitLevelChange("Ventilation - Total", "l4s4.rfl", "l5s1.rfl"),
				new SplitLevelChange("Geothermal - Section 1", "l5s1.rfl", "l5s2.rfl"),
				new SplitLevelChange("Geothermal - Section 2", "l5s2.rfl", "l5s3.rfl"),
				new SplitLevelChange("Geothermal - Section 3", "l5s3.rfl", "l5s4.rfl"),
				new SplitLevelChange("Geothermal Plant - Total", "l5s4.rfl", "l6s1.rfl"),
				new SplitLevelChange("Administration - Section 1", "l6s1.rfl", "l6s2.rfl"),
				new SplitLevelChange("Administration - Section 2", "l6s2.rfl", "l6s3.rfl"),
				new SplitLevelChange("Administration - Total", "l6s3.rfl", "l7s1.rfl"),
				new SplitLevelChange("Maintenance - Section 1", "l7s1.rfl", "l7s2.rfl"),
				new SplitLevelChange("Maintenance - Section 2", "l7s2.rfl", "l7s3.rfl"),
				new SplitLevelChange("Maintenance - Section 3", "l7s3.rfl", "l7s4.rfl"),
				new SplitLevelChange("Maintenance - Total", "l7s4.rfl", "l8s1.rfl"),
				new SplitLevelChange("Med Labs - Section 1", "l8s1.rfl", "l8s2.rfl"),
				new SplitLevelChange("Med Labs - Section 2", "l8s2.rfl", "l8s3.rfl"),
				new SplitLevelChange("Med Labs - Section 3", "l8s3.rfl", "l8s4.rfl"),
				new SplitLevelChange("Medical Labs - Total", "l8s4.rfl", "l9s1.rfl"),
				new SplitLevelChange("Caves - Section 1", "l9s1.rfl", "l9s2.rfl"),
				new SplitLevelChange("Caves - Section 2", "l9s2.rfl", "l9s3.rfl"),
				new SplitLevelChange("Caves - Section 3", "l9s3.rfl", "l9s4.rfl"),
				new SplitLevelChange("Caves - Total", "l9s4.rfl", "l10s1.rfl"),
				// lv10 skips s3 and s4 checkpoints/load zones
				new SplitLevelChange("The \"Zoo\" - Section 1", "l10s1.rfl", "l10s2.rfl"),
				new SplitLevelChange("The \"Zoo\" - Total", "l10s4.rfl", "l11s1.rfl"),
				new SplitLevelChange("Inner Sanctum - Section 1", "l11s1.rfl", "l11s2.rfl"),
				new SplitLevelChange("Inner Sanctum - Section 2", "l11s2.rfl", "l11s3.rfl"),
				new SplitLevelChange("Inner Sanctum - Total", "l11s3.rfl", "l12s1.rfl"),
				new SplitLevelChange("Canyon - Total", "l12s1.rfl", "l13s1.rfl"),
				new SplitLevelChange("Satelite Control - Section 1", "l13s1.rfl", "l13s3.rfl"),
				new SplitLevelChange("Satelite Control - Total", "l13s3.rfl", "l14s1.rfl"),
				new SplitLevelChange("Tramway - Section 1", "l14s1.rfl", "l14s2.rfl"),
				new SplitLevelChange("Tramway - Section 2", "l14s2.rfl", "l14s3.rfl"),
				new SplitLevelChange("Tramway - Total", "l14s3.rfl", "l15s1.rfl"),
				new SplitLevelChange("Catch a Shuttle - Section 1", "l15s1.rfl", "l15s2.rfl"),
				new SplitLevelChange("Catch a Shuttle - Total", "l15s4.rfl", "l17s1.rfl"), //Chapter 16 missing, don't panic
				new SplitLevelChange("Space Station - Section 1", "l17s1.rfl", "l17s2.rfl"),
				//new SplitLevelChange("Space Station - Section 2", "l17s2.rfl", "l17s3.rfl"),
				//new SplitLevelChange("Space Station - Section 3", "l17s3.rfl", "l17s4.rfl"),
				new SplitLevelChange("Space Station - Total", "l17s4.rfl", "l18s1.rfl"),
				new SplitLevelChange("Back on Mars - Section 1", "l18s1.rfl", "l18s2.rfl"),
				new SplitLevelChange("Back on Mars - Section 2", "l18s2.rfl", "l18s3.rfl"),
				new SplitLevelChange("Back on Mars - Total", "l18s3.rfl", "l19s1.rfl"),
				new SplitLevelChange("Merc Centre - Section 1", "l19s1.rfl", "l19s2a.rfl"),
				new SplitLevelChange("Merc Centre - Section 2a", "l19s2a.rfl", "l19s2b.rfl"),
				new SplitLevelChange("Merc Centre - Total", "l19s3.rfl", "l20s1.rfl"),
				// l20s1
				new SplitLevelChange("Finale - Section 1", "l20s1.rfl", "l20s2.rfl"),
				new SplitLevelChange("Finale - Total", "l20s2.rfl", "l20s3.rfl"),
				new SplitVideoPlays("A Bomb!", "l20s3.rfl")
			}),
			new Mod("Kava", new List<SplitStructOverall>()
			{
				new SplitLevelChange("Tram Station", "l1s1.rfl", "rfrev_kva00b.rfl"),
				new SplitLevelChange("Surface of the Red Planet", "rfrev_kva00b.rfl", "rfrev_kva00c.rfl"),
				new SplitLevelChange("Generator Room", "rfrev_kva00c.rfl", "rfrev_kva00d.rfl"),
				new SplitLevelChange("Power Plant", "rfrev_kva00d.rfl", "rfrev_kva00e.rfl"),
				new SplitLevelChange("Research Facility", "rfrev_kva00e.rfl", "rfrev_kva00f.rfl"),
				new SplitLevelChange("Solar Array", "rfrev_kva00f.rfl", "rfrev_kva01.rfl"),
				new SplitLevelChange("Storage Depot", "rfrev_kva01.rfl", "rfrev_kva02.rfl"),
				new SplitLevelChange("Barracks Basement", "rfrev_kva02.rfl", "rfrev_kva03.rfl"),
				new SplitLevelChange("Sewer Flow Control", "rfrev_kva03.rfl", "rfrev_kva04.rfl"),
				new SplitLevelChange("Sewer Control Area", "rfrev_kva04.rfl", "rfrev_kva05.rfl"),
				new SplitLevelChange("Sewage Treatment Plant", "rfrev_kva05.rfl", "rfrev_kva06.rfl"),
				new SplitLevelChange("Storage Area", "rfrev_kva06.rfl", "rfrev_kva07.rfl"),
				new SplitLevelChange("Central Command - Level 1", "rfrev_kva07.rfl", "rfrev_kva08.rfl"),
				new SplitLevelChange("Central Command - Level 2", "rfrev_kva08.rfl", "rfrev_kva09.rfl"),
				new SplitLevelChange("Central Command - Level 3", "rfrev_kva09.rfl", "rfrev_kva10.rfl"),
				new SplitLevelChange("Docking Bay Finale", "rfrev_kva10.rfl", "rfrev_kvaend.rfl")
			}),
		};

		public Mod[] Mods;

		public List<SplitStructOverall> CurrentSplits => Mods[ModIndex].Splits;

		public RedFactionSettings()
		{
			InitializeComponent();

			this.chkAutoReset.DataBindings.Add("Checked", this, "AutoReset", false, DataSourceUpdateMode.OnPropertyChanged);
			this.chkAutoStart.DataBindings.Add("Checked", this, "AutoStart", false, DataSourceUpdateMode.OnPropertyChanged);
			this.Cbox_Mod.Items.Clear();
			foreach (var mod in DEFAULT_MODS)
			{
				this.Cbox_Mod.Items.Add(mod);
			}

			this.Cbox_Mod.DataBindings.Add("SelectedIndex", this, "ModIndex", false, DataSourceUpdateMode.OnPropertyChanged);

			// defaults
			this.AutoReset = DEFAULT_AUTORESET;
			this.AutoStart = DEFAULT_AUTOSTART;
			this.ModIndex = DEFAULT_MODINDEX;
			this.Mods = new Mod[DEFAULT_MODS.Length];
			for (int i = 0; i < Mods.Length; i++)
			{
				Mods[i] = (Mod)DEFAULT_MODS[i].Clone();
			}
		}

		public XmlNode GetSettings(XmlDocument doc)
		{
			XmlElement settingsNode = doc.CreateElement("Settings");

			settingsNode.AppendChild(ToElement(doc, "Version", Assembly.GetExecutingAssembly().GetName().Version.ToString(3)));

			settingsNode.AppendChild(ToElement(doc, "AutoReset", this.AutoReset));
			settingsNode.AppendChild(ToElement(doc, "AutoStart", this.AutoStart));
			settingsNode.AppendChild(ToElement(doc, "ModIndex", this.ModIndex));
			settingsNode.AppendChild(ToElement(doc, "ModStates", this.Mods));

			return settingsNode;
		}

		public void SetSettings(XmlNode settings)
		{
			this.AutoReset = ParseBool(settings, "AutoReset", DEFAULT_AUTORESET);
			this.AutoStart = ParseBool(settings, "AutoStart", DEFAULT_AUTOSTART);
			this.ModIndex = ParseInt(settings, "ModIndex", DEFAULT_MODINDEX, 0, DEFAULT_MODS.Length - 1);
			Mods = ParseXML(settings, "ModStates", DEFAULT_MODS);
		}

		private Mod[] ParseXML(XmlNode settings, string setting, Mod[] default_)
		{
			var modParse = new Mod[default_.Length];
			for (int i = 0; i < modParse.Length; i++)
			{
				modParse[i] = (Mod)default_[i].Clone();
			}

			if (settings[setting] is null)
				return modParse;
			else
			{
				var node = settings[setting];
				foreach (XmlElement mod in node)
				{
					var modName = mod.Attributes["Name"];
					if (modName != null)
					{
						var foundMod = modParse.FirstOrDefault(x => x.ModName == modName.InnerText);
						if (foundMod != null)
						{
							if (mod["Splits"] != null)
							{
								var splitsNode = mod["Splits"];
								foreach (XmlElement split in splitsNode)
								{
									if (split.Attributes["Name"] != null)
									{
										var splitName = split.Attributes["Name"];
										var foundSplit = foundMod.Splits.FirstOrDefault(x => x.Name == splitName.InnerText);
										if (foundSplit != null)
										{
											foundSplit.Split = bool.TryParse(split.InnerText, out var parsedVal) ? parsedVal : false;
										}
									}
								}
							}
						}
					}
				}

				return modParse;
			}
		}

		static bool ParseBool(XmlNode settings, string setting, bool default_ = false)
		{
			bool val;
			return settings[setting] != null ?
				(Boolean.TryParse(settings[setting].InnerText, out val) ? val : default_)
				: default_;
		}

		static int ParseInt(XmlNode settings, string setting, int default_ = 0, int min = int.MinValue, int max = int.MaxValue)
		{
			int val;
			if (settings[setting] != null)
			{
				if (int.TryParse(settings[setting].InnerText, out val))
				{
					val = MathStuff.Clamp(val, min, max);
					return val;
				}
				else
					return default_;

			}
			return default_;
		}

		static XmlElement ToElement<T>(XmlDocument document, string name, T value)
		{
			XmlElement str = document.CreateElement(name);
			str.InnerText = value.ToString();
			return str;
		}

		static XmlElement ToElement(XmlDocument document, string name, Mod[] value)
		{
			//God help me
			XmlElement str = document.CreateElement(name);
			for (int i = 0; i < value.Length; i++)
			{
				var node = ToElement(document, value[i]);
				str.AppendChild(node);
			}

			return str;
		}

		static XmlElement ToElement(XmlDocument document, Mod value)
		{
			XmlElement str = document.CreateElement("Mod");
			var attr = document.CreateAttribute("Name");
			attr.InnerText = value.ModName;
			str.Attributes.Append(attr);
			var node = ToElement(document, value.Splits);
			str.AppendChild(node);
			return str;
		}

		static XmlElement ToElement(XmlDocument document, List<SplitStructOverall> value)
		{
			XmlElement str = document.CreateElement("Splits");
			for (int i = 0; i < value.Count; i++)
			{
				var node = ToElement(document, value[i]);
				str.AppendChild(node);
			}
			return str;
		}

		static XmlElement ToElement(XmlDocument document, SplitStructOverall value)
		{
			XmlElement str = document.CreateElement("Split");
			var attr = document.CreateAttribute("Name");
			attr.InnerText = value.Name;
			str.Attributes.Append(attr);
			str.InnerText = value.Split.ToString();
			return str;
		}

		private bool Initializing;

		private void Cbox_Mod_SelectedIndexChanged(object sender, EventArgs e)
		{
			CBList_Splits.DataSource = Mods[((ComboBox)sender).SelectedIndex].Splits;
			CBList_Splits.DisplayMember = "Name";
			CBList_Splits.ValueMember = "Split";

			Initializing = true;
			for (int i = 0; i < CBList_Splits.Items.Count; i++)
			{
				var obj = (SplitStructOverall)CBList_Splits.Items[i];
				CBList_Splits.SetItemChecked(i, obj.Split);
			}
			Initializing = false;
		}

		private void CBList_Splits_ItemCheck(object sender, ItemCheckEventArgs e)
		{
			if (!Initializing)
			{
				CurrentSplits[e.Index].Split = e.NewValue == CheckState.Checked;
			}
		}
	}
}
