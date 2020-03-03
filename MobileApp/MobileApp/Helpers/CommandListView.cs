using System.Windows.Input;
using Xamarin.Forms;

namespace MobileApp.Helpers
{
    public sealed class CommandListView
    {
        public static readonly BindableProperty ItemTappedCommandProperty =
            BindableProperty.CreateAttached(
                "ItemTappedCommand",
                typeof(ICommand),
                typeof(CommandListView),
                default(ICommand),
                BindingMode.OneWay,
                null,
                PropertyChanged);

        private static void PropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is ListView listView)
            {
                listView.ItemTapped -= ListViewOnItemTapped;
                listView.ItemTapped += ListViewOnItemTapped;
            }
            else if (bindable is CollectionView collectionView)
            {
                collectionView.SelectionMode = SelectionMode.Single;
                collectionView.SelectionChanged -= CollectionViewSelectChanged;
                collectionView.SelectionChanged += CollectionViewSelectChanged;
            }
        }

        private static void CollectionViewSelectChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is CollectionView collection && collection.SelectedItem != null)
            {
                var firstItem = collection.SelectedItem;
                var command = GetItemTappedCommand(collection);
                command?.Execute(firstItem);
                collection.SelectedItem = null;
            }
        }

        private static void ListViewOnItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (sender is ListView list && list.IsEnabled && !list.IsRefreshing)
            {
                list.SelectedItem = null;
                var command = GetItemTappedCommand(list);
                if (command != null && command.CanExecute(e.Item))
                {
                    command.Execute(e.Item);
                }
            }
        }

        public static ICommand GetItemTappedCommand(BindableObject bindableObject)
        {
            return (ICommand)bindableObject.GetValue(ItemTappedCommandProperty);
        }

        public static void SetItemTappedCommand(BindableObject bindableObject, object value)
        {
            bindableObject.SetValue(ItemTappedCommandProperty, value);
        }
    }
}
