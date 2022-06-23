using MagicTheGatheringArenaDeckMaster.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MagicTheGatheringArenaDeckMaster.CustomControls
{
    /// <summary>A control that allows for dynamic viewing of cards (4 different views).</summary>
    public class CardView : Control, IDisposable
    {
        #region Fields

        private bool disposedValue;
        private Button resetZoom;

        #endregion

        #region Properties

        /// <summary>Gets or sets the collection of cards.</summary>
        public ObservableCollection<UniqueArtTypeViewModel> Cards
        {
            get { return (ObservableCollection<UniqueArtTypeViewModel>)GetValue(CardsProperty); }
            set { SetValue(CardsProperty, value); }
        }

        public static readonly DependencyProperty CardsProperty =
            DependencyProperty.Register("Cards", typeof(ObservableCollection<UniqueArtTypeViewModel>), typeof(CardView), new PropertyMetadata(new ObservableCollection<UniqueArtTypeViewModel>()));

        /// <summary>Gets or sets whether or not to show the eight column (type) card view. Default is false.</summary>
        public bool IsEightColumnView
        {
            get { return (bool)GetValue(IsEightColumnViewProperty); }
            set { SetValue(IsEightColumnViewProperty, value);}
        }

        public static readonly DependencyProperty IsEightColumnViewProperty =
            DependencyProperty.Register("IsEightColumnView", typeof(bool), typeof(CardView), new PropertyMetadata(false));

        /// <summary>Gets or sets whether or not to show the eight column (type) card view. Default is false.</summary>
        public bool IsEightColumnColorView
        {
            get { return (bool)GetValue(IsEightColumnColorViewProperty); }
            set { SetValue(IsEightColumnColorViewProperty, value); }
        }

        public static readonly DependencyProperty IsEightColumnColorViewProperty =
            DependencyProperty.Register("IsEightColumnColorView", typeof(bool), typeof(CardView), new PropertyMetadata(false));

        /// <summary>Gets or sets whether or not to show the one column card view. Default is true.</summary>
        public bool IsOneColumnView
        {
            get { return (bool)GetValue(IsOneColumnViewProperty); }
            set { SetValue(IsOneColumnViewProperty, value); }
        }

        public static readonly DependencyProperty IsOneColumnViewProperty =
            DependencyProperty.Register("IsOneColumnView", typeof(bool), typeof(CardView), new PropertyMetadata(true));

        /// <summary>Gets or sets whether or not to show the three column card view. Default is false.</summary>
        public bool IsThreeColumnView
        {
            get { return (bool)GetValue(IsThreeColumnViewProperty); }
            set { SetValue(IsThreeColumnViewProperty, value); }
        }

        public static readonly DependencyProperty IsThreeColumnViewProperty =
            DependencyProperty.Register("IsThreeColumnView", typeof(bool), typeof(CardView), new PropertyMetadata(false));

        /// <summary>Gets or sets the zoom factor for the ListBox. Default is 1.0.</summary>
        public double ZoomFactor
        {
            get { return (double)GetValue(ZoomFactorProperty); }
            set { SetValue(ZoomFactorProperty, value); }
        }

        public static readonly DependencyProperty ZoomFactorProperty =
            DependencyProperty.Register("ZoomFactor", typeof(double), typeof(CardView), new PropertyMetadata(1.0, ZoomFactorChanged, CoerceZoomFactor));

        /// <summary>Gets or sets the max zoom factor. Default is 2.0.</summary>
        public double ZoomMax
        {
            get { return (double)GetValue(ZoomMaxProperty); }
            set { SetValue(ZoomMaxProperty, value); }
        }

        public static readonly DependencyProperty ZoomMaxProperty =
            DependencyProperty.Register("ZoomMax", typeof(double), typeof(CardView), new PropertyMetadata(2.0));

        /// <summary>Gets or sets the min zoom factor. Default is 0.5.</summary>
        public double ZoomMin
        {
            get { return (double)GetValue(ZoomMinProperty); }
            set { SetValue(ZoomMinProperty, value); }
        }

        public static readonly DependencyProperty ZoomMinProperty =
            DependencyProperty.Register("ZoomMin", typeof(double), typeof(CardView), new PropertyMetadata(0.5));

        #endregion

        #region Constructors

        static CardView()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(CardView), new FrameworkPropertyMetadata(typeof(CardView)));
        }

        #endregion

        #region Methods

        private static void ZoomFactorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is not CardView cv) return;

            //if (zlv.itemsPresenter != null)
            //    zlv.itemsPresenter.LayoutTransform = new ScaleTransform(zlv.ZoomFactor, zlv.ZoomFactor);
        }

        private static object CoerceZoomFactor(DependencyObject d, object baseValue)
        {
            if (d is not CardView cv) return baseValue;

            double val = (double)baseValue;

            if (val < cv.ZoomMin) baseValue = cv.ZoomMin;
            else if (val > cv.ZoomMax) baseValue = cv.ZoomMax;

            return baseValue;
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            //itemsPresenter = GetTemplateChild("PART_ItemsPresenter") as ItemsPresenter;
            resetZoom = GetTemplateChild("PART_ResetZoom") as Button;

            if (resetZoom != null)
                resetZoom.Click += ResetZoom_Click;
        }

        private void ResetZoom_Click(object sender, RoutedEventArgs e)
        {
            SetValue(ZoomFactorProperty, 1.0);
        }

        private void ZoomableListView_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (Keyboard.Modifiers != ModifierKeys.Control)
                return;

            if (e.Delta < 0)
            {
                SetValue(ZoomFactorProperty, ZoomFactor - 0.1);
            }
            else if (e.Delta > 0)
            {
                SetValue(ZoomFactorProperty, ZoomFactor + 0.1);
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    PreviewMouseWheel -= ZoomableListView_PreviewMouseWheel;

                    if (resetZoom != null)
                        resetZoom.Click -= ResetZoom_Click;
                }

                disposedValue=true;
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
