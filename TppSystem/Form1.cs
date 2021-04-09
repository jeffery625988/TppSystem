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
            SerialPort serialPort1 = new SerialPort();
            serialPort1.PortName = "COM7";
            serialPort1.BaudRate = 9600;
            serialPort1.Parity = Parity.None;
            serialPort1.DataBits = 8;
            serialPort1.StopBits = StopBits.One;
        }
        

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
                    file_name = file[i].Split('\\').Last();                                                 //Get file name
                    if (file_name.ToList()[0] is 'A')                                                       //Abs file
                    {
                       
                        int iEnd = textBox2.SelectionLength;
                        textBox2.Text = textBox2.Text.Insert(iEnd, "\r\n");
                        iEnd = textBox2.SelectionLength;
                        textBox2.Text = textBox2.Text.Insert(iEnd, file_name);
                    }
                    else if (file_name.ToList()[0] is 'R')                                                  //Relative file
                    {
                        int iEnd = textBox3.SelectionLength;
                        textBox3.Text = textBox3.Text.Insert(iEnd, "\r\n");
                        iEnd = textBox3.SelectionLength;
                        textBox3.Text = textBox3.Text.Insert(iEnd, file_name);
                    }
                    else                                                                                    //point file
                    {
                        int iEnd = textBox1.SelectionLength;
                        textBox1.Text = textBox1.Text.Insert(iEnd, "\r\n");
                        iEnd = textBox1.SelectionLength;
                        textBox1.Text = textBox1.Text.Insert(iEnd, file_name);
                        StreamReader str_read = new StreamReader(file[i]);
                        textBox4.Text = str_read.ReadToEnd();
                        str_read.Close();
                    }
                }
                Refresh();
            }
            else
            {
                textBox1.Text = "";
                textBox2.Text = "";
                textBox3.Text = "";
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

       
        

        //PIStage class (need check each stage's connect process by check the app from PI company)
        public class PIStage
        {

            
            public int ID, velocity;
            public string axis, stageType,usbName;
            public double stageMin, stageMax, target, position, velocityMin, velocityMax;
            public bool Flag;
            

            //Constructor
            public PIStage()                                
            {

            }

            //Connection
            public void Connect()
            {
                //Long stage
                if (stageType == "L")
                {
                    //need add the dll file into debug dictionary
                    Flag = PI_API.C7XX_IsConnected(1);

                    //Connect and get stageID
                    switch (axis)
                    {
                        //XY Axis
                        case "X":
                            ID = PI_API.C7XX_ConnectTCPIP("192.168.0.2",50000);
                            //servo off
                            PI_API.C7XX_SVO(ID,"A",false);
                            break;
                        //Y Axis
                        case "Y":
                            ID = PI_API.C7XX_ConnectTCPIP("192.168.0.2", 50000);
                            //servo off
                            PI_API.C7XX_SVO(ID, "B", false);
                            break;
                        //Z Axis
                        case "Z":
                            ID = PI_API.PI_ConnectUSB(usbName);
                            //servo off
                            PI_API.PI_SVO(ID, "1", false);
                            break;
                    }
                 
                }
                //Short stage
                else
                {
                    //Connect and get stageID
                    switch (axis)
                    {
                        //XY Axis
                        case "X":
                            ID = PI_API.E7XX_ConnectNIgpib(0, 4);
                            //servo off
                            PI_API.E7XX_SVO(ID, "1", false);
                            break;
                        case "Y":
                            ID = PI_API.E7XX_ConnectNIgpib(0, 4);
                            //servo off
                            PI_API.E7XX_SVO(ID, "2", false);
                            break;
                        //Z Axis
                        case "Z":
                            ID = PI_API.PI_ConnectTCPIP("192.168.0.1",50000);
                            //servo off
                            PI_API.PI_SVO(ID, "1", false);
                            break;
                    }

                }
                

                //
            }
            
            //Disconnect
            public void Disconnect()
            {
                //Move Home
                Move(0,"A");

                //Turn off servo and Disconnect stage
                //Long stage
                if (stageType == "L")
                {

                    //Turn off Servo 
                    switch (axis)
                    {
                        //X Axis
                        case "X":
                            //servo off
                            PI_API.C7XX_SVO(ID, "A", false);
                            PI_API.C7XX_CloseConnection(ID);
                            break;
                        //Y Axis
                        case "Y":
                            //servo off
                            PI_API.C7XX_SVO(ID, "B", false);
                            PI_API.C7XX_CloseConnection(ID);
                            break;
                        //Z Axis
                        case "Z":
                            //servo off
                            PI_API.PI_SVO(ID, "1", false);
                            PI_API.PI_CloseConnection(ID);
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
                            //servo off
                            PI_API.E7XX_SVO(ID, "1", false);
                            PI_API.E7XX_CloseConnection(ID);
                            break;
                        //Y Axis
                        case "Y":
                            //servo off
                            PI_API.E7XX_SVO(ID, "2", false);
                            PI_API.E7XX_CloseConnection(ID);
                            break;
                        //Z Axis
                        case "Z":
                            //servo off
                            PI_API.PI_SVO(ID, "1", false);
                            PI_API.PI_CloseConnection(ID);
                            break;
                    }

                }

            }
            
            //Move
            public void Move(int distance, string moveStyle)
            {
                //moveStyle == A(Absolute) R(Relative)
                //Long stage
                if (stageType == "L")
                { 
                    switch (axis)
                    {
                        //X Axis
                        case "X":
                            if (moveStyle == "A")
                            {
                                PI_API.C7XX_MOV(ID, "A", distance);     
                            }
                            else
                            {
                                PI_API.C7XX_MVR(ID, "A", distance);
                            }                         
                            break;

                        //Y Axis
                        case "Y":
                            if (moveStyle == "A")
                            {
                                PI_API.C7XX_MOV(ID, "B", distance);
                            }
                            else
                            {
                                PI_API.C7XX_MVR(ID, "B", distance);
                            }
                            break;
                        //Z Axis
                        case "Z":
                            if (moveStyle == "A")
                            {
                                PI_API.PI_MOV(ID, "1", distance);
                            }
                            else
                            {
                                PI_API.PI_MVR(ID, "1", distance);
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
                                PI_API.E7XX_MOV(ID, "1", distance);
                            }
                            else
                            {
                                PI_API.E7XX_MVR(ID, "1", distance);
                            }                            
                            break;
                        //Y Axis
                        case "Y":
                            if (moveStyle == "A")
                            {
                                PI_API.E7XX_MOV(ID, "2", distance);
                            }
                            else
                            {
                                PI_API.E7XX_MVR(ID, "2", distance);
                            }
                            break;
                        //Z Axis
                        case "Z":
                            if (moveStyle == "A")
                            {
                                PI_API.PI_MOV(ID, "1", distance);
                            }
                            else
                            {
                                PI_API.PI_MVR(ID, "1", distance);
                            }
                            break;
                    }

                }
            }

            //Speed setting
            public void VelocitySet(int target)
            {

            }
        }

        //Aom class
        public class Aoms
        {
        }

        //CCD
        demoapp.CCD ccd = new demoapp.CCD();
        
        //LED serial port
        public SerialPort serialPort1 = new SerialPort();

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
        
        //Open file button
        private void button1_Click(object sender, EventArgs e)
        {
            ReadFile();

        }
        
        //Conncet button
        private void button8_Click(object sender, EventArgs e)
        {
            //Create Long , short Stage Need check parameter!!!
            PIStage LongX = new PIStage()
            {
                ID = 0,
                stageType = "L",
                stageMin = -100,
                stageMax = 100,
                velocityMax = 100,
                axis = "X",
                velocity = 25
            };
            PIStage LongY = new PIStage()
            {
                ID = 0,
                stageType = "L",
                stageMin = -100,
                stageMax = 100,
                velocityMax = 100,
                axis = "Y",
                velocity = 25
            };
            PIStage LongZ = new PIStage()
            {
                ID = 0,
                usbName = "PI E-861 PiezoWalk(R) Controller SN 0118030732",
                stageType = "L",
                stageMin = -100,
                stageMax = 100,
                velocityMax = 100,
                axis = "Z",
                velocity = 25,
            };
            PIStage ShortX = new PIStage()
            {
                ID = 0,
                stageType = "S",
                stageMin = -100,
                stageMax = 100,
                velocityMax = 100,
                axis = "XY",
                velocity = 25,
            };
            PIStage ShortY = new PIStage()
            {
                ID = 0,
                stageType = "S",
                stageMin = -100,
                stageMax = 100,
                velocityMax = 100,
                axis = "Y",
                velocity = 25,
            };
            PIStage ShortZ = new PIStage()
            {
                ID = 0,
                stageType = "S",
                stageMin = -100,
                stageMax = 100,
                velocityMax = 100,
                axis = "Z",
                velocity = 25,
            };
            //create CCD
            
            
            //Connect
            #region Connect
            //connect 
            LongX.Connect();
            //check connect
            if (LongX.ID < 0 )
            {
                textBox17.Text = "Fail";
                textBox18.Text = "Long X Connect Fail, Try Reconnect";
            }
            else
            {
                textBox17.Text = "ok";
            }
            
            //connect 
            LongY.Connect();
            //check connect
            if (LongY.ID < 0)
            {
                textBox17.Text = "Fail";
                textBox18.Text = "Long Y Connect Fail, Try Reconnect";
            }
            else
            {
                textBox17.Text = "ok";
            }
            //connect
            LongZ.Connect();
            //check the connection
            if (LongZ.ID < 0)
            {
                textBox19.Text = "Fail";
                textBox18.Text = "Long Z Connect Fail, Try Reconnect";
            }
            else
            {
                textBox19.Text = "ok";
            }

            //connect
            ShortX.Connect();
            //check the connection
            if (ShortX.ID < 0)
            {
                textBox23.Text = "Fail";
                textBox18.Text = "Short X Connect Fail, Try Reconnect";
            }
            else
            {
                textBox23.Text = "ok";
            }

            //connect
            ShortY.Connect();
            //check the connection
            if (ShortY.ID < 0)
            {
                textBox23.Text = "Fail";
                textBox18.Text = "Short Y Connect Fail, Try Reconnect";
            }
            else
            {
                textBox23.Text = "ok";
            }

            //connect
            ShortZ.Connect();
            //check the connection
            if (ShortZ.ID < 0)
            {
                textBox24.Text = "Fail";
                textBox18.Text = "Short Z Connect Fail, Try Reconnect";
            }
            else
            {
                textBox24.Text = "ok";
            }
            #endregion 
        }

        //Disconnect button
        private void button9_Click(object sender, EventArgs e)
        {
            //Call stages disconnect
            //close and clear timer 
            //sleep
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
        }
        
        //Close LED
        private void button15_Click(object sender, EventArgs e)
        {
            serialPort1.Write("0");
            serialPort1.Close(); 
        }

        private void backgroundWorker1_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {

        }

   
    }
}
