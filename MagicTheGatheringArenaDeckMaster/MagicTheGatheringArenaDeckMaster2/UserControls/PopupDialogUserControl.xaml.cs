using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace MagicTheGatheringArenaDeckMaster2.UserControls
{
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
            // needed because of multi-selection (not creating an attached property for selected items)
            ServiceLocator.Instance.MainWindowViewModel.PopupDialogViewModel.SettingsViewModel.AlchemyListBox = arenaAlchemySets;
            ServiceLocator.Instance.MainWindowViewModel.PopupDialogViewModel.SettingsViewModel.HistoricListBox = arenaHistoricSets;
            ServiceLocator.Instance.MainWindowViewModel.PopupDialogViewModel.SettingsViewModel.StandardListBox = arenaStandardSets;

            ServiceLocator.Instance.MainWindowViewModel.PopupDialogViewModel.AddSetToSettingsViewModel.AllSetsListBox = addSetsSetsListBox;
            ServiceLocator.Instance.MainWindowViewModel.PopupDialogViewModel.AddSetToSettingsViewModel.SettingsSetsListBox = addSetsSettingsListBox;
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

        #endregion
    }
}
