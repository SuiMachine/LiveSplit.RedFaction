﻿using System;
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
            new Mod("Base Game", new List<SplitStructOverall>()
            {
                new SplitLevelChange("Chapter 1 (Mines)", "l1s3.rfl", "l2s1.rfl"),
                new SplitLevelChange("Chapter 2 (Barracks)", "l2s3.rfl", "l3s1.rfl"),
                new SplitLevelChange("Chapter 3 (Reception && Docks)", "l3s4.rfl", "l4s1a.rfl"),
                new SplitLevelChange("Chapter 4 (Ventilation)", "l4s4.rfl", "l5s1.rfl"),
                new SplitLevelChange("Chapter 5 (Geothermal Plant)", "l5s4.rfl", "l6s1.rfl"),
                new SplitLevelChange("Chapter 6 (Administration)", "l6s3.rfl", "l7s1.rfl"),
                new SplitLevelChange("Chapter 7 (Backstage)", "l7s4.rfl", "l8s1.rfl"),
                new SplitLevelChange("Chapter 8 (Medical Labs)", "l8s4.rfl", "l9s1.rfl"),
                new SplitLevelChange("Chapter 9 (Caves)", "l9s4.rfl", "l10s1.rfl"),
                new SplitLevelChange("Chapter 10 (The \"Zoo\")", "l10s4.rfl", "l11s1.rfl"),
                new SplitLevelChange("Chapter 11 (Capek\'s Secret Facility)", "l11s3.rfl", "l12s1.rfl"),
                new SplitLevelChange("Chapter 12 (Canion)", "l12s1.rfl", "l13s1.rfl"),
                new SplitLevelChange("Chapter 13 (Satelite Control)", "l13s3.rfl", "l14s1.rfl"),
                new SplitLevelChange("Chapter 14 (Missile Command Center)", "l14s3.rfl", "l15s1.rfl"),
                new SplitLevelChange("Chapter 15 (Catch a Shuttle)", "l15s4.rfl", "l17s1.rfl"), //Chapter 16 missing, don't panic
				new SplitLevelChange("Chapter 16 (Space Station)", "l17s4.rfl", "l18s1.rfl"),
                new SplitLevelChange("Chapter 17 (Back on Mars)", "l18s3.rfl", "l19s1.rfl"),
                new SplitLevelChange("Chapter 18 (Merc\'s Base)", "l19s3.rfl", "l20s1.rfl"),
                new SplitLevelChange("Chapter 19 (Finale)", "l20s2.rfl", "l20s3.rfl"),
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
            new Mod("NGage SP Campaign Mod", new List<SplitStructOverall>()
            {
                new SplitLevelChange("The Mines (Pt. 1)", "l1s1.rfl", "ng1l2.rfl"),
                new SplitLevelChange("The Mines (Pt. 2)", "ng1l2.rfl", "ng1l3.rfl"),
                new SplitLevelChange("The Mines (Pt. 3)", "ng1l3.rfl", "ng2l1.rfl"),
                new SplitLevelChange("The Barracks (Pt. 1)", "ng2l1.rfl", "ng2l2.rfl"),
                new SplitLevelChange("The Barracks (Pt. 2)", "ng2l2.rfl", "ng3l1.rfl"),
                new SplitLevelChange("Docking Bay (Pt. 1)", "ng3l1.rfl", "ng3l2.rfl"), // docking bay 1
				new SplitLevelChange("Docking Bay (Pt. 2)", "ng3l2.rfl", "ng3l3.rfl"), // docking bay 2
				new SplitLevelChange("Docking Bay (Pt. 3)", "ng3l3.rfl", "ng4l1.rfl"), // docking bay 3
				new SplitLevelChange("Crevasse (Pt. 1)", "ng4l1.rfl", "ng4l2.rfl"), // crevasse 1
				new SplitLevelChange("Crevasse (Pt. 2)", "ng4l2.rfl", "ng5l1.rfl"), // crevasse 2
				new SplitLevelChange("Geothermal Power Plant", "ng5l1.rfl", "ng5l2.rfl"), // geothermal
				new SplitLevelChange("Underwater Tunnel", "ng5l2.rfl", "ng6l1.rfl"), // underwater tunnel
				new SplitLevelChange("Corporate HQ (Pt. 1)", "ng6l1.rfl", "ng6l2.rfl"), // corporate 1
				new SplitLevelChange("Corporate HQ (Pt. 2)", "ng6l2.rfl", "ng6l3.rfl"), // corporate 2
				new SplitLevelChange("Corporate HQ (Pt. 3)", "ng6l3.rfl", "ng7l1.rfl"), // corporate 3
				new SplitLevelChange("Medical Research (Pt. 1)", "ng7l1.rfl", "ng7l2.rfl"), // med lab 1
				new SplitLevelChange("Medical Research (Pt. 2)", "ng7l2.rfl", "ng8l1.rfl"), // med lab 2
				new SplitLevelChange("Capek\'s Lair", "ng8l1.rfl", "ng8l2.rfl"), // capeks lair
				new SplitLevelChange("Inner Sanctum", "ng8l2.rfl", "ng9l1.rfl"), // inner sanctum
				new SplitLevelChange("Merc Command Centre", "ng9l1.rfl", "ng9l2.rfl"), // merc centre
				new SplitLevelChange("Missile Battery", "ng9l2.rfl", "ng10l1.rfl"), // missile battery
				new SplitLevelChange("Masako\'s Lair (Pt. 1)", "ng10l1.rfl", "ng10l2.rfl"), // masako 1
				new SplitLevelChange("Masako\'s Lair (Pt. 2)", "ng10l2.rfl", "glass_house.rfl") // masako 2
			})
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
