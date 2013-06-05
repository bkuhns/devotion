using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DEVotion.Properties;

namespace DEVotion
{
	public partial class frmOptions : Form
	{
		public frmOptions()
		{
			InitializeComponent();
			btnFindFont.Font = DEVotion.Properties.Settings.Default.Style_Font;
		}

		private void btnFind_Click(object sender, EventArgs e)
		{
			dlgExeLocation.ShowDialog();
			if (!String.IsNullOrEmpty(dlgExeLocation.FileName))
			{
				txtEXELocation.Text = dlgExeLocation.FileName;
			}
		}

		private void picColor_Click(object sender, EventArgs e)
		{
			PictureBox p = (sender as PictureBox);
			if (p == null) return;

			dlgColor.Color = p.BackColor;
			if (dlgColor.ShowDialog() != DialogResult.Cancel)
			{
				p.BackColor = dlgColor.Color;
			}
		}

		private void btnCancel_Click(object sender, EventArgs e)
		{
			DEVotion.Properties.Settings.Default.Reload();
			Close();
		}

		private void btnOK_Click(object sender, EventArgs e)
		{
			DEVotion.Properties.Settings.Default.Save();
			Close();
		}

		private void btnFindFont_Click(object sender, EventArgs e)
		{
			dlgFont.Font = DEVotion.Properties.Settings.Default.Style_Font;
			if (dlgFont.ShowDialog() != DialogResult.Cancel)
			{
				DEVotion.Properties.Settings.Default.Style_Font = dlgFont.Font;
				btnFindFont.Font = dlgFont.Font;
			}
		}
	}
}
