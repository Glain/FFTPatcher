using FFTPatcher.Controls;
using FFTPatcher.Datatypes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Spinner = FFTPatcher.Controls.NumericUpDownWithDefault;

namespace FFTPatcher
{
    public partial class ViewStatForm : Form
    {
        private class StatValueSet<T>
        {
            public T HP { get; set; }
            public T MP { get; set; }
            public T SP { get; set; }
            public T PA { get; set; }
            public T MA { get; set; }
        }

        private class StatValueSet : StatValueSet<int> { };

        private StatValueSet[] baseStatSets = new StatValueSet[4] {
            new StatValueSet { HP = 507904, MP = 237568, SP = 98304, PA = 81920, MA = 65536 },
            new StatValueSet { HP = 475136, MP = 253952, SP = 98304, PA = 65536, MA = 81920 },
            new StatValueSet { HP = 507904, MP = 253952, SP = 98304, PA = 81920, MA = 81920 },
            new StatValueSet { HP = 598016, MP = 77880, SP = 81920, PA = 90112, MA = 90112 }
        };

        private Dictionary<Spinner, Label> baseStatControlMap; 

        private FFTPatch fftPatch;
        private List<Job> jobList;
        private int jobID;
        private ToolTip toolTip = new ToolTip();
        private bool autoUpdateGrid = false;

        public ViewStatForm(FFTPatch fftPatch, int jobID)
        {
            InitializeComponent();

            this.fftPatch = fftPatch;
            this.jobID = jobID;

            jobList = new List<Job>(fftPatch.Jobs.Jobs);

            SetupForm();
        }

        private void SetupForm()
        {
            baseStatControlMap = new Dictionary<Spinner, Label>()
            {
                { spinner_BaseHP, lbl_BaseHP_Num },
                { spinner_BaseMP, lbl_BaseMP_Num },
                { spinner_BaseSP, lbl_BaseSP_Num },
                { spinner_BasePA, lbl_BasePA_Num },
                { spinner_BaseMA, lbl_BaseMA_Num }
            };

            foreach (Spinner spinner in baseStatControlMap.Keys)
            {
                spinner.ValueChanged += baseStatSpinner_ValueChanged;
            }

            cmb_BaseStatsSet.Items.Clear();
            cmb_BaseStatsSet.Items.AddRange(new string[4] {
                "Male",
                "Female",
                "Ramza",
                "Monster"
            });
            cmb_BaseStatsSet.SelectedIndex = 0;

            cmb_Job.Items.Clear();
            for (int jobIndex = 0; jobIndex < jobList.Count; jobIndex++)
            {
                cmb_Job.Items.Add(jobList[jobIndex].ToString());
            }

            cmb_Job.SelectedIndex = jobID;

            autoUpdateGrid = true;
            UpdateGrid();
        }

        private void FillBaseStats(StatValueSet baseStats)
        {
            spinner_BaseHP.SetValueAndDefault(baseStats.HP, baseStats.HP, toolTip);
            spinner_BaseMP.SetValueAndDefault(baseStats.MP, baseStats.MP, toolTip);
            spinner_BaseSP.SetValueAndDefault(baseStats.SP, baseStats.SP, toolTip);
            spinner_BasePA.SetValueAndDefault(baseStats.PA, baseStats.PA, toolTip);
            spinner_BaseMA.SetValueAndDefault(baseStats.MA, baseStats.MA, toolTip);
        }

        private void FillGrowthsAndMultipliers(Job job)
        {
            spinner_GrowthHP.SetValueAndDefault(job.HPConstant, job.Default.HPConstant, toolTip);
            spinner_GrowthMP.SetValueAndDefault(job.MPConstant, job.Default.MPConstant, toolTip);
            spinner_GrowthSP.SetValueAndDefault(job.SpeedConstant, job.Default.SpeedConstant, toolTip);
            spinner_GrowthPA.SetValueAndDefault(job.PAConstant, job.Default.PAConstant, toolTip);
            spinner_GrowthMA.SetValueAndDefault(job.MAConstant, job.Default.MAConstant, toolTip);

            spinner_MultHP.SetValueAndDefault(job.HPMultiplier, job.Default.HPMultiplier, toolTip);
            spinner_MultMP.SetValueAndDefault(job.MPMultiplier, job.Default.MPMultiplier, toolTip);
            spinner_MultSP.SetValueAndDefault(job.SpeedMultiplier, job.Default.SpeedMultiplier, toolTip);
            spinner_MultPA.SetValueAndDefault(job.PAMultiplier, job.Default.PAMultiplier, toolTip);
            spinner_MultMA.SetValueAndDefault(job.MAMultiplier, job.Default.MAMultiplier, toolTip);
        }

        private void UpdateGrid()
        {
            dgv_Stats.Rows.Clear();

            StatValueSet stats = new StatValueSet()
            {
                HP = (int)(spinner_BaseHP.Value),
                MP = (int)(spinner_BaseMP.Value),
                SP = (int)(spinner_BaseSP.Value),
                PA = (int)(spinner_BasePA.Value),
                MA = (int)(spinner_BaseMA.Value)
            };

            StatValueSet growths = new StatValueSet()
            {
                HP = (int)(spinner_GrowthHP.Value),
                MP = (int)(spinner_GrowthMP.Value),
                SP = (int)(spinner_GrowthSP.Value),
                PA = (int)(spinner_GrowthPA.Value),
                MA = (int)(spinner_GrowthMA.Value)
            };

            StatValueSet multipliers = new StatValueSet()
            {
                HP = (int)(spinner_MultHP.Value),
                MP = (int)(spinner_MultMP.Value),
                SP = (int)(spinner_MultSP.Value),
                PA = (int)(spinner_MultPA.Value),
                MA = (int)(spinner_MultMA.Value)
            };

            for (int level = 1; level <= 99; level++)
            {
                StatValueSet multStats = new StatValueSet()
                {
                    HP = (stats.HP * multipliers.HP / 100),
                    MP = (stats.MP * multipliers.MP / 100),
                    SP = (stats.SP * multipliers.SP / 100),
                    PA = (stats.PA * multipliers.PA / 100),
                    MA = (stats.MA * multipliers.MA / 100)
                };

                StatValueSet<decimal> decimalStats = new StatValueSet<decimal>()
                {
                    HP = (multStats.HP / 16384.0M),
                    MP = (multStats.MP / 16384.0M),
                    SP = (multStats.SP / 16384.0M),
                    PA = (multStats.PA / 16384.0M),
                    MA = (multStats.MA / 16384.0M)
                };

                StatValueSet<decimal> displayStats = new StatValueSet<decimal>()
                {
                    HP = Math.Truncate(decimalStats.HP * 100.0M) / 100.0M,
                    MP = Math.Truncate(decimalStats.MP * 100.0M) / 100.0M,
                    SP = Math.Truncate(decimalStats.SP * 100.0M) / 100.0M,
                    PA = Math.Truncate(decimalStats.PA * 100.0M) / 100.0M,
                    MA = Math.Truncate(decimalStats.MA * 100.0M) / 100.0M
                };

                dgv_Stats.Rows.Add(level, displayStats.HP, displayStats.MP, displayStats.SP, displayStats.PA, displayStats.MA);

                stats.HP += (stats.HP / (growths.HP + level));
                stats.MP += (stats.MP / (growths.MP + level));
                stats.SP += (stats.SP / (growths.SP + level));
                stats.PA += (stats.PA / (growths.PA + level));
                stats.MA += (stats.MA / (growths.MA + level));
            }
        }

        private void UpdateJob(int jobID)
        {
            Job job = jobList[jobID];

            job.HPConstant = (byte)(spinner_GrowthHP.Value);
            job.MPConstant = (byte)(spinner_GrowthMP.Value);
            job.SpeedConstant = (byte)(spinner_GrowthSP.Value);
            job.PAConstant = (byte)(spinner_GrowthPA.Value);
            job.MAConstant = (byte)(spinner_GrowthMA.Value);

            job.HPMultiplier = (byte)(spinner_MultHP.Value);
            job.MPMultiplier = (byte)(spinner_MultMP.Value);
            job.SpeedMultiplier = (byte)(spinner_MultSP.Value);
            job.PAMultiplier = (byte)(spinner_MultPA.Value);
            job.MAMultiplier = (byte)(spinner_MultMA.Value);
        }

        private void cmb_BaseStatsSet_SelectedIndexChanged(object sender, EventArgs e)
        {
            FillBaseStats(baseStatSets[cmb_BaseStatsSet.SelectedIndex]);

            if (autoUpdateGrid)
                UpdateGrid();
        }

        private void cmb_Job_SelectedIndexChanged(object sender, EventArgs e)
        {
            FillGrowthsAndMultipliers(jobList[cmb_Job.SelectedIndex]);

            if (autoUpdateGrid)
                UpdateGrid();
        }

        private void baseStatSpinner_ValueChanged(object sender, EventArgs e)
        {
            Spinner spinner = (Spinner)sender;
            Label label = baseStatControlMap[spinner];

            int baseStatValue = (int)spinner.Value;
            decimal decimalValue = baseStatValue / 16384.0M;
            decimalValue = Math.Truncate(decimalValue * 100.0M) / 100.0M;
            label.Text = decimalValue.ToString("F2");
        }

        private void btn_Recalculate_Click(object sender, EventArgs e)
        {
            UpdateGrid();
        }

        private void btn_SaveToJob_Click(object sender, EventArgs e)
        {
            UpdateJob(cmb_Job.SelectedIndex);
            OnSaveToJobClicked();
            Close();
        }

        public event EventHandler<ReferenceEventArgs> SaveToJobClicked;
        private void OnSaveToJobClicked()
        {
            if (SaveToJobClicked != null)
            {
                SaveToJobClicked(this, new ReferenceEventArgs(cmb_Job.SelectedIndex));
            }
        }
    }
}
