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
#if MEASURESTRINGS
        public int TextColumnIndex { get { return textColumn.Index; } }
#else
        public const int TextColumnIndex = 2;
#endif

        private System.Drawing.Text.PrivateFontCollection fonts = new System.Drawing.Text.PrivateFontCollection();
        private Font ArialFont = new Font("Arial Unicode MS", 9);

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
#if !MEASURESTRINGS
            dataGridView.Columns.Remove(widthColumn);
#else
            widthColumn.DefaultCellStyle.Font = ArialFont;
#endif
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
#if MEASURESTRINGS
                dataGridView[widthColumn.Index, e.RowIndex].Value = GetWidthColumnString( s );
#endif
            }
        }

#if MEASURESTRINGS
        private string GetWidthColumnString( string s )
        {
            var widths = MeasureEachLineInFont(boundFile.CharMap, s, font);
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            var widthStrings = new List<string>( widths.Count );

            widths.ForEach( w => widthStrings.Add( string.Format( "{0}", w ) ) );
            return string.Join( Environment.NewLine, widthStrings.ToArray() );
        }
#endif

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

#if MEASURESTRINGS
        PatcherLib.Datatypes.FFTFont font;
#endif
        
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
#if MEASURESTRINGS
            font = file.Context == PatcherLib.Datatypes.Context.US_PSP ? PSPResources.PSPFont : PSXResources.PSXFont;
#endif
            boundFile = file;

            for( int i = 0; i < count; i++ )
            {
                DataGridViewRow row = new DataGridViewRow();
#if MEASURESTRINGS
                row.CreateCells( dataGridView, i, ourNames[i],
                    GetWidthColumnString( file[section, i] ?? string.Empty ), file[section, i] );

#else
                row.CreateCells(dataGridView, i, ourNames[i], file[section, i]);
#endif
                row.ReadOnly = disallowed != null && disallowed.Count > 0 && disallowed.Contains( i );
                rows[i] = row;
            }
            dataGridView.Rows.Clear();
            dataGridView.Rows.AddRange( rows );
            dataGridView.ResumeLayout();

            bool showSeparatorChoices = file is ISerializableFile && (file as ISerializableFile).Layout.AllowedTerminators.Count > 1;
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
        }

        private void separatorComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (separatorComboBox.Enabled && separatorComboBox.Visible && !ignoreChanges)
            {
                boundFile.SelectedTerminator = (byte)separatorComboBox.SelectedItem;
            }
        }

#if MEASURESTRINGS
        private int GetWidthForEncodedCharacter(UInt32 c, PatcherLib.Datatypes.FFTFont font)
        {
            if (c == 0xFA)
            {
                return 4;
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
#endif

    }
}
