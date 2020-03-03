using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace MobileApp.Controls
{
    public class StackListLayout : StackLayout
    {
        private bool _locked;
        private static readonly SemaphoreSlim semaphoreSlim = new SemaphoreSlim(1, 1);
        public string EmptyText { get; set; }
        public static readonly BindableProperty ItemsSourceProperty =
            BindableProperty.Create(nameof(ItemsSource),
                typeof(IEnumerable),
                typeof(StackListLayout),
                default(IEnumerable<object>),
                BindingMode.TwoWay, propertyChanged: ItemsSourceChanged);
        public static readonly BindableProperty SelectedCommandProperty =
            BindableProperty.Create(nameof(SelectedCommand), typeof(ICommand), typeof(StackListLayout), null);

        public static readonly BindableProperty ItemTemplateProperty =
            BindableProperty.Create(nameof(ItemTemplate), typeof(DataTemplate), typeof(StackLayout), default(DataTemplate));

        public ICommand SelectedCommand
        {
            get { return (ICommand)GetValue(SelectedCommandProperty); }
            set { SetValue(SelectedCommandProperty, value); }
        }
        public IEnumerable ItemsSource
        {
            get { return (IEnumerable)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        public DataTemplate ItemTemplate
        {
            get { return (DataTemplate)GetValue(ItemTemplateProperty); }
            set { SetValue(ItemTemplateProperty, value); }
        }

        private static void ItemsSourceChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var itemsLayout = (StackListLayout)bindable;
            itemsLayout.UpdateItems();
        }

        public StackListLayout()
        {

        }

        private async void UpdateItems()
        {
            await semaphoreSlim.WaitAsync();
            Children.Clear();
            if (ItemsSource != null)
            {


                var counter = 0;
                foreach (var item in ItemsSource)
                {
                    var v = GetItemView(item);
                    Children.Add(v);
                    counter++;
                }
                if (Children.Count == 0 && !string.IsNullOrWhiteSpace(EmptyText))
                {
                    AddEmpty();
                }

                if (ItemsSource is INotifyCollectionChanged notifyCollection)
                {
                    notifyCollection.CollectionChanged += NotifyCollection_CollectionChanged;
                }
            }
            semaphoreSlim.Release();
        }

        private async void AddEmpty()
        {
            if (_locked)
            {
                return;
            }
            _locked = true;
            await Task.Delay(100);
            Children.Clear();
            Children.Add(new Label()
            {
                HorizontalTextAlignment = TextAlignment.Center,
                Text = EmptyText,
            });
            _locked = false;
        }

        private async void NotifyCollection_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (_locked)
            {
                return;
            }

            _locked = true;
            await Task.Delay(250);  //Disable Consecutive Update
            UpdateItems();
            _locked = false;
        }

        protected virtual View GetItemView(object item)
        {
            var content = ItemTemplate.CreateContent();

            if (!(content is View view))
            {
                return null;
            }

            view.BindingContext = item;
            var gesture = new TapGestureRecognizer
            {
                Command = SelectedCommand,
                CommandParameter = item
            };
            view.GestureRecognizers.Add(gesture);
            return view;
        }
    }
}
