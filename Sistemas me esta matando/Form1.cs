using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Ports;

namespace Sistemas_me_esta_matando
{
    public partial class Form1 : Form
    {
        delegate void setTextDelegate(string val);
        public SerialPort ArduinoPort { get; }
        public Form1()
        {
            InitializeComponent();

            ArduinoPort = new SerialPort("COM3", 9600);
            ArduinoPort.DataReceived += new SerialDataReceivedEventHandler(DataRecievedHandler);

            try
            {
                ArduinoPort.Open(); // Abre el puerto
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al abrir el puerto: " + ex.Message);
            }
        }

        void DataRecievedHandler(object sender, SerialDataReceivedEventArgs e) // se recive el dato
        {
            string dato = ArduinoPort.ReadLine();
            string[] valores = dato.Split(','); // Divide los datos con comas
            // Verificar si hay 3 valores
            if (valores.Length == 3)
            {
                string temp = valores[0];
                string peso = valores[0];
                string humo = valores[0];

                ActualizarLabels(temp, peso, humo); // Se manda a escribir el dato en el label
            }            
        }

        void ActualizarLabels(string temp, string peso, string humo)
        {
            if (label3.InvokeRequired || label4.InvokeRequired || label6.InvokeRequired)
            {
                label3.Invoke(new Action(() =>
                {
                    label3.Text = $"Temperatura: {temp} °C";
                    label4.Text = $"Peso: {peso} kg";
                    label6.Text = $"Humo: {humo} %";
                }));
            }
            else
            {
                label3.Text = $"Temperatura: {temp} °C";
                label4.Text = $"Peso: {peso} kg";
                label6.Text = $"Humo: {humo} %";
            }
        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            ArduinoPort.Write("A");
        }

        private void BajarDown_Click(object sender, EventArgs e)
        {
            ArduinoPort.Write("B");
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Cierra el puerto serial al cerrar el formulario
            if (ArduinoPort.IsOpen)
            {
                ArduinoPort.Close();
            }
        }
    } 
}
