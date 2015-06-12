using System;
using System.Collections.Generic;
using System.Windows.Forms;

using DataStructures;

namespace WordGenerator.ChannelManager
{
    public partial class EditDevice : Form
    {
        ChannelManager cm;
        SelectedDevice sd;
        public EditDevice(SelectedDevice sd, ChannelManager cm)
        {
            InitializeComponent();

            this.cm = cm;
            this.sd = sd;

            // Initialize the fields with relevant information
            this.logicalIDText.Text = sd.logicalID.ToString();
            this.deviceTypeText.Text = sd.channelTypeString;
            this.deviceNameText.Text = sd.lc.Name;
            this.deviceDescText.Text = sd.lc.Description;
            // 14/12/2009 Benno: Unit conversion system
            this.deviceConversionEquation.Text = sd.lc.Conversion;
            //this.deviceUnitText.Text = sd.lc.Unit;

            this.availableUnitsCombo.Items.Clear();
            // Fill the availableUnitCombo with relevant items
            foreach (Units.Dimension ut in Units.Dimension.allDimensions)
                this.availableUnitsCombo.Items.Add(ut);
            this.availableUnitsCombo.SelectedItem = sd.lc.unit;

            this.availableHardwareChanCombo.Items.Clear();
            this.availableHardwareChanCombo.Items.Add(HardwareChannel.Unassigned);
            if (sd.lc.HardwareChannel!=null)
				this.availableHardwareChanCombo.Items.Add(sd.lc.HardwareChannel);
            
            // Fill the availableHardwareChanCombo with relevant items
            foreach (HardwareChannel hc in cm.knownHardwareChannels)
                if (hc.ChannelType == sd.channelType)
                    if (!Storage.settingsData.logicalChannelManager.AssignedHardwareChannels.Contains(hc))
                        this.availableHardwareChanCombo.Items.Add(hc);

			this.availableHardwareChanCombo.SelectedItem = sd.lc.HardwareChannel;

            togglingCheck.Checked = sd.lc.TogglingChannel;

            if (sd.channelType == HardwareChannel.HardwareConstants.ChannelTypes.analog)
            {
                checkBox1.Visible = true;
            }
            else
            {
                checkBox1.Visible = false;
            }

            // 11/12/2009 Benno: Check whether the units need to be shown
            //if (sd.channelType == HardwareChannel.HardwareConstants.ChannelTypes.analog ||
            //    sd.channelType == HardwareChannel.HardwareConstants.ChannelTypes.gpib)
            if (sd.channelType == HardwareChannel.HardwareConstants.ChannelTypes.analog)
            {
                this.deviceConversionEquation.Visible =
                this.lblConversion.Visible =
                this.availableUnitsCombo.Visible =
                this.lblUnit.Visible =
                    true;
            }
            else
            {
                this.deviceConversionEquation.Visible =
                this.lblConversion.Visible =
                this.availableUnitsCombo.Visible =
                this.lblUnit.Visible =
                    false;
            }

            if (sd.channelType == HardwareChannel.HardwareConstants.ChannelTypes.analog ||
                sd.channelType == HardwareChannel.HardwareConstants.ChannelTypes.digital)
            {
                togglingCheck.Visible = true;
            }
            else
            {
                togglingCheck.Visible = false;
            }

            checkBox1.Checked = sd.lc.AnalogChannelOutputNowUsesDwellWord;

        }

        private void okButton_Click(object sender, EventArgs e)
        {
            sd.lc.Name = this.deviceNameText.Text;
            sd.lc.Description = this.deviceDescText.Text;

            //sd.lc.unit = this.availableUnitsCombo.SelectedValue;
            if (this.availableUnitsCombo.SelectedItem is Units.Dimension)
                sd.lc.unit = (Units.Dimension)this.availableUnitsCombo.SelectedItem;
            else
                sd.lc.unit = Units.Dimension.V;


            sd.lc.Conversion = this.deviceConversionEquation.Text;
            sd.lc.AnalogChannelOutputNowUsesDwellWord = checkBox1.Checked;
            
            if (this.availableHardwareChanCombo.SelectedItem is HardwareChannel)
				sd.lc.HardwareChannel = (HardwareChannel)this.availableHardwareChanCombo.SelectedItem;
            else
				sd.lc.HardwareChannel = HardwareChannel.Unassigned;

            // Visual feedback
            cm.RefreshLogicalDeviceDataGrid();

            this.Close();
        }
        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void refreshHardwareButton_Click(object sender, EventArgs e)
        {
            cm.RefreshKnownHardwareChannels();

            this.availableHardwareChanCombo.Items.Clear();
            this.availableHardwareChanCombo.Items.Add(HardwareChannel.Unassigned);

            // Fill the availableHardwareChanCombo with relevant items
            foreach (HardwareChannel hc in cm.knownHardwareChannels)
                if (hc.ChannelType == sd.channelType) 
                    if (!Storage.settingsData.logicalChannelManager.AssignedHardwareChannels.Contains(hc))
                        this.availableHardwareChanCombo.Items.Add(hc);
        }

        private void availableHardwareChanCombo_DropDownClosed(object sender, EventArgs e)
        {
        }

        private void togglingCheck_CheckedChanged(object sender, EventArgs e)
        {
            sd.lc.TogglingChannel = togglingCheck.Checked;
        }

    }
}