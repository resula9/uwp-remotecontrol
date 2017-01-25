using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Enumeration;
using Windows.Devices.I2c;

namespace Devistator
{
    public class MotorController
    {
        enum MotorInstance
        {
            A,
            B
        };

        const byte MotorSpeedSet = 0x82;
        const byte PWMFrequenceSet = 0x84;
        const byte DirectionSet = 0xaa;
        const byte MotorSetA = 0xa1;
        const byte MotorSetB = 0xa5;
        const byte Nothing = 0x01;
        const byte EnableStepper = 0x1a;
        const byte UnenableStepper = 0x1b;
        const byte Stepernu = 0x1c;

        const byte I2CMotorDriverAdd = 0x0f;

        const string I2CControllerName = "I2C1";

        private I2cDevice motorController = null;

        public MotorController()
        {

        }

        public async Task initialize()
        {
            try
            {
                var settings = new I2cConnectionSettings(I2CMotorDriverAdd);
                settings.BusSpeed = I2cBusSpeed.StandardMode;

                string aqs = I2cDevice.GetDeviceSelector(I2CControllerName);
                var dis = await DeviceInformation.FindAllAsync(aqs);
                motorController = await I2cDevice.FromIdAsync(dis[0].Id, settings);
            }
            catch (Exception e)
            {
                Debug.WriteLine("Exception: " + e.Message + "\n" + e.StackTrace);
                throw;
            }
        }

        const byte LeftForward = 0x2;
        const byte LeftReverse = 0x1;

        const byte RightForward = 0x8;
        const byte RightReverse = 0x4;

        private void setSpeedCommon(MotorInstance inst, float percent)
        {
            try
            {
                // Direction is a bitmask and can only be set for both motors at once (though you can set different directions for each)
                byte dir = 0x02;
                if (percent < 0)
                {
                    dir = 0x01;
                    percent = Math.Abs(percent);
                }

                byte speed = (byte)Math.Floor(percent * 255);

                byte[] setSpeedBuffer = new byte[] { (inst == MotorInstance.A) ? MotorSetA : MotorSetB, dir, speed };

                motorController.Write(setSpeedBuffer);
            }
            catch (Exception e)
            {
                Debug.WriteLine("Exception: " + e.Message + "\n" + e.StackTrace);
                throw;
            }
        }

        public void setSpeedAB(float percentA, float percentB)
        {
            try
            {
                byte direction = 0x00;
                if (percentA < 0)
                {
                    direction = LeftReverse;
                }
                else
                {
                    direction = LeftForward;
                }

                if (percentB < 0)
                {
                    direction |= RightReverse;
                }
                else
                {
                    direction |= RightForward;
                }


                byte speedA = (byte)Math.Floor(Math.Abs(percentA) * 255);
                byte speedB = (byte)Math.Floor(Math.Abs(percentB) * 255);

                byte[] setDirectionBuffer = new byte[] { DirectionSet, direction };

                motorController.Write(setDirectionBuffer);

                byte[] setSpeedBuffer = new byte[] { MotorSpeedSet, speedA, speedB };

                motorController.Write(setSpeedBuffer);
            }
            catch (Exception e)
            {
                Debug.WriteLine("Exception: " + e.Message + "\n" + e.StackTrace);
            }
        }

        public void setSpeedA(float percent)
        {
            setSpeedCommon(MotorInstance.A, percent);
        }

        public void setSpeedB(float percent)
        {
            setSpeedCommon(MotorInstance.B, percent);
        }


    }
}