#region

using System.Windows;
using System.Windows.Controls;
using Microsoft.Xaml.Behaviors;

#endregion

namespace Packages.Windows.Behaviors
{
    /// <summary>
    ///     PasswordBox扩展
    /// </summary>
    public class PasswordBoxBehavior : Behavior<PasswordBox>
    {
        // Using a DependencyProperty as the backing store for Password.  This enables animation, styling, binding, etc...
        /// <summary>
        ///     PasswordProperty
        /// </summary>
        public static readonly DependencyProperty PasswordProperty =
            DependencyProperty.RegisterAttached("Password", typeof(string), typeof(PasswordBoxBehavior),
                new PropertyMetadata(default(string), OnPasswordPropertyChanged));

        // Using a DependencyProperty as the backing store for IsAttach.  This enables animation, styling, binding, etc...
        /// <summary>
        ///     IsAttachProperty
        /// </summary>
        public static readonly DependencyProperty IsAttachProperty =
            DependencyProperty.RegisterAttached("IsAttach", typeof(bool), typeof(PasswordBoxBehavior),
                new PropertyMetadata(default(bool), OnIsAttachPropertyChanged));

        // Using a DependencyProperty as the backing store for IsUpdating.  This enables animation, styling, binding, etc...
        private static readonly DependencyProperty IsUpdatingProperty =
            DependencyProperty.RegisterAttached("IsUpdating", typeof(bool), typeof(PasswordBoxBehavior),
                new PropertyMetadata(default(bool)));

        /// <summary>
        ///     PasswordContentProperty
        /// </summary>
        public static readonly DependencyProperty PasswordContentProperty = DependencyProperty.Register(
            nameof(PasswordContent), typeof(string), typeof(PasswordBoxBehavior),
            new PropertyMetadata(default(string), OnPasswordContentPropertyChanged));

        private bool _isUpdating;

        /// <summary>
        ///     密码内容
        /// </summary>
        public string PasswordContent
        {
            get => (string)GetValue(PasswordContentProperty);
            set => SetValue(PasswordContentProperty, value);
        }

        private static void OnPasswordContentPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is PasswordBoxBehavior behavior)
            {
                behavior.PasswordContentChanged();
            }
        }

        private void PasswordContentChanged()
        {
            if (_isUpdating)
            {
                return;
            }

            _isUpdating = true;
            AssociatedObject.Password = PasswordContent;
            _isUpdating = false;
        }

        private static void OnPasswordPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is PasswordBox passwordBox)
            {
                if (GetIsUpdating(passwordBox) || !GetIsAttach(passwordBox))
                {
                    return;
                }

                SetIsUpdating(passwordBox, true);
                passwordBox.Password = GetPassword(passwordBox);
                SetIsUpdating(passwordBox, false);
            }
        }

        private static void OnIsAttachPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is PasswordBox passwordBox)
            {
                passwordBox.PasswordChanged -= PasswordBox_PasswordChanged;
                if (GetIsAttach(passwordBox))
                {
                    passwordBox.PasswordChanged += PasswordBox_PasswordChanged;
                }
            }
        }

        private static void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (sender is PasswordBox passwordBox)
            {
                if (GetIsUpdating(passwordBox))
                {
                    return;
                }

                SetIsUpdating(passwordBox, true);
                SetPassword(passwordBox, passwordBox.Password);
                SetIsUpdating(passwordBox, false);
            }
        }


        /// <summary>
        ///     获取IsAttach
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static bool GetIsAttach(DependencyObject obj)
        {
            return (bool)obj.GetValue(IsAttachProperty);
        }

        /// <summary>
        ///     设置IsAttach
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="value"></param>
        public static void SetIsAttach(DependencyObject obj, bool value)
        {
            obj.SetValue(IsAttachProperty, value);
        }


        private static bool GetIsUpdating(DependencyObject obj)
        {
            return (bool)obj.GetValue(IsUpdatingProperty);
        }

        private static void SetIsUpdating(DependencyObject obj, bool value)
        {
            obj.SetValue(IsUpdatingProperty, value);
        }

        /// <summary>
        ///     获取Password
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string GetPassword(DependencyObject obj)
        {
            return (string)obj.GetValue(PasswordProperty);
        }

        /// <summary>
        ///     设置Password
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="value"></param>
        public static void SetPassword(DependencyObject obj, string value)
        {
            obj.SetValue(PasswordProperty, value);
        }

        /// <inheritdoc />
        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.PasswordChanged += AssociatedObject_PasswordChanged;
        }

        private void AssociatedObject_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (_isUpdating)
            {
                return;
            }

            _isUpdating = true;
            PasswordContent = AssociatedObject.Password;
            _isUpdating = false;
        }

        /// <inheritdoc />
        protected override void OnDetaching()
        {
            base.OnDetaching();
            AssociatedObject.PasswordChanged -= AssociatedObject_PasswordChanged;
        }
    }
}