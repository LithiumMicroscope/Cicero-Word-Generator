using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace DataStructures
{
    [Serializable, TypeConverter(typeof(ExpandableObjectConverter))]
    public class LogicalChannel
        //benno 2009
        //lauriane 01/03/2012
    {
        private bool enabled;

        public bool Enabled
        {
            get { return enabled; }
            set { enabled = value; }
        }

        //Hotkey for toggling the override value.
        public char hotkeyChar;

        //Hotkey for turning override on and off.
        public char overrideHotkeyChar;

        public bool overridden;

        public bool digitalOverrideValue;
        public double analogOverrideValue;

        private string name;

        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        private string description;

        public string Description
        {
            get { return description; }
            set { description = value; }
        }

        // 16/12/2009 Benno: @todo Change this such that all the types of the Units are used
        // Underneath is a sample code that can be used
        //public static Array allUnits = Enum.GetValues(typeof(Units.DimensionID));
        /*private Array unit = Enum.GetValues(typeof(Units.DimensionID));

        public string Unit
        {
            get { return unit; }
            set { unit = value; }
        }*/
        public Units.Dimension unit;
        /*public Units.Dimension Unit
        {
            get { return unit; }
            set
            {
                if (typeof(value) == string) {

                }
            }
        }*/

        private string conversion;

        public string Conversion
        {
            get { return conversion; }
            set { conversion = value; }
        }

        public HardwareChannel HardwareChannel;

        /// <summary>
        /// True if this channel will actually be a toggling channel... ie when the buffer is generated, it will oscillate between 0 and 1.
        /// </summary>
        private bool togglingChannel;

        public bool TogglingChannel
        {
            get { return togglingChannel; }
            set { togglingChannel = value; }
        }

        public LogicalChannel()
        {
            name = "";
            description = "";
            unit = Units.Dimension.V;
            conversion = "V";
            HardwareChannel = HardwareChannel.Unassigned;
            enabled = true;
            overridden = false;
            digitalOverrideValue = false;
            analogOverrideValue = 0;
            togglingChannel = false;
        }

        /// <summary>
        /// Meaningfull only for analog logical channels. If true, then when running "output now" the analog channel
        /// gets its value from the end of the dwell word. Otherwise, it gets its value from the end of the output word.
        /// </summary>
        private bool analogChannelOutputNowUsesDwellWord;

        public bool AnalogChannelOutputNowUsesDwellWord
        {
            get { return analogChannelOutputNowUsesDwellWord; }
            set { analogChannelOutputNowUsesDwellWord = value; }
        }

        //to the end: add 01/03/2012 lauriane
        private bool doOverrideDigitalColor;

        public bool DoOverrideDigitalColor
        {
            get { return doOverrideDigitalColor; }
            set { doOverrideDigitalColor = value; }
        }
        private System.Drawing.Color overrideColor;

        public System.Drawing.Color OverrideColor
        {
            get { return overrideColor; }
            set { overrideColor = value; }
        }

    }
}
