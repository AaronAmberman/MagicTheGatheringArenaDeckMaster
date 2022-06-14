using MagicTheGatheringArena.Core.MVVM;
using MagicTheGatheringArena.Core.Scryfall.Data;
using System;

namespace MagicTheGatheringArenaDeckMaster.ViewModels
{
    internal class UniqueArtTypeViewModel : ViewModelBase
    {
        #region Fields

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
                    // isolate artifacts that are colorless and not artifact lands
                    score = 16;
                }

                return score;
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

                    if (temp == "W") total += 1;
                    else if (temp == "U") total += 1;
                    else if (temp == "B") total += 1;
                    else if (temp == "R") total += 1;
                    else if (temp == "G") total += 1;
                    else if (temp == "X") total += 20;
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
    }
}
