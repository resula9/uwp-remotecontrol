using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Enumeration;
using Windows.Devices.SerialCommunication;
using Windows.Storage.Streams;
using Windows.UI.Xaml;

// 1. A robot may not injure a human being or, through inaction, allow a human being to come to harm.
// 2. A robot must obey orders given it by human beings except where such orders would conflict with the First Law.
// 3. A robot must protect its own existence as long as such protection does not conflict with the First or Second Law.
//          - Isaac Asimov

// Implemenation of the MultiWii serial protocol
// http://www.multiwii.com/wiki/index.php?title=Multiwii_Serial_Protocol
namespace MultiWiiDrone
{
    class MSP
    {
        public struct IMU
        {
            public Vector3 accelerometer;
            public Vector3 gyroscope;
            public Vector3 magnetometer;
        };

        public IMU imu;

        public enum Channels
        {
            Roll,
            Pitch,
            Yaw,
            Throttle,
            Arm,
            Aux1,
            Aux2,
            Aux4
        }

        public delegate void RCChannelsUpdatedDelegate();

        public event RCChannelsUpdatedDelegate ChannelDelegate;

        private enum MSP_Op : byte
        {
            None = 0,
            ApiVersion = 1,
            FlightControllerVariant = 2,
            FlightControllerVerson = 3,
            BoardInfo = 4,
            BuidlInfo = 5,

            // MSP for Cleanflight
            BatteryConfig = 32,
            SetBatteryConfig = 33,
            ModeRanges = 34,
            SetModeRange = 35,
            Feature = 36,
            SetFeature = 37,
            BoardAlignment = 38,
            SetBoardAlignment = 39,
            AmpMeterConfig = 40,
            SetAmpMeterCofnig = 41,
            Mixer = 42,
            SetMixer = 43,
            ReceiverConfig = 44,
            SetReceiverConfig = 45,

            // todo : LEDS

            Sonar = 58,
            PidController = 59,
            SetPidController = 60,
            ArmingConfig = 61,
            SetArmingConfig = 62,

            // todo: rest of cleanflight extensions.

            VoltMeter = 128,
            BatteryState = 130,

            // original msp commands
            Identify = 100,
            Status = 101,
            RawIMU = 102,
            Servo = 103,
            Motor = 104,
            RC = 105,

            Atitude = 108,
            Altitude = 109,

            SetRawRCChannels = 200,
        }
        
        private SerialDevice _device;
        private DataWriter writer = null;
        private DataReader reader = null;

        const UInt16 kStickMin = 1100;
        const UInt16 kStickMid = 1500;
        const UInt16 kStickMax = 1900;
        const UInt16 kChannelArmValue = 1500;
        const UInt16 kChannelDisarmValue = 900;
        const UInt16 kChannelCount = 8;

        // heartbeat
        UInt16 roll = kStickMid;
        UInt16 pitch = kStickMid;
        UInt16 yaw = kStickMid;
        UInt16 throttle = kStickMin;
        UInt16 aux1 = kStickMid;
        UInt16 aux2 = kStickMid;
        UInt16 arm = kChannelDisarmValue;
        UInt16 aux4 = kStickMid;

        bool channelChanged = true;     // reset to defaults
        bool sendIdent = true;
        bool sendVariant = true;
        bool sendGetRC = false;

        public MSP()
        {
        }

        public async Task connect(string identifyingSubStr = "UART0")
        {
            string selector = SerialDevice.GetDeviceSelector();
            var deviceCollection = await DeviceInformation.FindAllAsync(selector);

            if (deviceCollection.Count == 0)
                return;

            for (int i = 0; i < deviceCollection.Count; ++i)
            {
                if (deviceCollection[i].Name.Contains(identifyingSubStr) || deviceCollection[i].Id.Contains(identifyingSubStr))
                {
                    _device = await SerialDevice.FromIdAsync(deviceCollection[i].Id);
                    if (_device != null)
                    {
                        _device.BaudRate = 115200;
                        _device.Parity = SerialParity.None;
                        _device.DataBits = 8;
                        _device.StopBits = SerialStopBitCount.One;
                        _device.Handshake = SerialHandshake.None;
                        _device.ReadTimeout = TimeSpan.FromSeconds(5);
                        _device.WriteTimeout = TimeSpan.FromSeconds(5);
                        _device.IsRequestToSendEnabled = false;


                        writer = new DataWriter(_device.OutputStream);
                        reader = new DataReader(_device.InputStream);
                        reader.InputStreamOptions = InputStreamOptions.Partial;

                        startHeartbeat();
                        startWatchingResponses();

                        return;
                    }
                }
            }
        }

        private void startHeartbeat()
        {
            Task t = Task.Run(async () =>
            {
                while (true)
                {
                    if (!channelChanged && !sendIdent && !sendVariant && !sendGetRC)
                    {
                        await sendMessage(MSP_Op.Status);
                    }

                    if (channelChanged)
                    {
                        MemoryStream stream = new MemoryStream();
                        using (BinaryWriter byteWriter = new BinaryWriter(stream))
                        {
                            byteWriter.Write(roll);
                            byteWriter.Write(pitch);
                            byteWriter.Write(yaw);
                            byteWriter.Write(throttle);
                            byteWriter.Write(aux1);
                            byteWriter.Write(aux2);
                            byteWriter.Write(arm);
                            byteWriter.Write(aux4);
                        }

                        var payload = stream.ToArray();
                        await sendMessage(MSP_Op.SetRawRCChannels, payload);

                        channelChanged = false;
                    }

                    if (sendIdent)
                    {
                        await sendMessage(MSP_Op.Identify);
                        sendIdent = false;
                    }

                    if (sendVariant)
                    {
                        await sendMessage(MSP_Op.FlightControllerVariant);
                        sendVariant = false;
                    }

                    if (sendGetRC)
                    {
                        await sendMessage(MSP_Op.RC);
                        sendGetRC = false;
                    }

                    await Task.Delay(100);
                }
            });
        }


        enum ReadState
        {
            Idle,
            StartToken, // $
            Preamble, // M
            Direction, // < or >
            Length, // byte
            OpCode, // from MspOp
            Payload, // 1+ bytes
            ProcessPayload
        };

        enum MessageDirection
        {
            Inbound,
            Outbound
        };

        public void Arm()
        {
            Debug.WriteLine("S: ARMED");
            arm = kChannelArmValue;
            channelChanged = true;
        }

        public void Disarm()
        {
            Debug.WriteLine("S: DISARMED");
            arm = kChannelDisarmValue;
            channelChanged = true;
        }

        public void ToggleArm()
        {
            if (arm == kChannelArmValue)
            {
                Disarm();
            }
            else
            {
                Arm();
            }
        }

        public double Throttle
        {
            get
            {
                return (throttle - kStickMin) / (double)(kStickMax - kStickMin);
            }

            set
            {

                setThrottleChannel(Channels.Throttle, value);
            }
        }

        public double Yaw
        {
            get
            {
                return (yaw - kStickMin) / (double)(kStickMax - kStickMin);
            }

            set
            {

                setChannel(Channels.Yaw, value);
            }
        }

        public double Roll
        {
            get
            {
                return (roll - kStickMin) / (double)(kStickMax - kStickMin);
            }

            set
            {

                setChannel(Channels.Roll, value);
            }
        }

        public double Pitch
        {
            get
            {
                return (pitch - kStickMin) / (double)(kStickMax - kStickMin);
            }

            set
            {

                setChannel(Channels.Pitch, value);
            }
        }

        private async Task sendMessage(MSP_Op op, byte[] bytes = null)
        {
            byte opCode = (byte)op;
            byte dataLength = 0;
            if (bytes != null)
            {
                if (bytes.Length > 255)
                {
                    Debug.WriteLine("Sending a message longer than an MSP message");
                    return;
                }
                else
                {
                    dataLength = (byte)bytes.Length;
                }
            }
            writer.WriteByte(36); // $
            writer.WriteByte(77); // M
            writer.WriteByte(60); // < 
            writer.WriteByte(dataLength);
            writer.WriteByte(opCode);

            byte checksum = (byte)(dataLength ^ opCode);

            if (dataLength == 0)
            {
                writer.WriteByte(checksum);
            }
            else
            {
                foreach (var b in bytes)
                {
                    writer.WriteByte(b);

                    checksum ^= b;
                }
                writer.WriteByte(checksum);
            }

            await writer.StoreAsync();
        }

        private void setChannel(Channels channel, UInt16 value)
        {
            if (writer == null)
            {
                return;
            }

            switch (channel)
            {
                case Channels.Roll:
                    roll = value;
                    break;
                case Channels.Pitch:
                    pitch = value;
                    break;
                case Channels.Yaw:
                    yaw = value;
                    break;
                case Channels.Throttle:
                    throttle = value;
                    break;
                case Channels.Arm:
                    arm = value;
                    break;
                case Channels.Aux1:
                    aux1 = value;
                    break;
                case Channels.Aux2:
                    aux2 = value;
                    break;
                case Channels.Aux4:
                    aux4 = value;
                    break;
            }

            Debug.WriteLine($"S: {channel.ToString()} - {value}");

            channelChanged = true;

        }

        private void setThrottleChannel(Channels channel, double value)
        {
            double f = (value * (double)(kStickMax - kStickMin));
            UInt16 val = (UInt16)(f + kStickMin);

            setChannel(channel, val);
        }

        private void setChannel(Channels channel, double value)
        {
            UInt16 val;
            if (value < 0.0)
            {
                double f = (value * (double)(kStickMid - kStickMin)) + kStickMid;
                val = (UInt16)(Math.Abs(f));
            }
            else
            {
                double f = (value * (double)(kStickMax - kStickMid));
                val = (UInt16)(f + kStickMid);
            }
            setChannel(channel, val);
        }

        private const byte MspPayloadSize = 255;

        private void startWatchingResponses()
        {
            Task t = Task.Run(async () =>
            {
                byte[] messagehistory = new byte[1024000];
                ReadState[] statehistory = new ReadState[1024000];
                uint messagehistoryindex = 0;
                byte[] payload = new byte[MspPayloadSize];
                ReadState readState = ReadState.Idle;
                //MessageDirection direction = MessageDirection.Inbound;
                byte checksum = 0;
                byte specifiedChecksum = 0;
                byte messageLengthExpectation = 0;
                byte messageIndex = 0;

                MSP_Op opcode = MSP_Op.None ;

                while (true)
                {
                    var result = await reader.LoadAsync(1);
                    byte readByte = reader.ReadByte();
                    messagehistory[messagehistoryindex] = readByte;
                    statehistory[messagehistoryindex] = readState;
                    messagehistoryindex++;

                    switch (readState)
                    {
                        case ReadState.Idle:
                            if (readByte == Convert.ToByte('$'))
                            {
                                readState = ReadState.Preamble;
                            }
                            break;

                        case ReadState.Preamble:
                            if (readByte == Convert.ToByte('M'))
                            {
                                readState = ReadState.Direction;
                            }
                            else
                            {
                                readState = ReadState.Idle;
                            }
                            break;

                        case ReadState.Direction:
                            if (readByte == Convert.ToByte('>'))
                            {
                                //direction = MessageDirection.Inbound;
                            }
                            else if (readByte == Convert.ToByte('<'))
                            {
                                //direction = MessageDirection.Outbound;
                            }
                            else if (readByte == Convert.ToByte('!'))
                            {
                                Debug.WriteLine("Flight controller reports an unsupported command");
                            }
                            else
                            {
                                Debug.WriteLine("Unknown token reading Direction - " + readByte.ToString("x"));
                            }

                            // Advance anyway;
                            readState = ReadState.Length;
                            break;

                        case ReadState.Length:
                            messageLengthExpectation = readByte;
                            checksum = readByte;

                            readState = ReadState.OpCode;
                            break;

                        case ReadState.OpCode:
                            opcode = (MSP_Op)readByte;
                            checksum ^= readByte;
                            if (messageLengthExpectation > 0)
                            {
                                readState = ReadState.Payload;
                            }
                            else
                            {
                                readState = ReadState.ProcessPayload;
                            }
                            break;

                        case ReadState.Payload:
                            payload[messageIndex++] = readByte;
                            checksum ^= readByte;

                            if (messageIndex >= messageLengthExpectation)
                            {
                                readState = ReadState.ProcessPayload;
                            }
                            break;

                        case ReadState.ProcessPayload:
                            specifiedChecksum = readByte;
                            if (specifiedChecksum == checksum)
                            {
                                processMessage(opcode, payload, messageLengthExpectation);
                            }
                            else
                            { 
                                Debug.WriteLine($"Processing Opcode {opcode} Checksum failed: Seen {checksum} but expected {specifiedChecksum}");
                            }
                            readState = ReadState.Idle;
                            opcode = MSP_Op.None;
                            messageIndex = 0;
                            break;
                    }
                }
            });
        }

        float RCChannelToFloat(byte[] bytes, byte offset)
        {
            UInt16 channelSignal = BitConverter.ToUInt16(bytes, offset);
            if (channelSignal < kStickMin)
            {
                channelSignal = kStickMin;
            }

            float value = ((float)channelSignal - kStickMin) / (float)(kStickMax - kStickMin);

            return value;
        }

        void processMessage(MSP_Op code, byte[] bytes, byte length)
        {
            //Debug.WriteLine("message received: " + code.ToString());

            switch (code)
            {
                case MSP_Op.Status:
                    {
                        ushort pidDeltaUs = BitConverter.ToUInt16(bytes, 0);
                        ushort i2cError = BitConverter.ToUInt16(bytes, 2);
                        ushort activeSensors = BitConverter.ToUInt16(bytes, 4);
                        ushort mode = BitConverter.ToUInt16(bytes, 6);
                        ushort profile = BitConverter.ToUInt16(bytes, 10);
                        ushort cpuload = BitConverter.ToUInt16(bytes, 11);
                        ushort gyroDeltaUs = BitConverter.ToUInt16(bytes, 13);

                        if ((mode & 0x1) == 1)
                        {
                            if (arm != kChannelArmValue)
                            {
                                Debug.WriteLine("Ack! integrity error");
                            }
                        }
                    }
                    break;
                case MSP_Op.Identify:
                    {
                        Debug.WriteLine("Received Identity if you care");
                    }
                    break;

                case MSP_Op.FlightControllerVariant:
                    {
                        StringBuilder builder = new StringBuilder();
                        ASCIIEncoding ai = new ASCIIEncoding();
                        for (int i = 0; i < 4; i++)
                        {
                            builder.Append(ai.GetString(bytes, i, 1));
                        }

                        Debug.WriteLine($"Flight Controller ID :{builder.ToString()}");
                    }
                    break;

                case MSP_Op.RawIMU:
                    {
                        imu.accelerometer.X = BitConverter.ToInt16(bytes, 0) / 512.0f;
                        imu.accelerometer.Y = BitConverter.ToInt16(bytes, 2) / 512.0f;
                        imu.accelerometer.Z = BitConverter.ToInt16(bytes, 4) / 512.0f;

                        imu.gyroscope.X = BitConverter.ToInt16(bytes, 6) * 4.0f / 16.4f;
                        imu.gyroscope.Y = BitConverter.ToInt16(bytes, 8) * 4.0f / 16.4f;
                        imu.gyroscope.Z = BitConverter.ToInt16(bytes, 10) * 4.0f / 16.4f;

                        imu.magnetometer.X = BitConverter.ToInt16(bytes,12) / 1090;
                        imu.magnetometer.Y = BitConverter.ToInt16(bytes, 14) / 1090;
                        imu.magnetometer.Z = BitConverter.ToInt16(bytes, 16) / 1090;
                    }
                    break;

                case MSP_Op.RC:
                    {
                        // Should only happen at startup...
                        int activeChannels = length / 2;

                        if (activeChannels > 0)
                        {
                            roll = BitConverter.ToUInt16(bytes, 0);
                        }

                        if (activeChannels > 1)
                        {
                            pitch = BitConverter.ToUInt16(bytes, 2);
                        }

                        if (activeChannels > 2)
                        {
                            yaw = BitConverter.ToUInt16(bytes, 4);
                        }

                        if (activeChannels > 3)
                        {
                            throttle = BitConverter.ToUInt16(bytes, 6);
                        }

                        if (activeChannels > 4)
                        {
                            aux1 = BitConverter.ToUInt16(bytes, 8);
                        }

                        if (activeChannels > 5)
                        {
                            aux2 = BitConverter.ToUInt16(bytes, 10);
                        }

                        if (activeChannels > 6)
                        {
                            arm = BitConverter.ToUInt16(bytes, 12);
                        }

                        if (activeChannels > 7)
                        {
                            aux4 = BitConverter.ToUInt16(bytes, 14);
                        }

                        ChannelDelegate?.Invoke();
                    }
                    break;
            }

        }

    }
}
