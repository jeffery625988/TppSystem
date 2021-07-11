using System;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Windows.Forms;
using System.Drawing;
using System.Runtime.InteropServices;
using TppSystem;
using demoapp;
using System.Threading;
using Thorlabs.PM100D;


namespace TppSystem
{

    public partial class Form1 : Form
    {

        public Form1()
        {
            InitializeComponent();
            this.textBox1.WordWrap = false;
            this.textBox1.ScrollBars = System.Windows.Forms.ScrollBars.Horizontal;
            this.textBox2.WordWrap = false;
            this.textBox2.ScrollBars = System.Windows.Forms.ScrollBars.Horizontal;
            this.textBox3.WordWrap = false;
            this.textBox3.ScrollBars = System.Windows.Forms.ScrollBars.Horizontal;
        }


        private void Form1_Load(object sender, EventArgs e)
        {

        }

        #region Parameter
        #endregion


        //PIStage  (need check each stage's connect process by check the app from PI company)
        public class PIStage
        {


            public int ID,exposureType,error;//if upright exposureType = 1 , if Inverted exposureType = -1 
            public string axis, stageType, usbName;
            public double stageMin, stageMax, velocityMin, velocityMax;
            public double[] pos = new double[1];//current position
            public double[] tar_pos = new double[1];
            public double[] velocity = new double[1];
            public double[] defaultVel = new double[1];
            public double[] movTarget = new double[1];
            public bool Flag;
            public bool[] svoState = new bool[1];
            public bool[] iniState = new bool[1];
            //Constructor
            public PIStage()
            {
                defaultVel[0] = 25000;
            }

            //Connection
            public void Connect()
            {
                //Long stage
                if (stageType == "L")
                {
                    //need add the dll file into debug dictionary
                    //Flag = PI_API.C7XX_IsConnected(1);
                    svoState[0] = false;
                    //Connect and get stageID
                    switch (axis)
                    {
                        
                        //X Axis
                        case "X":
                            ID = PI_API.C7XX_ConnectTCPIP("192.168.0.2", 50000);
                            //servo off
                            PI_API.C7XX_SVO(ID, "A", svoState);
                            break;
                        //Y Axis
                        case "Y":
                            //servo off
                            PI_API.C7XX_SVO(ID, "B", svoState);
                            break;
                        //Z Axis
                        case "Z":
                            ID = PI_API.PI_ConnectUSB(usbName);
                            //servo off
                            PI_API.PI_SVO(ID, "1", svoState);
                            break;
                    }

                }
                //Short stage
                else
                {
                    svoState[0] = false;
                    //Connect and get stageID
                    switch (axis)
                    {
                        //XY Axis
                        case "X":
                            ID = PI_API.E7XX_ConnectNIgpib(0, 4);
                            //servo off
                            PI_API.E7XX_SVO(ID, "1", svoState);
                            
                            break;
                        case "Y":
                            //servo off
                            PI_API.E7XX_SVO(ID, "2", svoState);
                            char[] axes = new char[8];
                            PI_API.E7XX_qSAI(ID, axes, 7);
                            break;
                        //Z Axis
                        case "Z":
                            ID = PI_API.PI_ConnectTCPIP("192.168.0.1", 50000);
                            //servo off
                            PI_API.PI_SVO(ID, "1", svoState);
                            break;
                    }

                }


                //
            }

            //Initialization
            public bool Initialize()
            {
                try
                {
                    //Long stae
                    if (stageType == "L")
                    {
                        movTarget[0] = 0;
                        svoState[0] = true;
                        switch (axis)
                        {
                            case "X":
                                PI_API.C7XX_INI(ID, "");
                                PI_API.C7XX_SVO(ID, "A", svoState);
                                PI_API.C7XX_SVO(ID, "B", svoState);
                                Thread.Sleep(500);
                                //move to reference position
                                PI_API.C7XX_GcsCommandset(ID, "REF");
                                Thread.Sleep(500);

                                VelocitySet(defaultVel);

                                
                                Move(movTarget,"A");
                                return PI_API.C7XX_IsReferenceOK(ID, "",iniState);
                            case "Y":
                                Thread.Sleep(500);

                                VelocitySet(defaultVel);
                                Move(movTarget, "A");
                                return PI_API.C7XX_IsReferenceOK(ID, "", iniState);
                            case "Z":
                                svoState[0] = true;
                                PI_API.PI_SVO(ID, "1", svoState);
                                Thread.Sleep(500);
                                PI_API.PI_GcsCommandset(ID, "FRF");
                                Thread.Sleep(500);

                                VelocitySet(defaultVel);
                                Move(movTarget, "A");
                                Thread.Sleep(500);
                                PI_API.PI_GOH(ID, "");
                                return PI_API.PI_qFRF(ID, "", iniState);
                        }
                        return false;
                    }
                    //Short stage
                    else
                    {
                        movTarget[0] = 0;
                        svoState[0] = true;
                        switch (axis)
                        {
                            case "X":
                                char[] axes = new char[10];
                                PI_API.E7XX_qSAI(ID, axes, 9);
                                PI_API.E7XX_INI(ID, "");
                                Thread.Sleep(500);
                                if (!PI_API.E7XX_qSAI(ID, axes, 9))
                                {
                                    return false;
                                }
                                if (!PI_API.E7XX_INI(ID, ""))
                                {
                                    return false;
                                }
                                else
                                {
                                    PI_API.E7XX_SVO(ID, "1", svoState);
                                    Thread.Sleep(500);
                                    VelocitySet(defaultVel);
                                    Move(movTarget, "A");
                                    Thread.Sleep(500);
                                    return true;
                                }

                            case "Y":
                                //PI_API.E7XX_SVO(ID, "2", svoState);
                                PI_API.E7XX_SVO(ID, "2", svoState);
                                VelocitySet(defaultVel);
                                Move(movTarget, "A");
                                Thread.Sleep(500);
                                return true;// PI_API.E7XX_INI(ID, "2");
                            case "Z":     
                                PI_API.PI_SVO(ID, "1", svoState);
                                Thread.Sleep(500);
                                VelocitySet(defaultVel);
                                Move(movTarget, "A");
                                Thread.Sleep(500);
                                return PI_API.PI_qFRF(ID, "", iniState);
                        }
                        return false;
                    }
                }
                catch
                {
                    return false;
                }
            }
            //Disconnect
            public void Disconnect()
            {
                //Move Home
                double[] target = new double[1];
                target[0] = 0;
                Move(target, "A");

                //Turn off servo and Disconnect stage
                //Long stage
                if (stageType == "L")
                {
                    svoState[0] = false;
                    //Turn off Servo 
                    switch (axis)
                    {
                        //X Axis
                        case "X":
                            //servo off
                            PI_API.C7XX_SVO(ID, "A", svoState);
                            PI_API.C7XX_CloseConnection(ID);
                            break;
                        //Y Axis
                        case "Y":
                            //servo off
                            PI_API.C7XX_SVO(ID, "B", svoState);
                            PI_API.C7XX_CloseConnection(ID);
                            break;
                        //Z Axis
                        case "Z":
                            //servo off
                            svoState[0] = false;
                            PI_API.PI_SVO(ID, "1", svoState);
                            PI_API.PI_CloseConnection(ID);
                            break;
                    }

                }
                //Short stage
                else
                {
                    svoState[0] = false;
                    //Disconnect
                    switch (axis)
                    {
                        //X Axis
                        case "X":
                            //servo off
                            PI_API.E7XX_SVO(ID, "1", svoState);
                            PI_API.E7XX_CloseConnection(ID);
                            break;
                        //Y Axis
                        case "Y":
                            //servo off
                            PI_API.E7XX_SVO(ID, "2", svoState);
                            PI_API.E7XX_CloseConnection(ID);
                            break;
                        //Z Axis
                        case "Z":
                            //servo off
                            svoState[0] = false;
                            PI_API.PI_SVO(ID, "1", svoState);
                            PI_API.PI_CloseConnection(ID);
                            break;
                    }

                }

            }

            //Move
            public void Move(double[] distance, string moveStyle)
            {
                //moveStyle == A(Absolute) R(Relative)
                //Long stage
                if (stageType == "L")
                {
                    
                    switch (axis)
                    {
                        //X Axis
                        case "X":
                            //set 50000um as zero of axis
                            distance[0] = -0.001 * distance[0];
                         
                            if (moveStyle == "A")
                            {
                                distance[0] = 50 + distance[0];
                                //limit check
                                if (distance[0] > stageMax || distance[0] < stageMin)
                                {
                                    MessageBox.Show("Out of limit", "Your command out of stage limit", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    break;
                                }
                                else
                                {
                                    PI_API.C7XX_MOV(ID, "A", distance);
                                }
                                
                            }
                            else
                            {
                                if (distance[0] + pos[0] > stageMax || distance[0] + pos[0]< stageMin)
                                {
                                    MessageBox.Show("Out of limit", "Your command out of stage limit", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    break;
                                }
                                else
                                {
                                    PI_API.C7XX_MVR(ID, "A", distance);
                                }
                                
                            }
                            break;

                        //Y Axis
                        case "Y":
                            distance[0] = -0.001 * distance[0] * exposureType; 

                            if (moveStyle == "A")
                            {
                                distance[0] = 50 + distance[0];
                                if (distance[0] > stageMax || distance[0] < stageMin)
                                {
                                    MessageBox.Show("Out of limit", "Your command out of stage limit", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    break;
                                }
                                else
                                {
                                    PI_API.C7XX_MOV(ID, "B", distance);
                                }
                            }
                            else
                            {
                                if (distance[0] + pos[0] > stageMax || distance[0] + pos[0] < stageMin)
                                {
                                    MessageBox.Show("Out of limit", "Your command out of stage limit", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    break;
                                }
                                else
                                {
                                    PI_API.C7XX_MVR(ID, "B", distance);
                                }
                            }
                            break;
                        //Z Axis
                        case "Z":
                            distance[0] = 0.001 * distance[0] * exposureType;

                            if (moveStyle == "A")
                            {
                                if (distance[0] > stageMax || distance[0] < stageMin)
                                {
                                    MessageBox.Show("Out of limit", "Your command out of stage limit", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    break;
                                }
                                else
                                {
                                    PI_API.PI_MOV(ID, "1", distance);
                                }
                            }
                            else
                            {
                                if (distance[0]  + pos[0] > stageMax || distance[0] + pos[0] < stageMin)
                                {
                                    MessageBox.Show("Out of limit", "Your command out of stage limit", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    break;
                                }
                                else
                                {
                                    PI_API.PI_MVR(ID, "1", distance);
                                }
                            }
                            break;
                    }

                }
                //Short stage
                else
                {
                    //Disconnect
                    switch (axis)
                    {
                        //X Axis
                        case "X":
                            
                            if (moveStyle == "A")
                            {
                                distance[0] = 750 + distance[0] ;
                                if (distance[0] > stageMax || distance[0] < -stageMin)
                                {
                                    MessageBox.Show("Out of limit", "Your command out of stage limit", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    break;
                                }
                                else
                                {
                                    PI_API.E7XX_MOV(ID, "1", distance);
                                }
                            }
                            else
                            {
                                if (distance[0] > stageMax || distance[0] < stageMin)
                                {
                                    MessageBox.Show("Out of limit", "Your command out of stage limit", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    break;
                                }
                                else
                                {
                                    PI_API.E7XX_MVR(ID, "1", distance);
                                }
                                
                            }
                            break;
                        //Y Axis
                        case "Y":

                            if (moveStyle == "A")
                            {
                                distance[0] = 750 - distance[0] * exposureType;

                                if (distance[0] > stageMax || distance[0] < stageMin)
                                {
                                    MessageBox.Show("Out of limit", "Your command out of stage limit", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    break;
                                }
                                else
                                {
                                    PI_API.E7XX_MOV(ID, "2", distance);
                                }
                                
                            }
                            else
                            {
                                distance[0] = - distance[0] * exposureType;
                                if (distance[0] + pos[0] > stageMax || distance[0] + pos[0] < stageMin)
                                {
                                    MessageBox.Show("Out of limit", "Your command out of stage limit", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    break;
                                }
                                else
                                {
                                    PI_API.E7XX_MVR(ID, "2", distance);
                                }
                            }
                            break;
                        //Z Axis
                        case "Z":

                            
                            if (moveStyle == "A")
                            {
                                distance[0] = 125 - distance[0] * exposureType;
                                if (distance[0] > stageMax || distance[0] < stageMin)
                                {
                                    MessageBox.Show("Out of limit", "Your command out of stage limit", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    break;
                                }
                                else
                                {
                                    PI_API.PI_MOV(ID, "1", distance);
                                }

                            }
                            else
                            {
                                distance[0] = - distance[0] * exposureType;
                                if (distance[0] + pos[0] > stageMax || distance[0] + pos[0] < stageMin)
                                {
                                    MessageBox.Show("Out of limit", "Your command out of stage limit", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    break;
                                }
                                else
                                {
                                    PI_API.PI_MVR(ID, "1", distance);
                                }

                            }
                            break;
                    }

                }
            }

            //Speed setting
            public void VelocitySet(double[] target)
            {
                
                if (stageType == "L")
                {
                    target[0] = target[0] * 0.001;
                    switch (axis)
                    {
                        case "X":
                            
                            PI_API.C7XX_VEL(ID, "A", target);
                            break;
                        case "Y":
                            PI_API.C7XX_VEL(ID, "B", target);
                            break;
                        case "Z":
                            PI_API.PI_VEL(ID, "1", target);
                            break;
                    }
                       
                }
                else
                {
                    
                    switch (axis)
                    {
                        case "X":
                            target[0] = target[0] * 0.001;
                            PI_API.E7XX_VEL(ID, "1", target);
                            break;
                        case "Y":
                            target[0] = target[0] * 0.001;
                            PI_API.E7XX_VEL(ID, "2", target);
                            break;
                        case "Z":
                            PI_API.PI_VEL(ID, "1", target);
                            break;
                    }
                }
            }

            //Get current position
            public void GetPosition()
            {
                if (stageType == "L")
                {
                    switch (axis)
                    {
                        case "X":
                            PI_API.C7XX_qPOS(ID, "A", pos);
                            pos[0] = -1000 * pos[0] + 50000;
                            break;
                        case "Y":
                            PI_API.C7XX_qPOS(ID, "B", pos);
                            pos[0] = exposureType*(-1000 * pos[0] + 50000);
                            break;
                        case "Z":
                            PI_API.PI_qPOS(ID, "1", pos);
                            pos[0] = 1000 * pos[0] * exposureType;
                            break;
                    }

                }
                else
                {
                    switch (axis)
                    {
                        case "X":
                            PI_API.E7XX_qPOS(ID, "1", pos);
                            pos[0] = pos[0] - 750;
                            break;
                        case "Y":
                            PI_API.E7XX_qPOS(ID, "2", pos);
                            pos[0] = (750 - pos[0]) * exposureType;
                            break;
                        case "Z":
                            PI_API.PI_qPOS(ID, "1", pos);
                            pos[0] = (125 - pos[0]) * exposureType;
                            break;
                    }
                }


            }

            //Get target position 
            public void GetTarpos()
            {
                if (stageType == "L")
                {
                    switch (axis)
                    {
                        case "X":
                            PI_API.C7XX_qMOV(ID, "A", tar_pos);
                            tar_pos[0] = -1000 * tar_pos[0] + 50000;
                            break;
                        case "Y":
                            PI_API.C7XX_qMOV(ID, "B", tar_pos);
                            tar_pos[0] = exposureType * (-1000 * tar_pos[0] + 50000);
                            break;
                        case "Z":
                            PI_API.PI_qMOV(ID, "1", tar_pos);
                            tar_pos[0] = 1000 * tar_pos[0] * exposureType;
                            break;
                    }

                }
                else
                {
                    switch (axis)
                    {
                        case "X":
                            PI_API.E7XX_qMOV(ID, "1", tar_pos);
                            tar_pos[0] = tar_pos[0] - 750;
                            break;
                        case "Y":
                            PI_API.E7XX_qMOV(ID, "2", tar_pos);
                            tar_pos[0] = (750 - tar_pos[0]) * exposureType;
                            break;
                        case "Z":
                            PI_API.PI_qMOV(ID, "1", tar_pos);
                            tar_pos[0] = (125 - tar_pos[0]) * exposureType;
                            break;
                    }
                }
            }
            //Get speed
            public void GetVelocity()
            {
                if (stageType == "L")
                {
                    switch (axis)
                    {
                        case "X":
                            PI_API.C7XX_qVEL(ID, "A", velocity);
                            velocity[0] = 1000 * velocity[0];
                            break;
                        case "Y":
                            PI_API.C7XX_qVEL(ID, "B", velocity);
                            velocity[0] = 1000 * velocity[0];
                            break;
                        case "Z":
                            PI_API.PI_qVEL(ID, "1", velocity);
                            velocity[0] = 1000 * velocity[0];
                            break;
                    }

                }
                else
                {
                    switch (axis)
                    {
                        case "X":
                            PI_API.E7XX_qVEL(ID, "1", velocity);
                            velocity[0] = 1000 * velocity[0];
                            break;
                        case "Y":
                            PI_API.E7XX_qVEL(ID, "2", velocity);
                            velocity[0] = 1000 * velocity[0];
                            break;
                        case "Z":
                            PI_API.PI_qVEL(ID, "1", velocity);
                            break;
                    }
                }
            }
        }

        

        //PIStage instance
        PIStage LongX = new PIStage()
        {
            ID = 0,
            stageType = "L",
            stageMin = 0,//mm
            stageMax = 100,//mm
            velocityMax = 100,
            axis = "X",
            exposureType = -1,
            
        };
        PIStage LongY = new PIStage()
        {
            ID = 0,
            stageType = "L",
            stageMin = 0,//mm
            stageMax = 100,//mm
            velocityMax = 100,
            axis = "Y",
            exposureType = -1
        };
        PIStage LongZ = new PIStage()
        {
            ID = 0,
            usbName = "PI E-861 PiezoWalk(R) Controller SN 0118030732",
            stageType = "L",
            stageMin = -1,
            stageMax = 1,
            velocityMax = 100,
            axis = "Z",
            exposureType = -1
        };
        PIStage ShortX = new PIStage()
        {
           
            stageType = "S",
            stageMin = 0,
            stageMax = 1500,//um
            velocityMax = 100,
            axis = "X",
            exposureType = -1
        };
        PIStage ShortY = new PIStage()
        {
            ID = 0,
            stageType = "S",
            stageMin = 0,
            stageMax = 1500,//um
            velocityMax = 100,
            axis = "Y",
            exposureType = -1
        };
        PIStage ShortZ = new PIStage()
        {
            ID = 0,
            stageType = "S",
            stageMin = 0,
            stageMax = 250,//um
            velocityMax = 100,
            axis = "Z",
            exposureType = -1
        };

        //Aom 
        public class Aom
        {
            //Parameter
            public double workVoltage;
            private double nowVoltage;

            //real voltage offset
            float voltageOffset = 0.1f;

            //real input
            float fvoltage = 0f;
            ushort channel_1 = 0;
            ushort channel_2 = 1;
            ushort cardID = 0;

            //Constructor
            public Aom(float voltage)
            {
                SetVoltage(voltage);
                workVoltage = 0;

            }
            //Connection
            public bool EPCIO_Connect()
            {
                
                //set DAC output 
                EPCIO_API.EPCIO_DAC_SetOutput(channel_1, voltageOffset, cardID);
                EPCIO_API.EPCIO_DAC_SetOutput(channel_2, 0, cardID);
                //turnoff DAC for safety
                EPCIO_API.EPCIO_DAC_StopConv(cardID);
                //close EPCIO fot safety
                EPCIO_API.EPCIO_Close(cardID);
                return true;

            }
            
            //Disconnection
            public bool EPCIO_Disconnect()
            {
                //set DAC output 
                EPCIO_API.EPCIO_DAC_SetOutput(channel_1, voltageOffset, cardID);
                EPCIO_API.EPCIO_DAC_SetOutput(channel_2, 0, cardID);
                //turnoff DAC for safety
                EPCIO_API.EPCIO_DAC_StopConv(cardID);
                //close EPCIO fot safety
                EPCIO_API.EPCIO_Close(cardID);
                return false;
            }

            //Initialize
            public bool EPCIO_Init()
            {
                //initalize EPCIO-6000/6005 module
                bool nRet = EPCIO_API.EPCIO6000_Init(null, null, null, null, null, null, null, null, null, cardID);
                if (nRet)
                {
                    return false;
                }
                else
                {
                    //Reset Module
                    EPCIO_API.EPCIO_ResetModule(0x0A, cardID);                       
                    
                    //Open pulse and DAC output
                    EPCIO_API.EPCIO_LIO_EnablePulseDAC(cardID);  
                    
                    //Servo on 
                    EPCIO_API.EPCIO_LIO_ServoOn(channel_1, cardID);                   
                    EPCIO_API.EPCIO_LIO_ServoOn(channel_2, cardID);  
                    
                    //Set DAC serial Write frequency divided value = 10
                    EPCIO_API.EPCIO_DAC_SetClockDivider(10, cardID);                      

                    //Set DAC command to software control
                    EPCIO_API.EPCIO_DAC_SetCmdSource(channel_1, 0x0, cardID);
                    EPCIO_API.EPCIO_DAC_SetCmdSource(channel_2, 0x0, cardID);

                    //Start convertion
                    EPCIO_API.EPCIO_DAC_StartConv(cardID);
                    return true;                        
                }
                
            }
            //Set voltage(%)
            public void  SetVoltage(float voltageP)
            {
                //Offset voltage
                if (voltageP > 100)
                {
                    fvoltage = 1.0f + voltageOffset;
                }
                else if (voltageP < 0)
                {
                    fvoltage = voltageOffset;
                }
                else
                {
                    fvoltage = voltageOffset + 0.01f * voltageP;
                    //Set output
                    EPCIO_API.EPCIO_DAC_SetOutput(channel_1, fvoltage, cardID);
                    //It's form previous version don't know why need second channel and it's voltage setting
                    EPCIO_API.EPCIO_DAC_SetOutput(channel_2, 4.8f, cardID);
                }
                
                //
            }
            //Now Voltage
        }

        //AOM instance
        Aom AOM = new Aom(0);

        //Shutter 
        public class Shutter
        {
            public bool Connected = false;
            public bool State = false;
            const string COM = "COM2";
            private SerialPort port = new SerialPort(COM, 9600, Parity.None, 8);
            //Connect
            public bool Connect()
            {
                port.Open();
                Thread.Sleep(100);
                Connected = port.IsOpen;
                port.Write("MA0\r\n");
                port.Write("DH\r\n");
                port.Write("SV150000\r\n");
                return Connected;
            }
            //Disconnect
            public bool Disconnect()
            {
                Close();
                port.Close();
                Thread.Sleep(100);
                Connected = port.IsOpen;
                return Connected;
            }
            //Open shutter
            public void Open()
            {
                //setAOM(0);
                port.Write("MA100000\r\n");
                State = true;
                port.ReadExisting();
            }
            //Close shutter
            public void Close()
            {
                //setAOM(0);
                port.Write("GH\r\n");
                State = false;
                port.ReadExisting();

            }
        }

        //Shutter instance 
        Shutter shutter = new Shutter();

        //CCD
        demoapp.CCD ccd = new demoapp.CCD();
        
        //LED serial port
        public SerialPort serialPort1 = new SerialPort("COM5", 9600, Parity.None, 8);

        // Function Read File
        private void ReadFile()
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Multiselect = true; // let us select multifile.
            dialog.Title = "Please select your file";
            dialog.Filter = "filepath(*.dat)|*.dat";
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)                                //Read file successful
            {
                string[] file = dialog.FileNames;
                string file_name;
                for (int i = 0; i < file.Length; i++)
                {
                    //Get file name
                    file_name = file[i].Split('\\').Last();
                    switch (file_name.ToList()[0])
                    {
                        //Abs file
                        case 'A':
                            int iEnd = textBox2.SelectionLength;
                            textBox2.Text = textBox2.Text.Insert(iEnd, "\r\n");
                            iEnd = textBox2.SelectionLength;
                            textBox2.Text = textBox2.Text.Insert(iEnd, file_name);
                            break;
                        //Relative file
                        case 'R':
                            iEnd = textBox3.SelectionLength;
                            textBox3.Text = textBox3.Text.Insert(iEnd, "\r\n");
                            iEnd = textBox3.SelectionLength;
                            textBox3.Text = textBox3.Text.Insert(iEnd, file_name);
                            break;
                        //point file
                        default:
                            iEnd = textBox1.SelectionLength;
                            textBox1.Text = textBox1.Text.Insert(iEnd, "\r\n");
                            iEnd = textBox1.SelectionLength;
                            textBox1.Text = textBox1.Text.Insert(iEnd, file_name);
                            //StreamReader str_read = new StreamReader(file[i]);
                            //textBox4.Text = str_read.ReadToEnd();
                            //str_read.Close();
                            break;



                    }
                }
            }
        }

        //Function AutoFocus (Input start point ,step distance and Which interface ; Output focus zPostion )
        private double AutoFocus(double startpoint, float ds,string Interface)
        {
            
            double zPosition = startpoint;
            float maxIntensity = 0;

            //choose interface
            switch (Interface)
            {
                case "Glass":
                    for (int i = -5; i <= 5; i++)
                    {
                        //Get ccd image intensity
                        float intensity = 0;
                        //check is intensity larger than maxintensity 
                        if (maxIntensity <= intensity)
                        {
                            maxIntensity = intensity;
                            zPosition = startpoint + ds * i;
                        }
                        return zPosition;
                    }

                    break;
                case "Air":
                    
                    break;
                default :
                    break;
            }
            

            return zPosition;
        }


        //Tilt correction
       private void TiltCorrection()
        {

        }
               
       //
        

        //Function Create CCD
        private void CCDCreate()
        {
            if (!ccd.Visible)
            {
                ccd.ShowDialog();
            }
            else
            {
                MessageBox.Show("CCD is ready");
            }
        }


        #region button
        
        //Conncet button
        private void button8_Click(object sender, EventArgs e)
        {
            
            //Connect stage
            #region Connect stage
            //connect long X
            LongX.Connect();
            //check connect
            if (LongX.ID < 0 )
            {
                textBox17.Text = "Fail";
                textBox18.Text = "Long X Connect Fail, Try Disconnect then Reconnect";
                return;
            }
            else
            {
                textBox17.Text = "ok";
                LongY.ID = LongX.ID;
                Thread.Sleep(500);
                LongY.Connect();
            }
             
            //check connect
            if (LongY.ID < 0)
            {
                textBox17.Text = "Fail";
                textBox18.Text = "Long Y Connect Fail, Try Disconnect then Reconnect";
                return;
            }
            else
            {
                textBox17.Text = Convert.ToString(LongY.ID);
                Thread.Sleep(500);
                LongZ.Connect();
            }

            //check the connection
            if (LongZ.ID < 0)
            {
                textBox19.Text = "Fail";
                textBox18.Text = "Long Z Connect Fail, Try Disconnect then Reconnect";
                return;
            }
            else
            {
                textBox19.Text = Convert.ToString(LongZ.ID);
                Thread.Sleep(500);
                ShortX.Connect();
            }

            //check the connection
            if (ShortX.ID < 0)
            {
                textBox23.Text = "Fail";
                textBox18.Text = "Short X Connect Fail, Try Disconnect then Reconnect";
                return;
            }
            else
            {
                textBox23.Text = "ok";
                ShortY.ID = ShortX.ID;
                Thread.Sleep(500);
                ShortY.Connect();
            }

            //check the connection
            if (ShortY.ID < 0)
            {
                textBox23.Text = "Fail";
                textBox18.Text = "Short Y Connect Fail, Try Disconnect then Reconnect";
                return;
            }
            else
            {
                textBox23.Text = Convert.ToString(ShortY.ID);
                Thread.Sleep(500);
                ShortZ.Connect();
            }

            //check the connection
            if (ShortZ.ID < 0)
            {
                textBox24.Text = "Fail";
                textBox18.Text = "Short Z Connect Fail, Try Disconnect then Reconnect";
                return;
            }
            else
            {
                textBox18.Text = "Connect controller ok";
                textBox24.Text = Convert.ToString(ShortZ.ID);
                return;
            }
            #endregion
            
            //Connect and Initialize shutter
            #region Connect shutter
            //shutter.Connect();
            #endregion

            //Connect AOM
            

        }
        
        //Initialize
        private void button10_Click(object sender, EventArgs e)
        {
            #region Initialize PIStages
            bool flag;
            
            flag = LongX.Initialize();
            //Initialize LongX check
            if (flag)
            {
                textBox18.Text = "LongX INI ok";
                flag = LongY.Initialize();
                Thread.Sleep(500);
            }
            else
            {
                textBox18.Text = "LongX INI fail";
                return;
            }
            //Initialize LongY check
            if (flag)
            {
                textBox18.Text = "LongY INI ok";
                flag = LongZ.Initialize();
                Thread.Sleep(500);
            }
            else
            {
                textBox18.Text = "LongY INI fail";
                return;
            }
            //Initialize LongZ check
            if (flag)
            {
                textBox18.Text = "LongZ INI ok";
                flag = ShortX.Initialize();
                Thread.Sleep(500);
            }
            else
            {
                textBox18.Text = "LongZ INI fail";
                return;
            }
            //Initialize ShortX check
            if (flag)
            {
                textBox18.Text = "ShortX INI ok";
                flag = ShortY.Initialize();
                Thread.Sleep(500);
            }
            else
            {
                textBox18.Text = "ShortX INI fail";
                return;
            }
            //Initialize ShortY check
            if (flag)
            {
                textBox18.Text = "ShortY INI ok";
                flag = ShortZ.Initialize();
                Thread.Sleep(500);
            }
            else
            {
                textBox18.Text = "ShortY INI fail";
                return;
            }
            //Initialize ShortZ check
            if (flag)
            {
                textBox18.Text = "ShortZ INI ok";
                timer1.Enabled = true;
                Thread.Sleep(500);
            }
            else
            {
                textBox18.Text = "ShortZ INI fail";
                return;
            }
            #endregion
            
            /*
            #region Initialize AOM
            if (AOM.EPCIO_Init())
            {
                textBox18.Text = "AOM INI ok";
                textBox25.Text = "ok";
            }
            else
            {
                textBox18.Text = "AOM INI fail";
                textBox25.Text = "fail";
            }
            #endregion
            */

        }
        //Disconnect button
        private void button9_Click(object sender, EventArgs e)
        {
            #region PIStages disconnection
            LongX.Disconnect();
            LongY.Disconnect();
            LongZ.Disconnect();
            ShortX.Disconnect();
            ShortY.Disconnect();
            ShortZ.Disconnect();
            textBox18.Text = "";
            #endregion

            #region Turn off AOM
            //AOM.EPCIO_Disconnect();
            #endregion

            #region Shutter Disconnection
            //shutter.Disconnect();
            #endregion
            Thread.Sleep(500);
        }

        //Close button
        private void button11_Click(object sender, EventArgs e)
        {
            Close();
        }

        //Open CCD 
        private void button12_Click(object sender, EventArgs e)
        {
            
            Thread CCDThread = new Thread(CCDCreate);
            CCDThread.Name = "CCD";
            CCDThread.IsBackground = true;
            CCDThread.Start();
            
        }
        
        //Open LED
        private void button14_Click(object sender, EventArgs e)
        {
            
            serialPort1.Open();
            serialPort1.Write("1");
            button15.Enabled = true;
            button14.Enabled = false;
        }
        
        //Close LED
        private void button15_Click(object sender, EventArgs e)
        {
            serialPort1.Write("0");
            serialPort1.Close();
            button14.Enabled = true;
            button15.Enabled = false;
        }

        private void backgroundWorker1_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {

        }

        //Open File
        private void 檔案ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ReadFile();
        }

        //Exposure Type
        private void radioButton7_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton7.Checked == true)
            {
                LongX.exposureType = 1;
                LongY.exposureType = 1;
                LongZ.exposureType = 1;
                ShortX.exposureType = 1;
                ShortY.exposureType = 1;
                ShortZ.exposureType = 1;
            }
            else
            {
                LongX.exposureType = -1;
                LongY.exposureType = -1;
                LongZ.exposureType = -1;
                ShortX.exposureType = -1;
                ShortY.exposureType = -1;
                ShortZ.exposureType = -1;
            }
        }

        //Move X
        private void button2_Click(object sender, EventArgs e)
        {
            string moveType = "A";
            //Get move type
            if (radioButton1.Checked == true)
            {
                moveType = "A";
            }
            else
            {
                moveType = "R";
            }

            //short stage
            if(radioButton4.Checked == true)
            {
                double[] target = new double[1];
                target[0] = Convert.ToDouble(textBox5.Text);
                ShortX.Move(target, moveType);
            }
            else
            {
                double[] target = new double[1];
                target[0] = Convert.ToDouble(textBox5.Text);
                LongX.Move(target, moveType);
            }
        }

        //Move Y
        private void button3_Click(object sender, EventArgs e)
        {
            string moveType = "A";
            //Get move type
            if (radioButton1.Checked == true)
            {
                moveType = "A";
            }
            else
            {
                moveType = "R";
            }

            //short stage
            if (radioButton4.Checked == true)
            {
                double[] target = new double[1];
                target[0] = Convert.ToDouble(textBox6.Text);
                ShortY.Move(target, moveType);
            }
            else
            {
                double[] target = new double[1];
                target[0] = Convert.ToDouble(textBox6.Text);
                LongY.Move(target, moveType);
            }
        }

        //Move Z
        private void button4_Click(object sender, EventArgs e)
        {
            string moveType = "A";
            //Get move type
            if (radioButton1.Checked == true)
            {
                moveType = "A";
            }
            else
            {
                moveType = "R";
            }

            //short stage
            if (radioButton4.Checked == true)
            {
                double[] target = new double[1];
                target[0] = Convert.ToDouble(textBox7.Text);
                ShortZ.Move(target, moveType);
            }
            else
            {
                double[] target = new double[1];
                target[0] = Convert.ToDouble(textBox7.Text);
                LongZ.Move(target, moveType);
            }
        }

        //Set speed X
        private void button7_Click(object sender, EventArgs e)
        {
            if (radioButton4.Checked == true)
            {
                double[] target = new double[1];
                target[0] = Convert.ToDouble(textBox22.Text);
                ShortX.VelocitySet(target);
            }
            else
            {
                double[] target = new double[1];
                target[0] = Convert.ToDouble(textBox22.Text);
                LongX.VelocitySet(target);
            }
        }

        //Set speed Y 
        private void button6_Click(object sender, EventArgs e)
        {
            if (radioButton4.Checked == true)
            {
                double[] target = new double[1];
                target[0] = Convert.ToDouble(textBox21.Text);
                ShortY.VelocitySet(target);
            }
            else
            {
                double[] target = new double[1];
                target[0] = Convert.ToDouble(textBox21.Text);
                LongY.VelocitySet(target);
            }
        }

        //Set speed Z
        private void button5_Click(object sender, EventArgs e)
        {
            if (radioButton4.Checked == true)
            {
                double[] target = new double[1];
                target[0] = Convert.ToDouble(textBox20.Text);
                ShortZ.VelocitySet(target);
            }
            else
            {
                double[] target = new double[1];
                target[0] = Convert.ToDouble(textBox20.Text);
                LongZ.VelocitySet(target);
            }
        }

        //Update parameter
        private void timer1_Tick(object sender, EventArgs e)
        {
            //update current position
            ShortX.GetPosition();
            ShortY.GetPosition();
            ShortZ.GetPosition();
            LongX.GetPosition();
            LongY.GetPosition();
            LongZ.GetPosition();
            if (radioButton3.Checked == true)
            {
                textBox13.Text = Convert.ToString(LongX.pos[0]);
                textBox12.Text = Convert.ToString(LongY.pos[0]);
                textBox11.Text = Convert.ToString(LongZ.pos[0]);
            }
            else
            {
                textBox13.Text = Convert.ToString(ShortX.pos[0]);
                textBox12.Text = Convert.ToString(ShortY.pos[0]);
                textBox11.Text = Convert.ToString(ShortZ.pos[0]);
            }

            //update target position
            ShortX.GetTarpos();
            ShortY.GetTarpos();
            ShortZ.GetTarpos();
            LongX.GetTarpos();
            LongY.GetTarpos();
            LongZ.GetTarpos();
            if (radioButton3.Checked == true)
            {
                textBox8.Text = Convert.ToString(LongX.tar_pos[0]);
                textBox9.Text = Convert.ToString(LongY.tar_pos[0]);
                textBox10.Text = Convert.ToString(LongZ.tar_pos[0]);
            }
            else
            {
                textBox8.Text = Convert.ToString(ShortX.tar_pos[0]);
                textBox9.Text = Convert.ToString(ShortY.tar_pos[0]);
                textBox10.Text = Convert.ToString(ShortZ.tar_pos[0]);
            }
            //Update velocity
            ShortX.GetVelocity();
            ShortY.GetVelocity();
            ShortZ.GetVelocity();
            LongX.GetVelocity();
            LongY.GetVelocity();
            LongZ.GetVelocity();
            if (radioButton3.Checked == true)
            {
                textBox16.Text = Convert.ToString(LongX.velocity[0]);
                textBox15.Text = Convert.ToString(LongY.velocity[0]);
                textBox14.Text = Convert.ToString(LongZ.velocity[0]);
            }

            else
            {
                textBox16.Text = Convert.ToString(ShortX.velocity[0]);
                textBox15.Text = Convert.ToString(ShortY.velocity[0]);
                textBox14.Text = Convert.ToString(ShortZ.velocity[0]);
            }

                

        }

        
    }
    #endregion
}
