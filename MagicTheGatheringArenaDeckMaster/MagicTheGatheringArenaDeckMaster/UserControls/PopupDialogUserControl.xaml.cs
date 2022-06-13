using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace MagicTheGatheringArenaDeckMaster.UserControls
{
    /// <summary>UserControl that holds all of the popup dialogs for the software.</summary>
    public partial class PopupDialogUserControl : UserControl
    {
        #region Fields

        private Point mouseLeftDownPos;

        #endregion

        #region Copnstructors

        public PopupDialogUserControl()
        {
            InitializeComponent();
        }

        #endregion

        #region Methods

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            // make it so our load event doesn't make the designer complain (if is in design mode then just leave)
            if (System.ComponentModel.DesignerProperties.GetIsInDesignMode(this)) return;

            // needed because of multi-selection (not creating an attached property for selected items)
            ServiceLocator.Instance.MainWindowViewModel.PopupDialogViewModel.SettingsViewModel.AlchemyListBox = arenaAlchemySets;
            ServiceLocator.Instance.MainWindowViewModel.PopupDialogViewModel.SettingsViewModel.HistoricListBox = arenaHistoricSets;
            ServiceLocator.Instance.MainWindowViewModel.PopupDialogViewModel.SettingsViewModel.StandardListBox = arenaStandardSets;

            ServiceLocator.Instance.MainWindowViewModel.PopupDialogViewModel.AddSetToSettingsViewModel.AllSetsListBox = addSetsSetsListBox;
            ServiceLocator.Instance.MainWindowViewModel.PopupDialogViewModel.AddSetToSettingsViewModel.SettingsSetsListBox = addSetsSettingsListBox;

            ServiceLocator.Instance.MainWindowViewModel.PopupDialogViewModel.SetFilterViewModel.AllSetsListBox = setsFilterSetsListBox;
            ServiceLocator.Instance.MainWindowViewModel.PopupDialogViewModel.SetFilterViewModel.FilterSetsListBox = setsFilterListBox;
        }

        private void addSetsSetsListBox_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            mouseLeftDownPos = e.GetPosition(null);
        }

        private void addSetsSetsListBox_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                Point mousePos = e.GetPosition(null);

                if (Math.Abs(mousePos.X - mouseLeftDownPos.X) > SystemParameters.MinimumHorizontalDragDistance ||
                    Math.Abs(mousePos.Y - mouseLeftDownPos.Y) > SystemParameters.MinimumVerticalDragDistance)
                {
                    // check to make sure we have selected items to drag before we drag
                    if (addSetsSetsListBox.SelectedItems.Count == 0) return;

                    DragDrop.DoDragDrop(addSetsSetsListBox, addSetsSetsListBox.SelectedItems.Cast<string>().ToList(), DragDropEffects.Move);
                }
            }
        }

        private void addSetsSettingsListBox_DragEnter(object sender, DragEventArgs e)
        {
            addSetsSettingsListBox.BorderBrush = new SolidColorBrush(Color.FromRgb(0, 123, 255));
            addSetsSettingsListBox.BorderThickness = new Thickness(2);
        }

        private void addSetsSettingsListBox_DragLeave(object sender, DragEventArgs e)
        {
            addSetsSettingsListBox.ClearValue(BorderBrushProperty);
            addSetsSettingsListBox.ClearValue(BorderThicknessProperty);
        }

        private void addSetsSettingsListBox_DragOver(object sender, DragEventArgs e)
        {
            addSetsSettingsListBox.BorderBrush = new SolidColorBrush(Color.FromRgb(0, 123, 255));
            addSetsSettingsListBox.BorderThickness = new Thickness(2);
        }

        private void addSetsSettingsListBox_Drop(object sender, DragEventArgs e)
        {
            addSetsSettingsListBox.ClearValue(BorderBrushProperty);
            addSetsSettingsListBox.ClearValue(BorderThicknessProperty);

            List<string> data = (List<string>)e.Data.GetData(typeof(List<string>));

            ServiceLocator.Instance.MainWindowViewModel.PopupDialogViewModel.AddSetToSettingsViewModel.AddSets(data);
        }

        private void setsFilterSetsListBox_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            mouseLeftDownPos = e.GetPosition(null);
        }

        private void setsFilterSetsListBox_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                Point mousePos = e.GetPosition(null);

                if (Math.Abs(mousePos.X - mouseLeftDownPos.X) > SystemParameters.MinimumHorizontalDragDistance ||
                    Math.Abs(mousePos.Y - mouseLeftDownPos.Y) > SystemParameters.MinimumVerticalDragDistance)
                {
                    // check to make sure we have selected items to drag before we drag
                    if (setsFilterSetsListBox.SelectedItems.Count == 0) return;

                    DragDrop.DoDragDrop(setsFilterSetsListBox, setsFilterSetsListBox.SelectedItems.Cast<string>().ToList(), DragDropEffects.Move);
                }
            }
        }

        private void setsFilterSettingsListBox_DragEnter(object sender, DragEventArgs e)
        {
            addSetsSettingsListBox.BorderBrush = new SolidColorBrush(Color.FromRgb(0, 123, 255));
            addSetsSettingsListBox.BorderThickness = new Thickness(2);
        }

        private void setsFilterSettingsListBox_DragLeave(object sender, DragEventArgs e)
        {
            setsFilterSetsListBox.ClearValue(BorderBrushProperty);
            setsFilterSetsListBox.ClearValue(BorderThicknessProperty);
        }

        private void setsFilterSettingsListBox_DragOver(object sender, DragEventArgs e)
        {
            addSetsSettingsListBox.BorderBrush = new SolidColorBrush(Color.FromRgb(0, 123, 255));
            addSetsSettingsListBox.BorderThickness = new Thickness(2);
        }

        private void setsFilterSettingsListBox_Drop(object sender, DragEventArgs e)
        {
            setsFilterSetsListBox.ClearValue(BorderBrushProperty);
            setsFilterSetsListBox.ClearValue(BorderThicknessProperty);

            List<string> data = (List<string>)e.Data.GetData(typeof(List<string>));

            ServiceLocator.Instance.MainWindowViewModel.PopupDialogViewModel.SetFilterViewModel.AddSets(data);
        }

        #endregion
    }
}
