using System;
using System.Windows;
using System.Windows.Media;

namespace Jikame {
    public static class XAMLExt {
        public static DependencyObject Find(this DependencyObject parent, String childName) {
            if (parent != null) {
                Int32 childrenCount = VisualTreeHelper.GetChildrenCount(parent);
                for (int i = 0; i < childrenCount; i++) {
                    DependencyObject child = VisualTreeHelper.GetChild(parent, i);
                    FrameworkElement childFE = child as FrameworkElement;
                    if (childFE != null && childFE.Name == childName)
                        return childFE;
                    child = child.Find(childName);
                    if (child != null)
                        return child;
                }
            }

            return null;
        }
    }
}