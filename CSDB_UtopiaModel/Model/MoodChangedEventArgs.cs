using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSDB_UtopiaModel.Model
{
    public class MoodChangedEventArgs : EventArgs
    {
        public int Mood;
        public MoodChangedEventArgs(int newValue)
        {
            Mood = newValue;
        }
    }
}
