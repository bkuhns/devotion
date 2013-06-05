using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace DEVotion
{
	public partial class frmProjectProperties : Form
	{
		private string _config;
		private Dictionary<string, string> _settings = new Dictionary<string, string>();

		public frmProjectProperties(string ConfigFile)
		{
			InitializeComponent();

			_config = ConfigFile;
			LoadConfig();
		}
		private void LoadConfig()
		{
			if (File.Exists(_config))
			{
				string[] f = File.ReadAllLines(_config);

				_settings.Clear();
				foreach (string s in f)
				{
					List<string> bits = new List<string>(s.Split('='));
					string key = bits[0].Trim();
					bits.RemoveAt(0);

					string val = String.Join("=", bits.ToArray()).Trim();
					val = val.Trim('"');
					_settings.Add(key, val);
				}

				txtAuthor.Text = _settings.ContainsKey("author") ? _settings["author"] ?? "" : "";
				txtIcon.Text = _settings.ContainsKey("icon") ? _settings["icon"] ?? "" : "";
				txtTitle.Text = _settings.ContainsKey("title") ? _settings["title"] ?? "" : "";
				txtVersion.Text = _settings.ContainsKey("love_version") ? _settings["love_version"] ?? "0.5.0" : "";

				ddFSAABuffers.SelectedIndex = _settings.ContainsKey("fsaa") ? ddFSAABuffers.Items.IndexOf(_settings["fsaa"]) : 0;
				if (_settings.ContainsKey("width"))
				{
					if (ddResolution.Items.Contains(_settings["width"] + "x" + _settings["height"]))
					{
						ddResolution.SelectedIndex = ddResolution.Items.IndexOf(_settings["width"] + "x" + _settings["height"]);
						optStandard.Checked = true;
						chkDisplayAuto.Checked = true;
					}
					else
					{
						txtWidth.Text = _settings["width"];
						txtHeight.Text = _settings["height"];
						optManual.Checked = true;
						chkDisplayAuto.Checked = true;
					}
				}
				else
				{
					optUnspecified.Checked = true;
					ddResolution.SelectedIndex = -1;
					txtWidth.Text = txtHeight.Text = "";
					chkDisplayAuto.Checked = false;
				}
				chkDisplayAuto.Checked = _settings.ContainsKey("display_auto") ? (_settings["display_auto"] == "true") : false;
				chkFullscreen.Checked = _settings.ContainsKey("fullscreen") ? (_settings["fullscreen"] == "true") : false;
				chkVSync.Checked = _settings.ContainsKey("vsync") ? (_settings["vsync"] == "true") : false;
			}
		}
		private void PutSetting(string key, string value)
		{
			if (_settings.ContainsKey(key))
				_settings[key] = value;
			else
				_settings.Add(key, value);
		}
		private void picFindIcon_Click(object sender, EventArgs e)
		{
			OpenFileDialog dlg = new OpenFileDialog();
			dlg.DefaultExt = "ico";
			dlg.Filter = "Icon (*.ico)|*.ico";
			dlg.Title = "Select a Project Icon";
			if (dlg.ShowDialog() != DialogResult.Cancel)
			{
				txtIcon.Text = dlg.FileName;
			}
		}

		private void optManual_CheckedChanged(object sender, EventArgs e)
		{
			ddResolution.Enabled = false;
			txtHeight.Enabled = txtWidth.Enabled = true;
			chkDisplayAuto.Checked = true;
		}

		private void optStandard_CheckedChanged(object sender, EventArgs e)
		{
			ddResolution.Enabled = true;
			txtHeight.Enabled = txtWidth.Enabled = false;
			chkDisplayAuto.Checked = true;
		}

		private void btnOK_Click(object sender, EventArgs e)
		{
			// save changes to .conf
			PutSetting("author", "\"" + txtAuthor.Text + "\"");
			PutSetting("title", "\"" + txtTitle.Text + "\"");
			PutSetting("love_version", "\"" + txtVersion.Text + "\"");
			PutSetting("icon", "\"" + txtIcon.Text + "\"");
			if (optManual.Checked)
			{
				PutSetting("width", txtWidth.Text);
				PutSetting("height", txtHeight.Text);
			}
			else if (optStandard.Checked)
			{
				string[] dims = ddResolution.SelectedItem.ToString().Split('x');
				PutSetting("width", dims[0]);
				PutSetting("height", dims[1]);
			}
			else
			{
				PutSetting("width", "");
				PutSetting("height", "");
			}
			PutSetting("display_auto", chkDisplayAuto.Checked ? "true" : "false");
			PutSetting("fullscreen", chkFullscreen.Checked ? "true" : "false");
			PutSetting("vsync", chkVSync.Checked ? "true" : "false");
			PutSetting("fsaa", ddFSAABuffers.SelectedItem.ToString());

			StringBuilder conf = new StringBuilder();
			foreach (string key in _settings.Keys)
				if( !String.IsNullOrEmpty(_settings[key].Trim().Trim('"')))
					conf.AppendLine(string.Format("{0} = {1}", key, _settings[key]));

			File.WriteAllText(_config, conf.ToString());
			Close();
		}

		private void optUnspecified_CheckedChanged(object sender, EventArgs e)
		{
			ddResolution.Enabled = txtHeight.Enabled = txtWidth.Enabled = false;
			chkDisplayAuto.Checked = false;
		}
	}
}
