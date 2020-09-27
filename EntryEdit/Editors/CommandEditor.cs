using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using PatcherLib.Controls;

namespace EntryEdit.Editors
{
    public partial class CommandEditor : UserControl
    {
        private class ParameterData
        {
            public bool IsSpinner { get; set; }
            public ShortGroupBox GroupBox { get; set; }
            public NumericUpDown Spinner { get; set; }
            public ComboBox ComboBox { get; set; }

            public ParameterData(bool isSpinner, ShortGroupBox groupBox, NumericUpDown spinner, ComboBox comboBox)
            {
                this.IsSpinner = isSpinner;
                this.GroupBox = groupBox;
                this.Spinner = spinner;
                this.ComboBox = comboBox;
            }
        }

        private Command _command;
        public Command Command
        {
            get
            {
                return _command;
            }
        }

        private CommandData _commandData;
        private Dictionary<string, int> _parameterWidthMaps;
        private List<ParameterData> _parameterDataList;

        public bool IsChecked()
        {
            return chk_Include.Checked;
        }

        public void SetCheckedState(bool checkedState)
        {
            chk_Include.Checked = checkedState;
        }

        public void ToggleCheckedState()
        {
            chk_Include.Checked = !chk_Include.Checked;
        }

        public CommandEditor()
        {
            InitializeComponent();
            InitData();
        }

        public void Init(CommandData commandData)
        {
            this._commandData = commandData;
            InitCommandComboBox(commandData.CommandNames);
            InitParameters();
        }

        public void Populate(Command command)
        {
            _command = command;
            //chk_Include.Checked = false;
            cmb_Command.SelectedIndex = command.Template.ID;
            SetParameters(command.Parameters);
        }

        public void SaveFormCommand()
        {
            Command formCommand = GetFormCommand();
            if (!_command.Equals(formCommand))
                _command = formCommand;
        }

        private void SetNewCommand(int commandID)
        {
            CommandTemplate template = null;
            _commandData.CommandMap.TryGetValue(commandID, out template);
            
            if (template != null)
                Populate(new Command(template));
            else
                Populate(new Command(commandID, _commandData.DefaultCommandByteLength, _commandData.CommandType));
        }

        private void InitData()
        {
            _parameterWidthMaps = new Dictionary<string, int>();
        }

        private void SetParameters(List<CommandParameter> parameters)
        {
            //flp_Parameters.Controls.Clear();
            ClearParameters();

            if ((parameters != null) && (parameters.Count > 0))
            {
                //List<Control> controls = new List<Control>();
                int index = 0;
                foreach (CommandParameter parameter in parameters)
                {
                    bool isHex = parameter.Template.IsHex;
                    bool isSigned = parameter.Template.IsSigned;
                    int range = (1 << (parameter.Template.ByteLength << 3));

                    ParameterData parameterData = _parameterDataList[index];

                    //GroupBox groupBox = new GroupBox();
                    ShortGroupBox groupBox = parameterData.GroupBox;
                    NumericUpDown spinner = parameterData.Spinner;
                    ComboBox comboBox = parameterData.ComboBox;
                    //if (parameter.Template.Type == CommandParameterType.Number)
                    Dictionary<int, string> parameterValueMap = null;
                    //groupBox.AutoSize = true;
                    if (!_commandData.ParameterValueMaps.TryGetValue(parameter.Template.Type, out parameterValueMap))
                    {
                        parameterData.IsSpinner = true;
                        //NumericUpDown spinner = new NumericUpDown();
                        spinner.Width = (parameter.GetByteLength() * 10) + 35;
                        spinner.Minimum = isSigned ? (-(range / 2)) : 0;
                        spinner.Maximum = isSigned ? ((range / 2) - 1) : (range - 1);
                        spinner.Hexadecimal = isHex;
                        spinner.Value = (parameter.Value > spinner.Maximum) ? -(range - parameter.Value) : parameter.Value;
                        //spinner.Location = new Point((groupBox.Width - spinner.Width) / 2, 15);
                        //groupBox.Controls.Add(spinner);
                        spinner.Visible = true;
                        comboBox.Visible = false;
                    }
                    else
                    {
                        parameterData.IsSpinner = false;
                        //ComboBox comboBox = new ComboBox();
                        List<string> entryNames = DataHelper.GetParameterEntryNames(parameter.Template, parameterValueMap);
                        comboBox.Width = GetComboBoxWidth(comboBox, parameter.Template.Type, entryNames);

                        comboBox.Items.Clear();
                        comboBox.Items.AddRange(entryNames.ToArray());
                        //comboBox.Width = Math.Max(comboBox.Width, GetComboBoxWidth(comboBox, parameter.Template.Type, entryNames));
                        comboBox.SelectedIndex = parameter.Value;
                        //comboBox.Location = new Point((groupBox.Width - comboBox.Width) / 2, 15);
                        //groupBox.Controls.Add(comboBox);
                        comboBox.Visible = true;
                        spinner.Visible = false;
                    }

                    string name = (parameter.Template.Name == "Unknown") ? "?" : parameter.Template.Name;
                    groupBox.Text = name + (isHex ? " (h)" : "");
                    //groupBox.AutoSize = true;
                    //controls.Add(groupBox);
                    //groupBox.Padding = new Padding(groupBox.Padding.Left, groupBox.Padding.Top, groupBox.Padding.Right, 0);
                    //groupBox.Margin = new Padding(groupBox.Margin.Left, groupBox.Margin.Top, groupBox.Margin.Right, 0);
                    //groupBox.AutoSize = false;
                    groupBox.Visible = true;

                    index++;
                }

                //flp_Parameters.Controls.AddRange(controls.ToArray());
            }
        }

        private void InitCommandComboBox(IEnumerable<string> commandNames)
        {
            cmb_Command.Items.Clear();

            if (commandNames != null)
            {
                foreach (string commandName in commandNames)
                {
                    cmb_Command.Items.Add(commandName);
                }
            }
            else
            {
                for (int index = 0; index < 256; index++)
                {
                    cmb_Command.Items.Add(index.ToString("X2"));
                }
            }
        }

        private void InitParameters()
        {
            _parameterDataList = new List<ParameterData>();
            ShortGroupBox[] groupBoxes = new ShortGroupBox[_commandData.MaxParameters];

            for (int index = 0; index < _commandData.MaxParameters; index++)
            {
                ShortGroupBox groupBox = new ShortGroupBox();
                groupBox.AutoSize = true;
                groupBox.Visible = false;
                //flp_Parameters.Controls.Add(groupBox);

                NumericUpDown spinner = new NumericUpDown();
                spinner.Visible = false;

                ComboBox comboBox = new ComboBox();
                comboBox.Visible = false;
                comboBox.DropDownStyle = ComboBoxStyle.DropDownList;

                groupBox.Controls.Add(spinner);
                spinner.Location = new Point(5, 15);
                //spinner.Location = new Point((groupBox.Width - spinner.Width) / 2, 15);

                groupBox.Controls.Add(comboBox);
                comboBox.Location = spinner.Location;
                //comboBox.Location = new Point((groupBox.Width - comboBox.Width) / 2, 15);

                groupBoxes[index] = groupBox;
                _parameterDataList.Add(new ParameterData(true, groupBox, spinner, comboBox));

                //groupBox.Location = new Point(groupBox.Location.X, 0);
                //groupBox.Padding = new Padding(groupBox.Padding.Left, groupBox.Padding.Top, groupBox.Padding.Right, 0);
                //groupBox.Margin = new Padding(groupBox.Margin.Left, groupBox.Margin.Top, groupBox.Margin.Right, 0);
            }

            flp_Parameters.Controls.AddRange(groupBoxes);
        }

        private void ClearParameters()
        {
            for (int index = 0; index < _commandData.MaxParameters; index++)
            {
                _parameterDataList[index].GroupBox.Visible = false;
            }
        }

        private List<int> GetFormParameterValues()
        {
            List<int> result = new List<int>();
            for (int index = 0; index < _command.Parameters.Count; index++)
            {
                result.Add(GetFormParameterValue(index));
            }

            return result;
        }

        private int GetFormParameterValue(int index)
        {
            return _parameterDataList[index].IsSpinner ? (int)_parameterDataList[index].Spinner.Value : _parameterDataList[index].ComboBox.SelectedIndex;
        }

        private Command GetFormCommand()
        {
            return _command.CopyWithValues(GetFormParameterValues());
        }

        private int GetComboBoxWidth(ComboBox cb, string type, IEnumerable<string> entryNames)
        {
            int width = 0;
            if (_parameterWidthMaps.TryGetValue(type, out width))
                return width;

            int result = GetMaxWidth(entryNames, cb.Font);
            _parameterWidthMaps.Add(type, result);
            return result;
        }

        private int GetMaxWidth(IEnumerable<string> strings, Font font)
        {
            int result = 0;
            foreach (string str in strings)
            {
                result = Math.Max(result, TextRenderer.MeasureText(str, font).Width);
            }
            return result + 15;
        }

        private void cmb_Command_SelectedIndexChanged(object sender, EventArgs e)
        {
            int selectedIndex = cmb_Command.SelectedIndex;
            if (selectedIndex != _command.Template.ID)
            {
                SetNewCommand(selectedIndex);
            }
        }
    }
}
