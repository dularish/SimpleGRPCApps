using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace ScoreUpdaterConsole.ViewModels
{
    class ScoreboardVM : INotifyPropertyChanged
    {
        private int _ballsBowled;
        private int _runsScored;
        private int _wicketsLost;

        public int BallsBowled
        {
            get => _ballsBowled; set
            {
                _ballsBowled = value;
                onPropertyChanged();
            }
        }

        public int RunsScored
        {
            get => _runsScored; set
            {
                _runsScored = value;
                onPropertyChanged();
            }
        }
        public int WicketsLost
        {
            get => _wicketsLost; set
            {
                _wicketsLost = value;
                onPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void onPropertyChanged([CallerMemberName]string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
