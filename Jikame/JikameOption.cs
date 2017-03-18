using System;
using System.ComponentModel;
using Microsoft.VisualStudio.Shell;

namespace Jikame {
    public class JikameOption : DialogPage {
        private String format;

        [Category("General")]
        [DisplayName("Format")]
        [Description("Using DateTime String.Format to format the time.")]
        public String Format {
            get {
                return format;
            }
            set {
                format = value;
                var package = GetService(typeof(JikamePackage)) as JikamePackage;
                package?.UpdateFormatInstantly(value);
            }
        }

        public JikameOption() {
            if (String.IsNullOrEmpty(format)) format = Constant.DefaultFormat;
        }
    }
}