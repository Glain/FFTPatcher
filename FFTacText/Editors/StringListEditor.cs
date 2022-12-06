/*
    Copyright 2007, Joe Davidson <joedavidson@gmail.com>

    This file is part of FFTPatcher.

    FFTPatcher is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    FFTPatcher is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with FFTPatcher.  If not, see <http://www.gnu.org/licenses/>.
*/

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using PatcherLib.Utilities;
using PatcherLib.TextUtilities;
using PatcherLib;

namespace FFTPatcher.TextEditor
{
    /// <summary>
    /// An editor that edits lists of strings.
    /// </summary>
    partial class StringListEditor : UserControl
    {
        private const string FFTFontFilename = "Altima_8.ttf";

        private IFile boundFile;
        private int boundSection;

        private bool ignoreChanges = false;
        private int TextColumnIndex { get { return textColumn.Index; } }

        private System.Drawing.Text.PrivateFontCollection fonts = new System.Drawing.Text.PrivateFontCollection();
        private Font ArialFont = new Font("Arial Unicode MS", 9);

        private PatcherLib.Datatypes.FFTFont font;

        private Font fftFont = null;
        private Font FFTFont
        {
            get
            {
                if (fftFont == null)
                    LoadFFTFont();

                return fftFont;
            }
        }
        
        /// <summary>
        /// Gets the current row.
        /// </summary>
        /// <value>The current row.</value>
        public int CurrentRow
        {
            get { return dataGridView.CurrentRow != null ? (int)dataGridView.CurrentRow.Cells[numberColumn.Name].Value : -1; }
        }

        private string Filter { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="StringListEditor"/> class.
        /// </summary>
        public StringListEditor()
        {
            InitializeComponent();
            dataGridView.CellEndEdit += dataGridView_CellEndEdit;
            dataGridView.EditingControlShowing += dataGridView_EditingControlShowing;
            dataGridView.CellValidating += new DataGridViewCellValidatingEventHandler( dataGridView_CellValidating );
            dataGridView.CellValidated += new DataGridViewCellEventHandler( dataGridView_CellValidated );
            textColumn.DefaultCellStyle.Font = ArialFont;
            widthColumn.DefaultCellStyle.Font = ArialFont;
        }

        private void LoadFFTFont()
        {
            fonts.AddFontFile(FFTFontFilename);
            fftFont = new Font(fonts.Families[0], 12);
        }
        
        public void UpdateTextFont(bool useFFTFont)
        {
            textColumn.DefaultCellStyle.Font = useFFTFont ? FFTFont : ArialFont;
        }

        public void ApplyFilter(string filter)
        {
            numberColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            hexColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            nameColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            widthColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            textColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;

            for (int rowIndex = 0; rowIndex < dataGridView.Rows.Count; rowIndex++)
            {
                DataGridViewRow row = dataGridView.Rows[rowIndex];
                row.Visible = IsRowVisibleThroughFilter(rowIndex, filter);
            }

            numberColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            hexColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.NotSet;
            nameColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            widthColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            textColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        }

        public void ApplyCurrentFilter()
        {
            ApplyFilter(Filter);
        }

        public void SetFilter(string filter)
        {
            Filter = filter;
            ApplyFilter(filter);
        }

        public void ClearFilter()
        {
            SetFilter(string.Empty);
        }

        private bool IsRowVisibleThroughFilter(int rowIndex, string filter = null)
        {
            filter = filter ?? Filter;

            if (string.IsNullOrEmpty(filter))
                return true;

            object nameValue = dataGridView[nameColumn.Index, rowIndex].Value;
            object textValue = dataGridView[textColumn.Index, rowIndex].Value;

            string name = (nameValue == null) ? "" : nameValue.ToString().ToLower().Trim();
            string text = (textValue == null) ? "" : textValue.ToString().ToLower().Trim();

            filter = filter.ToLower().Trim();

            return name.Contains(filter) || text.Contains(filter);
        }

        private void dataGridView_CellValidating( object sender, DataGridViewCellValidatingEventArgs e )
        {
            if ( !ignoreChanges &&
                e.ColumnIndex == TextColumnIndex &&
                CellValidating != null )
            {
                CellValidating( this, e );
            }
        }

        private void dataGridView_CellValidated( object sender, DataGridViewCellEventArgs e )
        {
            if ( !ignoreChanges && 
                 e.ColumnIndex == TextColumnIndex )
            {
                string s = (string)dataGridView[e.ColumnIndex, e.RowIndex].Value ?? string.Empty;
                boundFile[boundSection, CurrentRow] = s;
                dataGridView[widthColumn.Index, e.RowIndex].Value = GetWidthColumnString( s );
            }
        }

        private string GetWidthColumnString( string s )
        {
            var widths = MeasureEachLineInFont(boundFile.CharMap, s, font);
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            var widthStrings = new List<string>( widths.Count );

            widths.ForEach( w => widthStrings.Add( string.Format( "{0}", w ) ) );
            return string.Join( Environment.NewLine, widthStrings.ToArray() );
        }

        public event DataGridViewCellValidatingEventHandler CellValidating;

        /// <summary>
        /// Occurs when text in a textbox has changed.
        /// </summary>
        public event EventHandler TextBoxTextChanged;

        private void dataGridView_CellEndEdit( object sender, DataGridViewCellEventArgs e )
        {
            if( dataGridView.EditingControl is TextBox )
            {
                dataGridView.EditingControl.TextChanged -= tb_TextChanged;
            }
        }

        private void dataGridView_EditingControlShowing( object sender, DataGridViewEditingControlShowingEventArgs e )
        {
            if( !ignoreChanges &&
                dataGridView.CurrentCell != null && 
                dataGridView.CurrentCell.ColumnIndex == textColumn.Index && 
                dataGridView.EditingControl is TextBox )
            {
                TextBox tb = dataGridView.EditingControl as TextBox;
                tb.Font = new Font( "Arial Unicode MS", 9 );
                tb.TextChanged += tb_TextChanged;
            }
        }

        private void tb_TextChanged( object sender, EventArgs e )
        {
            if( !ignoreChanges && 
                TextBoxTextChanged != null )
            {
                TextBoxTextChanged( sender, e );
            }
        }

        /// <summary>
        /// Binds this editor to a list of strings.
        /// </summary>
        public void BindTo( IList<string> names, IFile file, int section )
        {
            ignoreChanges = true;
            SuspendLayout();
            int count = file.SectionLengths[section];
            List<string> ourNames = new List<string>( names );
            for ( int i = names.Count; i < count; i++ )
            {
                ourNames.Add( string.Empty );
            }

            IList<int> disallowed = ( file is ISerializableFile ) ? ( (ISerializableFile)file ).Layout.DisallowedEntries[section] : null;

            DataGridViewRow[] rows = new DataGridViewRow[count];
            dataGridView.SuspendLayout();
            //dataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
            font = file.Context == PatcherLib.Datatypes.Context.US_PSP ? PSPResources.PSPFont : PSXResources.PSXFont;
            boundFile = file;

            for( int i = 0; i < count; i++ )
            {
                DataGridViewRow row = new DataGridViewRow();

                string hex = (count < 256) ? i.ToString("X2") : i.ToString("X4");
                row.CreateCells( dataGridView, i, hex, ourNames[i], GetWidthColumnString( file[section, i] ?? string.Empty ), file[section, i] );

                row.ReadOnly = disallowed != null && disallowed.Count > 0 && disallowed.Contains( i );
                //row.Visible = IsRowVisibleThroughFilter(i);
                rows[i] = row;
            }
            dataGridView.Rows.Clear();
            dataGridView.Rows.AddRange( rows );
            //dataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dataGridView.ResumeLayout();

            //bool showSeparatorChoices = file is ISerializableFile && (file as ISerializableFile).Layout.AllowedTerminators.Count > 1;
            bool showSeparatorChoices = false;

            separatorComboBox.Visible = showSeparatorChoices;
            separatorComboBox.Enabled = showSeparatorChoices;
            separatorLabel.Visible = showSeparatorChoices;
            if (showSeparatorChoices)
            {
                PatcherLib.Datatypes.Set<byte> seps = (file as ISerializableFile).Layout.AllowedTerminators;
                separatorComboBox.BeginUpdate();
                separatorComboBox.Items.Clear();
                separatorComboBox.FormatString = "X2";
                seps.ForEach(b => separatorComboBox.Items.Add(b));
                separatorComboBox.SelectedItem = file.SelectedTerminator;
                separatorComboBox.EndUpdate();
            }

            boundSection = section;
            ignoreChanges = false;
            ResumeLayout();

            ApplyCurrentFilter();
        }

        private void separatorComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (separatorComboBox.Enabled && separatorComboBox.Visible && !ignoreChanges)
            {
                boundFile.SelectedTerminator = (byte)separatorComboBox.SelectedItem;
            }
        }

        private int GetWidthForEncodedCharacter(UInt32 c, PatcherLib.Datatypes.FFTFont font)
        {
            if (c == 0xFA)
            {
                return (boundFile.Context == PatcherLib.Datatypes.Context.US_PSP) ? 10 : 4;
            }
            else if (c <= 0xCF)
            {
                return font.Glyphs[(int)c].Width;
            }
            else if ((c & 0xFF00) >= 0xD100 && (c & 0xFF00) <= 0xDA00 && (c & 0x00FF) <= 0xCF &&
                      ((c & 0xFF00) != 0xDA00 || (c & 0x00FF) <= 0x77))
            {
                return font.Glyphs[(int)((((c & 0xFF00) >> 8) - 0xD0) * 0xD0 + (c & 0x00FF))].Width;
            }
            else
            {
                return 0;
            }
        }

        public IList<int> MeasureEachLineInFont(GenericCharMap charMap, string s, PatcherLib.Datatypes.FFTFont font)
        {
            string[] strings = s.Split(new string[] { "{Newline}", "{Close}" }, StringSplitOptions.RemoveEmptyEntries);
            int[] result = new int[strings.Length];
            for (int i = 0; i < strings.Length; i++)
            {
                result[i] =
                    strings[i].Length == 0 ? 0 :
                                             MeasureSingleLineInFont(charMap, strings[i], font);
            }
            return result.AsReadOnly();
        }

        private int MeasureSingleLineInFont(GenericCharMap charMap, string s, PatcherLib.Datatypes.FFTFont font)
        {
            IList<UInt32> everyChar = charMap.GetEachEncodedCharacter(s);
            int sum = 0;
            foreach (UInt32 c in everyChar)
            {
                sum += GetWidthForEncodedCharacter(c, font);
            }
            return sum;
        }

        public int MeasureStringInFont(GenericCharMap charMap, string s, PatcherLib.Datatypes.FFTFont font)
        {
            var widths = MeasureEachLineInFont(charMap, s, font);
            int width = int.MinValue;
            widths.ForEach(w => width = Math.Max(width, w));
            return width;
        }
    }
}
