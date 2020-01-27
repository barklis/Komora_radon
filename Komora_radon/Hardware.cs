using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.IO.Ports;

namespace Komora_radon
{
    class Hardware
    {
        static bool _continue;
        static SerialPort _serialPort;
        Thread thread;

        static string data_buffer;
        //Control parameters:
        bool electro_valve1_open;
        int valve1_flow;
        int valve2_flow;
        int valve3_flow;

        //Environmental data:
        double temperature;
        double pressure;
        double huminity;


        public Hardware(string port_name, int boundrate = 9600, Parity parity = Parity.None, int data_bits = 8, StopBits stop_bits = StopBits.One, Handshake handshake = Handshake.None)
        {
            _serialPort = new SerialPort();
            thread = new Thread(Read);

            _serialPort.PortName = port_name;
            _serialPort.BaudRate = boundrate;
            _serialPort.Parity = parity;
            _serialPort.DataBits = data_bits;
            _serialPort.StopBits = stop_bits;
            _serialPort.Handshake = handshake;

            _serialPort.ReadTimeout = 500;
            _serialPort.WriteTimeout = 500;


            _continue = false;
        }

        public void Start()
        {
            if (false == _continue)
            {
                _serialPort.Open();
                thread.Start();
                _continue = true;
            }
        }

        public void Stop()
        {
            if (true == _continue)
            {
                thread.Join(); 
                _serialPort.Close();
                _continue = false;
            }
        }

        public static void Read()
        {
            while (_continue)
            {
                try
                {
                    string message = _serialPort.ReadLine();


                    data_buffer = message;
                }
                catch (TimeoutException) { }
            }
        }

        public void Write(string command)
        {
            _serialPort.WriteLine(command);
        }

        public string Get_data()
        {
            return data_buffer;
        }

        public static string[] Get_ports()
        {
            string[] ports = SerialPort.GetPortNames();
            return ports;
        }
    }
}
