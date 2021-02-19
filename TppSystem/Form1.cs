using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Drawing;

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


        private void button1_Click(object sender, EventArgs e)                                          //Open File
        {
            ReadFile();

        }
        
        // Read File
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

        //AutoFocus (Input start point ,step distance and Which interface ; Output focus zPostion )
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

        //PIStage 
        public class PIStage
        {
            public int ID;

            //Constructor
            public PIStage()                                
            {

            }

            //Connection
            public void Connect()
            {

            }
            public void Disconnect()
            {

            }
            
            //move
            public void Move(int target, string moveStyle)
            {

            }

            //Speed setting
            public void VelocitySet(int target)
            {

            }
        }
    }
}
