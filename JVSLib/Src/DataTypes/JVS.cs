using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JVSLib.Src.DataTypes
{
    public class JVSCodes
    {
        public const byte JVSSync = 0xE0;
        public const byte JVSMark = 0xD0;
    }

    public enum JVSStatus
    {
        Normal = 1,
        UnknownCommand = 2,
        ChecksumError = 3,
        AckOverflow = 4
    }

    public enum JVSReport
    {
        Normal = 1,
        IncorrectParams = 2,
        InvalidParams = 3,
        Busy = 4
    }

    public class JVSReq
    {
        public byte nodeNum;
        public byte[] data;
    }

    public class JVSRes
    {
        public byte nodeNum;
        public JVSStatus status;
        public byte[] data;
    }

    public class JVSSetAddress
    {
        public JVSStatus status;
        public JVSReport report;
    }

    public class JVSIOIdentify
    {
        public JVSStatus status;
        public JVSReport report;
        public string identifier;
    }

    public class JVSCmdRev
    {
        public JVSStatus status;
        public JVSReport report;
        public byte rev;
    }

    public class JVSRev
    {
        public JVSStatus status;
        public JVSReport report;
        public byte rev;
    }

    public class JVSCommVer
    {
        public JVSStatus status;
        public JVSReport report;
        public byte ver;
    }

    public class JVSFeatCheck
    {
        public enum FeatType
        {
            Switch = 1,
            Coin = 2,
            Analog = 3,
            Rotary = 4,
            Keycode = 5,
            ScreenPos = 6,
            MiscSwitch = 7,
            
            CardSystem = 0x10,
            MedalHopper = 0x11,
            GeneralPurposeOut = 0x12,
            AnalogOut = 0x13,
            CharacterOut = 0x14,
            Backup = 0x15
        }

        public class FeatureParam
        {
            public byte param1;
            public byte param2;
            public byte param3;
        }

        public JVSStatus status;
        public JVSReport report;
        public Dictionary<FeatType, FeatureParam> features = new Dictionary<FeatType, FeatureParam>();
    }

    public class JVSMainID
    {
        public JVSStatus status;
        public JVSReport report;
    }

    public class JVSSwInp
    {
        public JVSStatus status;
        public JVSReport report;
        public bool[] systemInputs;
        public bool[,] inputs;
    }

    public class JVSCoinInp
    {
        public enum SlotCondition
        {
            Normal = 0,
            CoinJam = 1,
            NoCounter = 2,
            Busy = 3
        }

        public class CoinSlot
        {
            public SlotCondition condition;
            public short coinCount;
        }

        public JVSStatus status;
        public JVSReport report;
        public List<CoinSlot> slots = new List<CoinSlot>();
    }

    public class JVSAnalogInp
    {
        public JVSStatus status;
        public JVSReport report;
        public List<ushort> channels = new List<ushort>();
    }

    public class JVSRotaryInp
    {
        public JVSStatus status;
        public JVSReport report;
        public List<short> channels = new List<short>();
    }

    public class JVSKeyInp
    {
        public JVSStatus status;
        public JVSReport report;
        public byte keyCode;
    }

    public class JVSScrPosInp
    {
        public JVSStatus status;
        public JVSReport report;
        public ushort X;
        public ushort Y;
    }
}
