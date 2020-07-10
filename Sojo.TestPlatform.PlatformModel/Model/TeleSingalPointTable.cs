using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sojo.TestPlatform.PlatformModel.Model
{
    /// <summary>
    /// 遥信点号
    /// </summary>
    public struct TeleSingalPointTable
    {
        public int switchOpenStatus;      //开关分位点号
        public int switchCloseStatus;    //开关合位点号
        public int energyStorage;       //储能点号
        public int cyclone;            //低气压点号
        public int powerFailureAlarm;   //电源故障告警
        public int batteryUnderVAlarm;  //电池欠压告警
        public int protectivePlate;    //保护压板 


        public TeleSingalPointTable(int switchOpenStatus, int switchCloseStatus, int energyStorage,
            int cyclone, int powerFailureAlarm, int batteryUnderVAlarm, int protectivePlate)
        {
            this.switchCloseStatus = switchCloseStatus;
            this.switchOpenStatus = switchOpenStatus;
            this.energyStorage = energyStorage;
            this.cyclone = cyclone;
            this.powerFailureAlarm = powerFailureAlarm;
            this.batteryUnderVAlarm = batteryUnderVAlarm;
            this.protectivePlate = protectivePlate;
        }
    }
}
