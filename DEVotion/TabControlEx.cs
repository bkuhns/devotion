using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.ComponentModel;

using System.Windows.Forms;

namespace DEVotion
{
	public delegate bool OnCloseTab(int indx);
	public delegate void OnAfterCloseTab();

	class TabControlEx : TabControl
	{
		public TabControlEx() : base()
		{
			OnClose = null;
			this.DrawMode = TabDrawMode.OwnerDrawFixed;
		}

		public OnCloseTab OnClose;
		public OnAfterCloseTab OnAfterClose;

		private bool confirmOnClose = false;
		public bool ConfirmOnClose
		{
			get
			{
				return this.confirmOnClose;
			}
			set
			{
				this.confirmOnClose = value;
			}
		}
		protected override void OnDrawItem(DrawItemEventArgs e)
		{
			RectangleF tabTextArea = RectangleF.Empty;
			
			for(int nIndex = 0 ; nIndex < this.TabCount ; nIndex++)
			{
				if (TabPages[nIndex].Text.Trim() == TabPages[nIndex].Text)
					TabPages[nIndex].Text += "    ";

				if( nIndex != this.SelectedIndex )
				{
					/*if not active draw ,inactive close button*/
					tabTextArea = (RectangleF)this.GetTabRect(nIndex);
					//using(Bitmap bmp = Properties.Resources.delete)
					//{
					//    e.Graphics.DrawImage(bmp,
					//        tabTextArea.X+tabTextArea.Width -16, 5, 13, 13);
					//}
				}
				else
				{
					tabTextArea = (RectangleF)this.GetTabRect(nIndex);
					LinearGradientBrush br = new LinearGradientBrush(tabTextArea,
						SystemColors.ControlLightLight,SystemColors.Control,
						LinearGradientMode.Vertical);
					e.Graphics.FillRectangle(br,tabTextArea);

					/*if active draw ,inactive close button*/
					using(Bitmap bmp = Properties.Resources.delete)
					{
						e.Graphics.DrawImage(bmp,
							tabTextArea.X+tabTextArea.Width -16, 5, 13, 13);
					}
					br.Dispose();
				}
				string str = this.TabPages[nIndex].Text;
				StringFormat stringFormat = new StringFormat();
				stringFormat.Alignment = StringAlignment.Center; 
				using(SolidBrush brush = new SolidBrush(
					this.TabPages[nIndex].ForeColor))
				{
					/*Draw the tab header text*/
					tabTextArea.Offset((nIndex == this.SelectedIndex) ? - 7.0f : 0.0f, 2.0f);
					e.Graphics.DrawString(str,this.Font, brush,	tabTextArea,stringFormat);
				}
			}
		}
		protected override void OnMouseDown(MouseEventArgs e)
		{
			RectangleF tabTextArea = (RectangleF)this.GetTabRect(SelectedIndex);
			tabTextArea =
				new RectangleF(tabTextArea.X + tabTextArea.Width - 16, 5, 13, 13);
			Point pt = new Point(e.X, e.Y);
			if (tabTextArea.Contains(pt))
			{
				if (confirmOnClose)
				{
					if (MessageBox.Show("You are about to close " +
						this.TabPages[SelectedIndex].Text.TrimEnd() +
						" tab. Are you sure you want to continue?", "Confirm close",
						MessageBoxButtons.YesNo) == DialogResult.No)
						return;
				}
				//Fire Event to Client
				if (OnClose != null)
				{
					if (!OnClose(SelectedIndex)) return;
				}
				this.TabPages.Remove(SelectedTab);
				if (OnAfterClose != null)
					OnAfterClose();
			}
		}
	}
}
