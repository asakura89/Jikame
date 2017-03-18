using System;
using System.Windows.Controls;

namespace Jikame {
    /// <summary>
    /// Interaction logic for JikamePanel.xaml
    /// </summary>
    public partial class JikamePanel : UserControl {
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

        public JikamePanel(String format) {
            InitializeComponent();
            Format = format;
        }

        private void UpdateTime() {
            String timeStr;
            try {
                timeStr = time.ToString(format);
            }
            catch (Exception) {
                timeStr = time.ToString(Constant.DefaultFormat);
            }
            txtTime.Text = timeStr;
        }
    }
}