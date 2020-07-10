/*
 *  ASDU.cs
 */
using FTU.Monitor.ViewModel;
using FTU.Monitor.DataService;
using FTU.Monitor.lib60870;
using FTU.Monitor.Model;

using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows;
using System.Windows.Threading;
using FTU.Monitor.Lib60870;
using FTU.Monitor.Util;

namespace lib60870
{
    /// <summary>
    /// ASDU����쳣��
    /// </summary>
    [Serializable]
    public class ASDUParsingException : Exception
    {
        /// <summary>
        /// �вι��췽��
        /// </summary>
        /// <param name="message">�쳣��Ϣ</param>
        public ASDUParsingException(string message)
            : base(message)
        {
        }

    }

    /// <summary>
    /// This class represents an application layer message. It contains some generic message information and
    /// one or more InformationObject instances of the same type. It is used to send and receive messages.
    /// </summary>
    public class ASDU
    {

        public const int IEC60870_5_104_MAX_ASDU_LENGTH = 249;

        private ConnectionParameters parameters;

        private TypeID typeId;
        private bool hasTypeId;//������

        private byte vsq; /* variable structure qualifier */

        private CauseOfTransmission cot; /* cause */
        private byte oa; /* originator address  ����ԭ����ֽ�*/
        private bool isTest; /* is message a test message */
        private bool isNegative; /* is message a negative confirmation */

        private int ca; /* Common address */
        private int spaceLeft = 0;

        private byte[] payload = null;
        private List<InformationObject> informationObjects = null;



        private int faultEventTelesignalisationNum = 0;//�����¼�ң�Ÿ���;
        public int FaultEventTelesignalisationNum
        {
            get { return faultEventTelesignalisationNum; }
            set { faultEventTelesignalisationNum = value; }
        }
        private int faultEventTelemeteringNum = 0;//�����¼�ң�����

        public int FaultEventTelemeteringNum
        {
            get { return faultEventTelemeteringNum; }
            set { faultEventTelemeteringNum = value; }
        }
        private int faultEventTelesignalisationType = 0;//����ң������;

        public int FaultEventTelesignalisationType
        {
            get { return faultEventTelesignalisationType; }
            set { faultEventTelesignalisationType = value; }
        }
        private int faultEventTelemeteringType = 0;//����ң������

        public int FaultEventTelemeteringType
        {
            get { return faultEventTelemeteringType; }
            set { faultEventTelemeteringType = value; }
        }



                                       //                          //����ԭ��     ������Ϣ     �񶨵�ȷ��     ��Դ��ַ   ������ַ     �Ƿ�����
        public ASDU(ConnectionParameters parameters, CauseOfTransmission cot, bool isTest, bool isNegative, byte oa, int ca, bool isSequence)
            : this(parameters, TypeID.M_SP_NA_1, cot, isTest, isNegative, oa, ca, isSequence)
        {
            this.hasTypeId = false;
        }
        #region ����ʹ��

        public ASDU(ConnectionParameters parameters, TypeID typeId, CauseOfTransmission cot, bool isTest, bool isNegative, byte oa, int ca, bool isSequence)
        {
            this.parameters = parameters;
            this.typeId = typeId;
            this.cot = cot;
            this.isTest = isTest;
            this.isNegative = isNegative;
            this.oa = oa;
            this.ca = CommunicationViewModel._ASDUAddress;
            this.spaceLeft = IEC60870_5_104_MAX_ASDU_LENGTH -
                parameters.SizeOfTypeId - parameters.SizeOfVSQ - parameters.SizeOfCA - parameters.SizeOfCOT;

            if (isSequence)
                this.vsq = 0x80;
            else
                this.vsq = 0;

            this.hasTypeId = true;
        }
        #endregion


        public ASDU(ConnectionParameters parameters, byte[] msg, int bufPos, int msgLength)
        {
            this.parameters = parameters;

            int asduHeaderSize = 2 + parameters.SizeOfCOT + parameters.SizeOfCA;

            if ((msgLength - bufPos) < asduHeaderSize)
                throw new ASDUParsingException("Message header too small");

            typeId = (TypeID)msg[bufPos++];

            vsq = msg[bufPos++];

            this.hasTypeId = true;

            byte cotByte = msg[bufPos++];

            if ((cotByte & 0x80) != 0)
                isTest = true;
            else
                isTest = false;

            if ((cotByte & 0x40) != 0)
                isNegative = true;
            else
                isNegative = false;

            cot = (CauseOfTransmission)(cotByte & 0x3f);

            if (parameters.SizeOfCOT == 2)
                oa = msg[bufPos++];

            ca = msg[bufPos++];//asdu������ַ

            if (parameters.SizeOfCA > 1)
                ca += (msg[bufPos++] * 0x100);

            if (typeId == TypeID.M_FT_NA_1)//�����¼���Ϣ
            {
                FaultEventTelesignalisationNum = msg[bufPos++];//��ʱ��ң�Ÿ���
                FaultEventTelesignalisationType = msg[bufPos++];
                if (parameters.SizeOfIOA == 2)
                {
                    FaultEventTelemeteringNum = msg[bufPos + 10 * FaultEventTelesignalisationNum];
                    FaultEventTelemeteringType = msg[bufPos + 10 * FaultEventTelesignalisationNum + 1];

                }
                else if (parameters.SizeOfIOA == 3)
                {
                    FaultEventTelemeteringNum = msg[bufPos + 11 * FaultEventTelesignalisationNum];
                    FaultEventTelemeteringType = msg[bufPos + 11 * FaultEventTelesignalisationNum + 1];

                }

            }
            int payloadSize = msgLength - bufPos;

            //TODO add plausibility check for payload length (using TypeID, SizeOfIOA, and VSQ)

            payload = new byte[payloadSize];

            /* save payload */
            Buffer.BlockCopy(msg, bufPos, payload, 0, payloadSize);
        }

        /// <summary>
        /// Adds an information object to the ASDU.
        /// </summary>
        /// This function add an information object (InformationObject) to the ASDU. NOTE: that all information objects �˺�������Ϣ����InformationObject����ӵ�ASDU��ע�⣺������Ϣ����
        /// have to be of the same type. Otherwise an ArgumentException will be thrown.  ������ͬһ���͡���������ArgumentException��
        /// The function returns true when the information object has been added to the ASDU. The function returns false if
        /// there is no space left in the ASDU to add the information object./����Ϣ������ӵ�asdu�󣬺�������true�����asdu��û��ʣ��ռ��������Ϣ����
        /// <returns><c>true</c>, if information object was added, <c>false</c> otherwise.</returns>
        /// <param name="io">The information object to add</param>
        public bool AddInformationObject(InformationObject io)
        {
            if (informationObjects == null)
                informationObjects = new List<InformationObject>();

            if (hasTypeId)
            {
                if (io.Type != typeId)
                    throw new ArgumentException("Invalid information object type: expected " + typeId.ToString() + " was " + io.Type.ToString());
            }
            else
            {
                typeId = io.Type;
                hasTypeId = true;
            }
            if (typeId == TypeID.C_RS_NA_1)
            {
                int objectSize = io.GetEncodedSize();
                if (objectSize <= spaceLeft)
                {
                    spaceLeft -= objectSize;
                    informationObjects.Add(io);

                    vsq = (byte)((objectSize - 2) / parameters.SizeOfIOA);

                    return true;
                }
                else
                    return false;
            }
            else if (typeId == TypeID.C_WS_NA_1)
            {
                int objectSize = io.GetEncodedSize();
                if (objectSize <= spaceLeft)
                {
                    spaceLeft -= objectSize;
                    informationObjects.Add(io);
                    vsq = (byte)((objectSize - 3) / (parameters.SizeOfIOA + 6));
                    return true;
                }
                else
                    return false;
            }
            else
            {
                int objectSize = io.GetEncodedSize();


                if (IsSquence == false)
                    objectSize += parameters.SizeOfIOA;
                else
                {
                    if (informationObjects.Count == 0) // is first object?
                        objectSize += parameters.SizeOfIOA;
                }

                if (objectSize <= spaceLeft)
                {

                    spaceLeft -= objectSize;
                    informationObjects.Add(io);

                    if (typeId == TypeID.F_SR_NA_1)
                    {
                        vsq = 0;
                    }
                    else
                    {
                        vsq = (byte)((vsq & 0x80) | informationObjects.Count);
                    }
                    return true;
                }
                else
                    return false;

            }


        }


        internal void Encode(Frame frame, ConnectionParameters parameters)
        {
            frame.SetNextByte((byte)typeId);
            frame.SetNextByte(vsq);

            byte cotByte = (byte)cot;

            if (isTest)
                cotByte = (byte)(cotByte | 0x80);

            if (isNegative)
                cotByte = (byte)(cotByte | 0x40);

            frame.SetNextByte(cotByte);

            if (parameters.SizeOfCOT == 2)
                frame.SetNextByte((byte)oa);

            frame.SetNextByte((byte)(ca % 256));

            if (parameters.SizeOfCA > 1)
                frame.SetNextByte((byte)(ca / 256));

            if (payload != null)
                frame.AppendBytes(payload);
            else
            {

                bool isFirst = true;

                foreach (InformationObject io in informationObjects)
                {

                    if (isFirst)
                    {
                        io.Encode(frame, parameters, false);
                        isFirst = false;
                    }
                    else
                    {
                        if (IsSquence)
                            io.Encode(frame, parameters, true);
                        else
                            io.Encode(frame, parameters, false);
                    }
                }
            }
        }

        public byte[] AsByteArray()
        {
            int expectedSize = IEC60870_5_104_MAX_ASDU_LENGTH - spaceLeft;

            BufferFrame frame = new BufferFrame(new byte[expectedSize], 0);

            Encode(frame, parameters);

            if (frame.GetMsgSize() == expectedSize)
                return frame.GetBuffer();
            else
                return null;
        }

        public TypeID TypeId
        {
            get
            {
                return this.typeId;
            }
        }

        public CauseOfTransmission Cot
        {
            get
            {
                return this.cot;
            }
            set
            {
                this.cot = value;
            }
        }

        public byte Oa
        {
            get
            {
                return this.oa;
            }
        }

        public bool IsTest
        {
            get
            {
                return this.isTest;
            }
        }

        public bool IsNegative
        {
            get
            {
                return this.isNegative;
            }
            set
            {
                isNegative = value;
            }
        }



        public int Ca
        {
            get
            {
                return this.ca;
            }
        }

        public bool IsSquence
        {
            get
            {
                if ((vsq & 0x80) != 0)
                    return true;
                else
                    return false;
            }
        }

        public int NumberOfElements
        {
            get
            {
                return (vsq & 0x7f);
            }
        }
        /// <summary>
        /// �����ض�����������Ԫ��
        /// </summary>
        /// <param name="index">��Ϣ������</param>
        /// <returns></returns>
        public InformationObject GetElement(int index)
        {
            InformationObject retVal = null;

            int elementSize;

            try
            {
                switch (typeId)
                {

                    case TypeID.M_SP_NA_1: /* 1 */

                        elementSize = 1;

                        if (IsSquence)
                        {
                            int ioa = InformationObject.ParseInformationObjectAddress(parameters, payload, 0);

                            retVal = new SinglePointInformation(parameters, payload, parameters.SizeOfIOA + (index * elementSize), true);

                            retVal.ObjectAddress = ioa + index;

                        }
                        else
                            //Todo retVal.ObjectAddress
                            retVal = new SinglePointInformation(parameters, payload, index * (parameters.SizeOfIOA + elementSize), false);

                        break;

                    case TypeID.M_SP_TA_1: /* 2 */

                        elementSize = 4;

                        if (IsSquence)
                        {
                            int ioa = InformationObject.ParseInformationObjectAddress(parameters, payload, 0);

                            retVal = new SinglePointWithCP24Time2a(parameters, payload, parameters.SizeOfIOA + (index * elementSize), true);

                            retVal.ObjectAddress = ioa + index;
                        }
                        else
                            retVal = new SinglePointWithCP24Time2a(parameters, payload, index * (parameters.SizeOfIOA + elementSize), false);

                        break;

                    case TypeID.M_DP_NA_1: /* 3 */

                        elementSize = 1;

                        if (IsSquence)
                        {
                            int ioa = InformationObject.ParseInformationObjectAddress(parameters, payload, 0);

                            retVal = new DoublePointInformation(parameters, payload, parameters.SizeOfIOA + (index * elementSize), true);

                            retVal.ObjectAddress = ioa + index;

                        }
                        else
                            retVal = new DoublePointInformation(parameters, payload, index * (parameters.SizeOfIOA + elementSize), false);

                        break;

                    case TypeID.M_DP_TA_1: /* 4 */

                        elementSize = 4;

                        if (IsSquence)
                        {
                            int ioa = InformationObject.ParseInformationObjectAddress(parameters, payload, 0);

                            retVal = new DoublePointWithCP24Time2a(parameters, payload, parameters.SizeOfIOA + (index * elementSize), true);

                            retVal.ObjectAddress = ioa + index;

                        }
                        else
                            retVal = new DoublePointWithCP24Time2a(parameters, payload, index * (parameters.SizeOfIOA + elementSize), false);

                        break;

                    case TypeID.M_ST_NA_1: /* 5 */

                        elementSize = 2;

                        if (IsSquence)
                        {
                            int ioa = InformationObject.ParseInformationObjectAddress(parameters, payload, 0);

                            retVal = new StepPositionInformation(parameters, payload, parameters.SizeOfIOA + (index * elementSize), true);

                            retVal.ObjectAddress = ioa + index;

                        }
                        else
                            retVal = new StepPositionInformation(parameters, payload, index * (parameters.SizeOfIOA + elementSize), false);

                        break;

                    case TypeID.M_ST_TA_1: /* 6 */

                        elementSize = 5;

                        if (IsSquence)
                        {
                            int ioa = InformationObject.ParseInformationObjectAddress(parameters, payload, 0);

                            retVal = new StepPositionWithCP24Time2a(parameters, payload, parameters.SizeOfIOA + (index * elementSize), true);

                            retVal.ObjectAddress = ioa + index;

                        }
                        else
                            retVal = new StepPositionWithCP24Time2a(parameters, payload, index * (parameters.SizeOfIOA + elementSize), false);

                        break;

                    case TypeID.M_BO_NA_1: /* 7 */

                        elementSize = 5;

                        if (IsSquence)
                        {
                            int ioa = InformationObject.ParseInformationObjectAddress(parameters, payload, 0);

                            retVal = new Bitstring32(parameters, payload, parameters.SizeOfIOA + (index * elementSize), true);

                            retVal.ObjectAddress = ioa + index;

                        }
                        else
                            retVal = new Bitstring32(parameters, payload, index * (parameters.SizeOfIOA + elementSize), false);

                        break;

                    case TypeID.M_BO_TA_1: /* 8 */

                        elementSize = 8;

                        if (IsSquence)
                        {
                            int ioa = InformationObject.ParseInformationObjectAddress(parameters, payload, 0);

                            retVal = new Bitstring32WithCP24Time2a(parameters, payload, parameters.SizeOfIOA + (index * elementSize), true);

                            retVal.ObjectAddress = ioa + index;

                        }
                        else
                            retVal = new Bitstring32WithCP24Time2a(parameters, payload, index * (parameters.SizeOfIOA + elementSize), false);

                        break;

                    case TypeID.M_ME_NA_1: /* 9 */

                        elementSize = 3;

                        if (IsSquence)
                        {
                            int ioa = InformationObject.ParseInformationObjectAddress(parameters, payload, 0);

                            retVal = new MeasuredValueNormalized(parameters, payload, parameters.SizeOfIOA + (index * elementSize), true);

                            retVal.ObjectAddress = ioa + index;

                        }
                        else
                            retVal = new MeasuredValueNormalized(parameters, payload, index * (parameters.SizeOfIOA + elementSize), false);

                        break;

                    case TypeID.M_ME_TA_1: /* 10 */

                        elementSize = 6;

                        if (IsSquence)
                        {
                            int ioa = InformationObject.ParseInformationObjectAddress(parameters, payload, 0);

                            retVal = new MeasuredValueNormalizedWithCP24Time2a(parameters, payload, parameters.SizeOfIOA + (index * elementSize), true);

                            retVal.ObjectAddress = ioa + index;

                        }
                        else
                            retVal = new MeasuredValueNormalizedWithCP24Time2a(parameters, payload, index * (parameters.SizeOfIOA + elementSize), false);

                        break;

                    case TypeID.M_ME_NB_1: /* 11 */

                        elementSize = 3;

                        if (IsSquence)
                        {
                            int ioa = InformationObject.ParseInformationObjectAddress(parameters, payload, 0);

                            retVal = new MeasuredValueScaled(parameters, payload, parameters.SizeOfIOA + (index * elementSize), true);

                            retVal.ObjectAddress = ioa + index;

                        }
                        else
                            retVal = new MeasuredValueScaled(parameters, payload, index * (parameters.SizeOfIOA + elementSize), false);

                        break;

                    case TypeID.M_ME_TB_1: /* 12 */

                        elementSize = 6;

                        if (IsSquence)
                        {
                            int ioa = InformationObject.ParseInformationObjectAddress(parameters, payload, 0);

                            retVal = new MeasuredValueScaledWithCP24Time2a(parameters, payload, parameters.SizeOfIOA + (index * elementSize), true);

                            retVal.ObjectAddress = ioa + index;

                        }
                        else
                            retVal = new MeasuredValueScaledWithCP24Time2a(parameters, payload, index * (parameters.SizeOfIOA + elementSize), false);

                        break;


                    case TypeID.M_ME_NC_1: /* 13 */

                        elementSize = 5;

                        if (IsSquence)
                        {
                            int ioa = InformationObject.ParseInformationObjectAddress(parameters, payload, 0);

                            retVal = new MeasuredValueShort(parameters, payload, parameters.SizeOfIOA + (index * elementSize), true);

                            retVal.ObjectAddress = ioa + index;

                        }
                        else
                            retVal = new MeasuredValueShort(parameters, payload, index * (parameters.SizeOfIOA + elementSize), false);

                        break;

                    case TypeID.M_ME_TC_1: /* 14 */

                        elementSize = 8;

                        if (IsSquence)
                        {
                            int ioa = InformationObject.ParseInformationObjectAddress(parameters, payload, 0);

                            retVal = new MeasuredValueShortWithCP24Time2a(parameters, payload, parameters.SizeOfIOA + (index * elementSize), true);

                            retVal.ObjectAddress = ioa + index;

                        }
                        else
                            retVal = new MeasuredValueShortWithCP24Time2a(parameters, payload, index * (parameters.SizeOfIOA + elementSize), false);

                        break;

                    case TypeID.M_IT_NA_1: /* 15 */

                        elementSize = 5;

                        if (IsSquence)
                        {
                            int ioa = InformationObject.ParseInformationObjectAddress(parameters, payload, 0);

                            retVal = new IntegratedTotals(parameters, payload, parameters.SizeOfIOA + (index * elementSize), true);

                            retVal.ObjectAddress = ioa + index;

                        }
                        else
                            retVal = new IntegratedTotals(parameters, payload, index * (parameters.SizeOfIOA + elementSize), false);

                        break;

                    case TypeID.M_IT_TA_1: /* 16 */

                        elementSize = 8;

                        if (IsSquence)
                        {
                            int ioa = InformationObject.ParseInformationObjectAddress(parameters, payload, 0);

                            retVal = new IntegratedTotalsWithCP24Time2a(parameters, payload, parameters.SizeOfIOA + (index * elementSize), true);

                            retVal.ObjectAddress = ioa + index;

                        }
                        else
                            retVal = new IntegratedTotalsWithCP24Time2a(parameters, payload, index * (parameters.SizeOfIOA + elementSize), false);

                        break;

                    case TypeID.M_EP_TA_1: /* 17 */

                        elementSize = 3;

                        if (IsSquence)
                        {
                            int ioa = InformationObject.ParseInformationObjectAddress(parameters, payload, 0);

                            retVal = new EventOfProtectionEquipment(parameters, payload, parameters.SizeOfIOA + (index * elementSize), true);

                            retVal.ObjectAddress = ioa + index;

                        }
                        else
                            retVal = new EventOfProtectionEquipment(parameters, payload, index * (parameters.SizeOfIOA + elementSize), false);

                        break;

                    case TypeID.M_EP_TB_1: /* 18 */

                        elementSize = 7;

                        if (IsSquence)
                        {
                            int ioa = InformationObject.ParseInformationObjectAddress(parameters, payload, 0);

                            retVal = new PackedStartEventsOfProtectionEquipment(parameters, payload, parameters.SizeOfIOA + (index * elementSize), true);

                            retVal.ObjectAddress = ioa + index;

                        }
                        else
                            retVal = new PackedStartEventsOfProtectionEquipment(parameters, payload, index * (parameters.SizeOfIOA + elementSize), false);

                        break;

                    case TypeID.M_EP_TC_1: /* 19 */

                        elementSize = 7;

                        if (IsSquence)
                        {
                            int ioa = InformationObject.ParseInformationObjectAddress(parameters, payload, 0);

                            retVal = new PackedOutputCircuitInfo(parameters, payload, parameters.SizeOfIOA + (index * elementSize), true);

                            retVal.ObjectAddress = ioa + index;

                        }
                        else
                            retVal = new PackedOutputCircuitInfo(parameters, payload, index * (parameters.SizeOfIOA + elementSize), false);

                        break;

                    case TypeID.M_PS_NA_1: /* 20 */

                        elementSize = 5;

                        if (IsSquence)
                        {
                            int ioa = InformationObject.ParseInformationObjectAddress(parameters, payload, 0);

                            retVal = new PackedSinglePointWithSCD(parameters, payload, parameters.SizeOfIOA + (index * elementSize), true);

                            retVal.ObjectAddress = ioa + index;

                        }
                        else
                            retVal = new PackedSinglePointWithSCD(parameters, payload, index * (parameters.SizeOfIOA + elementSize), false);


                        break;

                    case TypeID.M_ME_ND_1: /* 21 */

                        elementSize = 2;

                        if (IsSquence)
                        {
                            int ioa = InformationObject.ParseInformationObjectAddress(parameters, payload, 0);

                            retVal = new MeasuredValueNormalizedWithoutQuality(parameters, payload, parameters.SizeOfIOA + (index * elementSize), true);

                            retVal.ObjectAddress = ioa + index;

                        }
                        else
                            retVal = new MeasuredValueNormalizedWithoutQuality(parameters, payload, index * (parameters.SizeOfIOA + elementSize), false);

                        break;

                    /* 22 - 29 reserved */

                    case TypeID.M_SP_TB_1: /* 30 */

                        elementSize = 8;

                        if (IsSquence)
                        {
                            int ioa = InformationObject.ParseInformationObjectAddress(parameters, payload, 0);

                            retVal = new SinglePointWithCP56Time2a(parameters, payload, parameters.SizeOfIOA + (index * elementSize), true);

                            retVal.ObjectAddress = ioa + index;

                        }
                        else
                            retVal = new SinglePointWithCP56Time2a(parameters, payload, index * (parameters.SizeOfIOA + elementSize), false);

                        break;

                    case TypeID.M_DP_TB_1: /* 31 */

                        elementSize = 8;

                        if (IsSquence)
                        {
                            int ioa = InformationObject.ParseInformationObjectAddress(parameters, payload, 0);

                            retVal = new DoublePointWithCP56Time2a(parameters, payload, parameters.SizeOfIOA + (index * elementSize), true);

                            retVal.ObjectAddress = ioa + index;

                        }
                        else
                            retVal = new DoublePointWithCP56Time2a(parameters, payload, index * (parameters.SizeOfIOA + elementSize), false);

                        break;

                    case TypeID.M_ST_TB_1: /* 32 */

                        elementSize = 9;

                        if (IsSquence)
                        {
                            int ioa = InformationObject.ParseInformationObjectAddress(parameters, payload, 0);

                            retVal = new StepPositionWithCP56Time2a(parameters, payload, parameters.SizeOfIOA + (index * elementSize), true);

                            retVal.ObjectAddress = ioa + index;

                        }
                        else
                            retVal = new StepPositionWithCP56Time2a(parameters, payload, index * (parameters.SizeOfIOA + elementSize), false);

                        break;

                    case TypeID.M_BO_TB_1: /* 33 */

                        elementSize = 12;

                        if (IsSquence)
                        {
                            int ioa = InformationObject.ParseInformationObjectAddress(parameters, payload, 0);

                            retVal = new Bitstring32WithCP56Time2a(parameters, payload, parameters.SizeOfIOA + (index * elementSize), true);

                            retVal.ObjectAddress = ioa + index;

                        }
                        else
                            retVal = new Bitstring32WithCP56Time2a(parameters, payload, index * (parameters.SizeOfIOA + elementSize), false);

                        break;

                    case TypeID.M_ME_TD_1: /* 34 */

                        elementSize = 10;

                        if (IsSquence)
                        {
                            int ioa = InformationObject.ParseInformationObjectAddress(parameters, payload, 0);

                            retVal = new MeasuredValueNormalizedWithCP56Time2a(parameters, payload, parameters.SizeOfIOA + (index * elementSize), true);

                            retVal.ObjectAddress = ioa + index;

                        }
                        else
                            retVal = new MeasuredValueNormalizedWithCP56Time2a(parameters, payload, index * (parameters.SizeOfIOA + elementSize), false);

                        break;

                    case TypeID.M_ME_TE_1: /* 35 */

                        elementSize = 10;

                        if (IsSquence)
                        {
                            int ioa = InformationObject.ParseInformationObjectAddress(parameters, payload, 0);

                            retVal = new MeasuredValueScaledWithCP56Time2a(parameters, payload, parameters.SizeOfIOA + (index * elementSize), true);

                            retVal.ObjectAddress = ioa + index;

                        }
                        else
                            retVal = new MeasuredValueScaledWithCP56Time2a(parameters, payload, index * (parameters.SizeOfIOA + elementSize), false);

                        break;

                    case TypeID.M_ME_TF_1: /* 36 */

                        elementSize = 12;

                        if (IsSquence)
                        {
                            int ioa = InformationObject.ParseInformationObjectAddress(parameters, payload, 0);

                            retVal = new MeasuredValueShortWithCP56Time2a(parameters, payload, parameters.SizeOfIOA + (index * elementSize), true);

                            retVal.ObjectAddress = ioa + index;

                        }
                        else
                            retVal = new MeasuredValueShortWithCP56Time2a(parameters, payload, index * (parameters.SizeOfIOA + elementSize), false);

                        break;

                    case TypeID.M_IT_TB_1: /* 37 */

                        elementSize = 12;

                        if (IsSquence)
                        {
                            int ioa = InformationObject.ParseInformationObjectAddress(parameters, payload, 0);

                            retVal = new IntegratedTotalsWithCP56Time2a(parameters, payload, parameters.SizeOfIOA + (index * elementSize), true);

                            retVal.ObjectAddress = ioa + index;

                        }
                        else
                            retVal = new IntegratedTotalsWithCP56Time2a(parameters, payload, index * (parameters.SizeOfIOA + elementSize), false);

                        break;

                    case TypeID.M_EP_TD_1: /* 38 */

                        elementSize = 10;

                        if (IsSquence)
                        {
                            int ioa = InformationObject.ParseInformationObjectAddress(parameters, payload, 0);

                            retVal = new EventOfProtectionEquipmentWithCP56Time2a(parameters, payload, parameters.SizeOfIOA + (index * elementSize), true);

                            retVal.ObjectAddress = ioa + index;

                        }
                        else
                            retVal = new EventOfProtectionEquipmentWithCP56Time2a(parameters, payload, index * (parameters.SizeOfIOA + elementSize), false);

                        break;

                    case TypeID.M_EP_TE_1: /* 39 */

                        elementSize = 11;

                        if (IsSquence)
                        {
                            int ioa = InformationObject.ParseInformationObjectAddress(parameters, payload, 0);

                            retVal = new PackedStartEventsOfProtectionEquipmentWithCP56Time2a(parameters, payload, parameters.SizeOfIOA + (index * elementSize), true);

                            retVal.ObjectAddress = ioa + index;

                        }
                        else
                            retVal = new PackedStartEventsOfProtectionEquipmentWithCP56Time2a(parameters, payload, index * (parameters.SizeOfIOA + elementSize), false);

                        break;

                    case TypeID.M_EP_TF_1: /* 40 */

                        elementSize = 11;

                        if (IsSquence)
                        {
                            int ioa = InformationObject.ParseInformationObjectAddress(parameters, payload, 0);

                            retVal = new PackedOutputCircuitInfoWithCP56Time2a(parameters, payload, parameters.SizeOfIOA + (index * elementSize), true);

                            retVal.ObjectAddress = ioa + index;

                        }
                        else
                            retVal = new PackedOutputCircuitInfoWithCP56Time2a(parameters, payload, index * (parameters.SizeOfIOA + elementSize), false);

                        break;
                    case TypeID.M_FT_NA_1://42

                        string temp = "";
                        FaultEvent t = new FaultEvent();
                        for (int i = 0; i < FaultEventTelesignalisationNum; i++)
                        {

                            elementSize = 8;
                            SinglePointWithCP56Time2a val = new SinglePointWithCP56Time2a(parameters, payload, i * (parameters.SizeOfIOA + elementSize), false);
                            Console.WriteLine("  IOA: " + val.ObjectAddress + " SP value: " + val.Value);
                            Console.WriteLine("   " + val.Quality.ToString());
                            Console.WriteLine("   " + val.Timestamp.ToString());
                            MainViewModel.outputdata.ParseInformation += "  IOA: " + val.ObjectAddress + " SP value: " + val.Value + "\n";
                            MainViewModel.outputdata.ParseInformation += "   " + val.Quality.ToString() + "\n";
                            MainViewModel.outputdata.ParseInformation += "   " + val.Timestamp.ToString() + "\n";

                            t.Number = FaultEventViewModel.faultEventData.Count + 1;
                            //t.ID = val.ObjectAddress;
                            t.Time = val.Timestamp.ToStringDateTime();
                            temp += "  IOA: " + val.ObjectAddress.ToString() + " SP value: " + val.Value.ToString();
                            // temp += "   " + val.Quality.ToString() + "\n";
                            temp += "   " + val.Timestamp.ToString() + "\n";

                        }
                        for (int i = 0; i < FaultEventTelemeteringNum; i++)
                        {
                            elementSize = 4;
                            MeasuredValueShort mfv = new MeasuredValueShort(parameters, payload, i * (parameters.SizeOfIOA + elementSize) + FaultEventTelesignalisationNum * (parameters.SizeOfIOA + 8) + 2);
                            Console.WriteLine("  IOA: " + mfv.ObjectAddress + " float value: " + mfv.Value);

                            MainViewModel.outputdata.ParseInformation += "  IOA: " + mfv.ObjectAddress + " float value: " + mfv.Value + "\n";
                            temp += "  IOA: " + mfv.ObjectAddress.ToString() + " float value: " + mfv.Value.ToString();
                        }
                        t.Content = temp;
                        Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Normal,
                           (ThreadStart)delegate ()
                           {
                               FaultEventViewModel.faultEventData.Add(t);
                           });

                        break;

                    /* 41 - 44 reserved */

                    case TypeID.C_SC_NA_1: /* 45 */

                        elementSize = parameters.SizeOfIOA + 1;

                        retVal = new SingleCommand(parameters, payload, index * elementSize);

                        break;

                    case TypeID.C_DC_NA_1: /* 46 */

                        elementSize = parameters.SizeOfIOA + 1;

                        retVal = new DoubleCommand(parameters, payload, index * elementSize);

                        break;

                    case TypeID.C_RC_NA_1: /* 47 */

                        elementSize = parameters.SizeOfIOA + 1;

                        retVal = new StepCommand(parameters, payload, index * elementSize);

                        break;

                    case TypeID.C_SE_NA_1: /* 48 - Set-point command, normalized value */

                        elementSize = parameters.SizeOfIOA + 3;

                        retVal = new SetpointCommandNormalized(parameters, payload, index * elementSize);

                        break;

                    case TypeID.C_SE_NB_1: /* 49 - Set-point command, scaled value */

                        elementSize = parameters.SizeOfIOA + 3;

                        retVal = new SetpointCommandScaled(parameters, payload, index * elementSize);

                        break;

                    case TypeID.C_SE_NC_1: /* 50 - Set-point command, short floating point number */

                        elementSize = parameters.SizeOfIOA + 5;

                        retVal = new SetpointCommandShort(parameters, payload, index * elementSize);

                        break;

                    case TypeID.C_BO_NA_1: /* 51 - Bitstring command */

                        elementSize = parameters.SizeOfIOA + 4;

                        retVal = new Bitstring32Command(parameters, payload, index * elementSize);

                        break;

                    /* 52 - 57 reserved */

                    case TypeID.C_SC_TA_1: /* 58 - Single command with CP56Time2a */

                        elementSize = parameters.SizeOfIOA + 8;

                        retVal = new SingleCommandWithCP56Time2a(parameters, payload, index * elementSize);

                        break;

                    case TypeID.C_DC_TA_1: /* 59 - Double command with CP56Time2a */

                        elementSize = parameters.SizeOfIOA + 8;

                        retVal = new DoubleCommandWithCP56Time2a(parameters, payload, index * elementSize);

                        break;

                    case TypeID.C_RC_TA_1: /* 60 - Step command with CP56Time2a */

                        elementSize = parameters.SizeOfIOA + 8;

                        retVal = new StepCommandWithCP56Time2a(parameters, payload, index * elementSize);

                        break;

                    case TypeID.C_SE_TA_1: /* 61 - Setpoint command, normalized value with CP56Time2a */

                        elementSize = parameters.SizeOfIOA + 10;

                        retVal = new SetpointCommandNormalizedWithCP56Time2a(parameters, payload, index * elementSize);

                        break;

                    case TypeID.C_SE_TB_1: /* 62 - Setpoint command, scaled value with CP56Time2a */

                        elementSize = parameters.SizeOfIOA + 10;

                        retVal = new SetpointCommandScaledWithCP56Time2a(parameters, payload, index * elementSize);

                        break;

                    case TypeID.C_SE_TC_1: /* 63 - Setpoint command, short value with CP56Time2a */

                        elementSize = parameters.SizeOfIOA + 12;

                        retVal = new SetpointCommandShortWithCP56Time2a(parameters, payload, index * elementSize);

                        break;

                    case TypeID.C_BO_TA_1: /* 64 - Bitstring command with CP56Time2a */

                        elementSize = parameters.SizeOfIOA + 11;

                        retVal = new Bitstring32CommandWithCP56Time2a(parameters, payload, index * elementSize);

                        break;

                    /* TODO */

                    /* 65 - 69 reserved */

                    case TypeID.C_IC_NA_1: /* 100 - Interrogation command */

                        elementSize = parameters.SizeOfIOA + 1;

                        retVal = new InterrogationCommand(parameters, payload, index * elementSize);

                        break;

                    case TypeID.C_CI_NA_1: /* 101 - Counter interrogation command */

                        elementSize = parameters.SizeOfIOA + 1;

                        retVal = new CounterInterrogationCommand(parameters, payload, index * elementSize);

                        break;

                    case TypeID.C_RD_NA_1: /* 102 - Read command */

                        elementSize = parameters.SizeOfIOA;

                        retVal = new ReadCommand(parameters, payload, index * elementSize);

                        break;

                    case TypeID.C_CS_NA_1: /* 103 - Clock synchronization command */

                        elementSize = parameters.SizeOfIOA + 7;

                        retVal = new ClockSynchronizationCommand(parameters, payload, index * elementSize);

                        break;

                    case TypeID.C_RP_NA_1: /* 105 - Reset process command */

                        elementSize = parameters.SizeOfIOA + 1;

                        retVal = new ResetProcessCommand(parameters, payload, index * elementSize);

                        break;

                    case TypeID.C_CD_NA_1: /* 106 - Delay acquisition command */

                        elementSize = parameters.SizeOfIOA + 2;

                        retVal = new DelayAcquisitionCommand(parameters, payload, index * elementSize);

                        break;

                    /* C_TS_TA_1 (107) is handled by the stack automatically */

                    case TypeID.P_ME_NA_1: /* 110 - Parameter of measured values, normalized value */

                        elementSize = parameters.SizeOfIOA + 3;

                        retVal = new ParameterNormalizedValue(parameters, payload, index * elementSize);

                        break;

                    case TypeID.P_ME_NB_1: /* 111 - Parameter of measured values, scaled value */

                        elementSize = parameters.SizeOfIOA + 3;

                        retVal = new ParameterScaledValue(parameters, payload, index * elementSize);

                        break;

                    case TypeID.P_ME_NC_1: /* 112 - Parameter of measured values, short floating point number */

                        elementSize = parameters.SizeOfIOA + 5;

                        retVal = new ParameterFloatValue(parameters, payload, index * elementSize);

                        break;

                    case TypeID.P_AC_NA_1: /* 113 - Parameter for activation */

                        elementSize = parameters.SizeOfIOA + 1;

                        retVal = new ParameterActivation(parameters, payload, index * elementSize);

                        break;
                    case TypeID.C_RR_NA_1://����ֵ���� 201

                        elementSize = parameters.SizeOfIOA + 11;
                        retVal = new ReadParameterAreaCommand(parameters, payload, index * elementSize);

                        break;


                    case TypeID.C_RS_NA_1:// �������Ͷ�ֵ 202

                        elementSize = 6;
                        //+3 ��ֵ���� ����������ʶ 
                        //retVal = new ParameterValueShort(parameters, payload, index * (parameters.SizeOfIOA + elementSize) + 3, false);
                        retVal = new FixedValueParameter(parameters, payload, 0, vsq & 0x7F);
                        break;

                    case TypeID.F_FR_NA_1:// �ļ����� 210

                        retVal = new FileService(parameters, payload, parameters.SizeOfIOA, false);


                        break;
                    /* 114 - 119 reserved */

                    default:
                        throw new ASDUParsingException("Unknown ASDU type id:" + typeId);
                }
            }
            catch (Exception e)
            {
                //MessageBox.Show(e.ToString());
                LogHelper.Warn(typeof(ASDU), e.ToString());
            }

            return retVal;
        }


        public override string ToString()
        {
            string ret;

            ret = "TypeID: " + typeId.ToString() + " COT: " + cot.ToString();

            if (parameters.SizeOfCOT == 2)
                ret += " OA: " + oa;

            if (isTest)
                ret += " [TEST]";

            if (isNegative)
                ret += " [NEG]";

            if (IsSquence)
                ret += " [SEQ]";

            ret += " elements: " + NumberOfElements;

            ret += " CA: " + ca;

            return ret;
        }
    }
}

