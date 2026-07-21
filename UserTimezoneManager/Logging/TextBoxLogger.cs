using System;
using System.Drawing;
using System.Windows.Forms;

namespace Ronin.XrmToolbox.UserTimezoneManager.Logging
{
    /// <summary>
    /// Writes timestamped log entries to a <see cref="RichTextBox"/> with a dark-themed appearance.
    /// All operations are marshalled to the UI thread.
    /// </summary>
    public class TextBoxLogger : ILogger
    {
        private readonly RichTextBox _textBox;

        public TextBoxLogger(RichTextBox textBox)
        {
            _textBox = textBox ?? throw new ArgumentNullException(nameof(textBox));
            ApplyStyle();
        }

        private void ApplyStyle()
        {
            _textBox.BackColor = Color.Black;
            _textBox.ForeColor = Color.White;
            _textBox.Font = new Font("Consolas", 9f, FontStyle.Regular);
            _textBox.ReadOnly = true;
            _textBox.Multiline = true;
            _textBox.ScrollBars = RichTextBoxScrollBars.Vertical;
            _textBox.WordWrap = false;
        }

        /// <summary>Appends a timestamped info line.</summary>
        public void Log(string message)
            => Append(message, Color.White);

        /// <summary>Appends a timestamped error line in red.</summary>
        public void LogError(string message)
            => Append(message, Color.OrangeRed);

        /// <summary>Clears all log text.</summary>
        public void Clear()
        {
            if (_textBox.InvokeRequired)
            {
                _textBox.Invoke(new Action(Clear));
                return;
            }
            _textBox.Clear();
        }

        private void Append(string message, Color color)
        {
            if (_textBox.InvokeRequired)
            {
                _textBox.Invoke(new Action(() => Append(message, color)));
                return;
            }

            var timestamp = DateTime.Now.ToString("[HH:mm:ss]");
            var line = $"{timestamp} {message}{Environment.NewLine}";

            _textBox.SelectionStart = _textBox.TextLength;
            _textBox.SelectionLength = 0;
            _textBox.SelectionColor = color;
            _textBox.AppendText(line);
            _textBox.SelectionColor = _textBox.ForeColor;

            // Auto-scroll to the latest entry
            _textBox.ScrollToCaret();
        }
    }
}
