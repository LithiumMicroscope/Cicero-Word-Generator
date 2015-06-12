using System;
using System.Collections.Generic;
using System.Windows.Forms;

using DataStructures;

namespace WordGenerator.ChannelManager
{
    public partial class AddDevice : Form
    {
        ChannelManager cm;
        ChannelCollection selectedChannelCollection;
        HardwareChannel.HardwareConstants.ChannelTypes selectedChannelType;
        //Units.Dimension selectedUnit;

        /// <summary>
        /// Constructor for the AddDevice form. The ChannelManager parameter is used so that the AddDevice form can
        /// fetch information from the owner ChannelManager form.
        /// </summary>
        public AddDevice(ChannelManager cm)
        {
            InitializeComponent();
            this.cm = cm;   // We create the appropriate reference to the parent form ChannelManager.
                            // In particular so that we can access its availableHardwareChannels list

            okButton.Enabled = false; // We do not enable the OK button, until the user has selected a device type

            // Add entries to the deviceTypeCombo
            foreach (HardwareChannel.HardwareConstants.ChannelTypes ct in HardwareChannel.HardwareConstants.allChannelTypes)
                this.deviceTypeCombo.Items.Add(ct);

            // Add units to unitCombo
            /*foreach (Units.Dimension ut in Units.Dimension.allDimensions)
                this.unitCombo.Items.Add(ut);*/
        }

        private void refreshHardwareChanCombo()
        {
            // Initialize the availableHardwareChannelCombo appropriately
            this.availableHardwareChanCombo.Items.Clear();
            this.availableHardwareChanCombo.Items.Add(HardwareChannel.Unassigned);

            foreach (HardwareChannel hc in cm.knownHardwareChannels)
                if (hc.ChannelType == selectedChannelType)
                {
                    if (!Storage.settingsData.logicalChannelManager.AssignedHardwareChannels.Contains(hc))
                        this.availableHardwareChanCombo.Items.Add(hc);
                }
        }

        private void refreshUnitCombo()
        {
            // Initialize the availableHardwareChannelCombo appropriately
            this.availableUnitsCombo.Items.Clear();
            //this.availableUnitCombo.Items.Add(HardwareChannel.Unassigned);

            foreach (Units.Dimension ut in Units.Dimension.allDimensions)
                this.availableUnitsCombo.Items.Add(ut);
        }

        /// <summary>
        /// In the small AddDevice form, everything begins as soon as the user specifies which ChannelType is to 
        /// be modified. 
        /// </summary>
        private void deviceTypeCombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Point to the correct DeviceCollection
            string selectedTypeString = this.deviceTypeCombo.SelectedItem.ToString();
            selectedChannelType = HardwareChannel.HardwareConstants.ParseChannelTypeFromString(selectedTypeString);
            selectedChannelCollection = Storage.settingsData.logicalChannelManager.GetDeviceCollection(selectedChannelType);

            // Initialize and enable the Name, Description text entries and the HardwareChannel drop-down list
            this.deviceNameText.Enabled = 
            this.deviceDescText.Enabled = 
            this.availableUnitsCombo.Enabled = 
            this.deviceConversionEquation.Enabled =
            true;

            refreshHardwareChanCombo();
            refreshUnitCombo();
            this.availableHardwareChanCombo.Enabled = true;

            // Indicate the logical ID to be used
            this.logicalIDText.Text = selectedChannelCollection.GetNextSuggestedKey().ToString();

            okButton.Enabled = true; // Now we can allow the OK button

            if (selectedChannelType == HardwareChannel.HardwareConstants.ChannelTypes.analog)
            {
                checkBox1.Visible = true;
            }
            else
            {
                checkBox1.Visible = false;
            }

            // 11/12/2009 Benno: Check whether the units need to be shown
            //if (selectedChannelType == HardwareChannel.HardwareConstants.ChannelTypes.analog ||
            //    selectedChannelType == HardwareChannel.HardwareConstants.ChannelTypes.gpib)
            if (selectedChannelType == HardwareChannel.HardwareConstants.ChannelTypes.analog)
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

            if (selectedChannelType == HardwareChannel.HardwareConstants.ChannelTypes.analog ||
                selectedChannelType == HardwareChannel.HardwareConstants.ChannelTypes.digital)
            {
                togglingCheck.Visible = true;
            }
            else
            {
                togglingCheck.Visible = false;
            }
        }

        /// <summary>
        /// We refresh the list of available hardware by asking the owner ChannelManager form to update its list
        /// </summary>
        private void refreshHardwareButton_Click(object sender, EventArgs e)
        {
            cm.RefreshKnownHardwareChannels();
            refreshHardwareChanCombo();
        }
        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void okButton_Click(object sender, EventArgs e)
        {
            // Construct the logical channel
            LogicalChannel lc = new LogicalChannel();
            lc.Name = this.deviceNameText.Text;
            lc.Description = this.deviceDescText.Text;

            if (this.availableUnitsCombo.SelectedItem is Units.Dimension)
                lc.unit = (Units.Dimension)this.availableUnitsCombo.SelectedItem;
            else
                lc.unit = Units.Dimension.V;

            //lc.unit = this.deviceUnitText.Text;
            lc.Conversion = this.deviceConversionEquation.Text;
            lc.AnalogChannelOutputNowUsesDwellWord = checkBox1.Checked;
            lc.TogglingChannel = togglingCheck.Checked;

            if (this.availableHardwareChanCombo.SelectedItem is HardwareChannel)
				lc.HardwareChannel = (HardwareChannel)this.availableHardwareChanCombo.SelectedItem;
            else
				lc.HardwareChannel = HardwareChannel.Unassigned;

            // Add to the appropriate collection
            selectedChannelCollection.AddChannel(lc);
            
            // Refresh the screen of the ChannelManager for visual feedback
            cm.RefreshLogicalDeviceDataGrid();

            // Close
            this.Close();
        }

        private void togglingCheck_CheckedChanged(object sender, EventArgs e)
        {
            
        }


    }
}