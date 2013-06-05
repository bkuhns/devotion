using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using System.Xml;

using Alsing.SourceCode;
using Alsing.Windows.Forms;


namespace DEVotion
{
	public partial class Main : Form
	{
		LOVEProject project = null;
		SyntaxDefinition syntaxDefinition = null;
		frmOptions dlgOptions = new frmOptions();

		public Main()
		{
			InitializeComponent();
			
			SyntaxDefinitionLoader sl = new SyntaxDefinitionLoader();
			syntaxDefinition = sl.LoadXML(DEVotion.Properties.Resources.LUA.ToString());

			LoadSettings();

			tabDocs.OnClose += CloseTab;
			tabDocs.OnAfterClose += AfterCloseTab;
			UpdateButtons();
		}

		public bool IsDirty
		{
			get
			{
				foreach (TabPage p in tabDocs.TabPages)
				{
					SyntaxBoxControl syn = p.Controls[0] as SyntaxBoxControl;
					if (syn != null)
					{
						if (syn.Document.Modified) return true;
					}
				}
				return false;
			}
		}

		private void LoadSettings()
		{
			foreach (TextStyle t in syntaxDefinition.Styles)
			{
				if (t.Name != null)
				{
					string key = "Style_" + t.Name;
					t.ForeColor = (Color)Properties.Settings.Default[key + "_Color"];
					t.Bold = (bool)Properties.Settings.Default[key + "_Bold"];
					t.Italic = (bool)Properties.Settings.Default[key + "_Italic"];
				}
			}
		}

		private void SetUpSyntaxBox(SyntaxBoxControl syn)
		{
			syn.Document.Parser.Init(syntaxDefinition);
			
			syn.ShowLineNumbers = syn.ShowGutterMargin = true;
			syn.ReadOnly = false;
			syn.ParseOnPaste = true;
			syn.Indent = Alsing.Windows.Forms.SyntaxBox.IndentStyle.LastRow; // Alsing.Windows.Forms.SyntaxBox.IndentStyle.Smart;
			syn.ShowTabGuides = true;
			syn.Document.Folding = true;

			syn.AutoListIcons = ilTreeIcons;
						
			syn.Document.BreakPointAdded += new RowEventHandler(Document_BreakPointAdded);
			syn.KeyDown += new KeyEventHandler(syn_KeyDown);
			syn.SelectionChange += new EventHandler(syn_SelectionChange);
			syn.LostFocus += new EventHandler(syn_LostFocus);
			syn.OnAutoListTextInserted += new Alsing.Windows.Forms.SyntaxBox.EditViewControl.AutolistTextInsertedDelegate(InfoTipText);
			syn.InfoTipSelectedIndexChanged += new EventHandler(syn_InfoTipSelectedIndexChanged);
			Font f = Properties.Settings.Default.Style_Font;
			syn.FontName = f.FontFamily.Name;
			syn.FontSize = f.Size;
		}

		void syn_KeyDown(object sender, KeyEventArgs e)
		{
			switch (e.KeyCode)
			{
				case Keys.Left:
				case Keys.Right:
				case Keys.Down:
				case Keys.Up:
					break;
				case Keys.Tab:
					SyntaxBoxControl syn = tabDocs.SelectedTab.Controls[0] as SyntaxBoxControl;
					if (syn != null && syn.InfoTipVisible)
					{
						List<string> args = new List<string>();
						foreach (IntellisenseArgument a in grpOverloads.Overloads[syn.InfoTipSelectedIndex-1].Arguments)
							args.Add(a.Argument);

						syn.Document.InsertText(String.Join(", ", args.ToArray()), syn.Caret.Position.X, syn.Caret.Position.Y);
						e.Handled = true;
					}
					break;
				default:
					tmrInterval.Stop();
					tmrInterval.Start();
					break;
			}
		}

		void tmrInterval_Tick(object sender, EventArgs e)
		{
			tmrInterval.Stop();
			
			SyntaxBoxControl syn = (tabDocs.SelectedTab.Controls[0] as SyntaxBoxControl);
			if (syn == null) return;

			syn.AutoListVisible = false;
			
			bool matches = false;
			
			syn.AutoListClear();
			
			Word w = syn.Document.GetWordFromPos(syn.Caret.Position);
			if (w == null) return;

			string txt = w.Text, word = "";

			if (!String.IsNullOrEmpty(txt) && txt.Length > 2)
			{

				syn.AutoListBeginLoad();

				word = txt.Substring(0, 3);
				PatternCollection pc = (syntaxDefinition.SpanDefinitions[0].LookupTable[word] as PatternCollection);
				if (pc == null)
				{
					syn.AutoListEndLoad();
					return;
				}
				foreach (Pattern p in pc)
				{
					if (p.StringPattern.Length >= txt.Length && p.StringPattern.Substring(0, txt.Length) == txt)
					{
						syn.AutoListAdd(p.StringPattern, 0);
						matches = true;
					}
				}

				syn.AutoListEndLoad();
				if (matches)
				{
					syn.AutoListVisible = true;
					syn.InfoTipVisible = false;
				}
			}
			else if(txt.Trim()=="")
			{
				syn.InfoTipVisible = false;
			}
		}

		private struct IntellisenseArgument 
		{
			public string Argument;
			public string Description;

			public IntellisenseArgument(string arg, string desc) { Argument = arg; Description = desc; }
		}
		private struct IntellisenseOverload
		{
			public string ReturnArgument;
			public List<IntellisenseArgument> Arguments;

			public IntellisenseOverload(string returnarg, List<IntellisenseArgument> args)
			{
				ReturnArgument = returnarg;
				Arguments = args;
			}
		}
		private struct IntellisenseGroup
		{
			public int Index;
			public string Method;
			public List<IntellisenseOverload> Overloads;
		}

		IntellisenseGroup grpOverloads = new IntellisenseGroup();
		
		void syn_InfoTipSelectedIndexChanged(object sender, EventArgs e)
		{
			tmrInterval.Stop();
			SyntaxBoxControl syn = (tabDocs.SelectedTab.Controls[0] as SyntaxBoxControl);
			grpOverloads.Index = syn.InfoTipSelectedIndex;
			if (grpOverloads.Index > grpOverloads.Overloads.Count)
				return; // invalid index

			//syn.InfoTipCount = grpOverloads.Overloads.Count;
			List<string> args = new List<string>();
			List<string> desc = new List<string>();
			foreach (IntellisenseArgument a in grpOverloads.Overloads[grpOverloads.Index-1].Arguments)
			{
				args.Add(a.Argument);
				desc.Add(String.Format("<b>{0}</b>: {1}", a.Argument, a.Description));
			}
			syn.InfoTipPosition = new TextPoint(syn.Caret.Position.X, syn.Caret.Position.Y + 1);
			syn.InfoTipText = String.Format("<b>{0}(<i>{1}</i>)</b><hr /><br />{2}", grpOverloads.Method, String.Join(", ", args.ToArray()), String.Join("<br />", desc.ToArray()));
			//syn.InfoTipVisible = true;
		}

		void InfoTipText(string method)
		{
			XmlDocument doc = new XmlDocument();
			doc.Load((new FileInfo(Application.ExecutablePath)).DirectoryName + "\\Intellisense.xml");
			XmlNode node = doc.SelectSingleNode("signatures/signature[@name='" + method + "']");
			if (node != null)
			{
				grpOverloads = new IntellisenseGroup();
				grpOverloads.Overloads = new List<IntellisenseOverload>();

				// hide autolist, display infobox!
				SyntaxBoxControl syn = (tabDocs.SelectedTab.Controls[0] as SyntaxBoxControl);
				syn.AutoListVisible = false;

				syn.Document.InsertText("()", syn.Caret.Position.X, syn.Caret.Position.Y);
				syn.Caret.MoveRight(false);

				XmlNodeList nl = node.SelectNodes("overload");
				foreach (XmlNode n in nl)
				{
					List<IntellisenseArgument> args = new List<IntellisenseArgument>();
					foreach (XmlNode x in n.SelectNodes("arg"))
					{
						args.Add(new IntellisenseArgument(x.Attributes["name"].Value.ToString(), x.InnerText));
					}
					grpOverloads.Overloads.Add(new IntellisenseOverload(node["returns"].Value, args));

				}
				grpOverloads.Method = method;
				syn.InfoTipCount = grpOverloads.Overloads.Count;
				syn.InfoTipVisible = true;
				tmrInterval.Stop();
				syn.AutoListVisible = false;
				//MessageBox.Show(node.InnerXml);
			}
		}

		void syn_LostFocus(object sender, EventArgs e)
		{
			SyntaxBoxControl syn = (sender as SyntaxBoxControl);
			syn.AutoListVisible = false;
		}

		void Document_BreakPointAdded(object sender, RowEventArgs e)
		{
			e.Row.Bookmarked = !e.Row.Bookmarked;
			synCode.Document.ClearBreakpoints();
		}
	
		void syn_SelectionChange(object sender, EventArgs e)
		{
			UpdateButtons();
		}

		
		private void ResetTabs()
		{
			tabDocs.TabPages.Clear();
			//AddTabLUA("New Document", null, "");
			UpdateButtons();
		}
		private void AddTabLUA(string tabname, string tag, string content)
		{
			TabPage tab = new TabPage(tabname);
			SyntaxBoxControl con = new SyntaxBoxControl();
			con.Dock = DockStyle.Fill;
			SetUpSyntaxBox(con);
			
			tab.Tag = tag;
			con.Document.Text = content;

			tab.Controls.Add(con);
			tabDocs.TabPages.Add(tab);

			tabDocs.SelectedIndex = tabDocs.TabPages.Count - 1;
			synCode = (tabDocs.SelectedTab.Controls[0] as SyntaxBoxControl);

			UpdateButtons();
		}
		private bool CloseTab(int idx)
		{
			SyntaxBoxControl syn = tabDocs.TabPages[idx].Controls[0] as SyntaxBoxControl;
			if (syn != null)
			{
				if (syn.Document.Modified)
				{
					DialogResult r = MessageBox.Show("You have unsaved changes in \"" + tabDocs.TabPages[idx].Text.Trim() + "\". Save now?", "Unsaved Changes", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
					if (r == DialogResult.Cancel) return false;
					if (r == DialogResult.Yes)
					{
						string path = project.PathFromTag(tabDocs.TabPages[idx].Tag);
						File.WriteAllText(path, syn.Document.Text);
					}
				}
			}
			return true;
		}
		private void AfterCloseTab()
		{
			synCode = tabDocs.SelectedTab == null ? null : (tabDocs.SelectedTab.Controls[0] as SyntaxBoxControl);
			UpdateButtons();
		}
		private void AddTabImage(string tabname, string tag, string file)
		{
			TabPage tab = new TabPage(tabname);

			PictureBox pic = new PictureBox();
			pic.Load(file);
			//pic.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			pic.SizeMode = PictureBoxSizeMode.AutoSize;
			pic.SizeMode = PictureBoxSizeMode.Zoom;
			pic.Tag = 1;

			tab.Controls.Add(pic);
			tab.MouseWheel += new MouseEventHandler(tab_ImageMouseWheel);
			tab.Tag = tag;
			
			tabDocs.TabPages.Add(tab);

			pic.Left = (tab.Width - pic.Width) / 2;
			pic.Top = (tab.Height - pic.Height) / 2;

			tabDocs.SelectedIndex = tabDocs.TabPages.Count - 1;
			UpdateButtons();
		}

		void tab_ImageMouseWheel(object sender, MouseEventArgs e)
		{
			TabPage t = sender as TabPage;
			if (t != null)//
			{
				
				PictureBox pic = t.Controls[0] as PictureBox;
				pic.Hide();
				float s = 1.0f + (e.Delta / 120)*0.1f;
				pic.Scale(new SizeF(s, s));


				pic.Left = (t.Width - pic.Width) / 2;
				pic.Top = (t.Height - pic.Height) / 2;
				pic.Show();
			}
		}

		private void openProjectToolStripMenuItem_Click(object sender, EventArgs e)
		{
			OpenFileDialog dlg = new OpenFileDialog();
			dlg.DefaultExt = "love";
			dlg.Filter = "LÖVE Packages (*.love)|*.love";
			dlg.Title = "Select a Project";

			if (dlg.ShowDialog() != DialogResult.Cancel)
			{

				if (!String.IsNullOrEmpty(dlg.FileName))
				{
					if (project != null)
					{
						CloseProject();
						project = null;
					}

					project = new LOVEProject(dlg.FileName, treeProject);
					
					ResetTabs();

					UpdateButtons();
				}
			}
		}

		private TabPage FindTabByTag(object tag, bool isSyntaxBox)
		{
			TabPage blank = null;

			for (int i = 0; i < tabDocs.TabPages.Count; i++)
			{
				TabPage tab = tabDocs.TabPages[i];
				if (tab.Tag != null && tab.Tag.ToString() == tag.ToString())
				{
					tabDocs.SelectedIndex = i;
					tabDocs.SelectedTab.Focus();
					blank = tab;
					break;
				}
				if (tab.Tag == null && isSyntaxBox)
					blank = tab;
			}
			return blank;
		}
		private void treeProject_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
		{
			switch(e.Node.ImageKey) 
			{
				case "script":
				case "page":
				case "text":
					TabPage blank = FindTabByTag(e.Node.FullPath, true);

					string path = project.PathFromNode(e.Node);
					string data = (File.Exists(path)) ? File.ReadAllText(path) : "";

					if (blank != null)
					{
						// not found, is there a blank new one?
						SyntaxBoxControl syn = blank.Controls[0] as SyntaxBoxControl;

						if(syn != null)
						{
							blank.Text = e.Node.Text;
							blank.Tag = e.Node.FullPath;
							syn.Document.Text = data;
						}
					}
					else // new tab
					{
						AddTabLUA(e.Node.Text, e.Node.FullPath, data);
					}
					break;
				case "sound":
				case "music":
					// system default
					try
					{
						System.Diagnostics.Process.Start(project.PathFromNode(e.Node));
					}
					catch(Exception ex) {
						// no default handler
					}
					break;
				case "image":
					// internal image viewer?
					if(FindTabByTag(e.Node.FullPath, false)==null)
						AddTabImage(e.Node.Text, e.Node.FullPath, project.PathFromNode(e.Node));
					break;
				case "package":
				case "config":
					mnupopProperties_Click(null, null);
					break;
				default:
					break;
			}
			UpdateButtons();
		}

		private void SaveAll()
		{
			foreach (TabPage p in tabDocs.TabPages)
			{
				SyntaxBoxControl syn = p.Controls[0] as SyntaxBoxControl;
				if (syn != null)
				{
					if (syn.Document.Modified)
					{
						string path = project.PathFromTag(p.Tag);
						File.WriteAllText(path, (p.Controls[0] as SyntaxBoxControl).Document.Text);
						syn.Document.Modified = false;
					}
				}
			}
			UpdateButtons();
		}

		private void saveToolStripMenuItem_Click(object sender, EventArgs e)
		{
			// save current tab
			if (tabDocs.SelectedTab.Tag == null || synCode == null) return;

			string path = project.TempFolder + tabDocs.SelectedTab.Tag.ToString();
			File.WriteAllText(path, synCode.Document.Text);
			synCode.Document.Modified = false;

			UpdateButtons();
		}

		private void UpdateButtons()
		{
			if(project != null)
				btnBuild.Enabled = btnRun.Enabled = buildToolStripMenuItem.Enabled = true;

			bool editor = synCode != null;

			editToolStripMenuItem.Enabled = editor;

			btnSaveAll.Enabled = saveAllToolStripMenuItem.Enabled = editor && !String.IsNullOrEmpty(synCode.Document.Text);
			btnSaveFile.Enabled = saveToolStripMenuItem.Enabled = editor && (synCode.Document.Modified);
			btnUndo.Enabled = undoToolStripMenuItem.Enabled = editor && (synCode.Document.UndoBuffer.Count > 0);
			btnRedo.Enabled = redoToolStripMenuItem.Enabled = editor && (synCode.Document.UndoStep < synCode.Document.UndoBuffer.Count);
			btnCut.Enabled = cutToolStripMenuItem.Enabled =
				btnCopy.Enabled = copyToolStripMenuItem.Enabled = editor && (synCode.Selection.SelLength > 0);

			findAndReplaceToolStripMenuItem.Enabled = goToToolStripMenuItem.Enabled = selectAllToolStripMenuItem.Enabled = btnToggleBookmark.Enabled = editor;

			btnBookmarkNext.Enabled = btnBookmarkPrevious.Enabled = btnToggleBookmark.Enabled = bookmarksToolStripMenuItem.Enabled = editor && (project != null);
			btnPaste.Enabled = pasteToolStripMenuItem.Enabled = editor && (Clipboard.ContainsText());
		}

		private void CloseProject()
		{
			if (IsDirty)
			{
				if (MessageBox.Show("You have unsaved changes. Save now?", "Unsaved Changes", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
				{
					SaveAll();
				}
			}
		}
		private void Main_FormClosing(object sender, FormClosingEventArgs e)
		{
			CloseProject();
		}

		private void btnRun_Click(object sender, EventArgs e)
		{
			project.Zip();
			try
			{
				string exe = Properties.Settings.Default.Path_Executable;
				if (String.IsNullOrEmpty(exe))
					throw new ExecutionEngineException("No executable specified.");

				this.Enabled = false;
				System.Diagnostics.Process pr = new System.Diagnostics.Process();
				pr.StartInfo.FileName = exe;
                pr.StartInfo.WorkingDirectory = Path.GetDirectoryName(exe);
				pr.StartInfo.Arguments = string.Format("\"{0}\"", project.ProjectSource);
				pr.Start();
				while (pr.HasExited == false)
				{
					Application.DoEvents();
				}
				this.Enabled = true;
				this.Focus();
			}
			catch (ExecutionEngineException)
			{
				MessageBox.Show("Location of LÖVE executable not known. Please tell me where it is (in Tools -> Options).");
			}
			catch (Win32Exception)
			{
				MessageBox.Show(".LÖVE file association not found. Have you installed LÖVE?\n\nYou need to either install LÖVE or tell DEVötiön where it is (in Tools -> Options).", "Can't Handle LÖVE", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			}
		}

		private void btnSaveAll_Click(object sender, EventArgs e)
		{
			SaveAll();
		}

		private void treeProject_MouseDown(object sender, MouseEventArgs e)
		{
			if (e.Button != MouseButtons.Right) return;

			TreeNode clicked = treeProject.GetNodeAt(e.Location);
			if (clicked != null) treeProject.SelectedNode = clicked;

			if (treeProject.SelectedNode == null) return;

			mnupopAddFile.Enabled = (clicked.ImageKey == "folder" || clicked.ImageKey == "package");
			mnupopDelete.Enabled = (clicked.Name != "root");
			mnupopProperties.Enabled = (clicked.ImageKey == "package");

			UpdateButtons();
		}

		private void mnupopAddFolder_Click(object sender, EventArgs e)
		{
			TreeNode p = treeProject.SelectedNode;
			if (p == null) return;

			string path = p.FullPath.Replace(treeProject.Nodes[0].Text, "").Trim('\\');
			project.CreateFolder("New Folder", path + "\\");
			p = p.Nodes.Add("New Folder", "New Folder", "folder", "folder");
			p.Parent.Expand();
			UpdateButtons();
		}

		private void mnupopAddResource_Click(object sender, EventArgs e)
		{
			TreeNode p = treeProject.SelectedNode;
			if (p == null) return;

			OpenFileDialog dlg = new OpenFileDialog();
			dlg.DefaultExt = "";
			dlg.Filter = "All Files (*.*)|*.*";
			dlg.Title = "Select a File to Add";

			if (dlg.ShowDialog() != DialogResult.Cancel)
			{
				FileInfo fi = new FileInfo(dlg.FileName);
				File.Copy(dlg.FileName, project.PathFromNode(p)+ "\\" + fi.Name);
				project.PopulateNode(project.PathFromNode(p), p);

				UpdateButtons();
			}

		}

		private void mnupopAddScript_Click(object sender, EventArgs e)
		{
			TreeNode p = treeProject.SelectedNode;
			if (p == null) return;

			string path = p.FullPath.Replace(treeProject.Nodes[0].Text, "").Trim('\\');
			project.CreateFile("New Script", ".lua", path);
			p = p.Nodes.Add("New Script.lua", "New Script.lua", "script", "script");
			UpdateButtons();
		}

		private void treeProject_AfterLabelEdit(object sender, NodeLabelEditEventArgs e)
		{
			if (e.Label == null) return;

			// node renamed
			string oldpath = project.PathFromNode(e.Node);
			string newpath = oldpath.Replace(e.Node.Text, e.Label);
			
			project.RenameFolder(oldpath, newpath);
		
			UpdateButtons();
		}

		private TreeNode sourceNode = null;
		private void treeProject_ItemDrag(object sender, ItemDragEventArgs e)
		{
			sourceNode = (TreeNode)e.Item;
			DoDragDrop(e.Item.ToString(), DragDropEffects.Move | DragDropEffects.Copy);
		}

		private void treeProject_DragEnter(object sender, DragEventArgs e)
		{
			if (e.Data.GetDataPresent(DataFormats.Text))
				e.Effect = DragDropEffects.Move;
			else
				e.Effect = DragDropEffects.None;
		}

		private void treeProject_DragDrop(object sender, DragEventArgs e)
		{
			Point pos = treeProject.PointToClient(new Point(e.X, e.Y));
			TreeNode targetNode = treeProject.GetNodeAt(pos);
			
			if (targetNode != null) 
			{
				if (targetNode.ImageKey == "folder" || targetNode.ImageKey == "package")
				{
					string src = project.PathFromNode(sourceNode);

					sourceNode.Remove();
					TreeNode nn = new TreeNode(sourceNode.Text);
					nn.ImageKey = nn.SelectedImageKey = sourceNode.ImageKey;
					targetNode.Nodes.Add(nn);

					// move file
					File.Move(src, project.PathFromNode(nn));
					//File.Move(project.TempFolder + sourceNode.Name, project.TempFolder + targetNode.Name + "\\" + sourceNode.Text);

					treeProject.Invalidate();
					treeProject.Sort();
					targetNode.Expand();
				}
				UpdateButtons();
			}
		}

		private void btnBuild_Click(object sender, EventArgs e)
		{
			project.Zip();
		}

		private void buildLOVEMergedToolStripMenuItem_Click(object sender, EventArgs e)
		{
			string exe = Properties.Settings.Default.Path_Executable;

			try
			{
				if (String.IsNullOrEmpty(exe))
					throw new ExecutionEngineException("LÖVE executable not found.");

				project.Zip();

				SaveFileDialog dlg = new SaveFileDialog();
				dlg.DefaultExt = ".exe";
				dlg.Filter = "LÖVE Executable files (*.exe)|*.exe";
				dlg.InitialDirectory = project.ProjectPath;
				dlg.OverwritePrompt = true;

				dlg.FileName = project.ProjectFile + ".exe";

				if (dlg.ShowDialog() != DialogResult.Cancel)
				{
					string path = dlg.FileName; // project.ProjectPath + project.ProjectFile + ".exe";
					if (File.Exists(path)) File.Delete(path);
					FileStream prog = File.OpenWrite(path);
					byte[] data = File.ReadAllBytes(exe);

					prog.Write(data, 0, data.Length);
					data = File.ReadAllBytes(project.ProjectSource);
					prog.Write(data, 0, data.Length);

					prog.Close();
				}
			}
			catch (ExecutionEngineException)
			{
				// no file association found, check settings
				MessageBox.Show("LÖVE executable not found. Try setting it manually in Tools -> Options.", "No LÖVE", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, "Merged File Creation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private void btnToggleBookmark_Click(object sender, EventArgs e)
		{
			if (synCode != null)
				synCode.ToggleBookmark();
		}

		private void tabDocs_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (tabDocs.SelectedTab != null)
			{
				tabDocs.SelectedTab.Focus();
				UpdateButtons();
			}
		}

		private void btnBookmarkNext_Click(object sender, EventArgs e)
		{
			var syn = synCode;

			if (syn != null)
			{
				int curr = syn.Caret.CurrentRow.Index;
				int nextln = syn.Document.GetNextBookmark(curr);

				if (nextln > curr)
				{
					// next in page...
					syn.GotoNextBookmark();
				}
				else
				{
					int tab = tabDocs.SelectedIndex;

					// previous tab???
					for (int i = tab + 1; i < tabDocs.TabPages.Count; i++)
					{
						syn = tabDocs.TabPages[i].Controls[0] as SyntaxBoxControl;
						nextln = syn.Document.GetNextBookmark(-1);
						if (nextln > -1)
						{
							tabDocs.SelectedIndex = i;
							syn.GotoLine(nextln);
							return;
						}
					}

					// finish wrap
					for (int i = 0; i <= tab; i++)
					{
						syn = tabDocs.TabPages[i].Controls[0] as SyntaxBoxControl;
						nextln = syn.Document.GetNextBookmark(-1);
						if (nextln > -1)
						{
							tabDocs.SelectedIndex = i;
							syn.GotoLine(nextln);
							return;
						}
					}
				}
			}
		}

		private void btnBookmarkPrevious_Click(object sender, EventArgs e)
		{
			SyntaxBoxControl syn = synCode;

			if (syn != null)
			{
				int curr = syn.Caret.CurrentRow.Index;
				int nextln = syn.Document.GetPreviousBookmark(curr);

				if (nextln < curr)
				{
					// next in page...
					syn.GotoPreviousBookmark();
				}
				else
				{
					int tab = tabDocs.SelectedIndex;

					// previous tab???
					for (int i = tab-1; i >= 0; i--)
					{
						syn = tabDocs.TabPages[i].Controls[0] as SyntaxBoxControl;
						nextln = syn.Document.GetPreviousBookmark(0);
						if (nextln > -1)
						{
							tabDocs.SelectedIndex = i;
							syn.GotoLine(nextln);
							return;
						}
					}

					// finish wrap
					for (int i = tabDocs.TabPages.Count-1; i >= tab; i--)
					{
						syn = tabDocs.TabPages[i].Controls[0] as SyntaxBoxControl;
						nextln = syn.Document.GetPreviousBookmark(0);
						if (nextln > -1)
						{
							tabDocs.SelectedIndex = i;
							syn.GotoLine(nextln);
							return;
						}
					}
				}
			}
		}

		private void goToToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (synCode != null)
			{
				synCode.ShowGotoLine();
			}
		}

		private void findAndReplaceToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (synCode != null)
			{
				synCode.ShowFind();
			}
		}

		private void btnCut_Click(object sender, EventArgs e)
		{
			synCode.Cut();
			UpdateButtons();
		}

		private void btnCopy_Click(object sender, EventArgs e)
		{
			synCode.Copy();
			UpdateButtons();
		}

		private void btnPaste_Click(object sender, EventArgs e)
		{
			synCode.Paste();
			UpdateButtons();
		}

		private void btnUndo_Click(object sender, EventArgs e)
		{
			synCode.Undo();
			UpdateButtons();
		}

		private void btnRedo_Click(object sender, EventArgs e)
		{
			synCode.Redo();
			UpdateButtons();
		}

		private void clearAllBookmarksToolStripMenuItem_Click(object sender, EventArgs e)
		{
			foreach (TabPage p in tabDocs.TabPages)
			{
				SyntaxBoxControl s = p.Controls[0] as SyntaxBoxControl;
				if (s != null)
					s.Document.ClearBookmarks();
			}
		}

		private void mnupopDelete_Click(object sender, EventArgs e)
		{
			TreeNode p = treeProject.SelectedNode;
			if (p == null) return;

			if (MessageBox.Show("Are you sure you want to delete this file?\n\nYou cannot undo this!", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
			{
				string path = project.PathFromNode(p);

				// remove tab if open
				foreach (TabPage pg in tabDocs.TabPages)
					if (pg.Tag != null && pg.Tag.ToString() == p.FullPath) tabDocs.TabPages.Remove(pg);

				// remove from tree
				p.Remove();

				// remove file
				if (Directory.Exists(path))
				{
					Directory.Delete(path, true);
				}
				else if (File.Exists(path))
				{
					File.Delete(path);
				}

				UpdateButtons();
			}
		}
		
		private void mnupopProperties_Click(object sender, EventArgs e)
		{
			frmProjectProperties dlg = new frmProjectProperties(project.TempFolder + "\\game.conf");
			dlg.ShowDialog(this);
		}

		private void optionsToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (dlgOptions.ShowDialog(this) != DialogResult.Cancel)
			{
				LoadSettings();
				foreach (TabPage t in tabDocs.TabPages)
				{
					SyntaxBoxControl syn = t.Controls[0] as SyntaxBoxControl;
					if (syn != null)
					{
						SetUpSyntaxBox(syn);
					}
				}
			}
		}

		private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
		{
			frmAbout dlg = new frmAbout();
			dlg.ShowDialog(this);
		}

		private void selectAllToolStripMenuItem_Click(object sender, EventArgs e)
		{
			synCode.SelectAll();
		}

		private void exitToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Close();
		}

		private void newProjectToolStripMenuItem_Click(object sender, EventArgs e)
		{
			// get a new file
			SaveFileDialog dlg = new SaveFileDialog();
			dlg.DefaultExt = "love";
			dlg.Filter = "LÖVE Packages (*.love)|*.love";
			dlg.OverwritePrompt = true;
			dlg.Title = "Create New Project File";

			if (dlg.ShowDialog() != DialogResult.Cancel)
			{
				project = new LOVEProject(dlg.FileName, treeProject);
				ResetTabs();
				UpdateButtons();

				mnupopProperties_Click(null, null);
			}

		}
	}
}
