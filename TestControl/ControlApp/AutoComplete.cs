using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace TestControl.ControlApp
{

    public class AutoComplete:TextBox
    {
        private Popup _popup;
        private DataGrid _dataGrid;
        private Button _toggleButton;
        private TextBlock _warningTextBlock;
        static AutoComplete()
        {
            DefaultStyleKeyProperty.OverrideMetadata(
                typeof(AutoComplete),
                new FrameworkPropertyMetadata(typeof(AutoComplete))
            );
        }

        // Dependency Properties
        public static readonly DependencyProperty ItemsSourceProperty =
            DependencyProperty.Register("ItemsSource", typeof(IEnumerable), typeof(AutoComplete));

        public IEnumerable ItemsSource
        {
            get => (IEnumerable)GetValue(ItemsSourceProperty);
            set => SetValue(ItemsSourceProperty, value);
        }

        public static readonly DependencyProperty IsPopupOpenProperty =
            DependencyProperty.Register("IsPopupOpen", typeof(bool), typeof(AutoComplete));

        public bool IsPopupOpen
        {
            get => (bool)GetValue(IsPopupOpenProperty);
            set => SetValue(IsPopupOpenProperty, value);
        }

        // Thêm DependencyProperty mới
        public static readonly DependencyProperty DisplayMemberPathProperty =
            DependencyProperty.Register(
                "DisplayMemberPath",
                typeof(string),
                typeof(AutoComplete),
                new PropertyMetadata("Col1") // Giá trị mặc định
            );

        public string DisplayMemberPath
        {
            get => (string)GetValue(DisplayMemberPathProperty);
            set => SetValue(DisplayMemberPathProperty, value);
        }
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _popup = GetTemplateChild("PART_Popup") as Popup;
            _dataGrid = GetTemplateChild("PART_Datagrid") as DataGrid;
            _toggleButton = GetTemplateChild("PART_ButtonToggle") as Button;
            _warningTextBlock = GetTemplateChild("PART_WarningTextblock") as TextBlock;

            // Event handlers
            _toggleButton.Click += (s, e) => IsPopupOpen = !IsPopupOpen;
            _dataGrid.SelectionChanged += DataGrid_SelectionChanged;
            this.TextChanged += AutoCompleteTextBox_TextChanged;
        }

        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_dataGrid.SelectedItem != null)
            {
                var selectedItem = _dataGrid.SelectedItem;
                var propertyInfo = selectedItem.GetType().GetProperty(DisplayMemberPath);

                if (propertyInfo != null)
                {
                    this.Text = propertyInfo.GetValue(selectedItem)?.ToString();
                }

                IsPopupOpen = false;
            }

        }

        private void AutoCompleteTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(Text))
            {
                _warningTextBlock.Visibility = Visibility.Visible;
                _dataGrid.ItemsSource = null;
                return;
            }

            var filteredItems = ItemsSource?
                .Cast<object>()
                .Where(item =>
                {
                    var prop = item.GetType().GetProperty(DisplayMemberPath);
                    if (prop == null) return false;

                    var value = prop.GetValue(item)?.ToString()?.ToLower() ?? "";
                    return value.Contains(Text.ToLower());
                });

            _warningTextBlock.Visibility = filteredItems?.Any() == true
                ? Visibility.Collapsed
                : Visibility.Visible;

            _dataGrid.ItemsSource = filteredItems;
            IsPopupOpen = true;
        }
    }
}
