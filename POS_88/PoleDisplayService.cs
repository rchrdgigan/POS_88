using System;
using System.IO.Ports;

namespace POS_88
{
    public class PoleDisplayService
    {
        private readonly SerialPort _sp;
        private readonly bool _enabled;
        private readonly object _lock = new object();
        private const int MaxChars = 18;

        public PoleDisplayService(bool enabled, string portName)
        {
            _enabled = enabled && !string.IsNullOrWhiteSpace(portName);
            if (_enabled)
                _sp = new SerialPort(portName, 9600, Parity.None, 8, StopBits.One);
        }

        private static string Fit(string s)
        {
            s = s ?? "";
            return s.Length > MaxChars ? s.Substring(0, MaxChars) : s;
        }

        private void Send(Action<SerialPort> action)
        {
            if (!_enabled) return;
            lock (_lock)
            {
                try
                {
                    _sp.Open();
                    action(_sp);
                }
                catch (Exception)
                {
                    // display missing/unplugged: ignore so the POS keeps working
                }
                finally
                {
                    try { if (_sp.IsOpen) _sp.Close(); } catch { }
                }
            }
        }

        private static readonly byte[] ClearCmd = { 12 };
        private static readonly byte[] NewLineCmd = { 10, 13 };

        public void Show(string top, string bottom)
        {
            Send(sp =>
            {
                sp.Write(ClearCmd, 0, 1);
                sp.Write(Fit(top));
                sp.Write(NewLineCmd, 0, 2);
                sp.Write(Fit(bottom));
            });
        }

        public void WriteTop(string text, decimal qty)
        {
            string line = qty == 1M ? text : $"{qty:#.##}x {text}";
            Send(sp =>
            {
                sp.Write(ClearCmd, 0, 1);
                sp.Write(Fit(line));
            });
        }

        public void WriteBottom(string text)
        {
            Send(sp =>
            {
                sp.Write(NewLineCmd, 0, 2);
                sp.Write(Fit(text));
            });
        }
    }
}