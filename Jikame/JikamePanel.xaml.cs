using System;
using System.Windows.Controls;

namespace Jikame {
    /// <summary>
    /// Interaction logic for JikamePanel.xaml
    /// </summary>
    public partial class JikamePanel : UserControl {
        public const String DefaultFormat = "hh:mm:ss tt";
        private String format;
        private DateTime time;

        public String Format {
            set {
                format = value;
                UpdateTime();
            }
        }

        public DateTime Time {
            set {
                time = value;
                UpdateTime();
            }
        }

        public JikamePanel() : this(DefaultFormat) {}

        public JikamePanel(String format) {
            InitializeComponent();
            UpdateTime();
        }

        private void UpdateTime() {
            String timeStr = time.ToString(DefaultFormat);
            try {
                timeStr = time.ToString(format);
            }
            catch (Exception) {
                timeStr = time.ToString(DefaultFormat);
            }
            txtTime.Text = timeStr;
        }
    }
}