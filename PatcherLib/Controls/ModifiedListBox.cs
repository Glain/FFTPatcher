/*
 * Created by SharpDevelop.
 * User: Glain
 * Date: 4/26/2012
 * Time: 23:26
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

using PatcherLib.Helpers;
using System;
using System.Text;
using System.Windows.Forms;

namespace PatcherLib.Controls
{
	/// <summary>
	/// Description of ModifiedListBox.
	/// </summary>
	public class ModifiedListBox : System.Windows.Forms.ListBox
	{
		private StringBuilder selectionCache = new StringBuilder();
		private DateTime selectionLastUpdated = DateTime.Now;

		private bool _includePrefix = false;
		public bool IncludePrefix
		{
			get { return _includePrefix; }
			set { _includePrefix = value; }
		}

		protected override void OnEnter(EventArgs e)
		{
			// When control receives focus
			ResetSelectionCache();
			base.OnEnter(e);
		}

		protected override void OnMouseDown(MouseEventArgs e)
		{
			ResetSelectionCache();
			base.OnClick(e);
		}

		protected override void WndProc(ref Message m)
		{
			if (m.Msg == 0x0102) // WM_CHAR
			{
				char key = (char)m.WParam;
				
				if (key == 27) // Escape
				{
					ResetSelectionCache();
				}
				else if (key == 8) // Backspace
				{
					// Allow backspace to delete from the cache
					HandleSelectionCacheExpiry();
					if (selectionCache.Length > 0)
					{
						selectionCache.Length--;
						selectionLastUpdated = DateTime.Now;
						SelectFirstItemMatching(selectionCache.ToString());
					}
				}
				else if ((Char.IsLetterOrDigit(key)) || (key == ' '))
				{
					HandleSelectionCacheExpiry();
					selectionCache.Append(Char.ToLower(key));
					selectionLastUpdated = DateTime.Now;
					SelectFirstItemMatching(selectionCache.ToString());
				}
				
				// Do not call base.WndProc as we want to eat the message
				// so that the control does not respond to it.
				return;
			}
			else if (m.Msg == 0x0100)    // WM_KEYDOWN
			{
				char key = (char)m.WParam;

				if ((key == 0x26) || (key == 0x28))     // VK_UP or VK_DOWN
				{
					ResetSelectionCache();
				}
			}

			base.WndProc(ref m);
		}
		
		private void SelectFirstItemMatching(string selectionText)
		{
			for (int index = 0; index < Items.Count; index++)
			{
				string itemText = Items[index].ToString().ToLower();
				
				if (string.IsNullOrEmpty(itemText))
					continue;
				
				if (!_includePrefix)
				{
					int newStartIndex = itemText.IndexOf(' ') + 1;
					int newLength = itemText.Length - newStartIndex;
					itemText = itemText.Substring(newStartIndex, newLength);
				}
				
				if (itemText.StartsWith(selectionText))
				{
					if (index != SelectedIndex)
						SelectedIndex = index;
					
					break;
				}
			}
		}

		private void ResetSelectionCache()
		{
			selectionCache.Length = 0;
			selectionLastUpdated = DateTime.Now;
		}

		private void HandleSelectionCacheExpiry()
		{
			DateTime currentDateTime = DateTime.Now;
			double elapsedSeconds = (currentDateTime - selectionLastUpdated).TotalSeconds;

			if (elapsedSeconds > ControlHelper.TypeAheadTimeoutSeconds)
			{
				ResetSelectionCache();
			}
		}
	}
}
