using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace EntryEdit.Editors
{
    public partial class CommandEditor : UserControl
    {
        private Command _command;
        private List<string> _commandNames;
        private Dictionary<string, Dictionary<int, string>> _parameterValueMaps;

        public CommandEditor()
        {
            InitializeComponent();
        }

        public void InitCommandList(List<string> commandNames, Dictionary<string, Dictionary<int, string>> parameterValueMaps)
        {
            _commandNames = commandNames;
            _parameterValueMaps = parameterValueMaps;
            InitCommandComboBox(commandNames);
        }

        public void Populate(Command command)
        {
            _command = command;
            cmb_Command.SelectedIndex = command.Template.ID;
            SetParameters(command.Parameters);
        }

        private void SetParameters(List<CommandParameter> parameters)
        {
            flp_Parameters.Controls.Clear();

            if ((parameters != null) && (parameters.Count > 0))
            {
                List<Control> controls = new List<Control>();

                foreach (CommandParameter parameter in parameters)
                {
                    bool isHex = parameter.Template.IsHex;
                    bool isSigned = parameter.Template.IsSigned;
                    int range = (1 << (parameter.Template.ByteLength << 3));

                    GroupBox groupBox = new GroupBox();
                    //if (parameter.Template.Type == CommandParameterType.Number)
                    Dictionary<int, string> parameterValueMap = null;
                    if (!_parameterValueMaps.TryGetValue(parameter.Template.Type, out parameterValueMap))
                    {
                        NumericUpDown spinner = new NumericUpDown();
                        spinner.Width = (parameter.GetByteLength() * 20) + 20;
                        spinner.Minimum = isSigned ? (-(range / 2)) : 0;
                        spinner.Maximum = isSigned ? ((range / 2) - 1) : (range - 1);
                        spinner.Hexadecimal = isHex;
                        spinner.Value = (parameter.Value > spinner.Maximum) ? -(range - parameter.Value) : parameter.Value;
                        groupBox.Controls.Add(spinner);
                    }
                    else
                    {
                        ComboBox comboBox = new ComboBox();
                        List<string> entryNames = DataHelper.GetParameterEntryNames(parameter.Template, parameterValueMap);
                        comboBox.Items.AddRange(entryNames.ToArray());
                        comboBox.SelectedIndex = parameter.Value;
                        groupBox.Controls.Add(comboBox);
                    }

                    groupBox.Text = parameter.Template.Name + (isHex ? " (h)" : "");
                    groupBox.AutoSize = true;
                    controls.Add(groupBox);
                }

                flp_Parameters.Controls.AddRange(controls.ToArray());
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
    }
}
