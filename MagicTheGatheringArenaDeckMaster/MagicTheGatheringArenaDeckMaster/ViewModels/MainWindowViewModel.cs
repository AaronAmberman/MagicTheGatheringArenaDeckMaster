using MagicTheGatheringArena.Core;
using MagicTheGatheringArena.Core.MVVM;
using MagicTheGatheringArena.Core.Scryfall.Data;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Threading;
using WPF.InternalDialogs;

namespace MagicTheGatheringArenaDeckMaster.ViewModels
{
    internal class MainWindowViewModel : ViewModelBase
    {
        #region Fields

        private CollectionView? cardCollectionView;
        private ObservableCollection<UniqueArtTypeViewModel> cardData;
        private CollectionView? cardImageCollectionView;
        private ObservableCollection<UniqueArtTypeViewModel> cardImageData;
        private string? cardImageSearch;
        private DispatcherTimer? cardImageSearchTimer;
        private DispatcherOperation? cardImageSearchOperation;
        private string? cardNameSearch;
        private DispatcherTimer? cardNameSearchTimer;
        private DispatcherOperation? cardNameSearchOperation;
        private Dispatcher? dispatcher;
        private ICommand? downloadCardImagesCommand;
        private ICommand? downloadDataCommand;
        private InternalDialogUserControlViewModel? internalDialogViewModel;
        private bool? isAllSmallChecked = false;
        private bool? isAllNormalChecked = false;
        private bool? isAllLargeChecked = false;
        private bool? isAllPngChecked = false;
        private bool? isAllArtCropChecked = false;
        private bool? isAllBorderCropChecked = false;
        private int selectedImageCount;
        private CollectionView? setCollectionView;
        private ObservableCollection<SetNameViewModel> setNames;
        private string? setNameSearch;
        private DispatcherTimer? setNameSearchTimer;
        private DispatcherOperation? setNameSearchOperation;
        private bool showOnlySetAllowedInStandard;
        private bool showOnlySetAllowedInMtgaStandard;
        private List<string>? standardOnlySets;
        private string? statusMessage;
        private string? version;

        #endregion

        #region Properties

        public CollectionView? CardCollectionView
        {
            get => cardCollectionView;
            set
            {
                cardCollectionView = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<UniqueArtTypeViewModel> CardData
        {
            get => cardData;
            set
            {
                cardData = value;
                OnPropertyChanged();
            }
        }

        public CollectionView? CardImageCollectionView
        {
            get => cardImageCollectionView;
            set
            {
                cardImageCollectionView = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<UniqueArtTypeViewModel> CardImageData
        {
            get => cardImageData;
            set
            {
                cardImageData = value;
                OnPropertyChanged();
            }
        }

        public string? CardImageSearch
        {
            get => cardImageSearch;
            set
            {
                cardImageSearch = value;

                //CardImageCollectionView?.Refresh();

                if (cardImageSearchTimer != null && cardImageSearchTimer.IsEnabled)
                    cardImageSearchTimer.Stop();

                if (cardImageSearchOperation != null)
                    cardImageSearchOperation.Abort();

                if (cardImageSearchTimer != null)
                    cardImageSearchTimer.Start();

                OnPropertyChanged();
            }
        }

        public string? CardNameSearch
        {
            get => cardNameSearch;
            set
            {
                cardNameSearch = value;

                //CardCollectionView?.Refresh();

                //VerifyStateOfSmallCheckBoxAndSet();
                //VerifyStateOfNormalCheckBoxAndSet();
                //VerifyStateOfLargeCheckBoxAndSet();
                //VerifyStateOfPngCheckBoxAndSet();
                //VerifyStateOfArtCropCheckBoxAndSet();
                //VerifyStateOfBorderCropCheckBoxAndSet();

                if (cardNameSearchTimer != null && cardNameSearchTimer.IsEnabled)
                    cardNameSearchTimer.Stop();

                if (cardNameSearchOperation != null)
                    cardNameSearchOperation.Abort();

                if (cardNameSearchTimer != null)
                    cardNameSearchTimer.Start();

                OnPropertyChanged();
            }
        }

        public Dispatcher? Dispatcher 
        { 
            get => dispatcher; 
            set
            {
                dispatcher = value;

                cardImageSearchTimer = new DispatcherTimer(new TimeSpan(0, 0, 2), DispatcherPriority.Normal, CardImageSearchTick, value);
                cardNameSearchTimer = new DispatcherTimer(new TimeSpan(0, 0, 2), DispatcherPriority.Normal, CardNameSearchTick, value);
                setNameSearchTimer = new DispatcherTimer(new TimeSpan(0, 0, 2), DispatcherPriority.Normal, SetNameSearchTick, value);
            }
        }

        public ICommand? DownloadCardImagesCommand => downloadCardImagesCommand ??= new RelayCommand(DownloadCardData, CanDownloadCardImages);

        public ICommand? DownloadDataCommand => downloadDataCommand ??= new RelayCommand(DownloadData);

        public InternalDialogUserControlViewModel? InternalDialogViewModel
        {
            get => internalDialogViewModel;
            set
            {
                internalDialogViewModel = value;
                OnPropertyChanged();
            }
        }

        public bool? IsAllSmallChecked
        {
            get => isAllSmallChecked;
            set
            {
                isAllSmallChecked = value;

                if (value.HasValue)
                {
                    // if this nullable bool is null don't alter check box state
                    CheckAllSmallOptionsForFilteredCards(value.Value);
                }

                OnPropertyChanged();
            }
        }

        public bool? IsAllNormalChecked
        {
            get => isAllNormalChecked;
            set
            {
                isAllNormalChecked = value;

                if (value.HasValue)
                {
                    // if this nullable bool is null don't alter check box state
                    CheckAllNormalOptionsForFilteredCards(value.Value);
                }

                OnPropertyChanged();
            }
        }

        public bool? IsAllLargeChecked
        {
            get => isAllLargeChecked;
            set
            {
                isAllLargeChecked = value;

                if (value.HasValue)
                {
                    // if this nullable bool is null don't alter check box state
                    CheckAllLargeOptionsForFilteredCards(value.Value);
                }

                OnPropertyChanged();
            }
        }

        public bool? IsAllPngChecked
        {
            get => isAllPngChecked;
            set
            {
                isAllPngChecked = value;

                if (value.HasValue)
                {
                    // if this nullable bool is null don't alter check box state
                    CheckAllPngOptionsForFilteredCards(value.Value);
                }

                OnPropertyChanged();
            }
        }

        public bool? IsAllArtCropChecked
        {
            get => isAllArtCropChecked;
            set
            {
                isAllArtCropChecked = value;

                if (value.HasValue)
                {
                    // if this nullable bool is null don't alter check box state
                    CheckAllArtCropOptionsForFilteredCards(value.Value);
                }

                OnPropertyChanged();
            }
        }

        public bool? IsAllBorderCropChecked
        {
            get => isAllBorderCropChecked;
            set
            {
                isAllBorderCropChecked = value;

                if (value.HasValue)
                {
                    // if this nullable bool is null don't alter check box state
                    CheckAllBorderCropOptionsForFilteredCards(value.Value);
                }

                OnPropertyChanged();
            }
        }

        public int SelectedImageCount
        {
            get => selectedImageCount;
            set
            {
                if (value < 0) // it its zero at a minimum
                    selectedImageCount = 0;
                else
                    selectedImageCount = value;

                OnPropertyChanged();
            }
        }

        public CollectionView? SetCollectionView
        {
            get => setCollectionView;
            set
            {
                setCollectionView = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<SetNameViewModel> SetNames
        {
            get => setNames;
            set
            {
                setNames = value;
                OnPropertyChanged();
            }
        }

        public string? SetNameSearch
        {
            get => setNameSearch;
            set
            {
                setNameSearch = value;

                //SetCollectionView?.Refresh();

                if (setNameSearchTimer != null && setNameSearchTimer.IsEnabled)
                    setNameSearchTimer.Stop();

                if (setNameSearchOperation != null)
                    setNameSearchOperation.Abort();

                if (setNameSearchTimer != null)
                    setNameSearchTimer.Start();

                OnPropertyChanged();
            }
        }

        public bool ShowOnlySetAllowedInStandard
        {
            get => showOnlySetAllowedInStandard;
            set
            {
                showOnlySetAllowedInStandard = value;

                SetCollectionView?.Refresh();

                OnPropertyChanged();
            }
        }

        public bool ShowOnlySetAllowedInMtgaStandard
        {
            get => showOnlySetAllowedInMtgaStandard;
            set
            {
                showOnlySetAllowedInMtgaStandard = value;

                SetCollectionView?.Refresh();

                OnPropertyChanged();
            }
        }

        public string? StatusMessage
        {
            get => statusMessage;
            set
            {
                statusMessage = value;
                OnPropertyChanged();
            }
        }

        public string? Version
        {
            get => version;
            set
            {
                version = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Constructors

        public MainWindowViewModel()
        {
            cardData = new ObservableCollection<UniqueArtTypeViewModel>();
            cardImageData = new ObservableCollection<UniqueArtTypeViewModel>();
            setNames = new ObservableCollection<SetNameViewModel>();
        }

        #endregion

        #region Methods

        private void BackupReferencedFile()
        {
            if (InternalDialogViewModel == null) return;

            // copy on another thread so we don't block the UI...wait 5 seconds though so we can read the data first
            Task.Delay(5000).ContinueWith((task) =>
            {
                StatusMessage = "Backing up referenced file...";

                string[] files = Directory.GetFiles(ServiceLocator.Instance.PathingService.BaseDataPath, "*.json");

                if (files.Length > 0)
                {
                    // there will only ever be one backup file

                    string existingFilename = Path.GetFileName(files[0]);
                    string newFilename = Path.GetFileName(InternalDialogViewModel.FileLocation);

                    if ((existingFilename != newFilename) || existingFilename == newFilename && !InternalDialogViewModel.HasDataPreviously)
                    {
                        // either file names are not the same or they are but the user picked a new file with the same name, ok to delete old one

                        string filename = Path.Combine(ServiceLocator.Instance.PathingService.BaseDataPath, Path.GetFileName(InternalDialogViewModel.FileLocation));

                        File.Copy(InternalDialogViewModel.FileLocation, filename, true);
                        File.Delete(existingFilename);
                    }
                    else if (existingFilename == newFilename && InternalDialogViewModel.HasDataPreviously)
                    {
                        // leave the file alone because the user referenced this backed up file
                    }
                }
                else
                {
                    string filename = Path.Combine(ServiceLocator.Instance.PathingService.BaseDataPath, Path.GetFileName(InternalDialogViewModel.FileLocation));

                    File.Copy(InternalDialogViewModel.FileLocation, filename);
                }

                StatusMessage = "Referenced file successfully backed up.";

                // just wait 5 seconds to reset the status message
                Task.Delay(5000).ContinueWith(task =>
                {
                    StatusMessage = "Ready";
                });
            }).ContinueWith(task =>
            {
                if (task.Exception != null)
                {
                    ServiceLocator.Instance.LoggerService.Error($"An error occurred attempting to back up the referenced file.{Environment.NewLine}{task.Exception}");

                    StatusMessage = "Referenced file failed to back up. See log for more details.";
                }
            });
        }

        public void BeginInvokeOnDispatcher(Action action)
        {
            Dispatcher?.BeginInvoke(action, DispatcherPriority.Normal);
        }

        private bool CardFilterPredicate(object obj)
        {
            if (obj is not UniqueArtTypeViewModel uavm) return false;

            foreach (SetNameViewModel vm in SetNames)
            {
                if (vm.IsChecked)
                {
                    if (!string.IsNullOrWhiteSpace(CardNameSearch))
                    {
                        if (uavm.Set == vm.Name &&
                            uavm.Name.IndexOf(CardNameSearch, StringComparison.OrdinalIgnoreCase) > -1)
                            return true;
                    }
                    else
                    {
                        if (uavm.Set == vm.Name) return true;
                    }
                }
            }

            return false;
        }

        private bool CardImageFilterPredicate(object obj)
        {
            if (obj is not UniqueArtTypeViewModel uavm) return false;

            foreach (SetNameViewModel vm in SetNames)
            {
                if (vm.IsChecked)
                {
                    if (!string.IsNullOrWhiteSpace(CardImageSearch))
                    {
                        if (uavm.Set == vm.Name &&
                           (!string.IsNullOrWhiteSpace(uavm.ImagePathSmall) || !string.IsNullOrWhiteSpace(uavm.ImagePathNormal)) ||
                            !string.IsNullOrWhiteSpace(uavm.ImagePathLarge) || !string.IsNullOrWhiteSpace(uavm.ImagePathPng) &&
                            (uavm.Name.IndexOf(CardImageSearch, StringComparison.OrdinalIgnoreCase) > -1 ||
                             (!string.IsNullOrWhiteSpace(uavm.Model.oracle_text) &&
                              uavm.Model.oracle_text.IndexOf(CardImageSearch, StringComparison.OrdinalIgnoreCase) > -1)))
                            return true;
                    }
                    else
                    {
                        if (uavm.Set == vm.Name &&
                           (!string.IsNullOrWhiteSpace(uavm.ImagePathSmall) || !string.IsNullOrWhiteSpace(uavm.ImagePathNormal)) ||
                            !string.IsNullOrWhiteSpace(uavm.ImagePathLarge) || !string.IsNullOrWhiteSpace(uavm.ImagePathPng))
                            return true;
                    }
                }
            }

            return false;
        }

        private void CardImageSearchTick(object? sender, EventArgs e)
        {
            cardImageSearchOperation = Dispatcher?.BeginInvoke(() =>
            {
                CardImageCollectionView?.Refresh();
            }, DispatcherPriority.Normal);
        }

        private void CardNameSearchTick(object? sender, EventArgs e)
        {
            cardNameSearchOperation = Dispatcher?.BeginInvoke(() => 
            {
                CardCollectionView?.Refresh();

                VerifyStateOfSmallCheckBoxAndSet();
                VerifyStateOfNormalCheckBoxAndSet();
                VerifyStateOfLargeCheckBoxAndSet();
                VerifyStateOfPngCheckBoxAndSet();
                VerifyStateOfArtCropCheckBoxAndSet();
                VerifyStateOfBorderCropCheckBoxAndSet();
            }, DispatcherPriority.Normal);
        }

        private void CheckAllSmallOptionsForFilteredCards(bool isChecked)
        {
            CheckAllForImageTypeForFilteredCards(isChecked, ImageType.Small);
        }

        private void CheckAllNormalOptionsForFilteredCards(bool isChecked)
        {
            CheckAllForImageTypeForFilteredCards(isChecked, ImageType.Normal);
        }

        private void CheckAllLargeOptionsForFilteredCards(bool isChecked)
        {
            CheckAllForImageTypeForFilteredCards(isChecked, ImageType.Large);
        }

        private void CheckAllPngOptionsForFilteredCards(bool isChecked)
        {
            CheckAllForImageTypeForFilteredCards(isChecked, ImageType.Png);
        }

        private void CheckAllArtCropOptionsForFilteredCards(bool isChecked)
        {
            CheckAllForImageTypeForFilteredCards(isChecked, ImageType.ArtCrop);
        }

        private void CheckAllBorderCropOptionsForFilteredCards(bool isChecked)
        {
            CheckAllForImageTypeForFilteredCards(isChecked, ImageType.BorderCrop);
        }

        private void CheckAllForImageTypeForFilteredCards(bool isChecked, ImageType imageType)
        {
            if (CardCollectionView == null) return;

            for (int i = 0; i < CardCollectionView.Count; i++)
            {
                if (CardCollectionView.GetItemAt(i) is not UniqueArtTypeViewModel uavm) continue;

                uavm.IsInternallySettingValue = true;

                switch (imageType)
                {
                    case ImageType.Small:
                        uavm.DownloadSmallPicture = isChecked;
                        break;
                    case ImageType.Normal:
                        uavm.DownloadNormalPicture = isChecked;
                        break;
                    case ImageType.Large:
                        uavm.DownloadLargePicture = isChecked;
                        break;
                    case ImageType.Png:
                        uavm.DownloadPngPicture = isChecked;
                        break;
                    case ImageType.ArtCrop:
                        uavm.DownloadArtCropPicture = isChecked;
                        break;
                    case ImageType.BorderCrop:
                        uavm.DownloadBorderCropPicture = isChecked;
                        break;
                }

                uavm.IsInternallySettingValue = false;
            }
        }

        private bool CanDownloadCardImages()
        {
            return SelectedImageCount > 0;
        }

        private void DownloadCardData()
        {
            if (InternalDialogViewModel == null) return;

            InternalDialogViewModel.MessageBoxTitle = "Confirm Download";
            InternalDialogViewModel.MessageBoxMessage = $"Are you sure you wish download {SelectedImageCount} images? Images already downloaded will not be downloaded again.";
            InternalDialogViewModel.MessageBoxImage = MessageBoxInternalDialogImage.Help;
            InternalDialogViewModel.MessageBoxIsModal = true;
            InternalDialogViewModel.MessageBoxButton = MessageBoxButton.YesNo;
            InternalDialogViewModel.MessageBoxVisibility = Visibility.Visible;

            // because it is shown modally this code will not execute until it is closed
            InternalDialogViewModel.MessageBoxTitle = string.Empty;
            InternalDialogViewModel.MessageBoxMessage = string.Empty;
            InternalDialogViewModel.MessageBoxImage = MessageBoxInternalDialogImage.Information;
            InternalDialogViewModel.MessageBoxIsModal = false;
            InternalDialogViewModel.MessageBoxButton = MessageBoxButton.OK;

            if (InternalDialogViewModel.MessageBoxResult != MessageBoxResult.Yes)
            {
                return;
            }

            InternalDialogViewModel.CardProgressVisibility = Visibility.Visible;

            Task.Run(() =>
            {
                if (CardCollectionView != null)
                {
                    List<UniqueArtTypeViewModel> selectedCards = new List<UniqueArtTypeViewModel>();

                    for (int i = 0; i < CardData.Count; i++)
                    {
                        UniqueArtTypeViewModel? vm = CardData[i];

                        if (vm == null) continue;

                        if (vm.DownloadArtCropPicture || vm.DownloadBorderCropPicture || vm.DownloadLargePicture ||
                            vm.DownloadNormalPicture || vm.DownloadPngPicture || vm.DownloadSmallPicture)
                        {
                            selectedCards.Add(vm);
                        }
                    }

                    //ServiceLocator.Instance.ScryfallService.ImageProcessed += ScryfallService_ImageProcessed;

                    StatusMessage = "Downloading image files...";

                    // now that we have all our cards that have at least one download check box checked...lets download them
                    // we'll parallel process all the images the best we can
                    Parallel.ForEach(selectedCards, new ParallelOptions { MaxDegreeOfParallelism = 6 }, card =>
                    {
                        string setPath = Path.Combine(ServiceLocator.Instance.PathingService.CardImagePath, card.Model.set_name.ReplaceBadWindowsCharacters());

                        if (!Directory.Exists(setPath))
                            Directory.CreateDirectory(setPath);

                        //ServiceLocator.Instance.ScryfallService.DownloadArtworkFiles(card.Model, card.DownloadSmallPicture, card.DownloadNormalPicture,
                        //    card.DownloadLargePicture, card.DownloadPngPicture, card.DownloadArtCropPicture, card.DownloadBorderCropPicture, setPath);
                    });

                    // check to see if we have already downloaded cards
                    List<string> setPaths = Directory.GetDirectories(ServiceLocator.Instance.PathingService.CardImagePath).ToList();

                    foreach (string setPath in setPaths)
                    {
                        // let's first get all the cards in that set
                        List<UniqueArtTypeViewModel> setCards =
                            selectedCards.Where(card => card.Model.set_name.ReplaceBadWindowsCharacters() == setPath.Substring(setPath.LastIndexOf("\\", StringComparison.OrdinalIgnoreCase) + 1))
                            .ToList();

                        // loop through all our matching cards
                        foreach (UniqueArtTypeViewModel card in setCards)
                        {
                            // get all the paths in our set directory
                            List<string> cardPaths = Directory.GetDirectories(setPath).ToList();

                            foreach (string cardPath in cardPaths)
                            {
                                // if the card directory name matches the last part of our path (the card name part)
                                if (card.Model.name_field.ReplaceBadWindowsCharacters() == cardPath.Substring(cardPath.LastIndexOf("\\", StringComparison.OrdinalIgnoreCase) + 1))
                                {
                                    // check which images it has
                                    List<string> cardImagesFiles = Directory.GetFiles(cardPath).ToList();

                                    foreach (string cardImagePath in cardImagesFiles)
                                    {
                                        string name = Path.GetFileNameWithoutExtension(cardImagePath);

                                        if (string.IsNullOrWhiteSpace(name)) continue;

                                        if (name.Contains("small", StringComparison.OrdinalIgnoreCase))
                                        {
                                            card.ImagePathSmall = cardImagePath;
                                        }

                                        if (name.Contains("normal", StringComparison.OrdinalIgnoreCase))
                                        {
                                            card.ImagePathNormal = cardImagePath;
                                        }

                                        if (name.Contains("large", StringComparison.OrdinalIgnoreCase))
                                        {
                                            card.ImagePathLarge = cardImagePath;
                                        }

                                        if (name.Contains("png", StringComparison.OrdinalIgnoreCase))
                                        {
                                            card.ImagePathPng = cardImagePath;
                                        }

                                        if (name.Contains("artCrop", StringComparison.OrdinalIgnoreCase))
                                        {
                                            card.ImagePathArtCrop = cardImagePath;
                                        }

                                        if (name.Contains("borderCrop", StringComparison.OrdinalIgnoreCase))
                                        {
                                            card.ImagePathBorderCrop = cardImagePath;
                                        }
                                    }
                                }
                            }

                            // if our images doesn't contain our card then add because we added an image
                            InvokeOnDispatcher(() =>
                            {
                                if (!CardImageData.Contains(card))
                                    CardImageData.Add(card);
                            });
                        }
                    }

                    //ServiceLocator.Instance.ScryfallService.ImageProcessed -= ScryfallService_ImageProcessed;

                    InternalDialogViewModel.CardProgressVisibility = Visibility.Collapsed;
                    InternalDialogViewModel.CardProgressValue = 0;

                    StatusMessage = "Card images downloaded";

                    InternalDialogViewModel.MessageBoxTitle = "Card Images Downloaded";
                    InternalDialogViewModel.MessageBoxMessage = "The card images that could be downloaded have been downloaded. See please log for failures (if any).";
                    InternalDialogViewModel.MessageBoxVisibility = Visibility.Visible;

                    InvokeOnDispatcher(() =>
                    {
                        CardImageCollectionView?.Refresh();
                    });

                    Task.Delay(5000).ContinueWith(task =>
                    {
                        StatusMessage = "Ready";
                    });
                }
            });
        }

        private void ScryfallService_ImageProcessed(object? sender, int e)
        {
            if (InternalDialogViewModel == null) return;

            InternalDialogViewModel.CardProgressValue += e;
        }

        private void DownloadData()
        {
            if (InternalDialogViewModel == null) return;

            InternalDialogViewModel.ProgressDialogVisbility = Visibility.Visible;
            InternalDialogViewModel.ProgressMessage = "Contacting Scryfall for information...";
            InternalDialogViewModel.ProgressTitle = "Downloading Data";

            Task.Run(() =>
            {
                return ServiceLocator.Instance.ScryfallService.GetUniqueArtworkUri();
            }).ContinueWith(task =>
            {
                if (task.Exception == null)
                {
                    BulkDataType result = task.Result;

                    if (result == null)
                    {
                        InternalDialogViewModel.ProgressDialogVisbility = Visibility.Collapsed;
                        InternalDialogViewModel.ProgressMessage = string.Empty;
                        InternalDialogViewModel.ProgressTitle = string.Empty;
                        InternalDialogViewModel.MessageBoxVisibility = Visibility.Visible;
                        InternalDialogViewModel.MessageBoxMessage = "Could not download information about unique artwork file from Scryfall.";
                        InternalDialogViewModel.MessageBoxTitle = "Download Failure";
                    }
                    else
                    {
                        InternalDialogViewModel.ProgressMessage = "Information downloaded from Scryfall. Downloading unique artwork file...";

                        string filename = result.download_uri.Substring(result.download_uri.LastIndexOf("/", StringComparison.OrdinalIgnoreCase) + 1);
                        string fullPath = Path.Combine(ServiceLocator.Instance.PathingService.BaseDataPath, filename);

                        // clear all existing JSON files (the one)
                        string[] files = Directory.GetFiles(ServiceLocator.Instance.PathingService.BaseDataPath, "*.json");

                        foreach (string file in files)
                        {
                            File.Delete(file);
                        }

                        ServiceLocator.Instance.ScryfallService.DownloadUniqueArtworkFile(result, fullPath);

                        if (File.Exists(fullPath))
                        {
                            InternalDialogViewModel.FileLocation = fullPath;

                            InternalDialogViewModel.ProgressMessage = "Information downloaded from Scryfall.";

                            // after 5 seconds hide the message
                            Task.Delay(5000).ContinueWith((delayTask) =>
                            {
                                ProcessFileData();
                            });
                        }
                        else
                        {
                            InternalDialogViewModel.ProgressDialogVisbility = Visibility.Collapsed;
                            InternalDialogViewModel.ProgressMessage = string.Empty;
                            InternalDialogViewModel.ProgressTitle = string.Empty;
                            InternalDialogViewModel.MessageBoxVisibility = Visibility.Visible;
                            InternalDialogViewModel.MessageBoxMessage = "Unable to download file information from Scryfall. Please check internet connect, that Scryfall is currently online or look in the log to see if an error occurred.";
                            InternalDialogViewModel.MessageBoxTitle = "Download Failure";
                        }
                    }
                }
            }).ContinueWith(task =>
            {
                if (task.Exception != null)
                {
                    ServiceLocator.Instance.LoggerService.Error($"An error occurred attempting to load in file form browser.{Environment.NewLine}{task.Exception}");

                    InternalDialogViewModel.ProgressDialogVisbility = Visibility.Collapsed;
                    InternalDialogViewModel.ProgressMessage = string.Empty;
                    InternalDialogViewModel.ProgressTitle = string.Empty;
                    InternalDialogViewModel.MessageBoxVisibility = Visibility.Visible;
                    InternalDialogViewModel.MessageBoxMessage = $"An error occurred attempting to download the file. {task.Exception.Message}{Environment.NewLine}See the log for more details.";
                    InternalDialogViewModel.MessageBoxTitle = "Error Processing";
                }
            });
        }

        public void InvokeOnDispatcher(Action action)
        {
            Dispatcher?.Invoke(action, DispatcherPriority.Normal);
        }

        public void ProcessFileData()
        {
            if (InternalDialogViewModel == null) return;

            InternalDialogViewModel.ProgressDialogVisbility = Visibility.Visible;
            InternalDialogViewModel.ProgressMessage = "Processing selected file...";
            InternalDialogViewModel.ProgressTitle = "Processing Data";

            Task.Run(() =>
            {
                BackupReferencedFile();

                string text = File.ReadAllText(InternalDialogViewModel.FileLocation);

                List<UniqueArtType>? cards = JsonConvert.DeserializeObject<List<UniqueArtType>>(text);

                // get distinct card names within each set
                List<UniqueArtType> uniqueCards = new();

                var groupedBySet = cards?.GroupBy(c => c.set_name);

                if (groupedBySet != null)
                {
                    foreach (var group in groupedBySet)
                    {
                        var nameGrouped = group.GroupBy(c => c.name_field).ToList();

                        foreach (var nameGroup in nameGrouped)
                        {
                            uniqueCards.Add(nameGroup.First());
                        }
                    }
                }

                uniqueCards = uniqueCards.Where(card => card.image_uris != null).ToList();
                uniqueCards = uniqueCards.OrderBy(uat => uat.set_name).ThenBy(uat => uat.name_field).ToList();

                // build our vm representations
                List<UniqueArtTypeViewModel> vms = uniqueCards.Select(c => new UniqueArtTypeViewModel(c, this)).ToList();
                List<string> uniqueSetNames = uniqueCards.Select(c => c.set_name).Distinct().ToList();

                // we know what cards are allowed in standard mode, then we get the list of distinct set names just like above                
                standardOnlySets = uniqueCards.Where(c => c.legalities.standard == "legal").Select(c => c.set_name).Distinct().ToList();

                // check to see if we have already downloaded cards
                List<string> setPaths = Directory.GetDirectories(ServiceLocator.Instance.PathingService.CardImagePath).ToList();

                foreach (string setPath in setPaths)
                {
                    // let's first get all the cards in that set
                    List<UniqueArtTypeViewModel> setCards =
                        vms.Where(card => card.Model.set_name.ReplaceBadWindowsCharacters() == setPath.Substring(setPath.LastIndexOf("\\", StringComparison.OrdinalIgnoreCase) + 1))
                        .ToList();

                    // loop through all our matching cards
                    foreach (UniqueArtTypeViewModel card in setCards)
                    {
                        // get all the paths in our set directory
                        List<string> cardPaths = Directory.GetDirectories(setPath).ToList();

                        foreach (string cardPath in cardPaths)
                        {
                            // if the card directory name matches the last part of our path (the card name part)
                            if (card.Model.name_field.ReplaceBadWindowsCharacters() == cardPath.Substring(cardPath.LastIndexOf("\\", StringComparison.OrdinalIgnoreCase) + 1))
                            {
                                // check which images it has
                                List<string> cardImagesFiles = Directory.GetFiles(cardPath).ToList();

                                foreach (string cardImagePath in cardImagesFiles)
                                {
                                    string name = Path.GetFileNameWithoutExtension(cardImagePath);

                                    if (string.IsNullOrWhiteSpace(name)) continue;

                                    if (name.Contains("small", StringComparison.OrdinalIgnoreCase))
                                    {
                                        card.DownloadSmallPicture = true;
                                        card.ImagePathSmall = cardImagePath;
                                    }

                                    if (name.Contains("normal", StringComparison.OrdinalIgnoreCase))
                                    {
                                        card.DownloadNormalPicture = true;
                                        card.ImagePathNormal = cardImagePath;
                                    }

                                    if (name.Contains("large", StringComparison.OrdinalIgnoreCase))
                                    {
                                        card.DownloadLargePicture = true;
                                        card.ImagePathLarge = cardImagePath;
                                    }

                                    if (name.Contains("png", StringComparison.OrdinalIgnoreCase))
                                    {
                                        card.DownloadPngPicture = true;
                                        card.ImagePathPng = cardImagePath;
                                    }

                                    if (name.Contains("artCrop", StringComparison.OrdinalIgnoreCase))
                                    {
                                        card.DownloadArtCropPicture = true;
                                        card.ImagePathArtCrop = cardImagePath;
                                    }

                                    if (name.Contains("borderCrop", StringComparison.OrdinalIgnoreCase))
                                    {
                                        card.DownloadBorderCropPicture = true;
                                        card.ImagePathBorderCrop = cardImagePath;
                                    }
                                }
                            }
                        }
                    }
                }

                InvokeOnDispatcher(() =>
                {
                    CardData.Clear();
                    CardData.AddRange(vms);

                    CardImageData.Clear();
                    CardImageData.AddRange(vms.Where(vm => !string.IsNullOrWhiteSpace(vm.ImagePathPng) || !string.IsNullOrWhiteSpace(vm.ImagePathSmall) ||
                                                           !string.IsNullOrWhiteSpace(vm.ImagePathNormal) || !string.IsNullOrWhiteSpace(vm.ImagePathLarge)));

                    SetNames.Clear();
                    SetNames.AddRange(uniqueSetNames.Select(s => new SetNameViewModel(this) { Name = s }));

                    SetCollectionView = (CollectionView)CollectionViewSource.GetDefaultView(SetNames);
                    SetCollectionView.Filter = SetNameFilterPredicate;

                    CardCollectionView = (CollectionView)CollectionViewSource.GetDefaultView(CardData);
                    CardCollectionView.Filter = CardFilterPredicate;

                    CardImageCollectionView = (CollectionView)CollectionViewSource.GetDefaultView(CardImageData);
                    CardImageCollectionView.Filter = CardImageFilterPredicate;
                });
            }).ContinueWith((task) =>
            {
                InternalDialogViewModel.ProgressMessage = string.Empty;
                InternalDialogViewModel.ProgressTitle = string.Empty;
                InternalDialogViewModel.ProgressDialogVisbility = Visibility.Collapsed;
                InternalDialogViewModel.MessageBoxVisibility = Visibility.Visible;
                InternalDialogViewModel.NeedDataVisibility = Visibility.Collapsed;

                StatusMessage = "Ready";

                if (task.Exception != null)
                {
                    ServiceLocator.Instance.LoggerService.Error($"An error occurred attempting to load in file form browser.{Environment.NewLine}{task.Exception}");

                    InternalDialogViewModel.MessageBoxMessage = $"An error occurred attempting to process the file. {task.Exception.Message}{Environment.NewLine}See the log for more details.";
                    InternalDialogViewModel.MessageBoxTitle = "Error Processing";
                }
                else
                {
                    InternalDialogViewModel.MessageBoxMessage = "The file has been processed.";
                    InternalDialogViewModel.MessageBoxTitle = "File Processed";
                }
            });
        }

        private bool SetNameFilterPredicate(object obj)
        {
            if (obj is not SetNameViewModel snvm) return false;

            if (ShowOnlySetAllowedInMtgaStandard)
            {
                if (snvm.Name.Equals("Adventures in the Forgotten Realms", StringComparison.OrdinalIgnoreCase) ||
                    snvm.Name.Equals("Innistrad: Crimson Vow", StringComparison.OrdinalIgnoreCase) ||
                    snvm.Name.Equals("Innistrad: Midnight Hunt", StringComparison.OrdinalIgnoreCase) ||
                    snvm.Name.Equals("Kaldheim", StringComparison.OrdinalIgnoreCase) ||
                    snvm.Name.Equals("Strixhaven: School of Mages", StringComparison.OrdinalIgnoreCase) ||
                    snvm.Name.Equals("Zendikar Rising", StringComparison.OrdinalIgnoreCase) ||
                    snvm.Name.Equals("Kamigawa: Neon Dynasty", StringComparison.OrdinalIgnoreCase))
                {
                    if (string.IsNullOrWhiteSpace(SetNameSearch)) return true;

                    if (snvm.Name.IndexOf(SetNameSearch, StringComparison.OrdinalIgnoreCase) > -1) return true;
                    else return false;
                }

                return false;
            }

            if (ShowOnlySetAllowedInStandard)
            {
                if (standardOnlySets == null) return false;

                if (standardOnlySets.Contains(snvm.Name))
                {
                    if (string.IsNullOrWhiteSpace(SetNameSearch)) return true;

                    if (snvm.Name.IndexOf(SetNameSearch, StringComparison.OrdinalIgnoreCase) > -1) return true;
                    else return false;
                }
                else return false;
            }
            else
            {
                if (string.IsNullOrWhiteSpace(SetNameSearch)) return true;

                if (snvm.Name.IndexOf(SetNameSearch, StringComparison.OrdinalIgnoreCase) > -1) return true;
                else return false;
            }
        }

        private void SetNameSearchTick(object? sender, EventArgs e)
        {
            setNameSearchOperation = Dispatcher?.BeginInvoke(() =>
            {
                SetCollectionView?.Refresh();
            }, DispatcherPriority.Normal);
        }

        public void VerifyStateOfSmallCheckBoxAndSet()
        {
            VerifyStateOfImageTypeCheckBoxAndSet(ImageType.Small);
        }

        public void VerifyStateOfNormalCheckBoxAndSet()
        {
            VerifyStateOfImageTypeCheckBoxAndSet(ImageType.Normal);
        }

        public void VerifyStateOfLargeCheckBoxAndSet()
        {
            VerifyStateOfImageTypeCheckBoxAndSet(ImageType.Large);
        }

        public void VerifyStateOfPngCheckBoxAndSet()
        {
            VerifyStateOfImageTypeCheckBoxAndSet(ImageType.Png);
        }

        public void VerifyStateOfArtCropCheckBoxAndSet()
        {
            VerifyStateOfImageTypeCheckBoxAndSet(ImageType.ArtCrop);
        }

        public void VerifyStateOfBorderCropCheckBoxAndSet()
        {
            VerifyStateOfImageTypeCheckBoxAndSet(ImageType.BorderCrop);
        }

        private void VerifyStateOfImageTypeCheckBoxAndSet(ImageType imageType)
        {
            if (CardCollectionView == null) return;

            int allCount = CardCollectionView.Count;
            int ourCount = 0;

            for (int i = 0; i < allCount; i++)
            {
                if (CardCollectionView.GetItemAt(i) is not UniqueArtTypeViewModel uavm) continue;

                switch (imageType)
                {
                    case ImageType.Small:
                        if (uavm.DownloadSmallPicture)
                            ourCount++;
                        break;
                    case ImageType.Normal:
                        if (uavm.DownloadNormalPicture)
                            ourCount++;
                        break;
                    case ImageType.Large:
                        if (uavm.DownloadLargePicture)
                            ourCount++;
                        break;
                    case ImageType.Png:
                        if (uavm.DownloadPngPicture)
                            ourCount++;
                        break;
                    case ImageType.ArtCrop:
                        if (uavm.DownloadArtCropPicture)
                            ourCount++;
                        break;
                    case ImageType.BorderCrop:
                        if (uavm.DownloadBorderCropPicture)
                            ourCount++;
                        break;
                }
            }

            switch (imageType)
            {
                case ImageType.Small:
                    if (ourCount == 0) IsAllSmallChecked = false; // none = no check
                    else if (ourCount == allCount) IsAllSmallChecked = true; // all = check
                    else IsAllSmallChecked = null; // between 1 and count - 1 set to partial state
                    break;
                case ImageType.Normal:
                    if (ourCount == 0) IsAllNormalChecked = false; // none = no check
                    else if (ourCount == allCount) IsAllNormalChecked = true; // all = check
                    else IsAllNormalChecked = null; // between 1 and count - 1 set to partial state
                    break;
                case ImageType.Large:
                    if (ourCount == 0) IsAllLargeChecked = false; // none = no check
                    else if (ourCount == allCount) IsAllLargeChecked = true; // all = check
                    else IsAllLargeChecked = null; // between 1 and count - 1 set to partial state
                    break;
                case ImageType.Png:
                    if (ourCount == 0) IsAllPngChecked = false; // none = no check
                    else if (ourCount == allCount) IsAllPngChecked = true; // all = check
                    else IsAllPngChecked = null; // between 1 and count - 1 set to partial state
                    break;
                case ImageType.ArtCrop:
                    if (ourCount == 0) IsAllArtCropChecked = false; // none = no check
                    else if (ourCount == allCount) IsAllArtCropChecked = true; // all = check
                    else IsAllArtCropChecked = null; // between 1 and count - 1 set to partial state
                    break;
                case ImageType.BorderCrop:
                    if (ourCount == 0) IsAllBorderCropChecked = false; // none = no check
                    else if (ourCount == allCount) IsAllBorderCropChecked = true; // all = check
                    else IsAllBorderCropChecked = null; // between 1 and count - 1 set to partial state
                    break;
            }
        }

        #endregion
    }
}
