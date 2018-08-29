using System.Windows.Input;
using Xamarin.Forms;

namespace ProjectOnlineMobile2.Controls
{
    public class CustomFrame : Frame
    {
        public static readonly BindableProperty LongPressCommandProperty =
            BindableProperty.Create(nameof(LongPressCommand), typeof(ICommand), typeof(CustomFrame), default(ICommand));
        public static readonly BindableProperty CommandProperty =
            BindableProperty.Create(nameof(Command), typeof(ICommand), typeof(CustomFrame), default(ICommand));
        public static readonly BindableProperty CommandParameterProperty = 
            BindableProperty.Create(nameof(CommandParameter), typeof(object), typeof(CustomFrame), default(object));

        public ICommand LongPressCommand
        {
            get { return (ICommand)GetValue(LongPressCommandProperty); }
            set { SetValue(LongPressCommandProperty, value); }
        }

        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }

        public object CommandParameter
        {
            get { return GetValue(CommandParameterProperty); }
            set { SetValue(CommandParameterProperty, value); }
        }

    }
}
