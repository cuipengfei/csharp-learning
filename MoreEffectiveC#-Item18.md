# More Effective C#
## Item 18 优先考虑重写相关的方法，而不是创建事件处理程序

### Notes

- 如果是在子类中处理基类所发生的事件，那么总是应该重写相关的 `virtual` 方法。
- 只在当在某个与事件源没有继承关系的对象上处理事件时，可以考虑创建 `event handler`。
- 如果使用事件处理程序，那么这个事件的其他某个 `handler` 如果没有正常工作，那么我们自己写的 `handler` 可能就不会被调用到。
- 但是 `event handler` 的好处是可以临时修改选择的 `handler`，或者允许多个 `handler` 订阅事件。
- 这样可以避免由于其他 `handler` 出错而导致不被调用的情况，同时代码也更简洁。
### override

```xaml
<Window x:Class="WpfApp.OverrideWindow"
        xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
        xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
        xmlns:d="http://schemas.microsoft.com/expression/blend/2008"
        xmlns:mc="http://schemas.openxmlformats.org/markup-compatibility/2006"
        xmlns:local="clr-namespace:WpfApp"
        mc:Ignorable="d"
        Title="OverrideWindow" Height="450" Width="800">
</Window>
```

```C#
using System.Windows;
using System.Windows.Input;

namespace WpfApp
{
    public partial class OverrideWindow : Window
    {
        public OverrideWindow()
        {
            InitializeComponent();
        }

        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            MessageBox.Show("Hello from window!");
        }
    }
}
```

### event handler

```xaml
<Window x:Class="WpfApp.HandlerWindow" x:Name="rootWindow"
        xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
        xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
        xmlns:d="http://schemas.microsoft.com/expression/blend/2008"
        xmlns:mc="http://schemas.openxmlformats.org/markup-compatibility/2006"
        xmlns:local="clr-namespace:WpfApp"
        mc:Ignorable="d"
        Title="HandlerWindow" Height="450" Width="800">
</Window>
```

```C#
using System;
using System.Windows;
using System.Windows.Input;

namespace WpfApp
{
    public partial class HandlerWindow : Window
    {
        public HandlerWindow()
        {
            InitializeComponent();

            rootWindow.MouseDown += new MouseButtonEventHandler(Window_MouseDown_2);
            rootWindow.MouseDown += new MouseButtonEventHandler(Window_MouseDown_1);
        }

        private void Window_MouseDown_1(object sender, MouseButtonEventArgs e)
        {
            MessageBox.Show("Hello from window (1).");
        }

        private void Window_MouseDown_2(object sender, MouseButtonEventArgs e)
        {
            throw new Exception();
            MessageBox.Show("Hello from window (2).");
        }
    }
}
```