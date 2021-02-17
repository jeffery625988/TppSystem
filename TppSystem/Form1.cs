using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace TppSystem
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void groupBox5_Enter(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            ReadFile();
        }
        // Read File
        private void ReadFile()
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Multiselect = true; // let us select multifile.
            dialog.Title = "Please select your file";
            dialog.Filter = "filepath(*.txt)|*.txt";  
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string[] file = dialog.FileNames;
                StreamReader str_read = new StreamReader(file[0]);
                textBox1.Text = str_read.ReadToEnd();
                str_read.Close();
                Refresh();
            }


        }
    }
}
