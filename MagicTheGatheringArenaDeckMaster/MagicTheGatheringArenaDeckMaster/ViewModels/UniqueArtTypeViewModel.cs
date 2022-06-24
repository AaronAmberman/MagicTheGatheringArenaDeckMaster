using MagicTheGatheringArena.Core.MVVM;
using MagicTheGatheringArena.Core.Scryfall.Data;
using System;

namespace MagicTheGatheringArenaDeckMaster.ViewModels
{
    public class UniqueArtTypeViewModel : ViewModelBase
    {
        #region Fields

        private int deckBuilderDeckCount = 1;
        private UniqueArtType model;
        private string imagePath;

        #endregion

        #region Properties

        public bool IsAlchemyVariation
        {
            get
            {
                return Name.StartsWith("A-", StringComparison.OrdinalIgnoreCase);
            }
        }

        public int ColorScore
        {
            get
            {
                if (model == null) return 0;

                int score = 0;

                if (model.mana_cost.Contains("W"))
                {
                    score += 1;
                }
                else if (model.mana_cost.Contains("U"))
                {
                    score += 2;
                }
                else if (model.mana_cost.Contains("B"))
                {
                    score += 3;
                }
                else if (model.mana_cost.Contains("R"))
                {
                    score += 4;
                }
                else if (model.mana_cost.Contains("G"))
                {
                    score += 5;
                }
                else if (!model.mana_cost.Contains("W") && !model.mana_cost.Contains("U") && !model.mana_cost.Contains("B") && !model.mana_cost.Contains("R") && !model.mana_cost.Contains("G") &&
                    model.type_line.Contains("Artifact") && !model.type_line.Contains("Land"))
                {
                    score = 16;
                }
                else if (!model.mana_cost.Contains("W") && !model.mana_cost.Contains("U") && !model.mana_cost.Contains("B") && !model.mana_cost.Contains("R") && !model.mana_cost.Contains("G") &&
                    model.type_line.Contains("Artifact Land"))
                {
                    score = 16;
                }
                else if (!model.mana_cost.Contains("W") && !model.mana_cost.Contains("U") && !model.mana_cost.Contains("B") && !model.mana_cost.Contains("R") && !model.mana_cost.Contains("G") &&
                    model.type_line.Contains("Lesson") && model.colors.Count == 0)
                {
                    score = 16;
                }
                else if (!model.mana_cost.Contains("W") && !model.mana_cost.Contains("U") && !model.mana_cost.Contains("B") && !model.mana_cost.Contains("R") && !model.mana_cost.Contains("G") &&
                    !model.type_line.Contains("Artifact") && model.type_line.Contains("Land"))
                {
                    score = 17;
                }

                return score;
            }
        }

        public int DeckBuilderDeckCount 
        { 
            get => deckBuilderDeckCount; 
            set
            {
                deckBuilderDeckCount = value;
                OnPropertyChanged();
            }
        }

        public string ImagePath
        {
            get => imagePath;
            set
            {
                imagePath = value;
                OnPropertyChanged();
            }
        }

        public int ManaCostTotal
        {
            get
            {
                if (model == null) return 0;

                int total = 0;

                string[] valueGroup = model.mana_cost.Split("}", StringSplitOptions.RemoveEmptyEntries);

                foreach (string value in valueGroup)
                {
                    string temp = value.Replace("{", "");

                    // use contains because of either or color mana need such as G/W or W/B
                    if (temp.Contains("W")) total += 1;
                    else if (temp.Contains("U")) total += 1;
                    else if (temp.Contains("B")) total += 1;
                    else if (temp.Contains("R")) total += 1;
                    else if (temp.Contains("G")) total += 1;
                    else if (temp.Contains("X")) total += 20;
                    else if (int.TryParse(temp, out int convRes)) total += convRes;
                }

                return total;
            }
        }

        public UniqueArtType Model
        {
            get => model;
            set
            {
                model = value;
                OnPropertyChanged();
            }
        }

        public string Name => model.name_field;

        public int NumberOfColors
        {
            get
            {
                if (model == null) return 0;

                if (!model.mana_cost.Contains("W") && !model.mana_cost.Contains("U") && !model.mana_cost.Contains("B") && !model.mana_cost.Contains("R") && !model.mana_cost.Contains("G") &&
                    model.type_line.Contains("Artifact") && !model.type_line.Contains("Land"))
                {
                    // there aren't actually 6 colors but this will put artifacts at the end of the list
                    return 6;
                }
                else if (!model.mana_cost.Contains("W") && !model.mana_cost.Contains("U") && !model.mana_cost.Contains("B") && !model.mana_cost.Contains("R") && !model.mana_cost.Contains("G") &&
                    model.type_line.Contains("Artifact Land"))
                {
                    // there aren't actually 6 colors but this will put lands at the end of the list
                    return 6;
                }
                else if (!model.mana_cost.Contains("W") && !model.mana_cost.Contains("U") && !model.mana_cost.Contains("B") && !model.mana_cost.Contains("R") && !model.mana_cost.Contains("G") &&
                    model.type_line.Contains("Lesson") && model.colors.Count == 0)
                {
                    // there aren't actually 6 colors but this will put lands at the end of the list
                    return 6;
                }
                else if (!model.mana_cost.Contains("W") && !model.mana_cost.Contains("U") && !model.mana_cost.Contains("B") && !model.mana_cost.Contains("R") && !model.mana_cost.Contains("G") &&
                    !model.type_line.Contains("Artifact") && model.type_line.Contains("Land"))
                {
                    if (model.type_line.Contains("Basic Land"))
                    {
                        // there aren't actually 7 colors but this will put lands at the end of the list
                        return 7;
                    }
                    else if (model.type_line.Contains("Basic Snow Land"))
                    {
                        // there aren't actually 8 colors but this will put lands at the end of the list
                        return 8;
                    }
                    else
                    {
                        // there aren't actually 9 colors but this will put lands at the end of the list
                        return 9;
                    }
                }
                else
                {
                    return model.colors.Count;
                }
            }
        }

        public string Set => model.set_name;

        #endregion

        #region Constructors

        public UniqueArtTypeViewModel(UniqueArtType uat)
        {
            model = uat;
            imagePath = string.Empty;
        }

        #endregion

        #region Methods

        public UniqueArtTypeViewModel Clone()
        {
            return new UniqueArtTypeViewModel(Model)
            {
                ImagePath = ImagePath
            };
        }

        #endregion
    }
}
