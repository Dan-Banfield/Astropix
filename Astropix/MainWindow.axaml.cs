using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Astropix
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        #region Designer Code

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        #endregion
    }
}
