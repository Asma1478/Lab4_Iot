using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Dynamic;

namespace Lab4_
{
    public class Decoder
    {
        const byte TYPE_TEMP = 0x01; //temp 2 bytes -3276.8°C -->3276.7°C
        const byte TYPE_RH = 0x02; //Humidity 1 byte  0-100%
        const byte TYPE_ACC = 0x03; //acceleration 3 bytes X,Y,Z -128 --> 127 +/-63=1G
        const byte TYPE_LIGHT = 0x04; //Light 2 bytes 0-->65535 Lux
        const byte TYPE_MOTION = 0x05; //No of motion 1 byte  0-255
        const byte TYPE_CO2 = 0x06; //Co2 2 bytes 0-65535 ppm 
        const byte TYPE_VDD = 0x07; //VDD 2byte 0-65535mV
        const byte TYPE_ANALOG1 = 0x08; //VDD 2byte 0-65535mV
        const byte TYPE_GPS = 0x09; //3bytes lat 3bytes long binary
        const byte TYPE_PULSE1 = 0x0A; //2bytes relative pulse count
        const byte TYPE_PULSE1_ABS = 0x0B;  //4bytes no 0->0xFFFFFFFF
        const byte TYPE_EXT_TEMP1 = 0x0C;  //2bytes -3276.5C-->3276.5C
        const byte TYPE_EXT_DIGITAL = 0x0D;  //1bytes value 1 or 0
        const byte TYPE_EXT_DISTANCE = 0x0E;  //2bytes distance in mm
        const byte TYPE_ACC_MOTION = 0x0F;  //1byte number of vibration/motion
        const byte TYPE_IR_TEMP = 0x10;  //2bytes internal temp 2bytes external temp -3276.5C-->3276.5C
        const byte TYPE_OCCUPANCY = 0x11;  //1byte data
        const byte TYPE_WATERLEAK = 0x12;  //1byte data 0-255 
        const byte TYPE_GRIDEYE = 0x13;  //65byte temperature data 1byte ref+64byte external temp
        const byte TYPE_PRESSURE = 0x14;  //4byte pressure data (hPa)
        const byte TYPE_SOUND = 0x15;  //2byte sound data (peak/avg)
        const byte TYPE_PULSE2 = 0x16;  //2bytes 0-->0xFFFF
        const byte TYPE_PULSE2_ABS = 0x17;  //4bytes no 0->0xFFFFFFFF
        const byte TYPE_ANALOG2 = 0x18;  //2bytes voltage in mV
        const byte TYPE_EXT_TEMP2 = 0x19;  //2bytes -3276.5C-->3276.5C


        static int bin16dec(int bin)
        {
            var num = bin & 0xFFFF;

            if ((0x8000 & num) == 32768)
                num = -(0x010000 - num);
            return num;
        }
        static int bin8dec(int bin)
        {
            var num = bin & 0xFF;
            if ((0x80 & num) == 128)
                num = -(0x0100 - num);
            return num;
        }
        static IEnumerable<int> hexToByte(string s)
        {
            for (int i = 0; i < s.Length; i += 2)
            {
                yield return Convert.ToInt32(s.Substring(i, 2), 16);
            }
        }

        public static Data Decode(string hex)
        {
            var data = hexToByte(hex).ToArray();
            Data obj = new Data();
            for (int i = 0; i < data.Length; i++)
            {
                //console.log(data[i]);
                switch ((byte)data[i])
                {
                    case TYPE_TEMP: //Temperature
                        var temp = (data[i + 1] << 8) | (data[i + 2]);
                        temp = bin16dec(temp);
                        obj.Temperature = ((double)temp) / 10;
                        i += 2;
                        break;
                    case TYPE_RH: //Humidity
                        var rh = (data[i + 1]);
                        obj.Humidity = rh;
                        i += 1;
                        break;
                    case TYPE_ACC: //Acceleration
                        obj.X = bin8dec(data[i + 1]);
                        obj.Y = bin8dec(data[i + 2]);
                        obj.Z = bin8dec(data[i + 3]);
                        i += 3;
                        break;
                    case TYPE_LIGHT: //Light
                        obj.Light = (data[i + 1] << 8) | (data[i + 2]);
                        i += 2;
                        break;
                    case TYPE_MOTION: //Motion sensor(PIR)
                        obj.Motion = (data[i + 1]);
                        i += 1;
                        break;
                    case TYPE_CO2: //CO2
                        obj.Co2 = (data[i + 1] << 8) | (data[i + 2]);
                        i += 2;
                        break;
                    case TYPE_VDD: //Battery level
                        obj.Vdd = (data[i + 1] << 8) | (data[i + 2]);
                        i += 2;
                        break;
                    case TYPE_ANALOG1: //Analog input 1
                        obj.Analog1 = (data[i + 1] << 8) | (data[i + 2]);
                        i += 2;
                        break;
                    case TYPE_GPS: //gps
                        obj.Lat = (data[i + 1] << 16) | (data[i + 2] << 8) | (data[i + 3]);
                        obj.Lng = (data[i + 4] << 16) | (data[i + 5] << 8) | (data[i + 6]);
                        i += 6;
                        break;
                    case TYPE_PULSE1: //Pulse input 1
                        obj.Pulse1 = (data[i + 1] << 8) | (data[i + 2]);
                        i += 2;
                        break;
                    case TYPE_PULSE1_ABS: //Pulse input 1 absolute value
                        var pulseAbs = (data[i + 1] << 24) | (data[i + 2] << 16) | (data[i + 3] << 8) | (data[i + 4]);
                        obj.PulseAbs = pulseAbs;
                        i += 4;
                        break;
                    case TYPE_EXT_TEMP1: //External temp
                        var exttemp = (data[i + 1] << 8) | (data[i + 2]);
                        exttemp = bin16dec(exttemp);
                        obj.ExternalTemperature = ((double)exttemp) / 10;
                        i += 2;
                        break;
                    case TYPE_EXT_DIGITAL: //Digital input
                        obj.Digital = (data[i + 1]);
                        i += 1;
                        break;
                    case TYPE_EXT_DISTANCE: //Distance sensor input 
                        obj.Distance = (data[i + 1] << 8) | (data[i + 2]);
                        i += 2;
                        break;
                    case TYPE_ACC_MOTION: //Acc motion
                        obj.AccMotion = (data[i + 1]);
                        i += 1;
                        break;
                    case TYPE_IR_TEMP: //IR temperature
                        var iTemp = (data[i + 1] << 8) | (data[i + 2]);
                        iTemp = bin16dec(iTemp);
                        var eTemp = (data[i + 3] << 8) | (data[i + 4]);
                        eTemp = bin16dec(eTemp);
                        obj.IrInternalTemperature = ((double)iTemp) / 10;
                        obj.IrExternalTemperature = ((double)eTemp) / 10;
                        i += 4;
                        break;
                    case TYPE_OCCUPANCY: //Body occupancy
                        obj.Occupancy = (data[i + 1]);
                        i += 1;
                        break;
                    case TYPE_WATERLEAK: //Water leak
                        obj.Waterleak = (data[i + 1]);
                        i += 1;
                        break;
                    case TYPE_GRIDEYE: //Grideye data
                        i += 65;
                        break;
                    case TYPE_PRESSURE: //External Pressure
                        var pretemp = (data[i + 1] << 24) | (data[i + 2] << 16) | (data[i + 3] << 8) | (data[i + 4]);
                        obj.Pressure = ((double)pretemp) / 1000;
                        i += 4;
                        break;
                    case TYPE_SOUND: //Sound
                        obj.SoundPeak = data[i + 1];
                        obj.SoundAvg = data[i + 2];
                        i += 2;
                        break;
                    case TYPE_PULSE2: //Pulse 2
                        obj.Pulse2 = (data[i + 1] << 8) | (data[i + 2]);
                        i += 2;
                        break;
                    case TYPE_PULSE2_ABS: //Pulse input 2 absolute value
                        obj.PulseAbs2 = (data[i + 1] << 24) | (data[i + 2] << 16) | (data[i + 3] << 8) | (data[i + 4]);
                        i += 4;
                        break;
                    case TYPE_ANALOG2: //Analog input 2
                        obj.Analog2 = (data[i + 1] << 8) | (data[i + 2]);
                        i += 2;
                        break;
                    case TYPE_EXT_TEMP2: //External temp 2
                        var exttemp2 = (data[i + 1] << 8) | (data[i + 2]);
                        exttemp2 = bin16dec(exttemp2);
                        obj.ExternalTemperature2 = ((double)exttemp2) / 10;
                        i += 2;
                        break;
                    default: //somthing is wrong with data
                        i = data.Length;
                        break;
                }
            }
            return obj;
        }
    }
}
